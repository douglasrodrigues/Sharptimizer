namespace Optimizer.Metaheuristics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Math;

    /// <summary>
    /// Artificial Bee Colony -
    /// Reference:
    /// D. Karaboga and B. Basturk.
    /// A powerful and efficient algorithm for numerical function optimization: Artificial bee colony (ABC) algorithm.
    /// Journal of Global Optimization (2007).
    /// </summary>
    public class ABC : Metaheuristic
    {
        // Number of trial limits
        public int n_trials { get; set; }

        // Array of trials counter
        public int[] trials { get; set; }
        
        /// <summary>
        /// This constructor initializes the ABC.
        /// <param name="hyperparams">Contains key-value parameters to the meta-heuristics.</param>
        /// <param name="algorithm">Indicates the algorithm name.</param>
        /// </summary>
        public ABC(Dictionary<string, double> hyperparams = null, string algorithm = "ABC") : base(algorithm)
        {
            n_trials = 10;

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

            // Instanciating array of trials counter
            trials = new int[Agents.Count];
        }

        /// <summary>
        /// Evaluates a food source location and update its value if possible (eq. 2.2).
        /// <param name="agent">An agent.</param>
        /// <param name="neighbor">A neighbor agent.</param>
        /// <param name="function">A Function object serving as an objective function.</param>
        /// <param name="trial">A trial counter.</param>
        /// <returns>The number of trials for the current food source.</returns>
        /// </summary>
        private int EvaluateLocation(Agent agent, Agent neighbor, Function function, int trial)
        {
            // Generates an uniform random number
            double r1 = Stochastic.GenerateUniformRandomNumber(-1, 1);

            // Copies actual food source location
            Agent a = (Agent)agent.Clone();

            // Change its location according to equation 2.2
            for (int j = 0; j < a.NumberVariables; j++)
            {
                a.Position[j] = a.Position[j] + (a.Position[j] - neighbor.Position[j]) * r1;
            }

            // Check agent limits
            a.ClipLimits();

            // Evaluating its fitness
            a.FitnessValue = function.Calculate(a.Position);

            // Check if fitness is improved
            if (a.FitnessValue < agent.FitnessValue)
            {
                // If yes, reset the number of trials for this particular food source
                trial = 0;

                // Copies the new agent
                agent = (Agent)a.Clone();
            }
            // If not
            else
            {
                // We increse the trials counter
                trial += 1;
            }

            return trial;
        }

        /// <summary>
        /// Sends employee bees onto food source to evaluate its nectar.
        /// <param name="agents">List of agents.</param>
        /// <param name="function">A Function object serving as an objective function.</param>
        /// <param name="trials">Array of trials counter.</param>
        /// </summary>
        private void SendEmployee(List<Agent> agents, Function function, int[] trials)
        {
            // Iterate through all food sources
            for (int i = 0; i < agents.Count; i++)
            {
                // Gathering a random source to be used
                int source = Stochastic.GenerateIntegerRandomNumber(0, agents.Count - 1);

                // Measuring food source location
                trials[i] = EvaluateLocation(agents[i], agents[source], function, trials[i]);
            }
        }

        /// <summary>
        /// Sends onlooker bees to select new food sources (eq. 2.1).
        /// <param name="agents">List of agents.</param>
        /// <param name="function">A Function object serving as an objective function.</param>
        /// <param name="trials">Array of trials counter.</param>
        /// </summary>
        private void SendOnlooker(List<Agent> agents, Function function, int[] trials)
        {
            // Calculating the fitness somatory
            double total = agents.Sum(x => x.FitnessValue);

            // Defining food sources' counter
            int k = 0;

            // While counter is less than the amount of food sources
            while (k < agents.Count)
            {
                // We iterate through every agent
                for (int i = 0; i < agents.Count; i++)
                {
                    // Creates a random uniform number
                    double r1 = Stochastic.GenerateUniformRandomNumber();

                    // Calculates the food source's probability
                    var probs = (agents[i].FitnessValue / (total + Double.Epsilon)) + 0.1;

                    // If the random number is smaller than food source's probability
                    if (r1 < probs)
                    {
                        // We need to increment the counter
                        k += 1;

                        // Gathers a random source to be used
                        int source = Stochastic.GenerateIntegerRandomNumber(0, agents.Count - 1);

                        // Evaluate its location
                        trials[i] = EvaluateLocation(agents[i], agents[source], function, trials[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Sends scout bees to scout for new possible food sources.
        /// <param name="agents">List of agents.</param>
        /// <param name="function">A Function object serving as an objective function.</param>
        /// <param name="trials">Array of trials counter.</param>
        /// </summary>
        private void SendScout(List<Agent> agents, Function function, int[] trials)
        {
            // Calculating the maximum trial counter value and index
            var max = trials.Select((n, i) => (Value: n, Index: i)).Max();

            // If maximum trial is bigger than number of possible trials
            if (max.Value > n_trials)
            {
                // Resets the trial counter
                trials[max.Index] = 0;

                // Copies the current agent
                Agent a = (Agent)agents[max.Index].Clone();

                // Updates its position with a random shakeness
                for (int j = 0; j < a.NumberVariables; j++)
                {
                    a.Position[j] += Stochastic.GenerateUniformRandomNumber(-1, 1);
                }

                // Check agent limits
                a.ClipLimits();

                // Recalculates its fitness
                a.FitnessValue = function.Calculate(a.Position);

                // If fitness is better
                if (a.FitnessValue < agents[max.Index].FitnessValue)
                {
                    // We copy the temporary agent to the current one
                    agents[max.Index] = (Agent)a.Clone();
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
            // Sending employee bees step
            SendEmployee(Agents, function, trials);

            // Sending onlooker bees step
            SendOnlooker(Agents, function, trials);

            // Sending scout bees step
            SendScout(Agents, function, trials);

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