namespace Optimizer.Core
{
    using System.Collections.Generic;
    using System.Reflection;
    using Decorators;

    /// <summary>
    /// Metaheuristic base class.
    /// </summary>
    public abstract class Metaheuristic
    {
        /// <summary>
        /// Indicates the algorithm name.
        /// </summary>
        public string Algorithm { get; set; }

        /// <summary>
        /// List of agents.
        /// </summary>
        public List<Agent> Agents { get; set; }

        /// <summary>
        /// Indicates whether the optimizer is built.
        /// </summary>
        protected bool Built { get; set; }

        public Metaheuristic(string algorithm = null)
        {
            Algorithm = algorithm;

            Built = false;
        }

        /// <summary>
        /// This method serves as the object building process.
        /// <param name="hyperparams">Contains key-value parameters to the meta-heuristics.</param>
        /// </summary>
        protected void Build(Dictionary<string, double> hyperparams = null)
        {
            if (hyperparams != null)
            {
                foreach (KeyValuePair<string, double> kvp in hyperparams)
                {
                    PropertyInfo property = this.GetType().GetProperty(kvp.Key);

                    property.SetValue(this, kvp.Value, null);
                }
            }

            Agents = new List<Agent>();

            Built = true;
        }

        /// <summary>
        /// Evaluates the agents according to the objective function.
        /// <param name="agents">List of agents.</param>
        /// <param name="bestAgent">Global best agent.</param>
        /// <param name="function">A Function object serving as an objective function.</param>
        /// <returns>The updated global best agent.</returns>
        /// </summary>
        public virtual Agent Evaluate(Agent bestAgent, Function function)
        {
            foreach (Agent agent in Agents)
            {
                agent.FitnessValue = function.Calculate(agent.Position);

                if (agent.FitnessValue < bestAgent.FitnessValue)
                {
                    bestAgent = (Agent)agent.Clone();
                }
            }

            return bestAgent;
        }

        /// <summary>
        /// It decorates the agent.
        /// <param name="agents">List of agents.</param>
        /// </summary>
        public abstract void Decorate(List<Agent> agents);

        /// <summary>
        /// Method that wraps the update pipeline over all agents and variables.
        /// <param name="space">A Space object that will be evaluated.</param>
        /// <param name="function">A Function object serving as an objective function.</param>
        /// </summary>
        public abstract void Update(Space space, Function function, int iteration);
    }
}
