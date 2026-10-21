"""``Evaluate`` - polynomial / Chebyshev series evaluation (MathNet/SpecialFunctions/Evaluate.cs)."""
from __future__ import annotations


def chebyshev_a(coefficients, x: float) -> float:
    """``Evaluate.ChebyshevA`` (coefficients are stored highest order first)."""
    p = 0
    b0 = coefficients[p]
    p += 1
    b1 = 0.0
    i = len(coefficients) - 1
    while True:
        b2 = b1
        b1 = b0
        b0 = x * b1 - b2 + coefficients[p]
        p += 1
        i -= 1
        if i <= 0:
            break
    return 0.5 * (b0 - b2)
