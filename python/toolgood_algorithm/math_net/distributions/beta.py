"""Beta (C#: MathNet/Distributions/Beta.cs)."""
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
