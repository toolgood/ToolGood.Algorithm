using System;

namespace ToolGood.Algorithm.MathNet.Numerics
{
    internal sealed class Normal
    {
        public static decimal CDF(decimal mean, decimal stddev, decimal x)
        {
            return 0.5m * SpecialFunctions.Erfc((mean - x) / (stddev * Constants.Sqrt2));
        }

        public static decimal InvCDF(decimal mean, decimal stddev, decimal p)
        {
            return mean - (stddev * Constants.Sqrt2 * SpecialFunctions.ErfcInv(2.0m * p));
        }

        public static decimal PDF(decimal mean, decimal stddev, decimal x)
        {
            var d = (x - mean) / stddev;
            return MathEx.Exp(-0.5m * d * d) / (Constants.Sqrt2Pi * stddev);
        }
    }
}
