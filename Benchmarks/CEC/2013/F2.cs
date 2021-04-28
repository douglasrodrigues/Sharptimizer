namespace Optimizer.Benchmarks.CEC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// F2 class implements the Shifted Rastrigin's benchmarking function.
    /// </summary>
    /// <remarks>The function is commonly evaluated using x_i ∈ [−5, 5] ∣ i = {1,2,…,n}, n <= 1000.</remarks>
    /// <returns>The benchmarking function output `f(x)`.</returns>
    public class F2 : CECBenchmark
    {
        public F2(string name = "F2", string year = "2013", List<string> auxiliaryData = null, int dims = 1000,
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
        /// Executes the Shifted Rastrigin's benchmarking function.
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

            var f = new double[x.Length];

            for (int i = 0; i < x.Length; i++)
            {
                f[i] = Math.Pow(z[i], 2) - 10.0 * Math.Cos(2.0 * Math.PI * z[i]) + 10.0;
            }
            
            return f.Sum();
        }
    }
}