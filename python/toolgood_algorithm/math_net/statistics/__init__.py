"""Statistics helpers (``MathNet/Statistics/``), one module per C# file.

===================== ========================= ==============================
C# file               Python module             Contents
===================== ========================= ==============================
ArrayStatistics.cs    array_statistics.py       minimum / maximum / quantile
SortedArrayStatistics.cs sorted_array_statistics.py quantile_rank
Factorial.cs          factorial.py              factorial_ln / binomial
===================== ========================= ==============================
"""
from __future__ import annotations

from toolgood_algorithm.math_net.statistics.array_statistics import (
    maximum,
    mean,
    minimum,
    quantile_custom,
    variance,
)
from toolgood_algorithm.math_net.statistics.factorial import binomial, binomial_ln, factorial_ln
from toolgood_algorithm.math_net.statistics.sorted_array_statistics import quantile_rank

__all__ = [
    "minimum",
    "maximum",
    "quantile_custom",
    "quantile_rank",
    "mean",
    "variance",
    "factorial_ln",
    "binomial",
    "binomial_ln",
]
