"""Binomial (C#: MathNet/Distributions/Binomial.cs)."""
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
