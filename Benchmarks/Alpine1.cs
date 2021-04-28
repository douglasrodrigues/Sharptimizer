namespace Optimizer.Benchmarks
{
    using System;
    using System.Linq;
    using Utils;

    /// <summary>
    /// Alpine1 class implements the Alpine's 1st benchmarking function.
    /// </summary>
    /// <remarks>The function is commonly evaluated using x_i ∈ [−10, 10] ∣ i = {1,2,…,n}.</remarks>
    /// <returns>The benchmarking function output `f(x)`.</returns>
    public class Alpine1 : Benchmark
    {
        public Alpine1(string name = "Alpine1", int dims = -1,
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
        /// Executes the Alpine's 1st benchmarking function.
        /// </summary>
        /// <param name="x">An input array for calculating the function's output.</param>
        /// <returns>The benchmarking function output `f(x)`.</returns>
        public double Execute(double[] x)
        {
            // Calculating the Alpine's 1st function
            var f = Enumerable.Range(0, x.Length).Select(i => Math.Abs(x[i] * Math.Sin(x[i]) + 0.1 * x[i])).ToArray();
            
            return f.Sum();
        }
    }
}