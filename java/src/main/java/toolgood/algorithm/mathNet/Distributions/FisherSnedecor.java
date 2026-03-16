package toolgood.algorithm.mathNet.Distributions;

import java.math.BigDecimal;
import java.math.MathContext;

import toolgood.algorithm.mathNet.RootFinding.Brent;
import toolgood.algorithm.mathNet.SpecialFunctions;

public class FisherSnedecor {
    public static BigDecimal CDF(int d1, int d2, BigDecimal x) {
        return SpecialFunctions.BetaRegularized(
                new BigDecimal(d1).divide(new BigDecimal("2"), MathContext.DECIMAL128),
                new BigDecimal(d2).divide(new BigDecimal("2"), MathContext.DECIMAL128),
                new BigDecimal(d1).multiply(x).divide(new BigDecimal(d1).multiply(x).add(new BigDecimal(d2)), MathContext.DECIMAL128));
    }

    public static BigDecimal InvCDF(int d1, int d2, BigDecimal p) throws Exception {
        return Brent.FindRoot(
                x -> SpecialFunctions.BetaRegularized(
                        new BigDecimal(d1).divide(new BigDecimal("2"), MathContext.DECIMAL128),
                        new BigDecimal(d2).divide(new BigDecimal("2"), MathContext.DECIMAL128),
                        new BigDecimal(d1).multiply(x).divide(new BigDecimal(d1).multiply(x).add(new BigDecimal(d2)), MathContext.DECIMAL128))
                        .subtract(p),
                BigDecimal.ZERO, new BigDecimal("1000"), new BigDecimal("1e-12"));
    }
}
