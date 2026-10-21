"""StudentT (C#: MathNet/Distributions/StudentT.cs)."""
from __future__ import annotations

from __future__ import annotations
import math
from typing import Callable, List
from toolgood_algorithm.math_net.root_finding import find_root
from toolgood_algorithm.math_net.distributions.normal import Normal
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
