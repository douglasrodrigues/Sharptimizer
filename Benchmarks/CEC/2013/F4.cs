namespace Optimizer.Benchmarks.CEC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Utils;

    /// <summary>
    /// F4 class implements the 7-separable, 1-separable Shifted and Rotated Elliptic's benchmarking function.
    /// </summary>
    /// <remarks>The function is commonly evaluated using x_i ∈ [−100, 100] ∣ i = {1,2,…,n}, n <= 1000.</remarks>
    /// <returns>The benchmarking function output `f(x)`.</returns>
    public class F4 : CECBenchmark
    {
        public F4(string name = "F4", string year = "2013", List<string> auxiliaryData = null, int dims = 1000,
                                                                                    bool continuous = true,
                                                                                    bool convex = true,
                                                                                    bool differentiable = true,
                                                                                    bool multimodal = false,
                                                                                    bool separable = false) : base(name,
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
            return auxiliaryData = new List<string> { "o", "R25", "R50", "R100" };
        }

        /// <summary>
        /// Executes the 7-separable, 1-separable Shifted and Rotated Elliptic's benchmarking function.
        /// </summary>
        /// <param name="x">An input array for calculating the function's output.</param>
        /// <returns>The benchmarking function output `f(x)`.</returns>
        public double Execute(double[] x)
        {
            return 0;
        }
    }
}