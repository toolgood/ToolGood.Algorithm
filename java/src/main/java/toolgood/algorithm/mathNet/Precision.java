package toolgood.algorithm.mathNet;

import java.math.BigDecimal;
import java.math.MathContext;

public class Precision {
    public static final BigDecimal DecimalPrecision = new BigDecimal("0.0000000000000000000000000001");

    public static final BigDecimal PositiveDecimalPrecision = new BigDecimal("2").multiply(DecimalPrecision);

    private static final BigDecimal DefaultDecimalAccuracy = DecimalPrecision.multiply(new BigDecimal("10"));

    public static boolean AlmostEqualNormRelative(BigDecimal a, BigDecimal b, BigDecimal diff, BigDecimal maximumError) {
        if (a.abs().compareTo(DecimalPrecision) < 0 || b.abs().compareTo(DecimalPrecision) < 0) {
            return diff.abs().compareTo(maximumError) < 0;
        }

        if ((a.compareTo(BigDecimal.ZERO) == 0 && b.abs().compareTo(maximumError) < 0) 
                || (b.compareTo(BigDecimal.ZERO) == 0 && a.abs().compareTo(maximumError) < 0)) {
            return true;
        }

        return diff.abs().compareTo(maximumError.multiply(a.abs().max(b.abs()))) < 0;
    }

    public static boolean AlmostEqualRelative(BigDecimal a, BigDecimal b) {
        return AlmostEqualNormRelative(a, b, a.subtract(b), DefaultDecimalAccuracy);
    }

    public static boolean AlmostEqual(BigDecimal a, BigDecimal b) {
        return AlmostEqualNorm(a, b, a.subtract(b), DefaultDecimalAccuracy);
    }

    public static boolean AlmostEqualNorm(BigDecimal a, BigDecimal b, BigDecimal diff, BigDecimal maximumAbsoluteError) {
        return diff.abs().compareTo(maximumAbsoluteError) < 0;
    }
}
