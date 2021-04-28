namespace Optimizer.Benchmarks
{
    using System;
    using System.Linq;
    using Math;
    using Utils;

    /// <summary>
    /// Sphere class implements the Sphere's benchmarking function.
    /// </summary>
    /// <remarks>The function is commonly evaluated using x_i ∈ [−5.12, 5.12] ∣ i = {1,2,…,n}.</remarks>
    /// <returns>The benchmarking function output `f(x)`.</returns>
    public class Sphere : Benchmark
    {
        public Sphere(string name = "Sphere", int dims = -1,
                                                bool continuous = true,
                                                bool convex = true,
                                                bool differentiable = true,
                                                bool multimodal = false,
                                                bool separable = true) : base(name,
                                                                                dims,
                                                                                continuous,
                                                                                convex,
                                                                                differentiable,
                                                                                multimodal,
                                                                                separable)
        { }

        /// <summary>
        /// Executes the Sphere's benchmarking function.
        /// </summary>
        /// <param name="x">An input array for calculating the function's output.</param>
        /// <returns>The benchmarking function output `f(x)`.</returns>
        public double Execute(double[] x)
        {
            // Calculating the Sphere's function
            var f = Enumerable.Range(0, x.Length).Select(i => Math.Pow(x[i], 2)).ToArray();
            
            return f.Sum();
        }
    }
}