namespace Sharptimizer.Benchmarks
{
    using System;
    using Math;
    using Utils;

    /// <summary>
    /// Quartic class implements the Quartic's benchmarking function.
    /// </summary>
    /// <remarks>The function is commonly evaluated using x_i ∈ [−1.28, 1.28] ∣ i = {1,2,…,n}.</remarks>
    /// <returns>The benchmarking function output `f(x)`.</returns>
    public class Quartic : Benchmark
    {
        public Quartic(string name = "Quartic", int dims = -1,
                                                bool continuous = true,
                                                bool convex = false,
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
        /// Executes the Quartic's benchmarking function.
        /// </summary>
        /// <param name="x">An input array for calculating the function's output.</param>
        /// <returns>The benchmarking function output `f(x)`.</returns>
        public double Execute(double[] x)
        {
            // Instanciating function
            double f = 0;
            
            for (int i = 0; i < (x.Length - 1); i++)
            {
                // Calculating the Quartic's function
                f += (i + 1) * (Math.Pow(x[i], 4));
            }
            
            return f + Stochastic.GenerateUniformRandomNumber();
        }
    }
}