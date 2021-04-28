namespace Optimizer.Benchmarks.CEC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// F3 class implements the Shifted Ackley's benchmarking function.
    /// </summary>
    /// <remarks>The function is commonly evaluated using x_i ∈ [−32, 32] ∣ i = {1,2,…,n}, n <= 1000.</remarks>
    /// <returns>The benchmarking function output `f(x)`.</returns>
    public class F3 : CECBenchmark
    {
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
            // Re-calculates the input using the proposed transforms
            var m = T_diagonal(x.Length, 10.0);

            var aux = new double[x.Length];
            double[] o = null;

            if (DynamicProperties.ContainsKey("o"))
            {
                o = DynamicProperties["o"] as double[];
            }

            for (int i = 0; i < x.Length; i++)
            {
                aux[i] = x[i] - o[i];
            }

            var irregularity = T_irregularity(aux);

            var asymmetry = T_asymmetry(irregularity, 0.2);

            // dot product
            var z = asymmetry.Zip(m, (a, b) => a * b).ToArray();

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