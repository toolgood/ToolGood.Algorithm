"""Exponential (C#: MathNet/Distributions/Exponential.cs)."""
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


class Exponential:
    @staticmethod
    def pdf(rate: float, x: float) -> float:
        return 0.0 if x < 0.0 else rate * math.exp(-rate * x)

    @staticmethod
    def cdf(rate: float, x: float) -> float:
        return 0.0 if x < 0.0 else 1.0 - math.exp(-rate * x)
