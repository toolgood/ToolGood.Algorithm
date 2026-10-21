"""math_sum2: shared helpers and base classes (kept out of the per-class files)."""
from __future__ import annotations
import math


def _is_nan_or_inf(value: float) -> bool:
    return value != value or value in (float("inf"), float("-inf"))


def _expon_dist(x: float, rate: float, cumulative: bool) -> float:
    if cumulative:
        return 0.0 if x < 0.0 else 1.0 - math.exp(-rate * x)
    return 0.0 if x < 0.0 else rate * math.exp(-rate * x)
