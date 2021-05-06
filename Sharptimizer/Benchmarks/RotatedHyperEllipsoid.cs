namespace Sharptimizer.Benchmarks
{
    using System;
    using System.Linq;
    using Utils;

    /// <summary>
    /// RotatedHyperEllipsoid class implements the Rotated Hyper-Ellipsoid's benchmarking function.
    /// </summary>
    /// <remarks>The function is commonly evaluated using x_i ∈ [−65.536, 65.536] ∣ i = {1,2,…,n}.</remarks>
    /// <returns>The benchmarking function output `f(x)`.</returns>
    public class RotatedHyperEllipsoid : Benchmark
    {
        public RotatedHyperEllipsoid(string name = "RotatedHyperEllipsoid", int dims = -1,
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
        /// Executes the Rotated Hyper-Ellipsoid's benchmarking function.
        /// </summary>
        /// <param name="x">An input array for calculating the function's output.</param>
        /// <returns>The benchmarking function output `f(x)`.</returns>
        public double Execute(double[] x)
        {
            // Instantiating function
            var f = 0.0;

            // For every input dimension
            for (int i = 0; i < x.Length; i++)
            {
                // For `j` in `i` range
                for (int j = 0; j < i; j++)
                {
                    // Calculating the Rotated Hyper-Ellipsoid's function
                    f += Math.Pow(x[j], 2);
                }
            }
            
            return f;
        }
    }
}