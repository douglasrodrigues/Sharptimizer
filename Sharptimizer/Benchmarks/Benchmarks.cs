namespace Sharptimizer.Utils
{
    using System;

    public partial class Benchmarks
    {
        /// <summary>
        /// Ackley’s 1st benchmarking function.
        /// </summary>
        /// <remarks>The function is commonly evaluated using x_i ∈ [−32, 32] ∣ i = {1,2,…,n}</remarks>
        public static double Ackley1(double[] x)
        {
            double f = 0;
            double sum1 = 0;
            double sum2 = 0;

            // Calculating the 1 / n_term
            var inv = 1 / x.Length;

            for (int i = 0; i < (x.Length - 1); i++)
            {
                sum1 += Math.Pow(x[i], 2);
                sum2 += Math.Cos(2 * Math.PI * x[i]);
            }

            // Calculating first term
            var term1 = -0.2 * Math.Sqrt(inv * sum1);

            // Calculating second term
            var term2 = inv * sum2;

            // Calculating Ackley's 1st function
            f = 20 + Math.Exp(1) - Math.Exp(term2) - 20 * Math.Exp(term1);

            return f;
        }

        /// <summary>
        /// Alpine’s 1st benchmarking function.
        /// </summary>
        /// <remarks>The function is commonly evaluated using x_i ∈ [−10, 10] ∣ i = {1,2,…,n}</remarks>
        public static double Alpine1(double[] vector)
        {
            double f = 0;

            for (int i = 0; i < (vector.Length - 1); i++)
            {
                f += Math.Abs(vector[i] * Math.Sin(vector[i]) + 0.1 * vector[i]);
            }

            return f;
        }

        /// <summary>
        /// Griewank's benchmarking function.
        /// </summary>
        /// <remarks>The function is commonly evaluated using x_i ∈ [−100, 100] ∣ i = {1,2,…,n}</remarks>
        public static double Griewank(double[] vector)
        {
            double f = 0;

            double term1 = 1;
            double term2 = 0;

            for (int i = 0; i < (vector.Length - 1); i++)
            {
                term1 += Math.Pow(vector[i], 2) / 4000;
                term2 *= Math.Cos(vector[i] / Math.Sqrt(i + 1));
            }

            f = 1 + term1 - term2;

            return f;
        }

        /// <summary>
        /// Quartic's benchmarking function.
        /// </summary>
        /// <remarks>The function is commonly evaluated using x_i ∈ [−1.28, 1.28] ∣ i = {1,2,…,n}</remarks>
        public static double Quartic(double[] vector)
        {
            double f = 0;
            Random Rand = new Random();

            for (int i = 0; i < (vector.Length - 1); i++)
            {
                f += (i + 1) * Math.Pow(vector[i], 4);
            }

            return f + Rand.Next();
        }

        /// <summary>
        /// Rastrigin's benchmarking function.
        /// </summary>
        /// <remarks>The function is commonly evaluated using x_i ∈ [−5.12, 5.12] ∣ i = {1,2,…,n}</remarks>
        public static double Rastrigin(double[] vector)
        {
            double f = 0;

            for (int i = 0; i < (vector.Length - 1); i++)
            {
                f += Math.Pow(vector[i], 2) - 10 * Math.Cos(2 * Math.PI * vector[i]);
            }

            return 10 * vector.Length + f;
        }

        /// <summary>
        /// Rosenbrock's benchmarking function.
        /// </summary>
        /// <remarks>The function is commonly evaluated using x_i ∈ [−30, 30] ∣ i = {1,2,…,n}</remarks>
        public static double Rosenbrock(double[] vector)
        {
            double f = 0;

            for (int i = 0; i < (vector.Length - 1); i++)
            {
                f += 100 * (Math.Pow(vector[i + 1] - Math.Pow(vector[i], 2), 2)) + Math.Pow(vector[i] - 1, 2);
            }

            return f;
        }

        /// <summary>
        /// Salomon's benchmarking function.
        /// </summary>
        /// <remarks>The function is commonly evaluated using x_i ∈ [−100, 100] ∣ i = {1,2,…,n}</remarks>
        public static double Salomon(double[] vector)
        {
            double f = 0;
            double partial_sum = 0;

            for (int i = 0; i < vector.Length; i++)
            {
                partial_sum += Math.Pow(vector[i], 2);
            }

            f = 1 - Math.Cos(2 * Math.PI * Math.Sqrt(partial_sum)) + 0.1 * Math.Sqrt(partial_sum);

            return f;
        }

        /// <summary>
        /// Sphere's benchmarking function.
        /// </summary>
        /// <remarks>The function is commonly evaluated using x_i ∈ [−5.12, 5.12] ∣ i = {1,2,…,n}</remarks>
        public static double Sphere(double[] vector)
        {
            double f = 0;

            for (int i = 0; i < vector.Length; i++)
            {
                f += Math.Pow(vector[i], 2);
            }

            return f;
        }
    }
}