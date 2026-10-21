"""FisherSnedecor (C#: MathNet/Distributions/FisherSnedecor.cs)."""
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
