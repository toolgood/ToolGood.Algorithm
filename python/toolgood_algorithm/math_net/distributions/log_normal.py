"""LogNormal (C#: MathNet/Distributions/LogNormal.cs)."""
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
