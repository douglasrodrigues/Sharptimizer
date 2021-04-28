namespace Optimizer.Metaheuristics
{
    using System;
    using System.Collections.Generic;
    using Core;
    using Math;

    /// <summary>
    /// Water Cycle Algorithm -
    /// Reference:
    /// H. Eskandar.
    /// Water cycle algorithm – A novel metaheuristic optimization method for solving constrained engineering optimization problems.
    /// Computers & Structures (2012).
    /// </summary>
    public class WCA : Metaheuristic
    {
        // Number of rivers + sea
        public int nsr { get; set; }

        // Maximum evaporation condition
        public double d_max { get; set; }

        public int[] flows { get; set; }

        /// <summary>
        /// This constructor initializes the BH.
        /// <param name="hyperparams">Contains key-value parameters to the meta-heuristics.</param>
        /// <param name="algorithm">Indicates the algorithm name.</param>
        /// </summary>
        public WCA(Dictionary<string, double> hyperparams = null, string algorithm = "WCA") : base(algorithm)
        {
            nsr = 2;
            d_max = 0.1;

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

            flows = FlowIntensity(Agents);
        }

        /// <summary>
        /// Calculates the intensity of each possible flow (eq. 6).
        /// <param name="agents">List of agents.</param>
        /// <returns>It returns an array of flows' intensity.</returns>
        /// </summary>
        private int[] FlowIntensity(List<Agent> agents)
        {
            // Initial cost will be 0
            double cost = 0;

            // Creates an empty integer array of number of rivers + sea
            int[] flows = new int[this.nsr];

            // For every river + sea
            for (int i = 0; i < this.nsr; i++)
            {
                // We accumulates its fitness
                cost += agents[i].FitnessValue;
            }

            // Iterating again over rivers + sea
            for (int i = 0; i < this.nsr; i++)
            {
                // Calculates its particular flow intensity
                flows[i] = (int)Math.Round(Math.Abs(agents[i].FitnessValue / cost) * (agents.Count - this.nsr));
            }

            return flows;
        }

        /// <summary>
        /// Performs the raining process (eq. 12).
        /// <param name="agents">List of agents.</param>
        /// <param name="bestAgent">Global best agent.</param>
        /// </summary>
        private void RainingProcess(List<Agent> agents, Agent bestAgent)
        {
            // Iterate through every raindrop
            for (int i = this.nsr; i < agents.Count; i++)
            {
                // Calculate the euclidean distance between sea and raindrop / stream
                double distance = Distance.Euclidean(agents[i].Position, bestAgent.Position);

                // If distance if smaller than evaporation condition
                if (distance > this.d_max)
                {
                    // Generates a new random gaussian number
                    double r1 = Stochastic.GenerateGaussianRandomNumber(1, agents[i].NumberVariables);

                    // Changes the stream position
                    for (int j = 0; j < agents[i].NumberVariables; j++)
                    {
                        agents[i].Position[j] = bestAgent.Position[j] + Math.Sqrt(0.1) * r1;
                    }
                }
            }
        }

        /// <summary>
        /// Updates every stream position (eq. 8).
        /// <param name="agents">List of agents.</param>
        /// <param name="flows">Array of flows' intensity.</param>
        /// </summary>
        private void UpdateStream(List<Agent> agents, int[] flows)
        {
            // Defining a counter to the summation of flows
            int n_flows = 0;

            // For every river, ignoring the sea
            for (int k = 1; k < this.nsr; k++)
            {
                // Accumulate the number of flows
                n_flows += flows[k];

                // Iterate through every possible flow
                for (int i = (n_flows - flows[k]); i < n_flows; i++)
                {
                    // Calculates a random uniform number between 0 and 1
                    double r1 = Stochastic.GenerateUniformRandomNumber();

                    // Updates stream position
                    for (int j = 0; j < agents[i].NumberVariables; j++)
                    {
                        agents[i].Position[j] += r1 * 2 * (agents[i].Position[j] - agents[k].Position[j]);
                    }
                }
            }
        }

        /// <summary>
        /// Updates every river position (eq. 9).
        /// <param name="agents">List of agents.</param>
        /// <param name="bestAgent">Global best agent.</param>
        /// </summary>
        private void UpdateRiver(List<Agent> agents, IAgent bestAgent)
        {
            // For every river, ignoring the sea
            for (int k = 1; k < this.nsr; k++)
            {
                // Calculates a random uniform number between 0 and 1
                double r1 = Stochastic.GenerateUniformRandomNumber();

                // Updates river position
                for (int j = 0; j < agents[k].NumberVariables; j++)
                {
                    agents[k].Position[j] += r1 * 2 * (bestAgent.Position[j] - agents[k].Position[j]);
                }
            }
        }

        /// <summary>
        /// Method that wraps the update pipeline over all agents and variables.
        /// <param name="space">A Space object that will be evaluated.</param>
        /// <param name="function">A Function object serving as an objective function.</param>
        /// </summary>
        public override void Update(Space space, Function function, int iteration)
        {
            // Updates every stream position
            UpdateStream(Agents, flows);

            // Updates every river position
            UpdateRiver(Agents, space.BestAgent);

            // Checking if agents meet the bounds limits
            foreach (Agent agent in Agents)
            {
                agent.ClipLimits();
            }

            // After the update, we need to re-evaluate the search space
            space.BestAgent = Evaluate(space.BestAgent, function);

            // Sorting agents
            Agents.Sort();

            // Performs the raining process (eq. 12)
            RainingProcess(Agents, space.BestAgent);

            // Updates the evaporation condition
            this.d_max -= (this.d_max / space.NumberIterations);
        }
    }
}