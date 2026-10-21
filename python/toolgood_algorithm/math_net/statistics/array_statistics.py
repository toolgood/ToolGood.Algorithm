"""``ArrayStatistics`` - statistics on unsorted data (MathNet/Statistics/ArrayStatistics.cs).

Mirrors ``ArrayStatistics.Minimum`` / ``Maximum`` / ``QuantileCustomInplace``.  The
Python version does not mutate the caller's list, so ``quantile_custom`` sorts a
copy instead of performing an in-place quick select.
"""
from __future__ import annotations

from typing import List


def minimum(data: List[float]) -> float:
    return min(data)


def maximum(data: List[float]) -> float:
    return max(data)


def _select(sorted_data: List[float], rank: int) -> float:
    """``SelectInplace`` applied to an already ordered sequence."""
    if not sorted_data:
        return float("nan")
    if rank <= 0:
        return sorted_data[0]
    if rank >= len(sorted_data) - 1:
        return sorted_data[-1]
    return sorted_data[rank]


def quantile_custom(data: List[float], tau: float, definition: str = "R7") -> float:
    """R-7 (Excel ``PERCENTILE``) quantile, plus the other R definitions."""
    if tau < 0.0 or tau > 1.0 or not data:
        return float("nan")
    ordered = sorted(data)
    n = len(ordered)
    if tau == 0.0 or n == 1:
        return ordered[0]
    if tau == 1.0:
        return ordered[-1]

    if definition == "R1":
        h = n * tau + 0.5
        return ordered[int(-(-((h - 0.5))) // 1) - 1]
    if definition == "R2":
        h = n * tau + 0.5
        lower = ordered[int(-(-((h - 0.5))) // 1) - 1]
        upper = ordered[int(h + 0.5) - 1]
        return (lower + upper) * 0.5
    if definition == "R3":
        h = n * tau
        return ordered[round(h) - 1]
    if definition == "R4":
        h = n * tau
        hf = int(h)
        return ordered[hf - 1] + (h - hf) * (ordered[hf] - ordered[hf - 1])
    if definition == "R5":
        h = n * tau + 0.5
        hf = int(h)
        return ordered[hf - 1] + (h - hf) * (ordered[hf] - ordered[hf - 1])
    if definition == "R6":
        h = (n + 1) * tau
        hf = int(h)
        return ordered[hf - 1] + (h - hf) * (ordered[hf] - ordered[hf - 1])
    if definition == "R8":
        h = (n + 1 / 3.0) * tau + 1 / 3.0
        hf = int(h)
        return ordered[hf - 1] + (h - hf) * (ordered[hf] - ordered[hf - 1])
    if definition == "R9":
        h = (n + 0.25) * tau + 0.375
        hf = int(h)
        return ordered[hf - 1] + (h - hf) * (ordered[hf] - ordered[hf - 1])

    # R7 - the Excel default
    h = (n - 1) * tau + 1.0
    hf = int(h)
    lower = ordered[hf - 1]
    upper = ordered[hf] if hf < n else ordered[-1]
    return lower + (h - hf) * (upper - lower)


def mean(data: List[float]) -> float:
    return sum(data) / len(data)


def variance(data: List[float]) -> float:
    m = mean(data)
    return sum((x - m) ** 2 for x in data) / (len(data) - 1) if len(data) > 1 else 0.0
