namespace Sharptimizer.Benchmarks.CEC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Math;

    /// <summary>
    /// F3 class implements the Shifted Ackley's benchmarking function.
    /// </summary>
    /// <remarks>The function is commonly evaluated using x_i ∈ [−32, 32] ∣ i = {1,2,…,n}, n <= 1000.</remarks>
    /// <returns>The benchmarking function output `f(x)`.</returns>
    public class F3 : CECBenchmark
    {
        /// <summary>
        /// Initialization method.
        /// </summary>
        /// <param name="name">Name of the function.</param>
        /// <param name="year">Year of the function.</param>
        /// <param name="auxiliaryData">Auxiliary variables to be externally loaded.</param>
        /// <param name="dims">Number of allowed dimensions.</param>
        /// <param name="continuous">Whether the function is continuous.</param>
        /// <param name="convex">Whether the function is convex.</param>
        /// <param name="differentiable">Whether the function is differentiable.</param>
        /// <param name="multimodal">Whether the function is multimodal.</param>
        /// <param name="separable">Whether the function is separable.</param>
        /// <returns></returns>
        public F3(string name = "F3", string year = "2013", List<string> auxiliaryData = null, int dims = 1000,
                                                                                    bool continuous = true,
                                                                                    bool convex = true,
                                                                                    bool differentiable = true,
                                                                                    bool multimodal = true,
                                                                                    bool separable = true) : base(name,
                                                                                                                    year,
                                                                                                                    Initialize(auxiliaryData),
                                                                                                                    dims,
                                                                                                                    continuous,
                                                                                                                    convex,
                                                                                                                    differentiable,
                                                                                                                    multimodal,
                                                                                                                    separable)
        { }

        private static List<string> Initialize(List<string> auxiliaryData)
        {
            return auxiliaryData = new List<string> { "o" };
        }

        /// <summary>
        /// Executes the Shifted Ackley's benchmarking function.
        /// </summary>
        /// <param name="x">An input array for calculating the function's output.</param>
        /// <returns>The benchmarking function output `f(x)`.</returns>
        public double Execute(double[] x)
        {
            double[] o = null;
            if (DynamicProperties.ContainsKey("o"))
            {
                o = DynamicProperties["o"] as double[];
            }

            // Re-calculates the input using the proposed transforms
            var z = T_asymmetry(
                        T_irregularity(Matrix.Subtract(x, o)), 0.2)
                                .Zip(T_diagonal(x.Length, 10.0), (a, b) => a * b).ToArray();

            // Calculating the 1/n term
            var inv = 1.0 / x.Length;

            // Calculating first term
            var term1 = -0.2 * Math.Sqrt(inv * Enumerable.Range(0, z.Length).Select(i => Math.Pow(z[i], 2)).Sum());

            // Calculating second term
            var term2 = inv * Enumerable.Range(0, z.Length).Select(i => Math.Cos(2.0 * Math.PI * z[i])).Sum();

            // Calculating Shifted Ackley's function
            var f = 20.0 + Math.E - Math.Exp(term2) - 20.0 * Math.Exp(term1);
            
            return f;
        }
    }
}