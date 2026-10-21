"""Special functions (``MathNet/SpecialFunctions/``), one module per C# file.

============== ==========================================================
C# file        Python module
============== ==========================================================
Bessel.cs          bessel.py            bessel_j / bessel_y
Beta.cs            beta.py              gamma_ln / beta_regularized
Evaluate.cs        evaluate.py          chebyshev_a
Gamma.cs           gamma.py             gamma / gamma_lower_regularized
ModifiedBessel.cs  modified_bessel.py   bessel_i / bessel_k
SpecialFunctions.cs special_functions.py erf / erfc / erf_inv / erfc_inv
============== ==========================================================

``bessel_coefficients.py`` holds the Chebyshev coefficient tables extracted from
``SpecialFunctions.cs``; ``factorial_ln`` / ``binomial`` / ``binomial_ln`` live in
:mod:`toolgood_algorithm.math_net.statistics.factorial` (``Statistics/Factorial.cs``)
and are re-exported here for convenience.
"""
from __future__ import annotations

from toolgood_algorithm.math_net.special_functions._common import EPS, FPMIN, SQRT2, SQRT2PI
from toolgood_algorithm.math_net.special_functions.bessel import bessel_j, bessel_y
from toolgood_algorithm.math_net.special_functions.beta import beta_regularized, gamma_ln
from toolgood_algorithm.math_net.special_functions.evaluate import chebyshev_a
from toolgood_algorithm.math_net.special_functions.gamma import (
    gamma,
    gamma_lower_regularized,
    gamma_lower_regularized_inv,
)
from toolgood_algorithm.math_net.special_functions.modified_bessel import bessel_i, bessel_k
from toolgood_algorithm.math_net.special_functions.special_functions import (
    erf,
    erfc,
    erfc_inv,
    erf_inv,
    exponential_minus_one,
)
from toolgood_algorithm.math_net.statistics.factorial import binomial, binomial_ln, factorial_ln

__all__ = [
    "SQRT2",
    "SQRT2PI",
    "EPS",
    "FPMIN",
    "chebyshev_a",
    "gamma_ln",
    "beta_regularized",
    "gamma",
    "gamma_lower_regularized",
    "gamma_lower_regularized_inv",
    "bessel_i",
    "bessel_k",
    "bessel_j",
    "bessel_y",
    "erf",
    "erfc",
    "erfc_inv",
    "erf_inv",
    "exponential_minus_one",
    "factorial_ln",
    "binomial",
    "binomial_ln",
]
