namespace Sharptimizer.Benchmarks.CEC
{
    using System.Collections.Generic;
    using System.Linq;
    using Math;

    /// <summary>
    /// F14 class implements the Shifted Schwefel's with Conflicting Overlapping benchmarking function.
    /// </summary>
    /// <remarks>The function is commonly evaluated using x_i ∈ [−100, 100] ∣ i = {1,2,…,n}, n <= 1000.</remarks>
    /// <returns>The benchmarking function output `f(x)`.</returns>
    public class F14 : CECBenchmark
    {
        int[] S { get; set; }

        double[] W { get; set; }

        int[] C { get; set; }

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
        public F14(string name = "F14", string year = "2013", List<string> auxiliaryData = null, int dims = 1000,
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

            W = new double[] { 0.4753, 498729.4349, 328.1032, 0.3231, 136.4562, 9.0255, 0.0924, 0.0001, 0.0093, 299.6790,
                  4.9395, 81.3641, 0.6544, 11.6119, 2860774.3201, 8.5835e-05, 23.5695, 0.0481, 1.4318, 12.1697 };

            C = S.Aggregate(new List<int>(), (list, next) => { list.Add(list.LastOrDefault() + next); return list; }).ToArray();
        }

        private static List<string> Initialize(List<string> auxiliaryData)
        {
            return auxiliaryData = new List<string> { "o", "R25", "R50", "R100" };
        }

        /// <summary>
        /// Executes the Shifted Schwefel's with Conflicting Overlapping benchmarking function.
        /// </summary>
        /// <param name="x">An input array for calculating the function's output.</param>
        /// <returns>The benchmarking function output `f(x)`.</returns>
        public double Execute(double[] x)
        {
            // Defines the number of dimensions, an array of permutations, an overlap size
            // and the function itself
            var D = x.Length;
            var P = Stochastic.Permutation(null, D).ToArray();
            var m = 5;
            var f = 0.0;

            // Permutes the initial input
            var w = Enumerable.Range(0, D).Select(i => x[P[i]]).ToArray();

            int start_n;
            int start_shift;
            int end_n;
            int end_shift;

            double[] o = null;
            if (DynamicProperties.ContainsKey("o"))
            {
                o = DynamicProperties["o"] as double[];
            }

            double[] z = null;

            // // Iterates through every possible subset and weight
            var tuples = this.S.Zip(this.W, (s, w) => (s, w));
            foreach (var (tuple, index) in tuples.Select((v, i) => (v, i)))
            {
                // Checks if is the first iteration
                if (index == 0)
                {
                    // If yes, defines the starting index as 0
                    start_n = 0;
                    start_shift = 0;
                }
                // If is not the first iteration
                else
                {
                    // Calculates the starting index
                    start_n = this.C[index - 1] - index * m;
                    start_shift = this.C[index - 1];
                }

                // Calculates the ending index
                end_n = this.C[index] - index * m;
                end_shift = this.C[index];

                // Re-calculates the input
                var y = Matrix.Subtract(w[start_n..end_n], o[start_shift..end_shift]);

                // Checks if the subset has 25 features
                if (tuple.s == 25)
                {
                    double[][] R25 = null;

                    if (DynamicProperties.ContainsKey("R25"))
                    {
                        R25 = DynamicProperties["R25"] as double[][];
                    }

                    // Rotates the input based on rotation matrix
                    z = Matrix.Product(R25, y);
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
                    z = Matrix.Product(R50, y);
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
                    z = Matrix.Product(R100, y);
                }

                // Applies the irregulary and asymmetry transforms
                z = T_asymmetry(T_irregularity(z), 0.2);

                // Sums up the calculated fitness multiplied by its corresponding weight
                f += tuple.w * new RotatedHyperEllipsoid().Execute(z);
            }

            return f;
        }
    }
}