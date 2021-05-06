namespace Sharptimizer.Benchmarks
{
    using System;
    using System.Linq;
    using Math;
    using Utils;

    /// <summary>
    /// Salomon class implements the Salomon's benchmarking function.
    /// </summary>
    /// <remarks>The function is commonly evaluated using x_i ∈ [−100, 100] ∣ i = {1,2,…,n}.</remarks>
    /// <returns>The benchmarking function output `f(x)`.</returns>
    public class Salomon : Benchmark
    {
        public Salomon(string name = "Salomon", int dims = -1,
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
        /// Executes the Salomon's benchmarking function.
        /// </summary>
        /// <param name="x">An input array for calculating the function's output.</param>
        /// <returns>The benchmarking function output `f(x)`.</returns>
        public double Execute(double[] x)
        {
            // Calculating the Salomon's function
            var g = Enumerable.Range(0, x.Length).Select(i => Math.Pow(x[i], 2)).Sum();
            var f = 1 - Math.Cos(2 * Math.PI * Math.Sqrt(g)) + 0.1 * Math.Sqrt(g);
            
            return f;
        }
    }
}