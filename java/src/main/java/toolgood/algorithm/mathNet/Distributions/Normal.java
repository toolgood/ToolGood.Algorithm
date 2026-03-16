package toolgood.algorithm.mathNet.Distributions;

import java.math.BigDecimal;
import java.math.MathContext;

import toolgood.algorithm.mathNet.Constants;
import toolgood.algorithm.system.MathEx;
import toolgood.algorithm.mathNet.SpecialFunctions;

public class Normal {
    private static final BigDecimal Sqrt2 = Constants.Sqrt2;
    private static final BigDecimal Sqrt2Pi = Constants.Sqrt2Pi;

    public static BigDecimal PDF(BigDecimal mean, BigDecimal standardDev, BigDecimal x) {
        return PDF(mean, standardDev, x, MathContext.DECIMAL128);
    }

    public static BigDecimal PDF(BigDecimal mean, BigDecimal standardDev, BigDecimal x, MathContext mc) {
        BigDecimal zero = BigDecimal.ZERO;
        if (standardDev.compareTo(zero) <= 0) {
            throw new IllegalArgumentException("standardDev must be greater than zero");
        }

        BigDecimal num = MathEx.Expi(
                x.subtract(mean)
                        .pow(2)
                        .divide(
                                new BigDecimal("-2").multiply(standardDev.pow(2)),
                                mc)
        );

        BigDecimal den = standardDev.multiply(Sqrt2Pi);

        return num.divide(den, mc);
    }

    public static BigDecimal CDF(BigDecimal mean, BigDecimal standardDev, BigDecimal x) {
        return CDF(mean, standardDev, x, MathContext.DECIMAL128);
    }

    public static BigDecimal CDF(BigDecimal mean, BigDecimal standardDev, BigDecimal x, MathContext mc) {
        if (standardDev.compareTo(BigDecimal.ZERO) <= 0) {
            throw new IllegalArgumentException("standardDev must be greater than zero");
        }

        return new BigDecimal("0.5").multiply(
                BigDecimal.ONE.add(SpecialFunctions.Erf(
                        x.subtract(mean)
                                .divide(standardDev.multiply(Sqrt2), mc)
                ))
        );
    }

    public static BigDecimal InvCDF(BigDecimal mean, BigDecimal standardDev, BigDecimal probability) {
        if (probability.compareTo(BigDecimal.ZERO) < 0 || probability.compareTo(BigDecimal.ONE) > 0) {
            throw new IllegalArgumentException("probability must be between 0 and 1");
        }

        if (standardDev.compareTo(BigDecimal.ZERO) <= 0) {
            throw new IllegalArgumentException("standardDev must be greater than zero");
        }

        if (probability.compareTo(BigDecimal.ZERO) == 0) {
            return new BigDecimal("-79228162514264337593543950335");
        }

        if (probability.compareTo(BigDecimal.ONE) == 0) {
            return new BigDecimal("79228162514264337593543950335");
        }

        BigDecimal p = probability.subtract(new BigDecimal("0.5"));

        BigDecimal t = MathEx.Sqrt(new BigDecimal("-2").multiply(MathEx.Log(p.pow(2))));

        BigDecimal c0 = new BigDecimal("2.515517");
        BigDecimal c1 = new BigDecimal("0.802853");
        BigDecimal c2 = new BigDecimal("0.010328");
        BigDecimal d1 = new BigDecimal("1.432788");
        BigDecimal d2 = new BigDecimal("0.189269");
        BigDecimal d3 = new BigDecimal("0.001308");

        BigDecimal num = c0.add(c1.multiply(t)).add(c2.multiply(t.pow(2)));
        BigDecimal den = BigDecimal.ONE.add(d1.multiply(t)).add(d2.multiply(t.pow(2))).add(d3.multiply(t.pow(3)));

        BigDecimal x = t.subtract(num.divide(den, MathContext.DECIMAL128));

        return mean.add(standardDev.multiply(x));
    }
}
