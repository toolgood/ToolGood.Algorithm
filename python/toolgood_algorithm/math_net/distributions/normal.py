"""Normal (C#: MathNet/Distributions/Normal.cs)."""
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
