package toolgood.algorithm.mathNet.Distributions;

import java.math.BigDecimal;
import java.math.MathContext;

import toolgood.algorithm.system.MathEx;
import toolgood.algorithm.mathNet.SpecialFunctions;

public class Poisson {
    public static BigDecimal PMF(BigDecimal lambda, int k) {
        return SpecialFunctions.Expi(lambda.negate().add(new BigDecimal(k).multiply(MathEx.Log(lambda)).subtract(SpecialFunctions.GammaLn(new BigDecimal(k + 1)))));
    }

    public static BigDecimal CDF(BigDecimal lambda, BigDecimal x) {
        return BigDecimal.ONE.subtract(SpecialFunctions.GammaLowerRegularized(x.add(BigDecimal.ONE), lambda));
    }
}
