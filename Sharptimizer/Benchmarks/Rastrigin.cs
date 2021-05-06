namespace Sharptimizer.Benchmarks
{
    using System;
    using System.Linq;
    using Utils;

    /// <summary>
    /// Rastrigin class implements the Rastrigin's benchmarking function.
    /// </summary>
    /// <remarks>The function is commonly evaluated using x_i ∈ [−5.12, 5.12] ∣ i = {1,2,…,n}.</remarks>
    /// <returns>The benchmarking function output `f(x)`.</returns>
    public class Rastrigin : Benchmark
    {
        public Rastrigin(string name = "Rastrigin", int dims = -1,
                                                bool continuous = true,
                                                bool convex = true,
                                                bool differentiable = true,
                                                bool multimodal = true,
                                                bool separable = true) : base(name,
                                                                                dims,
                                                                                continuous,
                                                                                convex,
                                                                                differentiable,
                                                                                multimodal,
                                                                                separable)
        { }

        /// <summary>
        /// Executes the Rastrigin's benchmarking function.
        /// </summary>
        /// <param name="x">An input array for calculating the function's output.</param>
        /// <returns>The benchmarking function output `f(x)`.</returns>
        public double Execute(double[] x)
        {
            var f = new double[x.Length];

            for (int i = 0; i < x.Length; i++)
            {
                f[i] = Math.Pow(x[i], 2) - 10.0 * Math.Cos(2.0 * Math.PI * x[i]);
            }
            
            return 10 * x.Length + f.Sum();
        }
    }
}