using System;

namespace ToolGood.Algorithm.MathNet.Numerics
{
    internal static partial class Precision
    {
        public static bool AlmostEqualNormRelative(this double a, double b, double diff, double maximumError)
        {
            // If A or B are infinity (positive or negative) then
            // only return true if they are exactly equal to each other -
            // that is, if they are both infinities of the same sign.
            if (double.IsInfinity(a) || double.IsInfinity(b)) {
                return a == b;
            }

            // If A or B are a NAN, return false. NANs are equal to nothing,
            // not even themselves.
            if (double.IsNaN(a) || double.IsNaN(b)) {
                return false;
            }

            // If one is almost zero, fall back to absolute equality
            if (Math.Abs(a) < DoublePrecision || Math.Abs(b) < DoublePrecision) {
                return Math.Abs(diff) < maximumError;
            }

            if ((a == 0 && Math.Abs(b) < maximumError) || (b == 0 && Math.Abs(a) < maximumError)) {
                return true;
            }

            return Math.Abs(diff) < maximumError * Math.Max(Math.Abs(a), Math.Abs(b));
        }

        public static bool AlmostEqualRelative(this double a, double b)
        {
            return AlmostEqualNormRelative(a, b, a - b, DefaultDoubleAccuracy);
        }

        public static bool AlmostEqual(this double a, double b)
        {
            return AlmostEqualNorm(a, b, a - b, DefaultDoubleAccuracy);
        }

        public static bool AlmostEqualNorm(this double a, double b, double diff, double maximumAbsoluteError)
        {
            // If A or B are infinity (positive or negative) then
            // only return true if they are exactly equal to each other -
            // that is, if they are both infinities of the same sign.
            if (double.IsInfinity(a) || double.IsInfinity(b)) {
                return a == b;
            }

            // If A or B are a NAN, return false. NANs are equal to nothing,
            // not even themselves.
            if (double.IsNaN(a) || double.IsNaN(b)) {
                return false;
            }

            return Math.Abs(diff) < maximumAbsoluteError;
        }
    }
}