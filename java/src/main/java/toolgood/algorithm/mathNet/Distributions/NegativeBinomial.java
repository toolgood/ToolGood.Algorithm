package toolgood.algorithm.mathNet.Distributions;

import java.math.BigDecimal;

import toolgood.algorithm.system.MathEx;
import toolgood.algorithm.mathNet.SpecialFunctions;

public class NegativeBinomial {
    public static BigDecimal PMF(BigDecimal r, BigDecimal p, int k) {
        return MathEx.Exp(PMFLn(r, p, k));
    }

    public static BigDecimal PMFLn(BigDecimal r, BigDecimal p, int k) {
        return SpecialFunctions.GammaLn(r.add(new BigDecimal(k)))
                .subtract(SpecialFunctions.GammaLn(r))
                .subtract(SpecialFunctions.GammaLn(new BigDecimal(k + 1)))
                .add(r.multiply(MathEx.Log(p)))
                .add(new BigDecimal(k).multiply(MathEx.Log(BigDecimal.ONE.subtract(p))));
    }
}
