package toolgood.algorithm.mathNet.Distributions;

import java.math.BigDecimal;
import java.math.MathContext;

import toolgood.algorithm.system.MathEx;
import toolgood.algorithm.mathNet.SpecialFunctions;

public class Gamma {
    public static BigDecimal CDF(BigDecimal shape, BigDecimal rate, BigDecimal x) {
        if (shape.compareTo(BigDecimal.ZERO) == 0 && rate.compareTo(BigDecimal.ZERO) == 0) {
            return BigDecimal.ZERO;
        }

        return SpecialFunctions.GammaLowerRegularized(shape, x.multiply(rate));
    }

    public static BigDecimal PDF(BigDecimal shape, BigDecimal rate, BigDecimal x) {
        if (shape.compareTo(BigDecimal.ZERO) == 0 && rate.compareTo(BigDecimal.ZERO) == 0) {
            return BigDecimal.ZERO;
        }

        if (shape.compareTo(BigDecimal.ONE) == 0) {
            return rate.multiply(MathEx.Exp(rate.multiply(x).negate()));
        }

        if (shape.compareTo(new BigDecimal("160")) > 0) {
            return MathEx.Exp(PDFLn(shape, rate, x));
        }

        return MathEx.Pow(rate, shape)
                .multiply(MathEx.Pow(x, shape.subtract(BigDecimal.ONE)))
                .multiply(MathEx.Exp(rate.multiply(x).negate()))
                .divide(SpecialFunctions.Gamma(shape), MathContext.DECIMAL128);
    }

    public static BigDecimal PDFLn(BigDecimal shape, BigDecimal rate, BigDecimal x) {
        if (shape.compareTo(BigDecimal.ZERO) == 0 && rate.compareTo(BigDecimal.ZERO) == 0) {
            return new BigDecimal("-79228162514264337593543950335");
        }

        if (shape.compareTo(BigDecimal.ONE) == 0) {
            return MathEx.Log(rate).subtract(rate.multiply(x));
        }

        return shape.multiply(MathEx.Log(rate))
                .add(shape.subtract(BigDecimal.ONE).multiply(MathEx.Log(x)))
                .subtract(rate.multiply(x))
                .subtract(SpecialFunctions.GammaLn(shape));
    }

    public static BigDecimal InvCDF(BigDecimal shape, BigDecimal rate, BigDecimal p) {
        return SpecialFunctions.GammaLowerRegularizedInv(shape, p).divide(rate, MathContext.DECIMAL128);
    }
}
