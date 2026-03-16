package toolgood.algorithm.mathNet.Distributions;

import java.math.BigDecimal;
import java.math.MathContext;

import toolgood.algorithm.mathNet.Constants;
import toolgood.algorithm.system.MathEx;
import toolgood.algorithm.mathNet.SpecialFunctions;

public class LogNormal {
    public static BigDecimal CDF(BigDecimal mu, BigDecimal sigma, BigDecimal x) {
        if (x.compareTo(BigDecimal.ZERO) < 0) {
            return BigDecimal.ZERO;
        }

        return new BigDecimal("0.5").multiply(
                BigDecimal.ONE.add(SpecialFunctions.Erf(
                        MathEx.Log(x).subtract(mu)
                                .divide(sigma.multiply(Constants.Sqrt2), MathContext.DECIMAL128))));
    }

    public static BigDecimal InvCDF(BigDecimal mu, BigDecimal sigma, BigDecimal p) {
        if (p.compareTo(BigDecimal.ZERO) <= 0) {
            return BigDecimal.ZERO;
        }
        if (p.compareTo(BigDecimal.ONE) >= 0) {
            return new BigDecimal("79228162514264337593543950335");
        }

        return MathEx.Exp(mu.subtract(sigma.multiply(Constants.Sqrt2).multiply(SpecialFunctions.ErfInv(new BigDecimal("2").multiply(p)).negate())));
    }
}
