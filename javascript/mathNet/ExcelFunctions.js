

class ExcelFunctions{}


ExcelFunctions.NormSDist=function(z) {
    return Normal.CDF(0, 1, z);
}

ExcelFunctions.NormSInv=function(probability) {
    return Normal.InvCDF(0, 1, probability);
}

ExcelFunctions.NormDist=function(x, mean, standardDev, cumulative) {
    return cumulative ? Normal.CDF(mean, standardDev, x) : Normal.PDF(mean, standardDev, x);
}

ExcelFunctions.NormInv=function(probability, mean, standardDev) {
    return Normal.InvCDF(mean, standardDev, probability);
}

ExcelFunctions.TDist=function(x, degreesFreedom, tails) {
    switch (tails) {
        case 1:
            return 1 - StudentT.CDF(0, 1, degreesFreedom, x);
        case 2:
            return 1 - StudentT.CDF(0, 1, degreesFreedom, x) + StudentT.CDF(0, 1, degreesFreedom, -x);
        default:
            throw new Exception("tails");
    }
}

ExcelFunctions.TInv=function(probability, degreesFreedom) {
    return -StudentT.InvCDF(0, 1, degreesFreedom, probability / 2);
}

ExcelFunctions.FDist=function(x, degreesFreedom1, degreesFreedom2) {
    return 1 - FisherSnedecor.CDF(degreesFreedom1, degreesFreedom2, x);
}

ExcelFunctions.FInv=function(probability, degreesFreedom1, degreesFreedom2) {
    return FisherSnedecor.InvCDF(degreesFreedom1, degreesFreedom2, 1 - probability);
}

ExcelFunctions.BetaDist=function(x, alpha, beta) {
    return Beta.CDF(alpha, beta, x);
}

ExcelFunctions.BetaInv=function(probability, alpha, beta) {
    return Beta.InvCDF(alpha, beta, probability);
}

ExcelFunctions.GammaDist=function(x, alpha, beta, cumulative) {
    return cumulative ? Gamma.CDF(alpha, 1 / beta, x) : Gamma.PDF(alpha, 1 / beta, x);
}

ExcelFunctions.GammaInv=function(probability, alpha, beta) {
    return Gamma.InvCDF(alpha, 1 / beta, probability);
}

ExcelFunctions.Quartile=function(array, quant) {
    switch (quant) {
        case 0:
            return ArrayStatistics.Minimum(array);
        case 1:
            return Statistics.QuantileCustom(array,0.25, QuantileDefinition.R7);
        case 2:
            return Statistics.QuantileCustom(array,0.5, QuantileDefinition.R7);
        case 3:
            return Statistics.QuantileCustom(array,0.75, QuantileDefinition.R7);
        case 4:
            return ArrayStatistics.Maximum(array);
        default:
            throw new Exception("quant");
    }
}
ExcelFunctions.Percentile=function(array, k) {
    return Statistics.QuantileCustom(array,k, QuantileDefinition.R7);
}
ExcelFunctions.PercentRank=function(array, x) {
    return Statistics.QuantileRank(array,x);
    // return array.QuantileRank(x);
}

ExcelFunctions.GAMMALN=function(z) {
    return SpecialFunctions.GammaLn(z);
}

ExcelFunctions.ExponDist=function(x, rate, state) {
    if (state) {
        return Exponential.CDF(rate, x);
    }
    return Exponential.PDF(rate, x);

}

ExcelFunctions.HypgeomDist=function(k, draws, success, population) {
    return Hypergeometric.PMF(population, success, draws, k);
}

ExcelFunctions.NegbinomDist=function(k, r, p) {
    return NegativeBinomial.PMF(r, p, k);

}

ExcelFunctions.LognormDist=function(x, mu, sigma) {
    return LogNormal.CDF(mu, sigma, x);
}

ExcelFunctions.LogInv=function(p, mu, sigma) {
    return LogNormal.InvCDF(mu, sigma, p);
}

ExcelFunctions.BinomDist=function(k, n, p, state) {
    if (state == false) {
        return Binomial.PMF(p, n, k);
    }
    return Binomial.CDF(p, n, k);
}

ExcelFunctions.POISSON=function(k, lambda, state) {
    if (state == false) {
        return Poisson.PMF(lambda, k);
    }
    return Poisson.CDF(lambda, k);
}

ExcelFunctions.WEIBULL=function(x, shape, scale, state) {
    if (state == false) {
        return Weibull.PDF(shape, scale, x);
    }
    return Weibull.CDF(shape, scale, x);
}











module.exports=ExcelFunctions;