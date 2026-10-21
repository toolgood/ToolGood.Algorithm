"""NegativeBinomial (C#: MathNet/Distributions/NegativeBinomial.cs)."""
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


class NegativeBinomial:
    @staticmethod
    def pmf(r: float, p: float, k: int) -> float:
        return math.exp(NegativeBinomial.pmf_ln(r, p, k))

    @staticmethod
    def pmf_ln(r: float, p: float, k: int) -> float:
        return gamma_ln(r + k) - gamma_ln(r) - gamma_ln(k + 1.0) + r * math.log(p) + k * math.log(1.0 - p)
