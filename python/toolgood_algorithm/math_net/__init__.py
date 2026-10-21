"""MathNet compatible numeric helpers (``MathNet/`` in the C# tree).

Layout::

    MathNet/
      Constants.cs            -> constants.py
      ExcelFunctions.cs       -> excel_functions.py
      Precision.cs            -> precision.py
      Distributions/*.cs      -> distributions/
      RootFinding/Brent.cs    -> root_finding/
      SpecialFunctions/*.cs   -> special_functions/
      Statistics/*.cs         -> statistics/
"""
from __future__ import annotations

from . import distributions, excel_functions, special_functions, statistics
from .constants import SQRT2, SQRT2PI
from .excel_functions import ExcelFunctions
from .root_finding import find_root
from .special_functions import (
    bessel_i,
    bessel_j,
    bessel_k,
    bessel_y,
    beta_regularized,
    binomial,
    binomial_ln,
    erf,
    erfc,
    erfc_inv,
    erf_inv,
    factorial_ln,
    gamma,
    gamma_ln,
    gamma_lower_regularized,
    gamma_lower_regularized_inv,
)

__all__ = [
    "ExcelFunctions",
    "SpecialFunctions",
    "SQRT2",
    "SQRT2PI",
    "distributions",
    "excel_functions",
    "special_functions",
    "statistics",
    "find_root",
]


class SpecialFunctions:
    """CamelCase facade mirroring the C# / Java ``SpecialFunctions`` class."""

    BesselI = staticmethod(bessel_i)
    BesselJ = staticmethod(bessel_j)
    BesselK = staticmethod(bessel_k)
    BesselY = staticmethod(bessel_y)
    BetaRegularized = staticmethod(beta_regularized)
    Binomial = staticmethod(binomial)
    BinomialLn = staticmethod(binomial_ln)
    Erf = staticmethod(erf)
    Erfc = staticmethod(erfc)
    ErfcInv = staticmethod(erfc_inv)
    ErfInv = staticmethod(erf_inv)
    FactorialLn = staticmethod(factorial_ln)
    Gamma = staticmethod(gamma)
    GammaLn = staticmethod(gamma_ln)
    GammaLowerRegularized = staticmethod(gamma_lower_regularized)
    GammaLowerRegularizedInv = staticmethod(gamma_lower_regularized_inv)
