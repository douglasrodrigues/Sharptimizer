namespace Optimizer.Math
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    static class Stochastic
    {
        static readonly Random Random;
        
        static Stochastic()
        {
            Random = new Random();
        }

        public static double GenerateUniformRandomNumber(double low = 0, double high = 1)
        {
            return Random.NextDouble() * (high - low) + low;
        }

        public static int GenerateIntegerRandomNumber(int low = 0, int high = 1)
        {
            return Random.Next(low, high + 1);
        }

        public static double GenerateGaussianRandomNumber(double mean = 0, double variance = 1)
        {
            double u1 = Random.NextDouble();
            double u2 = Random.NextDouble();

            double rand_std_normal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);

            double normal = mean + variance * rand_std_normal;

            return normal;
        }

        public static IEnumerable<T> PickRandomItems<T>(IEnumerable<T> items, int maxCount)
        {
            Dictionary<double, T> randomSortTable = new Dictionary<double, T>();

            foreach (T item in items)
                randomSortTable[Random.NextDouble()] = item;

            return randomSortTable.OrderBy(KVP => KVP.Key).Take(maxCount).Select(KVP => KVP.Value);
        }
    }
}