"""Poisson (C#: MathNet/Distributions/Poisson.cs)."""
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


class Poisson:
    @staticmethod
    def pmf(lam: float, k: int) -> float:
        from toolgood_algorithm.math_net.special_functions import factorial_ln

        return math.exp(-lam + k * math.log(lam) - factorial_ln(k))

    @staticmethod
    def cdf(lam: float, x: float) -> float:
        return 1.0 - gamma_lower_regularized(x + 1, lam)
