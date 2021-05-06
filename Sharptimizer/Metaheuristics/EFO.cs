namespace Sharptimizer.Metaheuristics
{
    using System;
    using System.Collections.Generic;
    using Core;
    using Math;

    /// <summary>
    /// Electromagnetic Field Optimization - 
    /// References:
    /// H. Abedinpourshotorban, et al.
    /// Electromagnetic field optimization: A physics-inspired metaheuristic optimization algorithm.
    /// Swarm and Evolutionary Computation (2016).
    /// </summary>
    public class EFO : Metaheuristic
    {
        // Positive field proportion
        public double positive_field { get; set; }

        // Negative field proportion
        public double negative_field { get; set; }

        // Probability of selecting electromagnets
        public double ps_ratio { get; set; }

        // Probability of selecting a random electromagnet
        public double r_ratio { get; set; }

        // Defines the golden ratio
        public double phi { get; set; }

        // Defines the electromagnetic index
        public int RI { get; set; }

        /// <summary>
        /// This constructor initializes the EFO.
        /// <param name="hyperparams">Contains key-value parameters to the meta-heuristics.</param>
        /// <param name="algorithm">Indicates the algorithm name.</param>
        /// </summary>
        public EFO(Dictionary<string, double> hyperparams = null, string algorithm = "EFO") : base(algorithm)
        {
            positive_field = 0.1;
            negative_field = 0.5;
            ps_ratio = 0.1;
            r_ratio = 0.4;

            phi = (1 + Math.Sqrt(5)) / 2;

            RI = 0;

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
        /// Calculates the indexes of positive, negative and neutral particles.
        /// <param name="n_agents">Number of agents in the space.</param>
        /// <returns>Positive, negative and neutral particles' indexes.</returns>
        /// </summary>
        private (int, int, int) CalculateIndexes(int n_agents)
        {
            // Calculates a positive particle's index
            int positive_index = (int)Stochastic.GenerateUniformRandomNumber(0, n_agents * this.positive_field);

            // Calculates a negative particle's index
            int negative_index = (int)Stochastic.GenerateUniformRandomNumber(n_agents * (1 - this.negative_field), n_agents);

            // Calculates a neutral particle's index
            int neutral_index = (int)Stochastic.GenerateUniformRandomNumber(n_agents * this.positive_field, n_agents * (1 - this.negative_field));

            return (positive_index, negative_index, neutral_index);
        }

        /// <summary>
        /// Method that wraps the update pipeline over all agents and variables.
        /// <param name="space">A Space object that will be evaluated.</param>
        /// <param name="function">A Function object serving as an objective function.</param>
        /// </summary>
        public override void Update(Space space, Function function, int iteration)
        {
            // Sorts agents according to their fitness
            Agents.Sort();

            // Making a deepcopy of current's best agent
            Agent a = (Agent)Agents[0].Clone();

            // Generates a uniform random number for the force
            double force = Stochastic.GenerateUniformRandomNumber();

            // For every decision variable
            for (int j = 0; j < a.NumberVariables; j++)
            {
                // Calculates the index of positive, negative and neutral particles
                var (pos, neg, neu) = CalculateIndexes(Agents.Count);

                // Generates a uniform random number
                double r1 = Stochastic.GenerateUniformRandomNumber();

                // If random number is smaller than the probability of selecting electromagnets
                if (r1 < this.ps_ratio)
                {
                    // Applies agent's position as positive particle's position
                    a.Position[j] = Agents[pos].Position[j];
                }
                // If random number is bigger
                else
                {
                    // Calculates the new agent's position
                    a.Position[j] = Agents[neg].Position[j] + phi * force *
                                                            (Agents[pos].Position[j] - Agents[neu].Position[j]) - force *
                                                            (Agents[neg].Position[j] - Agents[neu].Position[j]);
                }
            }
            // Clips the agent's position to its limits
            a.ClipLimits();

            // Generates a third uniform random number
            double r2 = Stochastic.GenerateUniformRandomNumber();

            // If random number is smaller than probability of changing a random electromagnet
            if (r2 < this.r_ratio)
            {
                // Update agent's position based on RI
                a.Position[RI] = Stochastic.GenerateUniformRandomNumber(a.LowerBound[RI], a.UpperBound[RI]);

                // Increases RI by one
                RI += 1;

                // If RI exceeds the number of variables
                if (RI >= a.NumberVariables)
                {
                    // Resets it to one
                    RI = 1;
                }
            }

            // Calculates the agent's fitness
            a.FitnessValue = function.Calculate(a.Position);

            // If newly generated agent fitness is better than worst fitness
            if (a.FitnessValue < Agents[Agents.Count - 1].FitnessValue)
            {
                Agents[Agents.Count - 1] = (Agent)a.Clone();
            }

            // Checking if agents meet the bounds limits
            foreach (Agent agent in Agents)
            {
                agent.ClipLimits();
            }

            // TODO: change to UpdateBestAgent()
            // After the update, we need to re-evaluate the search space
            space.BestAgent = Evaluate(space.BestAgent, function);
        }
    }
}