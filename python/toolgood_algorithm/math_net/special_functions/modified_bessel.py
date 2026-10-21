"""``ModifiedBessel`` - modified Bessel functions (MathNet/SpecialFunctions/ModifiedBessel.cs)."""
from __future__ import annotations

import math

from toolgood_algorithm.math_net.special_functions.bessel_coefficients import BesselI0A, BesselI0B, BesselI1A, BesselI1B, BesselK0A, BesselK0B, BesselK1A, BesselK1B
from toolgood_algorithm.math_net.special_functions.evaluate import chebyshev_a


def _bessel_i0(x: float) -> float:
    x = abs(x)
    if x <= 8.0:
        y = (x / 2.0) - 2.0
        return math.exp(x) * chebyshev_a(BesselI0A, y)
    x1 = 32.0 / x - 2.0
    return math.exp(x) * chebyshev_a(BesselI0B, x1) / math.sqrt(x)


def _bessel_i1(x: float) -> float:
    z = abs(x)
    if z <= 8.0:
        y = (z / 2.0) - 2.0
        z = chebyshev_a(BesselI1A, y) * z * math.exp(z)
    else:
        x1 = 32.0 / z - 2.0
        z = math.exp(z) * chebyshev_a(BesselI1B, x1) / math.sqrt(z)
    return -z if x < 0.0 else z


def _bessel_k0(x: float) -> float:
    if x <= 2.0:
        y = x * x - 2.0
        return chebyshev_a(BesselK0A, y) - math.log(0.5 * x) * _bessel_i0(x)
    z = 8.0 / x - 2.0
    return math.exp(-x) * chebyshev_a(BesselK0B, z) / math.sqrt(x)


def _bessel_k1(x: float) -> float:
    z = 0.5 * x
    if x <= 2.0:
        y = x * x - 2.0
        return math.log(z) * _bessel_i1(x) + chebyshev_a(BesselK1A, y) / x
    x1 = 8.0 / x - 2.0
    return math.exp(-x) * chebyshev_a(BesselK1B, x1) / math.sqrt(x)


def bessel_i(n: int, x: float) -> float:
    if x < 0:
        return (1 if n % 2 == 0 else -1) * bessel_i(n, -x)
    if x == 0:
        return 1.0 if n == 0 else 0.0
    if abs(x) < 1e-10:
        return 1.0 if n == 0 else 0.0
    if n < 0:
        n = -n
    if abs(x) > 700:
        factor = math.exp(abs(x)) / math.sqrt(2 * math.pi * abs(x))
        return factor * (1.0 - (4.0 * n * n - 1.0) / (8.0 * abs(x)))
    if n == 0:
        return _bessel_i0(x)
    if n == 1:
        return _bessel_i1(x)
    i0 = _bessel_i0(x)
    i1 = _bessel_i1(x)
    result = 0.0
    for k in range(1, n):
        result = i0 - 2.0 * k / x * i1
        i0, i1 = i1, result
    return i1


def bessel_k(n: int, x: float) -> float:
    if n < 0:
        n = -n
    if n == 0:
        return _bessel_k0(x)
    if n == 1:
        return _bessel_k1(x)
    k0 = _bessel_k0(x)
    k1 = _bessel_k1(x)
    result = 0.0
    for k in range(1, n):
        result = k0 + 2.0 * k / x * k1
        k0, k1 = k1, result
    return k1
