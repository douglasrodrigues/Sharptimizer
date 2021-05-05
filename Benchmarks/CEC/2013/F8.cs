namespace Sharptimizer.Benchmarks.CEC
{
    using System.Collections.Generic;
    using System.Linq;
    using Math;

    /// <summary>
    /// F8 class implements the 20-nonseparable Shifted and Rotated Elliptic's benchmarking function.
    /// </summary>
    /// <remarks>The function is commonly evaluated using x_i ∈ [−100, 100] ∣ i = {1,2,…,n}, n <= 1000.</remarks>
    /// <returns>The benchmarking function output `f(x)`.</returns>
    public class F8 : CECBenchmark
    {
        int[] S { get; set; }

        double[] W { get; set; }

        /// <summary>
        /// Initialization method.
        /// </summary>
        /// <param name="name">Name of the function.</param>
        /// <param name="year">Year of the function.</param>
        /// <param name="auxiliaryData">Auxiliary variables to be externally loaded.</param>
        /// <param name="dims">Number of allowed dimensions.</param>
        /// <param name="continuous">Whether the function is continuous.</param>
        /// <param name="convex">Whether the function is convex.</param>
        /// <param name="differentiable">Whether the function is differentiable.</param>
        /// <param name="multimodal">Whether the function is multimodal.</param>
        /// <param name="separable">Whether the function is separable.</param>
        /// <returns></returns>
        public F8(string name = "F8", string year = "2013", List<string> auxiliaryData = null, int dims = 1000,
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
        {
            // Defines the subsets and weights to be evaluated
            S = new int[] { 50, 50, 25, 25, 100, 100, 25, 25, 50, 25, 100, 25, 100, 50, 25, 25, 25, 100, 50, 25 };

            W = new double[] { 4.6303, 0.6864, 1143756360.0887, 2.0077, 789.3671, 16.3332, 6.0749, 0.0646, 0.0756,
                  35.6725, 7.9725e-06, 10.7822, 4.1999e-06, 0.0019, 0.0016, 686.7975, 0.1571, 0.0441, 0.3543, 0.0060 };
        }

        private static List<string> Initialize(List<string> auxiliaryData)
        {
            return auxiliaryData = new List<string> { "o", "R25", "R50", "R100" };
        }

        /// <summary>
        /// Executes the 20-nonseparable Shifted and Rotated Elliptic's benchmarking function.
        /// </summary>
        /// <param name="x">An input array for calculating the function's output.</param>
        /// <returns>The benchmarking function output `f(x)`.</returns>
        public double Execute(double[] x)
        {
            // Defines the number of dimensions, an array of permutations, a counter
            // and the function itself
            var D = x.Length;
            var P = Stochastic.Permutation(null, D).ToArray();
            var n = 0;
            var f = 0.0;

            double[] o = null;
            if (DynamicProperties.ContainsKey("o"))
            {
                o = DynamicProperties["o"] as double[];
            }

            // Re-calculates the input and permutes its input
            var y = Enumerable.Range(0, D).Select(i => Matrix.Subtract(x, o)[P[i]]).ToArray();

            double[] z = null;

            // Iterates through every possible subset and weight
            foreach (var tuple in this.S.Zip(this.W, (s, w) => (s, w)))
            {
                // Checks if the subset has 25 features
                if (tuple.s == 25)
                {
                    double[][] R25 = null;

                    if (DynamicProperties.ContainsKey("R25"))
                    {
                        R25 = DynamicProperties["R25"] as double[][];
                    }

                    // Rotates the input based on rotation matrix
                    z = Matrix.Product(R25, y[n..(n + tuple.s)]);
                }
                // Checks if the subset has 50 features
                else if (tuple.s == 50)
                {
                    double[][] R50 = null;

                    if (DynamicProperties.ContainsKey("R50"))
                    {
                        R50 = DynamicProperties["R50"] as double[][];
                    }

                    // Rotates the input based on rotation matrix
                    z = Matrix.Product(R50, y[n..(n + tuple.s)]);
                }
                // Checks if the subset has 100 features
                else if (tuple.s == 100)
                {
                    double[][] R100 = null;

                    if (DynamicProperties.ContainsKey("R100"))
                    {
                        R100 = DynamicProperties["R100"] as double[][];
                    }

                    // Rotates the input based on rotation matrix
                    z = Matrix.Product(R100, y[n..(n + tuple.s)]);
                }

                // Sums up the calculated fitness multiplied by its corresponding weight
                f += tuple.w * new HighConditionedElliptic().Execute(T_irregularity(z));

                // Also increments the dimension counter
                n += tuple.s;
            }

            return f;
        }
    }
}