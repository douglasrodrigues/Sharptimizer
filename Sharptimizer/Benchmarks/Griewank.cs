namespace Sharptimizer.Benchmarks
{
    using System;
    using Utils;

    /// <summary>
    /// Griewank class implements the Griewank's benchmarking function.
    /// </summary>
    /// <remarks>The function is commonly evaluated using x_i ∈ [−100, 100] ∣ i = {1,2,…,n}.</remarks>
    /// <returns>The benchmarking function output `f(x)`.</returns>
    public class Griewank : Benchmark
    {
        public Griewank(string name = "Griewank", int dims = -1,
                                                bool continuous = true,
                                                bool convex = false,
                                                bool differentiable = true,
                                                bool multimodal = false,
                                                bool separable = false) : base(name,
                                                                                dims,
                                                                                continuous,
                                                                                convex,
                                                                                differentiable,
                                                                                multimodal,
                                                                                separable)
        { }

        /// <summary>
        /// Executes the Griewank's benchmarking function.
        /// </summary>
        /// <param name="x">An input array for calculating the function's output.</param>
        /// <returns>The benchmarking function output `f(x)`.</returns>
        public double Execute(double[] x)
        {
            // Initializing terms
            double term1 = 0;
            double term2 = 1.0;

            for (int i = 0; i < (x.Length - 1); i++)
            {
                term1 += Math.Pow(x[i], 2.0) / 4000;
                term2 *= Math.Cos(x[i] / Math.Sqrt(i + 1));
            }

            var f = 1.0 + term1 - term2;
            
            return f;
        }
    }
}