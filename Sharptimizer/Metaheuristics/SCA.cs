namespace Sharptimizer.Metaheuristics
{
    using System;
    using System.Collections.Generic;
    using Core;
    using Math;

    /// <summary>
    /// Sine Cosine Algorithm - 
    /// References:
    /// S. Mirjalili. SCA: A Sine Cosine Algorithm for solving optimization problems.
    /// Knowledge-Based Systems (2016).
    /// </summary>
    public class SCA : Metaheuristic
    {
        // Minimum function range
        public double r_min { get; set; }
        
        // Maximum function range
        public double r_max { get; set; }
        
        // Constant for defining the next position's region
        public double a { get; set; }

        /// <summary>
        /// This constructor initializes the SCA.
        /// <param name="hyperparams">Contains key-value parameters to the meta-heuristics.</param>
        /// <param name="algorithm">Indicates the algorithm name.</param>
        /// </summary>
        public SCA(Dictionary<string, double> hyperparams = null, string algorithm = "SCA") : base(algorithm)
        {
            r_min = 0;
            r_max = 2;
            a = 3;

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
        /// Updates a single particle position over a single variable (eq. 3.3).
        /// <param name="agents">List of agents.</param>
        /// <param name="bestAgent">Global best agent.</param>
        /// <param name="r1">Controls the next position's region.</param>
        /// <param name="r2">Defines how far the movement should be.</param>
        /// <param name="r3">Random weight for emphasizing or deemphasizing the movement.</param>
        /// <param name="r4">Random number to decide whether sine or cosine should be used.</param>
        /// <returns>A new position.</returns>
        /// </summary>
        private double[] UpdatePosition(Agent agent, Agent bestAgent, double r1, double r2, double r3, double r4)
        {
            double[] new_position = new double[agent.NumberVariables];

            // If random number is smaller than threshold
            if (r4 < 0.5)
            {
                // Updates the position using sine
                for (int j = 0; j < agent.NumberVariables; j++)
                {
                    new_position[j] = agent.Position[j] + r1 * Math.Sin(r2) * Math.Abs(r3 * bestAgent.Position[j] - agent.Position[j]);
                }
            }
            // If the random number is bigger than threshold
            else
            {
                // Updates the position using cosine
                for (int j = 0; j < agent.NumberVariables; j++)
                {
                    new_position[j] = agent.Position[j] + r1 * Math.Cos(r2) * Math.Abs(r3 * bestAgent.Position[j] - agent.Position[j]);
                }
            }

            return new_position;
        }

        /// <summary>
        /// Method that wraps the update pipeline over all agents and variables.
        /// <param name="space">A Space object that will be evaluated.</param>
        /// <param name="function">A Function object serving as an objective function.</param>
        /// </summary>
        public override void Update(Space space, Function function, int iteration)
        {
            // Adaptively changing the r1 parameter, which controls the next position's region
            double r1 = this.a - (iteration * this.a / space.NumberIterations);

            // The r2 parameter defines how far the movement should be
            double r2 = Stochastic.GenerateUniformRandomNumber() * (2 * Math.PI);

            // A random weight for emphasizing or deemphasizing the movement
            double r3 = (Stochastic.GenerateUniformRandomNumber() * (this.r_max - this.r_min)) + this.r_min;

            // A random number to decide whether sine or cosine should be used
            double r4 = Stochastic.GenerateUniformRandomNumber();

            // Iterate through all agents
            foreach (var agent in Agents)
            {
                // Updates agent's position
                agent.Position = UpdatePosition(agent, space.BestAgent, r1, r2, r3, r4);
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