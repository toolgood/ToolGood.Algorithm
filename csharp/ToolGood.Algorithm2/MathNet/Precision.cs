using System;

namespace ToolGood.Algorithm.MathNet.Numerics
{
    /// <summary>
    /// Precision
    /// </summary>
    static partial class Precision
    {
        /// <summary>
        /// Standard epsilon, the maximum relative precision of IEEE 754 double-precision floating numbers (64 bit).
        /// According to the definition of Prof. Demmel and used in LAPACK and Scilab.
        /// </summary>
        public static readonly double DoublePrecision = Math.Pow(2, -53);

        /// <summary>
        /// Standard epsilon, the maximum relative precision of IEEE 754 double-precision floating numbers (64 bit).
        /// According to the definition of Prof. Higham and used in the ISO C standard and MATLAB.
        /// </summary>
        public static readonly double PositiveDoublePrecision = 2 * DoublePrecision;

        /// <summary>
        /// Value representing 10 * 2^(-53) = 1.11022302462516E-15
        /// </summary>
        static readonly double DefaultDoubleAccuracy = DoublePrecision * 10;


        /// <summary>
        /// Increments a floating point number to the next bigger number representable by the data type.
        /// </summary>
        /// <param name="value">The value which needs to be incremented.</param>
        /// <param name="count">How many times the number should be incremented.</param>
        /// <remarks>
        /// The incrementation step length depends on the provided value.
        /// Increment(double.MaxValue) will return positive infinity.
        /// </remarks>
        /// <returns>The next larger floating point value.</returns>
        public static double Increment(this double value, int count = 1)
        {
            if (double.IsInfinity(value) || double.IsNaN(value) || count == 0) {
                return value;
            }

            // Translate the bit pattern of the double to an integer.
            // Note that this leads to:
            // double > 0 --> long > 0, growing as the double value grows
            // double < 0 --> long < 0, increasing in absolute magnitude as the double
            //                          gets closer to zero!
            //                          i.e. 0 - double.epsilon will give the largest long value!
            long intValue = BitConverter.DoubleToInt64Bits(value);
            if (intValue < 0) {
                intValue -= count;
            } else {
                intValue += count;
            }

            // Note that long.MinValue has the same bit pattern as -0.0.
            if (intValue == long.MinValue) {
                return 0;
            }

            // Note that not all long values can be translated into double values. There's a whole bunch of them
            // which return weird values like infinity and NaN
            return BitConverter.Int64BitsToDouble(intValue);
        }

        /// <summary>
        /// Evaluates the minimum distance to the next distinguishable number near the argument value.
        /// </summary>
        /// <param name="value">The value used to determine the minimum distance.</param>
        /// <returns>
        /// Relative Epsilon (positive double or NaN).
        /// </returns>
        /// <remarks>Evaluates the <b>negative</b> epsilon. The more common positive epsilon is equal to two times this negative epsilon.</remarks>
        ///// <seealso cref="PositiveEpsilonOf(double)"/>
        public static double EpsilonOf(this double value)
        {
            if (double.IsInfinity(value) || double.IsNaN(value)) {
                return double.NaN;
            }

            long signed64 = BitConverter.DoubleToInt64Bits(value);
            if (signed64 == 0) {
                signed64++;
                return BitConverter.Int64BitsToDouble(signed64) - value;
            }
            if (signed64-- < 0) {
                return BitConverter.Int64BitsToDouble(signed64) - value;
            }
            return value - BitConverter.Int64BitsToDouble(signed64);
        }

        /// <summary>
        /// Evaluates the minimum distance to the next distinguishable number near the argument value.
        /// </summary>
        /// <param name="value">The value used to determine the minimum distance.</param>
        /// <returns>Relative Epsilon (positive double or NaN)</returns>
        /// <remarks>Evaluates the <b>positive</b> epsilon. See also <see cref="EpsilonOf(double)"/></remarks>
        ///// <seealso cref="EpsilonOf(double)"/>
        public static double PositiveEpsilonOf(this double value)
        {
            return 2 * EpsilonOf(value);
        }

    }
}
