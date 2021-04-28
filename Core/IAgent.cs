namespace Optimizer.Core
{
    using System;
    
    public interface IAgent : ICloneable
    {
        /// <summary>
        /// Number of decision variables.
        /// </summary>
        int NumberVariables { get; set; }

        /// <summary>
        /// Value of the fitness function.
        /// </summary>
        double FitnessValue { get; set; }

        /// <summary>
        /// Agent's position in the search space.
        /// </summary>
        double[] Position { get; set; }

        /// <summary>
        /// Decision variables lower bound.
        /// </summary>
        double[] LowerBound { get; set; }

        /// <summary>
        /// Decision variables upper bound.
        /// </summary>
        double[] UpperBound { get; set; }

        public void ClipLimits();
    }
}