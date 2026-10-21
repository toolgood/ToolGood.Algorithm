"""``Factorial`` - log factorial and binomial coefficients (MathNet/Statistics/Factorial.cs)."""
from __future__ import annotations

import math


def factorial_ln(x: int) -> float:
    return math.lgamma(x + 1.0)


def binomial(n: int, k: int) -> float:
    if n < 0 or k < 0 or k > n:
        return 0.0
    return float(math.comb(n, k))


def binomial_ln(n: int, k: int) -> float:
    if k < 0 or k > n:
        return float("-inf")
    return factorial_ln(n) - factorial_ln(k) - factorial_ln(n - k)
