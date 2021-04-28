namespace Optimizer.Benchmarks
{
    using System;
    using Math;
    using Utils;

    /// <summary>
    /// Rosenbrock class implements the Rosenbrock's benchmarking function.
    /// </summary>
    /// <remarks>The function is commonly evaluated using x_i ∈ [−30, 30] ∣ i = {1,2,…,n}.</remarks>
    /// <returns>The benchmarking function output `f(x)`.</returns>
    public class Rosenbrock : Benchmark
    {
        public Rosenbrock(string name = "Rosenbrock", int dims = -1,
                                                bool continuous = true,
                                                bool convex = true,
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
        /// Executes the Rosenbrock's benchmarking function.
        /// </summary>
        /// <param name="x">An input array for calculating the function's output.</param>
        /// <returns>The benchmarking function output `f(x)`.</returns>
        public double Execute(double[] x)
        {
            // Instanciating function
            double f = 0;
            
            for (int i = 0; i < (x.Length - 1); i++)
            {
                // Calculating the Rosenbrock's function
                f += 100 * (Math.Pow(x[i + 1] - Math.Pow(x[i], 2), 2)) + Math.Pow((x[i] - 1), 2);
            }
            
            return f;
        }
    }
}