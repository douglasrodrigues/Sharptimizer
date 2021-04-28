using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Optimizer.Benchmarks.CEC
{
    using Utils;

    /// <summary>
    /// A CECBenchmark class is the root of CEC-based benchmarking function.
    /// It is composed by several properties that defines the traits of a function,
    /// as well as a non-implemented __call__ method.
    /// </summary>
    public class CECBenchmark : DynamicObject
    {
        // Name of the function
        public string Name { get; set; }

        // Year of the function
        public string Year { get; set; }

        // Number of allowed dimensions
        public int Dims { get; set; }

        // Continuous
        public bool Continuous { get; set; }

        // Convex
        public bool Convex { get; set; }

        // Differentiable
        public bool Differentiable { get; set; }

        // Multimodal
        public bool Multimodal { get; set; }

        // Separable
        public bool Separable { get; set; }

        // The inner dictionary to store field names and values.
        public Dictionary<string, object> DynamicProperties = new Dictionary<string, object>();
        
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
        public CECBenchmark(string name, string year, List<string> auxiliaryData, int dims = 1,
                                                                                    bool continuous = false,
                                                                                    bool convex = false,
                                                                                    bool differentiable = false,
                                                                                    bool multimodal = false,
                                                                                    bool separable = false)
        {
            this.Name = name;
            this.Year = year;
            this.Dims = dims;
            this.Continuous = continuous;
            this.Convex = convex;
            this.Differentiable = differentiable;
            this.Multimodal = multimodal;
            this.Separable = separable;
            this.LoadAuxiliaryData(name, year, auxiliaryData);
        }

        /// <summary>
        /// Set the property value.
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            DynamicProperties.Add(binder.Name, value);

            return true;
        }

        /// <summary>
        /// Get the property value.
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return DynamicProperties.TryGetValue(binder.Name, out result);
        }

        public void AddProperty(string key, object value)
        {
            DynamicProperties[key] = value;
        }

        /// <summary>
        /// Loads auxiliary data from a set of files.
        /// </summary>
        /// <param name="name">Name of the function.</param>
        /// <param name="year">Year of the function.</param>
        /// <param name="data">List holding the variables to be loaded.</param>
        public void LoadAuxiliaryData(string name, string year, List<string> data)
        {
            foreach (var item in data)
            {
                // Constructs the data file
                // Note that it will always be NAME_VARIABLE
                var dataFile = $"{name}_{item}";

                // Loads the data to a temporary variable
                var tmp = Loader.LoadCECAuxiliary(dataFile, year);

                this.AddProperty(item, tmp);
            }
        }

        /// <summary>
        /// Performs a transformation over the input to create smooth local irregularities.
        /// </summary>
        /// <param name="x">An array holding the input to be transformed.</param>
        /// <returns>The transformed input.</returns>
        public static double[] T_irregularity(double[] x)
        {
            // Defines the x_hat transformation
            var x_hat = Enumerable.Range(0, x.Length)
                                                .Select(i => x[i] != 0 ? System.Math.Log(System.Math.Abs(x[i] + Double.Epsilon)) : 0)
                                                .ToArray();

            // Defines both c_1 and c_2 transformations
            var c_1 = Enumerable.Range(0, x.Length).Select(i => x[i] > 0 ? 10 : 5.5).ToArray();
            var c_2 = Enumerable.Range(0, x.Length).Select(i => x[i] > 0 ? 7.9 : 3.1).ToArray();
            
            double[] x_t = new double[x.Length];

            // Re-calculates the input
            for (int i = 0; i < x.Length; i++)
            {
                x_t[i] = System.Math.Sign(x[i]) * System.Math.Exp(x_hat[i] + 0.049 * (System.Math.Sin(c_1[i] * x_hat[i]) + System.Math.Sin(c_2[i] * x_hat[i])));
            }

            return x_t;
        }

        /// <summary>
        /// Performs a transformation over the input to break the symmetry of the symmetric functions.
        /// </summary>
        /// <param name="x">An array holding the input to be transformed.</param>
        /// <param name="beta">Exponential value used to produce the asymmetry.</param>
        /// <returns>The transformed input.</returns>
        public static double[] T_asymmetry(double[] x, double beta)
        {
            // Gathers the amount of dimensions and calculates an equally-spaced interval between 0 and D-1
            var D = x.Length;
            var dims = Utils.LinSpace(0, D - 1, D).ToArray();

            // Re-calculates the input
            var x_t = Enumerable.Range(0, D).Select(i => x[i] > 0 ?
                                    System.Math.Pow(x[i], 1 + beta * (dims[i] / (D - 1)) * System.Math.Sqrt(x[i])) : x[i]).ToArray();

            return x_t;
        }

        /// <summary>
        /// Creates a transformed diagonal matrix used to provide ill-conditioning.
        /// </summary>
        /// <param name="D">Amount of dimensions.</param>
        /// <param name="alpha">Exponential value used to produce the ill-conditioning.</param>
        /// <returns>The transformed diagonal vector</returns>
        public static double[] T_diagonal(int D, double alpha)
        {
            // Calculates an equally-spaced interval between 0 and D-1
            var dims = Utils.LinSpace(0, D - 1, D).ToArray();

            var x = new double[D];
            
            for (int i = 0; i < D; i++)
            {
                var a = System.Math.Pow(alpha, 0.5);
                x[i] = (dims[i] / (D - 1)) * a;
            }
            
            // 
            return x;
        }
    }
}