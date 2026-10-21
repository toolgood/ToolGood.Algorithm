"""``SortedArrayStatistics`` - statistics on sorted data (MathNet/Statistics/SortedArrayStatistics.cs).

C# declares ``QuantileRank`` here; the Python version still sorts defensively so
callers may pass unsorted data as well.
"""
from __future__ import annotations

import bisect
from typing import List


def quantile_rank(data: List[float], x: float) -> float:
    if not data:
        return float("nan")
    ordered = sorted(data)
    n = len(ordered)
    if x < ordered[0]:
        return 0.0
    if x >= ordered[-1]:
        return 1.0
    index = bisect.bisect_left(ordered, x)
    if index < n and ordered[index] == x:
        left = index
        right = index
        while left > 0 and ordered[left - 1] == ordered[left]:
            left -= 1
        while right < n - 1 and ordered[right + 1] == ordered[right]:
            right += 1
        return left / (n - 1)
    right_index = index
    left_index = right_index - 1
    a = left_index / (n - 1)
    b = right_index / (n - 1)
    return ((ordered[right_index] - x) * a + (x - ordered[left_index]) * b) / (ordered[right_index] - ordered[left_index])
