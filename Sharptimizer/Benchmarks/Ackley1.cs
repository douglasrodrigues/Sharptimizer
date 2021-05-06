namespace Sharptimizer.Benchmarks
{
    using System;
    using System.Linq;
    using Utils;

    /// <summary>
    /// Ackley1 class implements the Ackley's 1st benchmarking function.
    /// </summary>
    /// <remarks>The function is commonly evaluated using x_i ∈ [−32, 32] ∣ i = {1,2,…,n}.</remarks>
    /// <returns>The benchmarking function output `f(x)`.</returns>
    public class Ackley1 : Benchmark
    {
        public Ackley1(string name = "Ackley1", int dims = -1,
                                                bool continuous = true,
                                                bool convex = false,
                                                bool differentiable = true,
                                                bool multimodal = true,
                                                bool separable = false) : base(name,
                                                                                dims,
                                                                                continuous,
                                                                                convex,
                                                                                differentiable,
                                                                                multimodal,
                                                                                separable)
        { }

        /// <summary>
        /// Executes the Ackley's 1st benchmarking function.
        /// </summary>
        /// <param name="x">An input array for calculating the function's output.</param>
        /// <returns>The benchmarking function output `f(x)`.</returns>
        public double Execute(double[] x)
        {
            // Calculating the 1/n term
            var inv = 1.0 / x.Length;

            // Calculating first term
            var term1 = -0.2 * Math.Sqrt(inv * Enumerable.Range(0, x.Length).Select(i => Math.Pow(x[i], 2.0)).Sum());

            // Calculating second term
            var term2 = inv * Enumerable.Range(0, x.Length).Select(i => Math.Cos(2.0 * Math.PI * x[i])).Sum();

            // Calculating Shifted Ackley's function
            var f = 20.0 + Math.E - Math.Exp(term2) - 20.0 * Math.Exp(term1);
            
            return f;
        }
    }
}