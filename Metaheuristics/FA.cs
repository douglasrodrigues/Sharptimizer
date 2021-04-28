namespace Optimizer.Metaheuristics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Utils;
    using Math;

    /// <summary>
    /// Firefly Algorithm -
    /// Reference:
    /// X.-S. Yang. Firefly algorithms for multimodal optimization.
    /// International symposium on stochastic algorithms (2009).
    /// </summary>
    public class FA : Metaheuristic
    {
        // Randomization parameter
        public double alpha { get; set; }

        // Attractiveness
        public double beta { get; set; }

        // Light absorption coefficient
        public double gamma { get; set; }

        /// <summary>
        /// This constructor initializes the FA.
        /// <param name="hyperparams">Contains key-value parameters to the meta-heuristics.</param>
        /// <param name="algorithm">Indicates the algorithm name.</param>
        /// </summary>
        public FA(Dictionary<string, double> hyperparams = null, string algorithm = "FA") : base(algorithm)
        {
            alpha = 0.5;
            beta = 0.2;
            gamma = 1.0;

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
        /// Method that wraps the update pipeline over all agents and variables.
        /// <param name="space">A Space object that will be evaluated.</param>
        /// <param name="function">A Function object serving as an objective function.</param>
        /// </summary>
        public override void Update(Space space, Function function, int iteration)
        {
            // Calculating current iteration delta
            double delta = 1 - Math.Pow(((10e-4) / 0.9), (1 / space.NumberIterations));

            // Applying update to alpha parameter
            this.alpha *= (1 - delta);

            // We copy a temporary list for iterating purposes
            List<Agent> temp_agents = Agents.Select(x => (Agent)x.Clone()).ToList();

            // Iterating through 'i' agents
            foreach (Agent agent in Agents)
            {
                // Iterating through 'k' agents
                foreach (Agent temp_agent in temp_agents)
                {
                    // Distance is calculated by an euclidean distance between 'i' and 'j' (eq. 8)
                    double distance = Distance.Euclidean(agent.Position, temp_agent.Position);

                    // If 'i' fit is bigger than 'k' fit
                    if (agent.FitnessValue > temp_agent.FitnessValue)
                    {
                        // Recalculate the attractiveness (eq. 6)
                        double beta = this.beta * Math.Exp(-this.gamma * distance);

                        // Generates a random uniform distribution
                        double r1 = Stochastic.GenerateUniformRandomNumber();

                        // Updates agent's position (eq. 9)
                        for (int j = 0; j < agent.NumberVariables; j++)
                        {
                            agent.Position[j] = beta *
                                                (agent.Position[j] + temp_agent.Position[j]) + this.alpha * (r1 - 0.5);
                        }
                    }
                }
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