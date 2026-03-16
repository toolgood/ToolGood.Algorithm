package toolgood.algorithm.mathNet.Distributions;

import java.math.BigDecimal;
import java.math.MathContext;

import toolgood.algorithm.mathNet.SpecialFunctions;
import toolgood.algorithm.mathNet.RootFinding.Brent;

public class Beta {

    public static BigDecimal CDF(BigDecimal a, BigDecimal b, BigDecimal x) {
        BigDecimal zero = BigDecimal.ZERO;
        BigDecimal one = BigDecimal.ONE;

        if (x.compareTo(zero) < 0) {
            return zero;
        }

        if (x.compareTo(one) >= 0) {
            return one;
        }

        if (a.compareTo(zero) == 0 && b.compareTo(zero) == 0) {
            if (x.compareTo(zero) >= 0 && x.compareTo(one) < 0) {
                return new BigDecimal("0.5");
            }
            return one;
        }

        if (a.compareTo(zero) == 0) {
            return one;
        }

        if (b.compareTo(zero) == 0) {
            return x.compareTo(one) >= 0 ? one : zero;
        }

        if (a.compareTo(one) == 0 && b.compareTo(one) == 0) {
            return x;
        }

        return SpecialFunctions.BetaRegularized(a, b, x);
    }

    public static BigDecimal InvCDF(BigDecimal a, BigDecimal b, BigDecimal p) throws Exception {
        return Brent.FindRoot(x -> SpecialFunctions.BetaRegularized(a, b, x).subtract(p), 
                BigDecimal.ZERO, BigDecimal.ONE, new BigDecimal("1e-12"));
    }
}
