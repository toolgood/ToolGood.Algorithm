"""Distribution functions used by the statistics functions."""
from __future__ import annotations

import math
from typing import Callable, List

from toolgood_algorithm.math_net.root_finding import find_root
from toolgood_algorithm.math_net.special_functions import (
    SQRT2,
    SQRT2PI,
    beta_regularized,
    erf,
    erfc,
    erfc_inv,
    gamma as _gamma,
    gamma_ln,
    gamma_lower_regularized,
    gamma_lower_regularized_inv,
)


class Normal:
    @staticmethod
    def cdf(mean: float, stddev: float, x: float) -> float:
        return 0.5 * erfc((mean - x) / (stddev * SQRT2))

    @staticmethod
    def inv_cdf(mean: float, stddev: float, p: float) -> float:
        return mean - (stddev * SQRT2 * erfc_inv(2.0 * p))

    @staticmethod
    def pdf(mean: float, stddev: float, x: float) -> float:
        d = (x - mean) / stddev
        return math.exp(-0.5 * d * d) / (SQRT2PI * stddev)


class StudentT:
    @staticmethod
    def cdf(location: float, scale: float, freedom: float, x: float) -> float:
        if math.isinf(freedom):
            return Normal.cdf(location, scale, x)
        k = (x - location) / scale
        h = freedom / (freedom + k * k)
        ib = 0.5 * beta_regularized(freedom / 2.0, 0.5, h)
        return ib if x <= location else 1.0 - ib

    @staticmethod
    def inv_cdf(location: float, scale: float, freedom: float, p: float) -> float:
        if math.isinf(freedom):
            return Normal.inv_cdf(location, scale, p)
        if p == 0.5:
            return location

        def f(x: float) -> float:
            k = (x - location) / scale
            h = freedom / (freedom + k * k)
            ib = 0.5 * beta_regularized(freedom / 2.0, 0.5, h)
            return ib - p if x <= location else 1.0 - ib - p

        return find_root(f, -800.0, 800.0, 1e-12)


class Beta:
    @staticmethod
    def cdf(a: float, b: float, x: float) -> float:
        if x < 0.0:
            return 0.0
        if x >= 1.0:
            return 1.0
        if math.isinf(a) and math.isinf(b):
            return 0.0 if x < 0.5 else 1.0
        if math.isinf(a):
            return 0.0 if x < 1.0 else 1.0
        if math.isinf(b):
            return 1.0 if x >= 0.0 else 0.0
        if a == 0.0 and b == 0.0:
            return 0.5 if 0.0 <= x < 1.0 else 1.0
        if a == 0.0:
            return 1.0
        if b == 0.0:
            return 1.0 if x >= 1.0 else 0.0
        if a == 1.0 and b == 1.0:
            return x
        return beta_regularized(a, b, x)

    @staticmethod
    def inv_cdf(a: float, b: float, p: float) -> float:
        def f(x: float) -> float:
            return beta_regularized(a, b, x) - p

        return find_root(f, 0.0, 1.0, 1e-12)


class Gamma:
    @staticmethod
    def cdf(shape: float, rate: float, x: float) -> float:
        if math.isinf(rate):
            return 1.0 if x >= shape else 0.0
        if shape == 0.0 and rate == 0.0:
            return 0.0
        return gamma_lower_regularized(shape, x * rate)

    @staticmethod
    def pdf(shape: float, rate: float, x: float) -> float:
        if math.isinf(rate):
            return float("inf") if x == shape else 0.0
        if shape == 0.0 and rate == 0.0:
            return 0.0
        if shape == 1.0:
            return rate * math.exp(-rate * x)
        if shape > 160.0:
            return math.exp(Gamma.pdf_ln(shape, rate, x))
        return math.pow(rate, shape) * math.pow(x, shape - 1.0) * math.exp(-rate * x) / _gamma(shape)

    @staticmethod
    def pdf_ln(shape: float, rate: float, x: float) -> float:
        if math.isinf(rate):
            return float("inf") if x == shape else float("-inf")
        if shape == 0.0 and rate == 0.0:
            return float("-inf")
        if shape == 1.0:
            return math.log(rate) - rate * x
        return shape * math.log(rate) - gamma_ln(shape) + (shape - 1.0) * math.log(x) - rate * x

    @staticmethod
    def inv_cdf(shape: float, rate: float, p: float) -> float:
        return gamma_lower_regularized_inv(shape, p) / rate


class FisherSnedecor:
    @staticmethod
    def cdf(d1: float, d2: float, x: float) -> float:
        if d1 <= 0.0 or d2 <= 0.0:
            return float("nan")
        return beta_regularized(d1 / 2.0, d2 / 2.0, d1 * x / (d1 * x + d2))

    @staticmethod
    def inv_cdf(d1: float, d2: float, p: float) -> float:
        def f(x: float) -> float:
            return beta_regularized(d1 / 2.0, d2 / 2.0, d1 * x / (d1 * x + d2)) - p

        return find_root(f, 0.0, 1000.0, 1e-12)


class Binomial:
    @staticmethod
    def pmf(p: float, n: int, k: int) -> float:
        from toolgood_algorithm.math_net.special_functions import binomial_ln

        if k < 0 or k > n:
            return 0.0
        if p == 0.0:
            return 1.0 if k == 0 else 0.0
        if p == 1.0:
            return 1.0 if k == n else 0.0
        return math.exp(binomial_ln(n, k) + k * math.log(p) + (n - k) * math.log(1.0 - p))

    @staticmethod
    def cdf(p: float, n: int, x: float) -> float:
        if x < 0.0:
            return 0.0
        if x > n:
            return 1.0
        k = math.floor(x)
        return beta_regularized(n - k, k + 1, 1 - p)


class Poisson:
    @staticmethod
    def pmf(lam: float, k: int) -> float:
        from toolgood_algorithm.math_net.special_functions import factorial_ln

        return math.exp(-lam + k * math.log(lam) - factorial_ln(k))

    @staticmethod
    def cdf(lam: float, x: float) -> float:
        return 1.0 - gamma_lower_regularized(x + 1, lam)


class Exponential:
    @staticmethod
    def pdf(rate: float, x: float) -> float:
        return 0.0 if x < 0.0 else rate * math.exp(-rate * x)

    @staticmethod
    def cdf(rate: float, x: float) -> float:
        return 0.0 if x < 0.0 else 1.0 - math.exp(-rate * x)


class Hypergeometric:
    @staticmethod
    def pmf(population: int, success: int, draws: int, k: int) -> float:
        from toolgood_algorithm.math_net.special_functions import binomial

        return binomial(success, k) * binomial(population - success, draws - k) / binomial(population, draws)


class NegativeBinomial:
    @staticmethod
    def pmf(r: float, p: float, k: int) -> float:
        return math.exp(NegativeBinomial.pmf_ln(r, p, k))

    @staticmethod
    def pmf_ln(r: float, p: float, k: int) -> float:
        return gamma_ln(r + k) - gamma_ln(r) - gamma_ln(k + 1.0) + r * math.log(p) + k * math.log(1.0 - p)


class LogNormal:
    @staticmethod
    def cdf(mu: float, sigma: float, x: float) -> float:
        if x < 0.0:
            return 0.0
        return 0.5 * (1.0 + erf((math.log(x) - mu) / (sigma * SQRT2)))

    @staticmethod
    def inv_cdf(mu: float, sigma: float, p: float) -> float:
        if p <= 0.0:
            return 0.0
        if p >= 1.0:
            return float("inf")
        return math.exp(mu - sigma * SQRT2 * erfc_inv(2.0 * p))


class Weibull:
    @staticmethod
    def pdf(shape: float, scale: float, x: float) -> float:
        if x < 0.0:
            return 0.0
        if x == 0.0 and shape == 1.0:
            return shape / scale
        return shape * math.pow(x / scale, shape - 1.0) * math.exp(-math.pow(x, shape) * math.pow(scale, -shape)) / scale

    @staticmethod
    def cdf(shape: float, scale: float, x: float) -> float:
        if x < 0.0:
            return 0.0
        return -math.expm1(-math.pow(x, shape) * math.pow(scale, -shape))
