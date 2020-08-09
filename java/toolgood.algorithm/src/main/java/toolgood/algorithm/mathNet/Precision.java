package toolgood.algorithm.mathNet;

public class Precision {
    /// <summary>
    /// Standard epsilon, the maximum relative precision of IEEE 754
    /// double-precision floating numbers (64 bit).
    /// According to the definition of Prof. Demmel and used in LAPACK and Scilab.
    /// </summary>
    public static double DoublePrecision = Math.pow(2, -53);

    /// <summary>
    /// Standard epsilon, the maximum relative precision of IEEE 754
    /// double-precision floating numbers (64 bit).
    /// According to the definition of Prof. Higham and used in the ISO C standard
    /// and MATLAB.
    /// </summary>
    public static double PositiveDoublePrecision = 2 * DoublePrecision;

    /// <summary>
    /// Value representing 10 * 2^(-53) = 1.11022302462516E-15
    /// </summary>
    static  double DefaultDoubleAccuracy = DoublePrecision * 10;

    public static double Increment(this double value)
    {
        return Increment(value,-1);
    }

    /// <summary>
    /// Increments a floating point number to the next bigger number representable
    /// by the data type.
    /// </summary>
    /// <param name="value">The value which needs to be incremented.</param>
    /// <param name="count">How many times the number should be incremented.</param>
    /// <remarks>
    /// The incrementation step length depends on the provided value.
    /// Increment(double.MaxValue) will return positive infinity.
    /// </remarks>
    /// <returns>The next larger floating point value.</returns>
    public static double Increment(this double value, int count )
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
    /// Evaluates the minimum distance to the next distinguishable number near the
    /// argument value.
    /// </summary>
    /// <param name="value">The value used to determine the minimum
    /// distance.</param>
    /// <returns>
    /// Relative Epsilon (positive double or NaN).
    /// </returns>
    /// <remarks>Evaluates the <b>negative</b> epsilon. The more common positive
    /// epsilon is equal to two times this negative epsilon.</remarks>
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
    /// Evaluates the minimum distance to the next distinguishable number near the
    /// argument value.
    /// </summary>
    /// <param name="value">The value used to determine the minimum
    /// distance.</param>
    /// <returns>Relative Epsilon (positive double or NaN)</returns>
    /// <remarks>Evaluates the <b>positive</b> epsilon. See also <see
    /// cref="EpsilonOf(double)"/></remarks>
    ///// <seealso cref="EpsilonOf(double)"/>
    public static double PositiveEpsilonOf(this double value)
        {
            return 2 * EpsilonOf(value);
        }

    public static boolean AlmostEqualNormRelative(this double a, double b, double diff, double maximumError)
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

    public static boolean AlmostEqualRelative(this double a, double b)
        {
            return AlmostEqualNormRelative(a, b, a - b, DefaultDoubleAccuracy);
        }

    public static boolean AlmostEqual(this double a, double b)
        {
            return AlmostEqualNorm(a, b, a - b, DefaultDoubleAccuracy);
        }

    public static boolean AlmostEqualNorm(this double a, double b, double diff, double maximumAbsoluteError)
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