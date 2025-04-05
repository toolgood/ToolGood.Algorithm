using System;

namespace ToolGood.Algorithm.MathNet.Numerics
{
    internal sealed class Normal
    {
        public static double CDF(double mean, double stddev, double x)
        {
            //if (stddev < 0.0) {
            //    throw new ArgumentException(Resources.InvalidDistributionParameters);
            //}

            return 0.5 * SpecialFunctions.Erfc((mean - x) / (stddev * Constants.Sqrt2));
        }

        public static double InvCDF(double mean, double stddev, double p)
        {
            //if (stddev < 0.0) {
            //    throw new ArgumentException(Resources.InvalidDistributionParameters);
            //}

            return mean - (stddev * Constants.Sqrt2 * SpecialFunctions.ErfcInv(2.0 * p));
        }

        public static double PDF(double mean, double stddev, double x)
        {
            //if (stddev < 0.0) {
            //    throw new ArgumentException(Resources.InvalidDistributionParameters);
            //}

            var d = (x - mean) / stddev;
            return Math.Exp(-0.5 * d * d) / (Constants.Sqrt2Pi * stddev);
        }
    }
}