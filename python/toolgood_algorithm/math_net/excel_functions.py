"""Excel compatible distribution helpers (``ExcelFunctions`` port)."""
from __future__ import annotations

from typing import List

from toolgood_algorithm.math_net.distributions import (
    Beta,
    Binomial,
    Exponential,
    FisherSnedecor,
    Gamma,
    Hypergeometric,
    LogNormal,
    NegativeBinomial,
    Normal,
    Poisson,
    StudentT,
    Weibull,
)
from toolgood_algorithm.math_net.special_functions import gamma_ln
from toolgood_algorithm.math_net.statistics import maximum, minimum, quantile_custom, quantile_rank


def norm_s_dist(z: float) -> float:
    return Normal.cdf(0.0, 1.0, z)


def norm_s_inv(probability: float) -> float:
    return Normal.inv_cdf(0.0, 1.0, probability)


def norm_dist(x: float, mean: float, standard_dev: float, cumulative: bool) -> float:
    return Normal.cdf(mean, standard_dev, x) if cumulative else Normal.pdf(mean, standard_dev, x)


def norm_inv(probability: float, mean: float, standard_dev: float) -> float:
    return Normal.inv_cdf(mean, standard_dev, probability)


def t_dist(x: float, degrees_freedom: int, tails: int) -> float:
    if tails == 1:
        return 1.0 - StudentT.cdf(0.0, 1.0, degrees_freedom, x)
    if tails == 2:
        return 1.0 - StudentT.cdf(0.0, 1.0, degrees_freedom, x) + StudentT.cdf(0.0, 1.0, degrees_freedom, -x)
    raise ValueError("tails")


def t_inv(probability: float, degrees_freedom: int) -> float:
    return -StudentT.inv_cdf(0.0, 1.0, degrees_freedom, probability / 2)


def f_dist(x: float, degrees_freedom1: int, degrees_freedom2: int) -> float:
    return 1.0 - FisherSnedecor.cdf(degrees_freedom1, degrees_freedom2, x)


def f_inv(probability: float, degrees_freedom1: int, degrees_freedom2: int) -> float:
    return FisherSnedecor.inv_cdf(degrees_freedom1, degrees_freedom2, 1.0 - probability)


def beta_dist(x: float, alpha: float, beta: float) -> float:
    return Beta.cdf(alpha, beta, x)


def beta_inv(probability: float, alpha: float, beta: float) -> float:
    return Beta.inv_cdf(alpha, beta, probability)


def gamma_dist(x: float, alpha: float, beta: float, cumulative: bool) -> float:
    return Gamma.cdf(alpha, 1 / beta, x) if cumulative else Gamma.pdf(alpha, 1 / beta, x)


def gamma_inv(probability: float, alpha: float, beta: float) -> float:
    return Gamma.inv_cdf(alpha, 1 / beta, probability)


def quartile(data, quant: int) -> float:
    if isinstance(data, list) and data and not isinstance(data[0], float):
        data = [float(v) for v in data]
    if quant == 0:
        return minimum(data)
    if quant == 1:
        return quantile_custom(data, 0.25, "R7")
    if quant == 2:
        return quantile_custom(data, 0.5, "R7")
    if quant == 3:
        return quantile_custom(data, 0.75, "R7")
    if quant == 4:
        return maximum(data)
    raise ValueError("quant")


def percentile(data, k: float) -> float:
    return quantile_custom(list(data), k, "R7")


def percent_rank(data, x: float) -> float:
    return quantile_rank(list(data), x)


def gammln(z: float) -> float:
    return gamma_ln(z)


def expon_dist(x: float, rate: float, state: bool) -> float:
    return Exponential.cdf(rate, x) if state else Exponential.pdf(rate, x)


def hypgeom_dist(k: int, draws: int, success: int, population: int) -> float:
    return Hypergeometric.pmf(population, success, draws, k)


def negbinom_dist(k: int, r: float, p: float) -> float:
    return NegativeBinomial.pmf(r, p, k)


def lognorm_dist(x: float, mu: float, sigma: float) -> float:
    return LogNormal.cdf(mu, sigma, x)


def log_inv(p: float, mu: float, sigma: float) -> float:
    return LogNormal.inv_cdf(mu, sigma, p)


def binom_dist(k: int, n: int, p: float, state: bool) -> float:
    return Binomial.cdf(p, n, k) if state else Binomial.pmf(p, n, k)


def poisson(k: int, lam: float, state: bool) -> float:
    return Poisson.cdf(lam, k) if state else Poisson.pmf(lam, k)


def weibull(x: float, shape: float, scale: float, state: bool) -> float:
    return Weibull.cdf(shape, scale, x) if state else Weibull.pdf(shape, scale, x)


class ExcelFunctions:
    """CamelCase facade mirroring the Java class."""

    NormSDist = staticmethod(norm_s_dist)
    NormSInv = staticmethod(norm_s_inv)
    NormDist = staticmethod(norm_dist)
    NormInv = staticmethod(norm_inv)
    TDist = staticmethod(t_dist)
    TInv = staticmethod(t_inv)
    FDist = staticmethod(f_dist)
    FInv = staticmethod(f_inv)
    BetaDist = staticmethod(beta_dist)
    BetaInv = staticmethod(beta_inv)
    GammaDist = staticmethod(gamma_dist)
    GammaInv = staticmethod(gamma_inv)
    Quartile = staticmethod(quartile)
    Percentile = staticmethod(percentile)
    PercentRank = staticmethod(percent_rank)
    GAMMALN = staticmethod(gammln)
    ExponDist = staticmethod(expon_dist)
    HypgeomDist = staticmethod(hypgeom_dist)
    NegbinomDist = staticmethod(negbinom_dist)
    LognormDist = staticmethod(lognorm_dist)
    LogInv = staticmethod(log_inv)
    BinomDist = staticmethod(binom_dist)
    POISSON = staticmethod(poisson)
    WEIBULL = staticmethod(weibull)
