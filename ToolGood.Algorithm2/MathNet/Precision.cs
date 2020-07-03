using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ToolGood.Algorithm.MathNet.Numerics
{
    public static partial class Precision
    {
        /// <summary>
        /// The number of binary digits used to represent the binary number for a double precision floating
        /// point value. i.e. there are this many digits used to represent the
        /// actual number, where in a number as: 0.134556 * 10^5 the digits are 0.134556 and the exponent is 5.
        /// </summary>
        const int DoubleWidth = 53;

        /// <summary>
        /// Standard epsilon, the maximum relative precision of IEEE 754 double-precision floating numbers (64 bit).
        /// According to the definition of Prof. Demmel and used in LAPACK and Scilab.
        /// </summary>
        public static readonly double DoublePrecision = Math.Pow(2, -DoubleWidth);

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
        /// Returns the magnitude of the number.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The magnitude of the number.</returns>
        public static int Magnitude(this double value)
        {
            // Can't do this with zero because the 10-log of zero doesn't exist.
            if (value.Equals(0.0)) {
                return 0;
            }

            // Note that we need the absolute value of the input because Log10 doesn't
            // work for negative numbers (obviously).
            double magnitude = Math.Log10(Math.Abs(value));
            var truncated = (int)Truncate(magnitude);

            // To get the right number we need to know if the value is negative or positive
            // truncating a positive number will always give use the correct magnitude
            // truncating a negative number will give us a magnitude that is off by 1 (unless integer)
            return magnitude < 0d && truncated != magnitude
                ? truncated - 1
                : truncated;
        }

 
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

            if (count < 0) {
                return Decrement(value, -count);
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
        /// Decrements a floating point number to the next smaller number representable by the data type.
        /// </summary>
        /// <param name="value">The value which should be decremented.</param>
        /// <param name="count">How many times the number should be decremented.</param>
        /// <remarks>
        /// The decrementation step length depends on the provided value.
        /// Decrement(double.MinValue) will return negative infinity.
        /// </remarks>
        /// <returns>The next smaller floating point value.</returns>
        public static double Decrement(this double value, int count = 1)
        {
            if (double.IsInfinity(value) || double.IsNaN(value) || count == 0) {
                return value;
            }

            if (count < 0) {
                return Decrement(value, -count);
            }

            // Translate the bit pattern of the double to an integer.
            // Note that this leads to:
            // double > 0 --> long > 0, growing as the double value grows
            // double < 0 --> long < 0, increasing in absolute magnitude as the double
            //                          gets closer to zero!
            //                          i.e. 0 - double.epsilon will give the largest long value!
            long intValue = BitConverter.DoubleToInt64Bits(value);

            // If the value is zero then we'd really like the value to be -0. So we'll make it -0
            // and then everything else should work out.
            if (intValue == 0) {
                // Note that long.MinValue has the same bit pattern as -0.0.
                intValue = long.MinValue;
            }

            if (intValue < 0) {
                intValue += count;
            } else {
                intValue -= count;
            }

            // Note that not all long values can be translated into double values. There's a whole bunch of them
            // which return weird values like infinity and NaN
            return BitConverter.Int64BitsToDouble(intValue);
        }

        /// <summary>
        /// Forces small numbers near zero to zero, according to the specified absolute accuracy.
        /// </summary>
        /// <param name="a">The real number to coerce to zero, if it is almost zero.</param>
        /// <param name="maximumAbsoluteError">The absolute threshold for <paramref name="a"/> to consider it as zero.</param>
        /// <returns>Zero if |<paramref name="a"/>| is smaller than <paramref name="maximumAbsoluteError"/>, <paramref name="a"/> otherwise.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown if <paramref name="maximumAbsoluteError"/> is smaller than zero.
        /// </exception>
        public static double CoerceZero(this double a, double maximumAbsoluteError)
        {
            if (maximumAbsoluteError < 0) {
                throw new ArgumentOutOfRangeException("maximumAbsoluteError");
            }

            if (double.IsInfinity(a) || double.IsNaN(a)) {
                return a;
            }

            if (Math.Abs(a) < maximumAbsoluteError) {
                return 0.0;
            }

            return a;
        }

        /// <summary>
        /// Determines the range of floating point numbers that will match the specified value with the given tolerance.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="maxNumbersBetween">The <c>ulps</c> difference.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown if <paramref name="maxNumbersBetween"/> is smaller than zero.
        /// </exception>
        /// <returns>Tuple of the bottom and top range ends.</returns>
        public static Tuple<double, double> RangeOfMatchingFloatingPointNumbers(this double value, long maxNumbersBetween)
        {
            // Make sure ulpDifference is non-negative
            if (maxNumbersBetween < 1) {
                throw new ArgumentOutOfRangeException("maxNumbersBetween");
            }

            // If the value is infinity (positive or negative) just
            // return the same infinity for the range.
            if (double.IsInfinity(value)) {
                return new Tuple<double, double>(value, value);
            }

            // If the value is a NaN then the range is a NaN too.
            if (double.IsNaN(value)) {
                return new Tuple<double, double>(double.NaN, double.NaN);
            }

            // Translate the bit pattern of the double to an integer.
            // Note that this leads to:
            // double > 0 --> long > 0, growing as the double value grows
            // double < 0 --> long < 0, increasing in absolute magnitude as the double
            //                          gets closer to zero!
            //                          i.e. 0 - double.epsilon will give the largest long value!
            long intValue = BitConverter.DoubleToInt64Bits(value);

            // We need to protect against over- and under-flow of the intValue when
            // we start to add the ulpsDifference.
            if (intValue < 0) {
                // Note that long.MinValue has the same bit pattern as
                // -0.0. Therefore we're working in opposite direction (i.e. add if we want to
                // go more negative and subtract if we want to go less negative)
                var topRangeEnd = Math.Abs(long.MinValue - intValue) < maxNumbersBetween
                    // Got underflow, which can be fixed by splitting the calculation into two bits
                    // first get the remainder of the intValue after subtracting it from the long.MinValue
                    // and add that to the ulpsDifference. That way we'll turn positive without underflow
                    ? BitConverter.Int64BitsToDouble(maxNumbersBetween + (long.MinValue - intValue))
                    // No problems here, move along.
                    : BitConverter.Int64BitsToDouble(intValue - maxNumbersBetween);

                var bottomRangeEnd = Math.Abs(intValue) < maxNumbersBetween
                    // Underflow, which means we'd have to go further than a long would allow us.
                    // Also we couldn't translate it back to a double, so we'll return -Double.MaxValue
                    ? -double.MaxValue
                    // intValue is negative. Adding the positive ulpsDifference means that it gets less negative.
                    // However due to the conversion way this means that the actual double value gets more negative :-S
                    : BitConverter.Int64BitsToDouble(intValue + maxNumbersBetween);

                return new Tuple<double, double>(bottomRangeEnd, topRangeEnd);
            } else {
                // IntValue is positive
                var topRangeEnd = long.MaxValue - intValue < maxNumbersBetween
                    // Overflow, which means we'd have to go further than a long would allow us.
                    // Also we couldn't translate it back to a double, so we'll return Double.MaxValue
                    ? double.MaxValue
                    // No troubles here
                    : BitConverter.Int64BitsToDouble(intValue + maxNumbersBetween);

                // Check the bottom range end for underflows
                var bottomRangeEnd = intValue > maxNumbersBetween
                    // No problems here. IntValue is larger than ulpsDifference so we'll end up with a
                    // positive number.
                    ? BitConverter.Int64BitsToDouble(intValue - maxNumbersBetween)
                    // Int value is bigger than zero but smaller than the ulpsDifference. So we'll need to deal with
                    // the reversal at the negative end
                    : BitConverter.Int64BitsToDouble(long.MinValue + (maxNumbersBetween - intValue));

                return new Tuple<double, double>(bottomRangeEnd, topRangeEnd);
            }
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
         

        static double Truncate(double value)
        {
#if PORTABLE
            return value >= 0.0 ? Math.Floor(value) : Math.Ceiling(value);
#else
            return Math.Truncate(value);
#endif
        }
 
    }
}
