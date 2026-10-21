"""Weibull (C#: MathNet/Distributions/Weibull.cs)."""
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
