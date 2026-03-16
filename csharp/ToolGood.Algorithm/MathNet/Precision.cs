using System;

namespace ToolGood.Algorithm.MathNet.Numerics
{
    internal static partial class Precision
    {
        public static readonly decimal DecimalPrecision = 0.0000000000000000000000000001M;

        public static readonly decimal PositiveDecimalPrecision = 2M * DecimalPrecision;

        private static readonly decimal DefaultDecimalAccuracy = DecimalPrecision * 10M;

        public static bool AlmostEqualNormRelative(this decimal a, decimal b, decimal diff, decimal maximumError)
        {
            if(Math.Abs(a) < DecimalPrecision || Math.Abs(b) < DecimalPrecision) {
                return Math.Abs(diff) < maximumError;
            }

            if((a == 0 && Math.Abs(b) < maximumError) || (b == 0 && Math.Abs(a) < maximumError)) {
                return true;
            }

            return Math.Abs(diff) < maximumError * Math.Max(Math.Abs(a), Math.Abs(b));
        }

        public static bool AlmostEqualRelative(this decimal a, decimal b)
        {
            return AlmostEqualNormRelative(a, b, a - b, DefaultDecimalAccuracy);
        }

        public static bool AlmostEqual(this decimal a, decimal b)
        {
            return AlmostEqualNorm(a, b, a - b, DefaultDecimalAccuracy);
        }

        public static bool AlmostEqualNorm(this decimal a, decimal b, decimal diff, decimal maximumAbsoluteError)
        {
            return Math.Abs(diff) < maximumAbsoluteError;
        }
    }
}
