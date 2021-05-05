namespace Sharptimizer.Metaheuristics
{
    using System.Collections.Generic;

    using Core;
    using Math;

    /// <summary>
    /// Harmony Search -
    /// Reference:
    /// Z. W. Geem, J. H. Kim, and G. V. Loganathan. A new heuristic optimization algorithm: Harmony search. Simulation (2001).
    /// </summary>
    public class HS : Metaheuristic
    {
        // Harmony memory considering rate
        public double WHMCR { get; set; }

        // Pitch adjusting rate
        public double PAR { get; set; }

        // Bandwidth parameter
        public double bw { get; set; }
        
        /// <summary>
        /// This constructor initializes the HS.
        /// <param name="hyperparams">Contains key-value parameters to the meta-heuristics.</param>
        /// <param name="algorithm">Indicates the algorithm name.</param>
        /// </summary>
        public HS(Dictionary<string, double> hyperparams = null, string algorithm = "HS") : base(algorithm)
        {
            WHMCR = 0.7;
            PAR = 0.7;
            bw = 1.0;

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
        /// It generates a new harmony.
        /// <param name="agents">List of agents.</param>
        /// <returns>A new agent (harmony) based on music generation process.</returns>
        /// </summary>
        private Agent GenerateNewHarmony(Agent agent)
        {
            // Mimics an agent position
            Agent a = (Agent)agent.Clone();

            // # Generates an uniform random number
            double r1 = Stochastic.GenerateUniformRandomNumber();

            // Using the harmony memory
            if (r1 < WHMCR)
            {
                // Generates a new uniform random number
                double r2 = Stochastic.GenerateUniformRandomNumber();

                // Checks if it needs a pitch adjusting
                if (r2 < PAR)
                {
                    // Generate between ]-1, 1[ interval
                    double r3 = Stochastic.GenerateUniformRandomNumber(-1, 1);

                    // Updates harmony position
                    for (int j = 0; j < a.NumberVariables; j++)
                    {
                        a.Position[j] = a.Position[j] + (r3 * bw);
                    }
                }
            }
            // If harmony memory is not used
            else
            {
                // Generate a uniform random number
                for (int j = 0; j < a.NumberVariables; j++)
                {
                    a.Position[j] = Stochastic.GenerateUniformRandomNumber(a.LowerBound[j], a.UpperBound[j]);
                }
            }

            return a;
        }

        /// <summary>
        /// Method that wraps the update pipeline over all agents and variables.
        /// <param name="space">A Space object that will be evaluated.</param>
        /// <param name="function">A Function object serving as an objective function.</param>
        /// </summary>
        public override void Update(Space space, Function function, int iteration)
        {
            // Generates a random index
            int k = Stochastic.GenerateIntegerRandomNumber(0, Agents.Count - 1);

            // Generates a new harmony
            Agent agent = GenerateNewHarmony(Agents[k]);

            // Checking agents limits
            agent.ClipLimits();

            // Calculates the new harmony fitness
            agent.FitnessValue = function.Calculate(agent.Position);

            // Sorting agents
            Agents.Sort();

            // If newly generated agent fitness is better
            if (agent.FitnessValue < Agents[Agents.Count - 1].FitnessValue)
            {
                Agents[Agents.Count - 1] = (Agent)agent.Clone();
            }

            // TODO: change to UpdateBestAgent()
            // After the update, we need to re-evaluate the search space
            space.BestAgent = Evaluate(space.BestAgent, function);
        }
    }
}