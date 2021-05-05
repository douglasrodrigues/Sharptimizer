namespace Sharptimizer.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Utils;

    /// <summary>
    /// A Space class for agents, variables and methods related to the search space.
    /// </summary>
    public abstract class Space
    {
        /// <summary>
        /// Number of agents.
        /// </summary>
        public int NumberAgents { get; set; }

        /// <summary>
        /// Number of decision variables.
        /// </summary>
        public int NumberVariables { get; set; }

        /// <summary>
        /// Number of iterations.
        /// </summary>
        public int NumberIterations { get; set; }

        /// <summary>
        /// List of agents.
        /// </summary>
        public List<Agent> Agents { get; set; }

        /// <summary>
        /// The best agent.
        /// </summary>
        public Agent BestAgent { get; set; }

        /// <summary>
        /// Decision variables lower bound.
        /// </summary>
        public double[] LowerBound { get; set; }

        /// <summary>
        /// Decision variables upper bound.
        /// </summary>
        public double[] UpperBound { get; set; }

        public bool Built { get; set; }

        /// <summary>
        /// This constructor initializes the space.
        /// <param name="numberAgents">Number of agents.</param>
        /// <param name="numberVariables">Number of variables.</param>
        /// <param name="numberIterations">Number of iterations.</param>
        /// </summary>
        public Space(int numberAgents = 1,
                     int numberVariables = 1,
                     int numberIterations = 10)
        {
            NumberAgents = numberAgents;
            NumberVariables = numberVariables;
            NumberIterations = numberIterations;
            LowerBound = new double[NumberVariables];
            UpperBound = new double[NumberVariables];
            Built = false;
        }

        /// <summary>
        /// Creates a list of agents and the best agent.
        /// <returns>A list of agents and the best agent.</returns>
        /// </summary>
        private (List<Agent>, Agent) CreateAgents()
        {
            Agents = new List<Agent>();

            for (int i = 0; i < NumberAgents; ++i)
                Agents.Add(new Agent(NumberVariables));

            Agent BestAgent = new Agent(NumberVariables);

            return (Agents, BestAgent);
        }

        public abstract void InitializeAgents();

        /// <summary>
        /// This method serves as the object building process.
        /// <param name="lowerBound">Lower bound array with the minimum possible values.</param>
        /// <param name="upperBound">Upper bound array with the maximum possible values.</param>
        /// </summary>
        public void Build(double[] lowerBound, double[] upperBound)
        {
            lowerBound.CopyTo(LowerBound, 0);
            upperBound.CopyTo(UpperBound, 0);

            (Agents, BestAgent) = CreateAgents();

            Built = true;
        }

        public abstract void ClipLimits();

        /// <summary>
        /// Runs the optimization pipeline.
        /// <param name="metaheuristic">A Metaheuristic object that will define how agents will be evaluated and updated.</param>
        /// <param name="function">A Function object serving as an objective function.</param>
        /// </summary>
        public dynamic Run(Metaheuristic metaheuristic, Function function)
        {
            metaheuristic.Decorate(this.Agents);

            this.BestAgent = metaheuristic.Evaluate(this.BestAgent, function);

            dynamic history = new History();

            for (var t = 0; t < NumberIterations; t++)
            {
                Console.WriteLine("Iteration {0}/{1}", t + 1, NumberIterations);

                metaheuristic.Update(this, function, t);
            }

            history.BestPosition = BestAgent.Position;
            history.BestFitness = BestAgent.FitnessValue;

            return history;
        }

        public dynamic Run(List<Metaheuristic> metaheuristics, Function function)
        {
            dynamic history = new History();

            var distr = SpreadAgents(metaheuristics, Agents);

            foreach (var item in distr)
            {
                Console.WriteLine(item.Key);

                item.Key.Decorate(item.Value);

                item.Key.Evaluate(this.BestAgent, function);

                for (var t = 0; t < NumberIterations; t++)
                {
                    item.Key.Update(this, function, t);
                }
            }

            history.BestPosition = BestAgent.Position;
            history.BestFitness = BestAgent.FitnessValue;

            return history;
        }

        public Dictionary<TU, List<T>> SpreadAgents<T, TU>(IEnumerable<TU> metaheuristics, IEnumerable<T> agents)
        {
            var eAgents = agents.ToList();
            var eMetaheuristics = metaheuristics.ToList();

            var map = metaheuristics.ToDictionary(i => i, i => new List<T>());
            var n_metaheuristics = eMetaheuristics.Count;

            var containerIndex = 0;

            foreach (var agent in eAgents)
            {
                map[eMetaheuristics[containerIndex]].Add(agent);
                containerIndex = (containerIndex + 1) % n_metaheuristics;
            }

            return map;
        }
    }
}