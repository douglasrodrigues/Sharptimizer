namespace Optimizer.Math
{
    public static class Distance
    {
        public static double Euclidean(double[] x, double[] y)
        {
            double sum = 0.0;

            for (int i = 0; i < x.Length; i++)
            {
                sum += System.Math.Pow(x[i] - y[i], 2);
            }

            return System.Math.Sqrt(sum);
        }
    }
}