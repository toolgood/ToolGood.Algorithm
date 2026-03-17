package toolgood.algorithm.mathNet.Distributions;

import java.math.BigDecimal;
import java.math.MathContext;

import toolgood.algorithm.mathNet.SpecialFunctions;
import toolgood.algorithm.mathNet.RootFinding.Brent;

public class StudentT {
    public static BigDecimal CDF(BigDecimal location, BigDecimal scale, int freedom, BigDecimal x) throws Exception {
        BigDecimal k = x.subtract(location).divide(scale, MathContext.DECIMAL128);
        BigDecimal h = new BigDecimal(freedom).divide(new BigDecimal(freedom).add(k.multiply(k)), MathContext.DECIMAL128);
        BigDecimal ib = new BigDecimal("0.5").multiply(SpecialFunctions.BetaRegularized(
                new BigDecimal(freedom).divide(new BigDecimal("2"), MathContext.DECIMAL128),
                new BigDecimal("0.5"),
                h));
        return x.compareTo(location) <= 0 ? ib : BigDecimal.ONE.subtract(ib);
    }

    public static BigDecimal InvCDF(BigDecimal location, BigDecimal scale, int freedom, BigDecimal p) throws Exception {
        if (p.compareTo(new BigDecimal("0.5")) == 0) {
            return location;
        }

        return Brent.FindRoot(x -> {
            BigDecimal k = x.subtract(location).divide(scale, MathContext.DECIMAL128);
            BigDecimal h = new BigDecimal(freedom).divide(new BigDecimal(freedom).add(k.multiply(k)), MathContext.DECIMAL128);
            try {
                BigDecimal ib = new BigDecimal("0.5").multiply(SpecialFunctions.BetaRegularized(
                        new BigDecimal(freedom).divide(new BigDecimal("2"), MathContext.DECIMAL128),
                        new BigDecimal("0.5"),
                        h));
                return x.compareTo(location) <= 0 ? ib.subtract(p) : BigDecimal.ONE.subtract(ib).subtract(p);
            } catch (Exception e) {
                return BigDecimal.ZERO;
            }
        }, new BigDecimal("-800"), new BigDecimal("800"), new BigDecimal("1e-12"));
    }
}
