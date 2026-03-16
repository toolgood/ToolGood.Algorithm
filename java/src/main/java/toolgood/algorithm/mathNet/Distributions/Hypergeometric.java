package toolgood.algorithm.mathNet.Distributions;

import java.math.BigDecimal;
import java.math.MathContext;

import toolgood.algorithm.system.MathEx;
import toolgood.algorithm.mathNet.SpecialFunctions;

public class Hypergeometric {
    public static BigDecimal PMF(int population, int success, int draws, int k) {
        if (k < 0 || k > draws || k > success || k < draws + success - population) {
            return BigDecimal.ZERO;
        }

        BigDecimal binomialLn = SpecialFunctions.GammaLn(new BigDecimal(success + 1))
                .subtract(SpecialFunctions.GammaLn(new BigDecimal(k + 1)))
                .subtract(SpecialFunctions.GammaLn(new BigDecimal(success - k + 1)))
                .add(SpecialFunctions.GammaLn(new BigDecimal(population - success + 1)))
                .subtract(SpecialFunctions.GammaLn(new BigDecimal(draws - k + 1)))
                .subtract(SpecialFunctions.GammaLn(new BigDecimal(population - success - draws + k + 1)))
                .subtract(SpecialFunctions.GammaLn(new BigDecimal(population + 1)))
                .add(SpecialFunctions.GammaLn(new BigDecimal(draws + 1)))
                .add(SpecialFunctions.GammaLn(new BigDecimal(population - draws + 1)));

        return MathEx.Exp(binomialLn);
    }
}
