namespace Sharptimizer.Benchmarks.CEC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Utils;
    using Math;

    /// <summary>
    /// F1 class implements the Shifted Elliptic's benchmarking function.
    /// </summary>
    /// <remarks>The function is commonly evaluated using x_i ∈ [−100, 100] ∣ i = {1,2,…,n}, n <= 1000.</remarks>
    /// <returns>The benchmarking function output `f(x)`.</returns>
    public class F1 : CECBenchmark
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
        public F1(string name = "F1", string year = "2013", List<string> auxiliaryData = null, int dims = 1000,
                                                                                    bool continuous = true,
                                                                                    bool convex = true,
                                                                                    bool differentiable = true,
                                                                                    bool multimodal = false,
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
        /// Executes the Shifted Elliptic's benchmarking function.
        /// </summary>
        /// <param name="x">An input array for calculating the function's output.</param>
        /// <returns>The benchmarking function output `f(x)`.</returns>
        public double Execute(double[] x)
        {
            // Defines the number of dimensions and an equally-spaced interval between 0 and D-1
            var D = x.Length;
            var dims = Utils.LinSpace(0, D - 1, D).ToArray();

            double[] o = null;
            if (DynamicProperties.ContainsKey("o"))
            {
                o = DynamicProperties["o"] as double[];
            }

            // Re-calculates the input using the proposed transform
            var z = T_irregularity(Matrix.Subtract(x, o));

            // Calculating the Shifted Elliptic's function
            for (int i = 0; i < z.Length; i++)
            {
                z[i] = Math.Pow(10e6, dims[i] / (D - 1)) * Math.Pow(z[i], 2);
            }

            return z.Sum();
        }
    }
}