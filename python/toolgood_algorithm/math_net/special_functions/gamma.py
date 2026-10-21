"""``Gamma`` - the gamma function and the incomplete gamma (MathNet/SpecialFunctions/Gamma.cs)."""
from __future__ import annotations

import math

from toolgood_algorithm.math_net.special_functions._common import _EPS, _FPMIN


def gamma(z: float) -> float:
    return math.gamma(z)


def gamma_lower_regularized(a: float, x: float) -> float:
    """Regularized lower incomplete gamma ``P(a, x)``."""
    if x <= 0.0:
        return 0.0
    if a <= 0.0:
        return float("nan")
    ln_prefix = -x + a * math.log(x) - math.lgamma(a)
    if x < a + 1.0:
        # series expansion
        ap = a
        total = 1.0 / a
        delta = total
        for _ in range(1000):
            ap += 1.0
            delta *= x / ap
            total += delta
            if abs(delta) < abs(total) * _EPS:
                break
        return total * math.exp(ln_prefix)
    # continued fraction for Q(a, x)
    b = x + 1.0 - a
    c = 1.0 / _FPMIN
    d = 1.0 / b
    h = d
    for i in range(1, 1000):
        an = -i * (i - a)
        b += 2.0
        d = an * d + b
        if abs(d) < _FPMIN:
            d = _FPMIN
        c = b + an / c
        if abs(c) < _FPMIN:
            c = _FPMIN
        d = 1.0 / d
        delta = d * c
        h *= delta
        if abs(delta - 1.0) < _EPS:
            break
    q = math.exp(ln_prefix) * h
    return 1.0 - q


def gamma_lower_regularized_inv(a: float, y0: float) -> float:
    """Inverse of the regularized lower incomplete gamma (bisection + Newton)."""
    if y0 <= 0.0:
        return 0.0
    if y0 >= 1.0:
        return float("inf")
    # bracket the root using the Wilson-Hilferty approximation as a start
    lo, hi = 0.0, max(1.0, a)
    while gamma_lower_regularized(a, hi) < y0:
        hi *= 2.0
        if hi > 1e300:
            break
    for _ in range(200):
        mid = 0.5 * (lo + hi)
        value = gamma_lower_regularized(a, mid)
        if value < y0:
            lo = mid
        else:
            hi = mid
        if hi - lo <= 1e-14 * max(1.0, hi):
            break
    return 0.5 * (lo + hi)
