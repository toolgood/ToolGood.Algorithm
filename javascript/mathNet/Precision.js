
class Precision { }
Precision.DoublePrecision = Math.pow(2, -53);
Precision.PositiveDoublePrecision = 2 * DoublePrecision;
Precision.DefaultDoubleAccuracy = DoublePrecision * 10;
Precision.Increment = function (value) {
    return Increment(value, -1);
}
Precision.Increment = function (value, count) {
    if (isFinite(value) == false || isNaN(value) || count == 0) {
        return value;
    }
    var intValue = value;
    if (intValue < 0) {
        intValue -= count;
    } else {
        intValue += count;
    }
    if (intValue == Long.MIN_VALUE) {
        return 0;
    }
    return intValue;
}
Precision.EpsilonOf = function (value) {
    if (isFinite(value) == false || isNaN(value)) {
        return Double.NaN;
    }
    var signed64 = value;
    if (signed64 == 0) {
        signed64++;
        return signed64 - value;
    }
    if (signed64-- < 0) {
        return signed64 - value;
    }
    return value - signed64;
}
Precision.PositiveEpsilonOf = function (value) {
    return 2 * EpsilonOf(value);
}
Precision.AlmostEqualNormRelative = function (a, b, diff, maximumError) {
    if (isFinite(a) == false || isFinite(b) == false) {
        return a == b;
    }
    if (isNaN(a) || isNaN(b)) {
        return false;
    }
    if (Math.abs(a) < DoublePrecision || Math.abs(b) < DoublePrecision) {
        return Math.abs(diff) < maximumError;
    }
    if ((a == 0 && Math.abs(b) < maximumError) || (b == 0 && Math.abs(a) < maximumError)) {
        return true;
    }
    return Math.abs(diff) < maximumError * Math.max(Math.abs(a), Math.abs(b));
}
Precision.AlmostEqualRelative = function (a, b) {
    return AlmostEqualNormRelative(a, b, a - b, DefaultDoubleAccuracy);
}
Precision.AlmostEqual = function (a, b) {
    return AlmostEqualNorm(a, b, a - b, DefaultDoubleAccuracy);
}
Precision.AlmostEqualNorm = function (a, b, diff, maximumAbsoluteError) {
    if (isFinite(a) == false || isFinite(b) == false) {
        return a == b;
    }
    if (isNaN(a) || isNaN(b)) {
        return false;
    }
    return Math.abs(diff) < maximumAbsoluteError;
}

module.exports = Precision;