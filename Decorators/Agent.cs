namespace Optimizer.Core
{
    using System;
    using System.Linq;

    /// <summary>
    /// An Agent class for all optimization techniques.
    /// </summary>
    public class Agent : IAgent, IComparable<Agent>
    {
        /// <summary>
        /// Number of decision variables.
        /// </summary>
        public int NumberVariables { get; set; }

        /// <summary>
        /// Value of the fitness function.
        /// </summary>
        public double FitnessValue { get; set; }

        /// <summary>
        /// Agent's position in the search space.
        /// </summary>
        public double[] Position { get; set; }

        /// <summary>
        /// Decision variables lower bound.
        /// </summary>
        public double[] LowerBound { get; set; }

        /// <summary>
        /// Decision variables upper bound.
        /// </summary>
        public double[] UpperBound { get; set; }

        /// <summary>
        /// This constructor initializes the agent.
        /// </summary>
        public Agent()
        {

        }

        /// <summary>
        /// This constructor initializes the agent.
        /// <param name="numberVariables">Number of variables.</param>
        /// </summary>
        public Agent(int numberVariables)
        {
            NumberVariables = numberVariables;
            FitnessValue = double.MaxValue;
            Position = new double[NumberVariables];
            LowerBound = new double[NumberVariables];
            UpperBound = new double[NumberVariables];
        }

        /// <summary>
        /// Clips the agent's decision variables to the bounds limits.
        /// </summary>
        public void ClipLimits()
        {
            var bounds = LowerBound.Zip(UpperBound, (n, w) => new { Lower = n, Upper = w });
            foreach (var (bound, index) in bounds.Select((v, i) => (v, i)))
            {
                Position[index] = Math.Clamp(Position[index], bound.Lower, bound.Upper);
            }
        }

        public int CompareTo(Agent other)
        {
            if (this.FitnessValue < other.FitnessValue) return -1;
            else if (this.FitnessValue > other.FitnessValue) return 1;
            else return 0;
        }

        public override string ToString()
        {
            string s = "[ ";
            for (int i = 0; i < this.Position.Length; ++i)
            {
                if (Position[i] >= 0.0) s += " ";
                s += Position[i].ToString("F2") + " ";
            }
            s += "]  Fitness = " + this.FitnessValue.ToString("F4");
            return s;
        }

        public object Clone()
        {
            return new Agent
            {
                NumberVariables = this.NumberVariables,
                FitnessValue = this.FitnessValue,
                Position = this.Position,
                LowerBound = this.LowerBound,
                UpperBound = this.UpperBound,
            };
        }
    }
}