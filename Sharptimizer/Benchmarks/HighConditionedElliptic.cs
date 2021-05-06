namespace Sharptimizer.Benchmarks
{
    using System;
    using System.Linq;
    using Utils;

    /// <summary>
    /// HighConditionedElliptic class implements the High Conditioned Elliptic's benchmarking function.
    /// </summary>
    /// <remarks>The function is commonly evaluated using x_i ∈ [−100, 100] ∣ i = {1,2,…,n}.</remarks>
    /// <returns>The benchmarking function output `f(x)`.</returns>
    public class HighConditionedElliptic : Benchmark
    {
        public HighConditionedElliptic(string name = "HighConditionedElliptic", int dims = -1,
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
        /// Executes the High Conditioned Elliptic's benchmarking function.
        /// </summary>
        /// <param name="x">An input array for calculating the function's output.</param>
        /// <returns>The benchmarking function output `f(x)`.</returns>
        public double Execute(double[] x)
        {
            // Calculates an equally-spaced interval between 0 and D-1
            var dims = Utils.LinSpace(0, x.Length - 1, x.Length).ToArray();

            var f = new double[x.Length];

            // Calculating the HighConditionedElliptic's function
            for (int i = 0; i < x.Length; i++)
            {
                var a = Math.Pow(x[i], 2);
                var b = Math.Pow(10e6, (dims[i] / (x.Length - 1)));
                f[i] = a * b;
            }
            
            return f.Sum();
        }
    }
}