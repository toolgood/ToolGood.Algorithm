package toolgood.algorithm.mathNet.Distributions;

import java.math.BigDecimal;
import java.math.MathContext;

import toolgood.algorithm.system.MathEx;
import toolgood.algorithm.mathNet.SpecialFunctions;

public class Binomial {
    public static BigDecimal PMF(BigDecimal p, int n, int k) {
        if (k < 0 || k > n) {
            return BigDecimal.ZERO;
        }

        if (p.compareTo(BigDecimal.ZERO) == 0) {
            return k == 0 ? BigDecimal.ONE : BigDecimal.ZERO;
        }

        if (p.compareTo(BigDecimal.ONE) == 0) {
            return k == n ? BigDecimal.ONE : BigDecimal.ZERO;
        }

        BigDecimal binomialLn = SpecialFunctions.GammaLn(new BigDecimal(n + 1))
                .subtract(SpecialFunctions.GammaLn(new BigDecimal(k + 1)))
                .subtract(SpecialFunctions.GammaLn(new BigDecimal(n - k + 1)));

        return MathEx.Exp(binomialLn
                .add(new BigDecimal(k).multiply(MathEx.Log(p)))
                .add(new BigDecimal(n - k).multiply(MathEx.Log(BigDecimal.ONE.subtract(p)))));
    }

    public static BigDecimal CDF(BigDecimal p, int n, BigDecimal x) {
        if (x.compareTo(BigDecimal.ZERO) < 0) {
            return BigDecimal.ZERO;
        }

        if (x.compareTo(new BigDecimal(n)) > 0) {
            return BigDecimal.ONE;
        }

        BigDecimal k = new BigDecimal(Math.floor(x.doubleValue()));
        return SpecialFunctions.BetaRegularized(new BigDecimal(n - k.intValue()), new BigDecimal(k.intValue() + 1), BigDecimal.ONE.subtract(p));
    }
}
