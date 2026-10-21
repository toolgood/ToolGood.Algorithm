"""Hypergeometric (C#: MathNet/Distributions/Hypergeometric.cs)."""
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


class Hypergeometric:
    @staticmethod
    def pmf(population: int, success: int, draws: int, k: int) -> float:
        from toolgood_algorithm.math_net.special_functions import binomial

        return binomial(success, k) * binomial(population - success, draws - k) / binomial(population, draws)
