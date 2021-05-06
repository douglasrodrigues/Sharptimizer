namespace Sharptimizer.Metaheuristics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Math;

    /// <summary>
    /// Black Hole -
    /// Reference:
    /// A. Hatamlou. Black hole: A new heuristic optimization approach for data clustering.
    /// Information Sciences (2013).
    /// </summary>
    public class BH : Metaheuristic
    {
        /// <summary>
        /// This constructor initializes the BH.
        /// <param name="hyperparams">Contains key-value parameters to the meta-heuristics.</param>
        /// <param name="algorithm">Indicates the algorithm name.</param>
        /// </summary>
        public BH(Dictionary<string, double> hyperparams = null, string algorithm = "BH") : base(algorithm)
        {
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
                this.Agents.Add((Agent)agent);
            }
        }

        /// <summary>
        /// It updates every star position and calculates their event's horizon cost (eq. 3).
        /// <param name="agents">List of agents.</param>
        /// <param name="bestAgent">Global best agent.</param>
        /// <param name="function">A Function object serving as an objective function.</param>
        /// <returns>The cost of the event horizon.</returns>
        /// </summary>
        private (double, Agent) UpdatePosition(List<Agent> agents, Agent bestAgent, Function function)
        {
            // Event's horizon cost
            double cost = 0;

            // Iterate through all agents
            foreach (Agent agent in agents)
            {
                // Creates a random uniform number
                double r1 = Stochastic.GenerateUniformRandomNumber();

                // Updates agent's position
                for (int j = 0; j < agent.Position.Length; j++)
                {
                    agent.Position[j] += r1 + (bestAgent.Position[j] - agent.Position[j]);
                }

                // Checking agents limits
                agent.ClipLimits();

                // Evaluates agent
                agent.FitnessValue = function.Calculate(agent.Position);

                // If new agent's fitness is better than best
                if (agent.FitnessValue < bestAgent.FitnessValue)
                {
                    // Swap their positions
                    for (int j = 0; j < agent.Position.Length; j++)
                    {
                        var tempPosition = agent.Position[j];

                        agent.Position[j] = bestAgent.Position[j];

                        bestAgent.Position[j] = tempPosition;
                    }

                    // Also swap their fitness
                    var tempFitness = agent.FitnessValue;

                    agent.FitnessValue = bestAgent.FitnessValue;

                    bestAgent.FitnessValue = tempFitness;
                }

                // Increment the cost with current agent's fitness
                cost += agent.FitnessValue;
            }

            return (cost, bestAgent);
        }

        /// <summary>
        /// It calculates the stars' crossing an event horizon (eq. 4).
        /// <param name="agents">List of agents.</param>
        /// <param name="bestAgent">Global best agent.</param>
        /// <param name="cost">The event's horizon cost.</param>
        /// </summary>
        private void EventHorizon(List<Agent> agents, Agent bestAgent, double cost)
        {
            // Calculates the radius of the event horizon
            double radius = bestAgent.FitnessValue / Math.Max(cost, Double.Epsilon);

            // Iterate through every agent
            foreach (var agent in agents)
            {
                // Calculates distance between star and black hole
                double distance = Distance.Euclidean(bestAgent.Position, agent.Position);

                // If distance is smaller than horizon's radius
                if (distance < radius)
                {
                    // Generates a new random star
                    var bounds = agent.LowerBound.Zip(agent.UpperBound, (n, w) => new { Lower = n, Upper = w });
                    foreach (var (bound, index) in bounds.Select((v, i) => (v, i)))
                    {
                        agent.Position[index] = Stochastic.GenerateUniformRandomNumber(bound.Lower, bound.Upper);
                    }
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
            // Initialize cost variable
            double cost = 0;

            // Updates stars position and calculate their cost (eq. 3)
            (cost, space.BestAgent) = UpdatePosition(Agents, space.BestAgent, function);

            // Performs the Event Horizon (eq. 4)
            EventHorizon(Agents, space.BestAgent, cost);
        }
    }
}