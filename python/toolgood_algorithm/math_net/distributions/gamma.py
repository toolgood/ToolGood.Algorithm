"""Gamma (C#: MathNet/Distributions/Gamma.cs)."""
from __future__ import annotations

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
