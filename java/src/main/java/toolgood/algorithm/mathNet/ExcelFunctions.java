package toolgood.algorithm.mathNet;

import java.math.BigDecimal;
import java.math.MathContext;
import java.util.Arrays;

import toolgood.algorithm.mathNet.Distributions.Beta;
import toolgood.algorithm.mathNet.Distributions.Binomial;
import toolgood.algorithm.mathNet.Distributions.FisherSnedecor;
import toolgood.algorithm.mathNet.Distributions.Gamma;
import toolgood.algorithm.mathNet.Distributions.Hypergeometric;
import toolgood.algorithm.mathNet.Distributions.LogNormal;
import toolgood.algorithm.mathNet.Distributions.NegativeBinomial;
import toolgood.algorithm.mathNet.Distributions.Normal;
import toolgood.algorithm.mathNet.Distributions.Poisson;
import toolgood.algorithm.mathNet.Distributions.StudentT;
import toolgood.algorithm.mathNet.Statistics.ArrayStatistics;
import toolgood.algorithm.mathNet.Statistics.SortedArrayStatistics;

public class ExcelFunctions {

    public static BigDecimal NormSDist(BigDecimal z) {
        return Normal.CDF(BigDecimal.ZERO, BigDecimal.ONE, z);
    }

    public static BigDecimal NormSInv(BigDecimal probability) {
        return Normal.InvCDF(BigDecimal.ZERO, BigDecimal.ONE, probability);
    }

    public static BigDecimal NormDist(BigDecimal x, BigDecimal mean, BigDecimal standardDev, boolean cumulative) {
        return cumulative ? Normal.CDF(mean, standardDev, x) : Normal.PDF(mean, standardDev, x);
    }

    public static BigDecimal NormInv(BigDecimal probability, BigDecimal mean, BigDecimal standardDev) {
        return Normal.InvCDF(mean, standardDev, probability);
    }

    public static BigDecimal TDist(BigDecimal x, int degreesFreedom, int tails) throws Exception {
        switch (tails) {
            case 1:
                return BigDecimal.ONE.subtract(StudentT.CDF(BigDecimal.ZERO, BigDecimal.ONE, degreesFreedom, x));
            case 2:
                return BigDecimal.ONE.subtract(StudentT.CDF(BigDecimal.ZERO, BigDecimal.ONE, degreesFreedom, x))
                        .add(StudentT.CDF(BigDecimal.ZERO, BigDecimal.ONE, degreesFreedom, x.negate()));
            default:
                throw new Exception("tails");
        }
    }

    public static BigDecimal TInv(BigDecimal probability, int degreesFreedom) throws Exception {
        return StudentT.InvCDF(BigDecimal.ZERO, BigDecimal.ONE, degreesFreedom, probability.divide(new BigDecimal("2"), MathContext.DECIMAL128)).negate();
    }

    public static BigDecimal FDist(BigDecimal x, int degreesFreedom1, int degreesFreedom2) {
        return BigDecimal.ONE.subtract(FisherSnedecor.CDF(degreesFreedom1, degreesFreedom2, x));
    }

    public static BigDecimal FInv(BigDecimal probability, int degreesFreedom1, int degreesFreedom2) throws Exception {
        return FisherSnedecor.InvCDF(degreesFreedom1, degreesFreedom2, BigDecimal.ONE.subtract(probability));
    }

    public static BigDecimal BetaDist(BigDecimal x, BigDecimal alpha, BigDecimal beta) {
        return Beta.CDF(alpha, beta, x);
    }

    public static BigDecimal BetaInv(BigDecimal probability, BigDecimal alpha, BigDecimal beta) throws Exception {
        return Beta.InvCDF(alpha, beta, probability);
    }

    public static BigDecimal GammaDist(BigDecimal x, BigDecimal alpha, BigDecimal beta, boolean cumulative) {
        return cumulative ? Gamma.CDF(alpha, new BigDecimal("1").divide(beta, MathContext.DECIMAL128), x) 
                : Gamma.PDF(alpha, new BigDecimal("1").divide(beta, MathContext.DECIMAL128), x);
    }

    public static BigDecimal GammaInv(BigDecimal probability, BigDecimal alpha, BigDecimal beta) {
        return Gamma.InvCDF(alpha, new BigDecimal("1").divide(beta, MathContext.DECIMAL128), probability);
    }

    public static BigDecimal GAMMALN(BigDecimal z) {
        return SpecialFunctions.GammaLn(z);
    }

    public static BigDecimal HypgeomDist(int k, int draws, int success, int population) {
        return Hypergeometric.PMF(population, success, draws, k);
    }

    public static BigDecimal NegbinomDist(int k, BigDecimal r, BigDecimal p) {
        return NegativeBinomial.PMF(r, p, k);
    }

    public static BigDecimal LognormDist(BigDecimal x, BigDecimal mu, BigDecimal sigma) {
        return LogNormal.CDF(mu, sigma, x);
    }

    public static BigDecimal LogInv(BigDecimal p, BigDecimal mu, BigDecimal sigma) {
        return LogNormal.InvCDF(mu, sigma, p);
    }

    public static BigDecimal BinomDist(int k, int n, BigDecimal p, boolean state) {
        if (state == false) {
            return Binomial.PMF(p, n, k);
        }
        return Binomial.CDF(p, n, new BigDecimal(k));
    }

    public static BigDecimal Poisson(int k, BigDecimal lambda, boolean state) {
        if (state == false) {
            return Poisson.PMF(lambda, k);
        }
        return Poisson.CDF(lambda, new BigDecimal(k));
    }

    public static BigDecimal Quartile(BigDecimal[] array, int quant) throws Exception {
        switch (quant) {
            case 0:
                return ArrayStatistics.Minimum(array);
            case 1:
                return QuantileCustom(array, new BigDecimal("0.25"));
            case 2:
                return QuantileCustom(array, new BigDecimal("0.5"));
            case 3:
                return QuantileCustom(array, new BigDecimal("0.75"));
            case 4:
                return ArrayStatistics.Maximum(array);
            default:
                throw new Exception("quant");
        }
    }

    public static BigDecimal Percentile(BigDecimal[] array, BigDecimal k) throws Exception {
        return QuantileCustom(array, k);
    }

    public static BigDecimal PercentRank(BigDecimal[] array, BigDecimal x) {
        BigDecimal[] sorted = array.clone();
        Arrays.sort(sorted, (a, b) -> a.compareTo(b));
        return SortedArrayStatistics.QuantileRank(sorted, x);
    }

    private static BigDecimal QuantileCustom(BigDecimal[] data, BigDecimal tau) throws Exception {
        return ArrayStatistics.QuantileCustomInplace(data, tau);
    }
}
