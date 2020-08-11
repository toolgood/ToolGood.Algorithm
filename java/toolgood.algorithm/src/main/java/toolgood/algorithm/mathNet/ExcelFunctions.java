package toolgood.algorithm.mathNet;

import toolgood.algorithm.mathNet.Distributions.Beta;
import toolgood.algorithm.mathNet.Distributions.Binomial;
import toolgood.algorithm.mathNet.Distributions.Exponential;
import toolgood.algorithm.mathNet.Distributions.FisherSnedecor;
import toolgood.algorithm.mathNet.Distributions.Gamma;
import toolgood.algorithm.mathNet.Distributions.Hypergeometric;
import toolgood.algorithm.mathNet.Distributions.LogNormal;
import toolgood.algorithm.mathNet.Distributions.NegativeBinomial;
import toolgood.algorithm.mathNet.Distributions.Normal;
import toolgood.algorithm.mathNet.Distributions.Poisson;
import toolgood.algorithm.mathNet.Distributions.StudentT;
import toolgood.algorithm.mathNet.Distributions.Weibull;
import toolgood.algorithm.mathNet.Statistics.ArrayStatistics;
import toolgood.algorithm.mathNet.Statistics.QuantileDefinition;
import toolgood.algorithm.mathNet.Statistics.Statistics;

public class ExcelFunctions {

    public static double NormSDist(double z) {
        return Normal.CDF(0d, 1d, z);
    }

    public static double NormSInv(double probability) {
        return Normal.InvCDF(0d, 1d, probability);
    }

    public static double NormDist(double x, double mean, double standardDev, boolean cumulative) {
        return cumulative ? Normal.CDF(mean, standardDev, x) : Normal.PDF(mean, standardDev, x);
    }

    public static double NormInv(double probability, double mean, double standardDev) {
        return Normal.InvCDF(mean, standardDev, probability);
    }

    public static double TDist(double x, int degreesFreedom, int tails) throws Exception {
        switch (tails) {
            case 1:
                return 1d - StudentT.CDF(0d, 1d, degreesFreedom, x);
            case 2:
                return 1d - StudentT.CDF(0d, 1d, degreesFreedom, x) + StudentT.CDF(0d, 1d, degreesFreedom, -x);
            default:
                throw new Exception("tails");
        }
    }

    public static double TInv(double probability, int degreesFreedom) throws Exception {
        return -StudentT.InvCDF(0d, 1d, degreesFreedom, probability / 2);
    }

    public static double FDist(double x, int degreesFreedom1, int degreesFreedom2) {
        return 1d - FisherSnedecor.CDF(degreesFreedom1, degreesFreedom2, x);
    }

    public static double FInv(double probability, int degreesFreedom1, int degreesFreedom2) throws Exception {
        return FisherSnedecor.InvCDF(degreesFreedom1, degreesFreedom2, 1d - probability);
    }

    public static double BetaDist(double x, double alpha, double beta) {
        return Beta.CDF(alpha, beta, x);
    }

    public static double BetaInv(double probability, double alpha, double beta) throws Exception {
        return Beta.InvCDF(alpha, beta, probability);
    }

    public static double GammaDist(double x, double alpha, double beta, boolean cumulative) {
        return cumulative ? Gamma.CDF(alpha, 1 / beta, x) : Gamma.PDF(alpha, 1 / beta, x);
    }

    public static double GammaInv(double probability, double alpha, double beta) {
        return Gamma.InvCDF(alpha, 1 / beta, probability);
    }

    public static double Quartile(double[] array, int quant) throws Exception {
        switch (quant) {
            case 0:
                return ArrayStatistics.Minimum(array);
            case 1:
                return Statistics.QuantileCustom(array,0.25, QuantileDefinition.Excel);
            case 2:
                return Statistics.QuantileCustom(array,0.5, QuantileDefinition.Excel);
            case 3:
                return Statistics.QuantileCustom(array,0.75, QuantileDefinition.Excel);
            case 4:
                return ArrayStatistics.Maximum(array);
            default:
                throw new Exception("quant");
        }
    }

    public static double Percentile(double[] array, double k) throws Exception {
        return Statistics.QuantileCustom(array,k, QuantileDefinition.Excel);
    }

    public static double PercentRank(double[] array, double x) {
        return Statistics.QuantileRank(array,x);
        // return array.QuantileRank(x);
    }

    public static double GAMMALN(double z) {
        return SpecialFunctions.GammaLn(z);
    }

    // public static double ChiDist(double x, double freedom)
    // {
    // return Chi.PDF(x, freedom);//Is Error
    // }

    public static double ExponDist(double x, double rate, boolean state) {
        if (state) {
            return Exponential.CDF(rate, x);
        }
        return Exponential.PDF(rate, x);

    }

    public static double HypgeomDist(int k, int draws, int success, int population) {
        return Hypergeometric.PMF(population, success, draws, k);
    }

    public static double NegbinomDist(int k, double r, double p) {
        return NegativeBinomial.PMF(r, p, k);

    }

    public static double LognormDist(double x, double mu, double sigma) {
        return LogNormal.CDF(mu, sigma, x);
    }

    public static double LogInv(double p, double mu, double sigma) {
        return LogNormal.InvCDF(mu, sigma, p);
    }

    public static double BinomDist(int k, int n, double p, boolean state) {
        if (state == false) {
            return Binomial.PMF(p, n, k);
        }
        return Binomial.CDF(p, n, k);
    }

    public static double POISSON(int k, double lambda, boolean state) {
        if (state == false) {
            return Poisson.PMF(lambda, k);
        }
        return Poisson.CDF(lambda, k);
    }

    public static double WEIBULL(double x, double shape, double scale, boolean state) {
        if (state == false) {
            return Weibull.PDF(shape, scale, x);
        }
        return Weibull.CDF(shape, scale, x);
    }

}