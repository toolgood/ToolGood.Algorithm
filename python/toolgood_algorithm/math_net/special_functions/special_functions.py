"""``SpecialFunctions`` - error functions (MathNet/SpecialFunctions/SpecialFunctions.cs)."""
from __future__ import annotations

import math

from toolgood_algorithm.math_net.special_functions._common import SQRT2


def _norm_inv(p: float) -> float:
    """Acklam's inverse normal CDF approximation (|rel err| < 1.15e-9)."""
    if p <= 0.0:
        return float("-inf")
    if p >= 1.0:
        return float("inf")
    a = (-3.969683028665376e01, 2.209460984245205e02, -2.759285104469687e02, 1.383577518672690e02, -3.066479806614716e01, 2.506628277459239e00)
    b = (-5.447609879822406e01, 1.615858368580409e02, -1.556989798598866e02, 6.680131188771972e01, -1.328068155288572e01)
    c = (-7.784894002430293e-03, -3.223964580411365e-01, -2.400758277161838e00, -2.549732539343734e00, 4.374664141464968e00, 2.938163982698783e00)
    d = (7.784695709041462e-03, 3.224671290700398e-01, 2.445134137142996e00, 3.754408661907416e00)
    plow = 0.02425
    if p < plow:
        q = math.sqrt(-2 * math.log(p))
        return (((((c[0] * q + c[1]) * q + c[2]) * q + c[3]) * q + c[4]) * q + c[5]) / ((((d[0] * q + d[1]) * q + d[2]) * q + d[3]) * q + 1)
    if p > 1 - plow:
        q = math.sqrt(-2 * math.log(1 - p))
        return -(((((c[0] * q + c[1]) * q + c[2]) * q + c[3]) * q + c[4]) * q + c[5]) / ((((d[0] * q + d[1]) * q + d[2]) * q + d[3]) * q + 1)
    q = p - 0.5
    r = q * q
    return (((((a[0] * r + a[1]) * r + a[2]) * r + a[3]) * r + a[4]) * r + a[5]) * q / (((((b[0] * r + b[1]) * r + b[2]) * r + b[3]) * r + b[4]) * r + 1)


def erf(x: float) -> float:
    return math.erf(x)


def erfc(x: float) -> float:
    return math.erfc(x)


def erfc_inv(z: float) -> float:
    """Inverse complementary error function (Newton refinement of a rational guess)."""
    if z <= 0.0:
        return float("inf")
    if z >= 2.0:
        return float("-inf")
    # initial guess via the inverse normal distribution
    x = -_norm_inv(z / 2.0) / SQRT2
    for _ in range(8):
        err = erfc(x) - z
        derivative = -2.0 / math.sqrt(math.pi) * math.exp(-x * x)
        if derivative == 0.0:
            break
        step = err / derivative
        x -= step
        if abs(step) < 1e-15 * max(1.0, abs(x)):
            break
    return x


def erf_inv(z: float) -> float:
    return erfc_inv(1.0 - z)


def exponential_minus_one(power: float) -> float:
    return math.expm1(power)
