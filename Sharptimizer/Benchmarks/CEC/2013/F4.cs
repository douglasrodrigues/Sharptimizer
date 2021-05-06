namespace Sharptimizer.Benchmarks.CEC
{
    using System.Collections.Generic;
    using System.Linq;
    using Math;
    using Utils;

    /// <summary>
    /// F4 class implements the 7-separable, 1-separable Shifted and Rotated Elliptic's benchmarking function.
    /// </summary>
    /// <remarks>The function is commonly evaluated using x_i ∈ [−100, 100] ∣ i = {1,2,…,n}, n <= 1000.</remarks>
    /// <returns>The benchmarking function output `f(x)`.</returns>
    public class F4 : CECBenchmark
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
        public F4(string name = "F4", string year = "2013", List<string> auxiliaryData = null, int dims = 1000,
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
            S = new int[] { 50, 25, 25, 100, 50, 25, 25 };

            W = new double[] { 45.6996, 1.5646, 18465.3234, 0.0110, 13.6259, 0.3015, 59.6078 };
        }

        private static List<string> Initialize(List<string> auxiliaryData)
        {
            return auxiliaryData = new List<string> { "o", "R25", "R50", "R100" };
        }

        /// <summary>
        /// Executes the 7-separable, 1-separable Shifted and Rotated Elliptic's benchmarking function.
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

            // Checks if number of dimensions is valid
            if (D < 302)
            {
                // Raises an error
                throw new SizeError("`D` should be greater than 302");
            }

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
                    z = Matrix.Product(R25, y[n..(n+tuple.s)]);
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
                    z = Matrix.Product(R50, y[n..(n+tuple.s)]);
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
                    z = Matrix.Product(R100, y[n..(n+tuple.s)]);
                }
                // Sums up the calculated fitness multiplied by its corresponding weight
                f += tuple.w * new HighConditionedElliptic().Execute(T_irregularity(z));

                // Also increments the dimension counter
                n += tuple.s;
            }

            // Lastly, gathers the remaining positions
            z = y[n..];
            
            // Calculates their fitness and sums up to produce the final result
            f += new HighConditionedElliptic().Execute(T_irregularity(z));

            return f;
        }
    }
}