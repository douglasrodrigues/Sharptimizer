namespace Optimizer.Metaheuristics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Math;

    /// <summary>
    /// Manta Ray Foraging Optimization - 
    /// References:
    /// W. Zhao, Z. Zhang and L. Wang.
    /// Manta Ray Foraging Optimization: An effective bio-inspired optimizer for engineering applications.
    /// Engineering Applications of Artificial Intelligence (2020).
    /// </summary>
    public class MRFO : Metaheuristic
    {
        /// <summary>
        /// Somersault foraging.
        /// </summary>
        public double S { get; set; }
        
        /// <summary>
        /// This constructor initializes the MRFO.
        /// <param name="hyperparams">Contains key-value parameters to the meta-heuristics.</param>
        /// <param name="algorithm">Indicates the algorithm name.</param>
        /// </summary>
        public MRFO(Dictionary<string, double> hyperparams = null, string algorithm = "MRFO") : base(algorithm)
        {
            S = 2;

            // Now, we need to build this class up
            Build(hyperparams);
        }

        /// <summary>
        /// It decorates the agent.
        /// <param name="agents">List of agents.</param>
        /// </summary>
        public override void Decorate(List<Agent> agents)
        {
            foreach (Agent agent in agents)
            {
                this.Agents.Add(agent);
            }
        }

        /// <summary>
        /// Performs the cyclone foraging procedure (eq. 3-7).
        /// <param name="agents">List of agents.</param>
        /// <param name="bestAgent">Global best agent.</param>
        /// <param name="i">Index of current manta ray.</param>
        /// <param name="iteration">Number of current iteration.</param>
        /// <param name="n_iterations">Maximum number of iterations.</param>
        /// <returns>A new cyclone foraging.</returns>
        /// </summary>
        private double[] CycloneForaging(List<Agent> agents, Agent bestAgent, int i, int iteration, int n_iterations)
        {
            double[] cyclone_foraging = new double[agents[i].NumberVariables];

            // Generates an uniform random number
            double r1 = Stochastic.GenerateUniformRandomNumber();

            // Calculates the beta constant
            double beta = 2 * Math.Exp(r1 * (n_iterations - iteration + 1) / n_iterations) * Math.Sin(2 * Math.PI * r1);

            // Generates an uniform random number
            double r2 = Stochastic.GenerateUniformRandomNumber();

            // Generates an uniform random number
            double r3 = Stochastic.GenerateUniformRandomNumber();

            // Check if current iteration proportion is smaller than random generated number
            if (iteration / n_iterations < r2)
            {
                // Generates an array for holding the random position
                double[] rand_position = new double[agents[i].NumberVariables];

                // For every decision variable
                var bounds = agents[i].LowerBound.Zip(agents[i].UpperBound, (n, w) => new { Lower = n, Upper = w });
                foreach (var (bound, index) in bounds.Select((v, i) => (v, i)))
                {
                    // Generates uniform random positions
                    rand_position[index] = Stochastic.GenerateUniformRandomNumber(bound.Lower, bound.Upper);
                }

                // Checks if the index is equal to zero
                if (i == 0)
                {
                    for (int j = 0; j < agents[i].NumberVariables; j++)
                    {
                        cyclone_foraging[j] = rand_position[j] + r3 * (rand_position[j] - agents[i].Position[j]) + beta * (rand_position[j] - agents[i].Position[j]);
                    }
                }
                // If index is different than zero
                else
                {
                    for (int j = 0; j < agents[i].NumberVariables; j++)
                    {
                        cyclone_foraging[j] = rand_position[j] + r3 * (agents[i - 1].Position[j] - agents[i].Position[j]) + beta * (rand_position[j] - agents[i].Position[j]);
                    }
                }
            }
            // If current iteration proportion is bigger than random generated number
            else
            {
                // Checks if the index is equal to zero
                if (i == 0)
                {
                    for (int j = 0; j < agents[i].NumberVariables; j++)
                    {
                        cyclone_foraging[j] = bestAgent.Position[j] + r3 * (bestAgent.Position[j] - agents[i].Position[j]) + beta * (bestAgent.Position[j] - agents[i].Position[j]);
                    }
                }
                // If index is different than zero
                else
                {
                    for (int j = 0; j < agents[i].NumberVariables; j++)
                    {
                        cyclone_foraging[j] = bestAgent.Position[j] + r3 * (agents[i - 1].Position[j] - agents[i].Position[j]) + beta * (bestAgent.Position[j] - agents[i].Position[j]);
                    }
                }
            }

            return cyclone_foraging;
        }

        /// <summary>
        /// Performs the chain foraging procedure (eq. 1-2).
        /// <param name="agents">List of agents.</param>
        /// <param name="bestAgent">Global best agent.</param>
        /// <param name="i">Index of current manta ray.</param>
        /// <returns>A new chain foraging.</returns>
        /// </summary>
        private double[] ChainForaging(List<Agent> agents, Agent bestAgent, int i)
        {
            double[] chain_foraging = new double[agents[i].NumberVariables];

            // Generates an uniform random number
            double r1 = Stochastic.GenerateUniformRandomNumber();

            // Calculates the alpha constant
            double alpha = 2 * r1 * Math.Sqrt(Math.Abs(Math.Log(r1)));

            // Generates an uniform random number
            double r2 = Stochastic.GenerateUniformRandomNumber();

            // Checks if the index is equal to zero
            if (i == 0)
            {
                // If yes, uses this equation
                for (int j = 0; j < agents[i].NumberVariables; j++)
                {
                    chain_foraging[j] = agents[i].Position[j] + r2 * (bestAgent.Position[j] - agents[i].Position[j]) + alpha * (bestAgent.Position[j] - agents[i].Position[j]);
                }
            }
            // If index is different than zero
            else
            {
                // Uses this equation
                for (int j = 0; j < agents[i].NumberVariables; j++)
                {
                    chain_foraging[j] = agents[i].Position[j] + r2 * (agents[i - 1].Position[j] - agents[i].Position[j]) + alpha * (bestAgent.Position[j] - agents[i].Position[j]);
                }
            }

            return chain_foraging;
        }

        /// <summary>
        /// Performs the somersault foraging procedure (eq. 8).
        /// <param name="agents">List of agents.</param>
        /// <param name="bestAgent">Global best agent.</param>
        /// <returns>A new somersault foraging.</returns>
        /// </summary>
        private double[] SomersaultForaging(Agent agent, Agent bestAgent)
        {
            double[] somersault_foraging = new double[agent.NumberVariables];

            // Generates an uniform random number
            double r1 = Stochastic.GenerateUniformRandomNumber();

            // Generates an uniform random number
            double r2 = Stochastic.GenerateUniformRandomNumber();

            // Calculates the somersault foraging
            for (int j = 0; j < agent.NumberVariables; j++)
            {
                somersault_foraging[j] = agent.Position[j] + this.S * (r1 * bestAgent.Position[j] - r2 * agent.Position[j]);
            }

            return somersault_foraging;
        }

        /// <summary>
        /// Method that wraps the update pipeline over all agents and variables.
        /// <param name="space">A Space object that will be evaluated.</param>
        /// <param name="function">A Function object serving as an objective function.</param>
        /// </summary>
        public override void Update(Space space, Function function, int iteration)
        {
            // Iterate through all agents
            for (int i = 0; i < Agents.Count; i++)
            {
                // Generates an uniform random number
                double r1 = Stochastic.GenerateUniformRandomNumber();

                // If random number is smaller than 1/2
                if (r1 < 0.5)
                {
                    // Performs the cyclone foraging
                    Agents[i].Position = CycloneForaging(Agents, space.BestAgent, i, iteration, space.NumberIterations);
                }
                else
                {
                    // Performs the chain foraging
                    Agents[i].Position = ChainForaging(Agents, space.BestAgent, i);
                }

                // Clips the agent's limits
                Agents[i].ClipLimits();

                // Evaluates the agent
                Agents[i].FitnessValue = function.Calculate(Agents[i].Position);

                // If new agent's fitness is better than best
                if (Agents[i].FitnessValue < space.BestAgent.FitnessValue)
                {
                    space.BestAgent = (Agent)Agents[i].Clone();
                }
            }

            // Iterate through all agents
            foreach (Agent agent in Agents)
            {
                // Performs the somersault foraging
                agent.Position = SomersaultForaging(agent, space.BestAgent);
            }

            // Checking if agents meet the bounds limits
            foreach (Agent agent in Agents)
            {
                agent.ClipLimits();
            }

            // After the update, we need to re-evaluate the search space
            space.BestAgent = Evaluate(space.BestAgent, function);
        }
    }
}