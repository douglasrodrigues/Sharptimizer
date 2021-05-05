namespace Sharptimizer.Statistics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Static class with statistical extension methods for <see cref="IEnumerable{T}"/>
    /// </summary>
    public static partial class Stats
    {
        /// <summary>
        /// Returns all non-null items in a sequence
        /// </summary>
        /// <typeparam name="T">The type of the sequence</typeparam>
        /// <param name="source">The Sequence</param>
        /// <returns>All non-null elements in the sequence</returns>
        public static IEnumerable<T> AllValues<T>(this IEnumerable<T?> source) where T : struct
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Where(x => x.HasValue).Select(x => (T)x);
        }

        /// <summary>
        /// Computes the sample Variance of a sequence of double values.
        /// </summary>
        /// <param name="source">A sequence of double values.</param>
        /// <returns>       
        ///     The Variance of the sequence of values.
        /// </returns>
        public static double Variance(this IEnumerable<double> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            long n = 0;
            double mean = 0;
            double M2 = 0;

            checked
            {
                foreach (var x in source)
                {
                    n++;

                    double delta = (double)x - mean;
                    mean += delta / n;
                    M2 += delta * ((double)x - mean);
                }
            }

            if (n < 2)
                throw new InvalidOperationException("Source must have at least 2 elements");

            return (double)(M2 / (n - 1));
        }

        /// <summary>
        /// Computes the sample Standard Deviation of a sequence of double values.
        /// </summary>
        /// <param name="source">A sequence of double values.</param>
        /// <returns>       
        ///     The Standard Deviation of the sequence of values.
        /// </returns>
        public static double StandardDeviation(this IEnumerable<double> source)
        {
            return (double)Math.Sqrt((double)source.Variance());
        }
    }
}