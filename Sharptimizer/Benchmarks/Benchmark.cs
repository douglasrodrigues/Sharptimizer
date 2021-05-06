namespace Sharptimizer.Utils
{
    /// <summary>
    /// A Benchmark class is the root of any benchmarking function.
    /// It is composed by several properties that defines the traits of a function,
    /// as well as a non-implemented __call__ method.
    /// </summary>
    public class Benchmark
    {
        // Name of the function
        public string Name { get; set; }

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
        
        /// <summary>
        /// Initialization method.
        /// </summary>
        /// <param name="name">Name of the function.</param>
        /// <param name="dims">Number of allowed dimensions.</param>
        /// <param name="continuous">Whether the function is continuous.</param>
        /// <param name="convex">Whether the function is convex.</param>
        /// <param name="differentiable">Whether the function is differentiable.</param>
        /// <param name="multimodal">Whether the function is multimodal.</param>
        /// <param name="separable">Whether the function is separable.</param>
        public Benchmark(string name = "Benchmark", int dims = 1,
                                                    bool continuous = false,
                                                    bool convex = false,
                                                    bool differentiable = false,
                                                    bool multimodal = false,
                                                    bool separable = false)
        {
            this.Name = name;
            this.Dims = dims;
            this.Continuous = continuous;
            this.Convex = convex;
            this.Differentiable = differentiable;
            this.Multimodal = multimodal;
            this.Separable = separable;
        }
    }
}