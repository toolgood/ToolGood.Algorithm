"""Distributions (C#: MathNet/Distributions/). One class per module."""
from __future__ import annotations

from .normal import Normal
from .student_t import StudentT
from .beta import Beta
from .gamma import Gamma
from .fisher_snedecor import FisherSnedecor
from .binomial import Binomial
from .poisson import Poisson
from .exponential import Exponential
from .hypergeometric import Hypergeometric
from .negative_binomial import NegativeBinomial
from .log_normal import LogNormal
from .weibull import Weibull

__all__ = [
    "Normal",
    "StudentT",
    "Beta",
    "Gamma",
    "FisherSnedecor",
    "Binomial",
    "Poisson",
    "Exponential",
    "Hypergeometric",
    "NegativeBinomial",
    "LogNormal",
    "Weibull",
]
