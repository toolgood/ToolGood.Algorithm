/**
 * Precision
 */
let Precision = {
    /**
     * Standard epsilon, the maximum relative precision of IEEE 754 double-precision floating numbers (64 bit).
     * According to the definition of Prof. Demmel and used in LAPACK and Scilab.
     */
    doublePrecision: Math.pow(2, -53),

    /**
     * Standard epsilon, the maximum relative precision of IEEE 754 double-precision floating numbers (64 bit).
     * According to the definition of Prof. Higham and used in the ISO C standard and MATLAB.
     */
    positiveDoublePrecision: 2 * Math.pow(2, -53),

    /**
     * Value representing 10 * 2^(-53) = 1.11022302462516E-15
     */
    defaultDoubleAccuracy: Math.pow(2, -53) * 10,

    /**
     * Converts a double to a 64-bit integer representation
     */
    doubleToInt64Bits: function(value) {
        let buffer = new ArrayBuffer(8);
        let float64Array = new Float64Array(buffer);
        let int64Array = new BigInt64Array(buffer);
        float64Array[0] = value;
        return int64Array[0];
    },

    /**
     * Converts a 64-bit integer representation to a double
     */
    int64BitsToDouble: function(bits) {
        let buffer = new ArrayBuffer(8);
        let int64Array = new BigInt64Array(buffer);
        let float64Array = new Float64Array(buffer);
        int64Array[0] = bits;
        return float64Array[0];
    },

    /**
     * Increments a floating point number to the next bigger number representable by the data type.
     * @param {number} value - The value which needs to be incremented.
     * @param {number} count - How many times the number should be incremented.
     * @returns {number} The next larger floating point value.
     */
    increment: function(value, count = 1) {
        if (isNaN(value) || !isFinite(value) || count === 0) {
            return value;
        }

        // Translate the bit pattern of the double to an integer
        let intValue = this.doubleToInt64Bits(value);
        if (intValue < 0) {
            intValue -= BigInt(count);
        } else {
            intValue += BigInt(count);
        }

        // Note that BigInt(-9223372036854775808) has the same bit pattern as -0.0
        if (intValue === BigInt(-9223372036854775808)) {
            return 0;
        }

        return this.int64BitsToDouble(intValue);
    },

    /**
     * Evaluates the minimum distance to the next distinguishable number near the argument value.
     * @param {number} value - The value used to determine the minimum distance.
     * @returns {number} Relative Epsilon (positive double or NaN).
     */
    epsilonOf: function(value) {
        if (isNaN(value) || !isFinite(value)) {
            return NaN;
        }

        let signed64 = this.doubleToInt64Bits(value);
        if (signed64 === 0n) {
            signed64++;
            return this.int64BitsToDouble(signed64) - value;
        }
        if (signed64 < 0n) {
            signed64--;
            return this.int64BitsToDouble(signed64) - value;
        }
        signed64--;
        return value - this.int64BitsToDouble(signed64);
    },

    /**
     * Evaluates the minimum distance to the next distinguishable number near the argument value.
     * @param {number} value - The value used to determine the minimum distance.
     * @returns {number} Relative Epsilon (positive double or NaN).
     */
    positiveEpsilonOf: function(value) {
        return 2 * this.epsilonOf(value);
    },

    /**
     * @param {number} a
     * @param {number} b
     * @param {number} diff
     * @param {number} maximumError
     * @returns {boolean}
     */
    almostEqualNormRelative: function(a, b, diff, maximumError) {
        // If A or B are infinity (positive or negative) then
        // only return true if they are exactly equal to each other -
        // that is, if they are both infinities of the same sign.
        if (!isFinite(a) || !isFinite(b)) {
            return a === b;
        }

        // If A or B are a NAN, return false. NANs are equal to nothing,
        // not even themselves.
        if (isNaN(a) || isNaN(b)) {
            return false;
        }

        // If one is almost zero, fall back to absolute equality
        if (Math.abs(a) < this.doublePrecision || Math.abs(b) < this.doublePrecision) {
            return Math.abs(diff) < maximumError;
        }

        if ((a === 0 && Math.abs(b) < maximumError) || (b === 0 && Math.abs(a) < maximumError)) {
            return true;
        }

        return Math.abs(diff) < maximumError * Math.max(Math.abs(a), Math.abs(b));
    },

    /**
     * @param {number} a
     * @param {number} b
     * @returns {boolean}
     */
    almostEqualRelative: function(a, b) {
        return this.almostEqualNormRelative(a, b, a - b, this.defaultDoubleAccuracy);
    },

    /**
     * @param {number} a
     * @param {number} b
     * @returns {boolean}
     */
    almostEqual: function(a, b) {
        return this.almostEqualNorm(a, b, a - b, this.defaultDoubleAccuracy);
    },

    /**
     * @param {number} a
     * @param {number} b
     * @param {number} diff
     * @param {number} maximumAbsoluteError
     * @returns {boolean}
     */
    almostEqualNorm: function(a, b, diff, maximumAbsoluteError) {
        // If A or B are infinity (positive or negative) then
        // only return true if they are exactly equal to each other -
        // that is, if they are both infinities of the same sign.
        if (!isFinite(a) || !isFinite(b)) {
            return a === b;
        }

        // If A or B are a NAN, return false. NANs are equal to nothing,
        // not even themselves.
        if (isNaN(a) || isNaN(b)) {
            return false;
        }

        return Math.abs(diff) < maximumAbsoluteError;
    }
};

export { Precision };