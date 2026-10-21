"""Special functions (erf, gamma, incomplete beta/gamma, Bessel).

The numeric core mirrors ``toolgood.algorithm.mathNet.SpecialFunctions``.  Where
the Python standard library offers an equivalent (``math.erf``, ``math.lgamma``,
``math.gamma``) it is used directly; the remaining functions are implemented
with the classic series / continued fraction algorithms of the same accuracy.
"""
from __future__ import annotations

import math
from typing import Callable

from toolgood_algorithm.math_net.special_functions.bessel_coefficients import BesselI0A, BesselI0B, BesselI1A, BesselI1B, BesselK0A, BesselK0B, BesselK1A, BesselK1B

SQRT2 = math.sqrt(2.0)
SQRT2PI = math.sqrt(2.0 * math.pi)
_EPS = 1e-16
_FPMIN = 1e-300


# ---------------------------------------------------------------------------
# elementary helpers
# ---------------------------------------------------------------------------
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


def gamma_ln(z: float) -> float:
    if z <= 0 and float(z).is_integer():
        return float("inf")
    return math.lgamma(z)


def gamma(z: float) -> float:
    return math.gamma(z)


def erf(x: float) -> float:
    return math.erf(x)


def erfc(x: float) -> float:
    return math.erfc(x)


def exponential_minus_one(power: float) -> float:
    return math.expm1(power)


# ---------------------------------------------------------------------------
# inverse error function
# ---------------------------------------------------------------------------
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


# ---------------------------------------------------------------------------
# incomplete beta / gamma
# ---------------------------------------------------------------------------
def _beta_continued_fraction(a: float, b: float, x: float) -> float:
    """Continued fraction for the incomplete beta function (Lentz's method)."""
    qab = a + b
    qap = a + 1.0
    qam = a - 1.0
    c = 1.0
    d = 1.0 - qab * x / qap
    if abs(d) < _FPMIN:
        d = _FPMIN
    d = 1.0 / d
    h = d
    for m in range(1, 400):
        m2 = 2 * m
        aa = m * (b - m) * x / ((qam + m2) * (a + m2))
        d = 1.0 + aa * d
        if abs(d) < _FPMIN:
            d = _FPMIN
        c = 1.0 + aa / c
        if abs(c) < _FPMIN:
            c = _FPMIN
        d = 1.0 / d
        h *= d * c
        aa = -(a + m) * (qab + m) * x / ((a + m2) * (qap + m2))
        d = 1.0 + aa * d
        if abs(d) < _FPMIN:
            d = _FPMIN
        c = 1.0 + aa / c
        if abs(c) < _FPMIN:
            c = _FPMIN
        d = 1.0 / d
        delta = d * c
        h *= delta
        if abs(delta - 1.0) < _EPS:
            break
    return h


def beta_regularized(a: float, b: float, x: float) -> float:
    """Regularized incomplete beta ``I_x(a, b)``."""
    if x <= 0.0:
        return 0.0
    if x >= 1.0:
        return 1.0
    if a <= 0.0 or b <= 0.0:
        return float("nan")
    ln_beta = math.lgamma(a + b) - math.lgamma(a) - math.lgamma(b)
    front = math.exp(ln_beta + a * math.log(x) + b * math.log1p(-x))
    if x < (a + 1.0) / (a + b + 2.0):
        return front * _beta_continued_fraction(a, b, x) / a
    return 1.0 - front * _beta_continued_fraction(b, a, 1.0 - x) / b


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


# ---------------------------------------------------------------------------
# Bessel functions
# ---------------------------------------------------------------------------
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


def _bessel_j0(x: float) -> float:
    ax = abs(x)
    if ax < 8.0:
        y1 = x * x
        ans1 = 57568490574.0 + y1 * (-13362590354.0 + y1 * (651619640.7 + y1 * (-11214424.18 + y1 * (77392.33017 + y1 * (-184.9052456)))))
        ans2 = 57568490411.0 + y1 * (1029532985.0 + y1 * (9494680.718 + y1 * (59272.64853 + y1 * (267.8532712 + y1 * 1.0))))
        return ans1 / ans2
    z = 8.0 / ax
    y2 = z * z
    xx = ax - 0.78539816339744830962
    ans3 = 1.0 + y2 * (-0.1098628627e-2 + y2 * (0.2734510407e-4 + y2 * (-0.2073370639e-5 + y2 * 0.2093887211e-6)))
    ans4 = -0.1562499995e-1 + y2 * (0.1430488765e-3 + y2 * (-0.6911147651e-5 + y2 * (0.7621095161e-6 - y2 * 0.934935152e-7)))
    return math.sqrt(0.63661977236758134308 / ax) * (math.cos(xx) * ans3 - z * math.sin(xx) * ans4)


def _bessel_j1(x: float) -> float:
    ax = abs(x)
    if ax < 8.0:
        y1 = x * x
        ans1 = x * (72362614232.0 + y1 * (-7895059235.0 + y1 * (242396853.1 + y1 * (-2972611.439 + y1 * (15704.48260 + y1 * (-30.16036606))))))
        ans2 = 144725228442.0 + y1 * (2300535178.0 + y1 * (18583304.74 + y1 * (99447.43394 + y1 * (376.9991397 + y1 * 1.0))))
        return ans1 / ans2
    z = 8.0 / ax
    y2 = z * z
    xx = ax - 2.35619449019234492885
    ans3 = 1.0 + y2 * (0.183105e-2 + y2 * (-0.3516396496e-4 + y2 * (0.2457520174e-5 + y2 * (-0.240337019e-6))))
    ans4 = 0.04687499995 + y2 * (-0.2002690873e-3 + y2 * (0.8449199096e-5 + y2 * (-0.88228987e-6 + y2 * 0.105787412e-6)))
    ans = math.sqrt(0.63661977236758134308 / ax) * (math.cos(xx) * ans3 - z * math.sin(xx) * ans4)
    return -ans if x < 0 else ans


def _bessel_y0(x: float) -> float:
    if x < 8.0:
        y1 = x * x
        ans1 = -2957821389.0 + y1 * (7062834065.0 + y1 * (-512359803.6 + y1 * (10879881.29 + y1 * (-86327.92757 + y1 * 228.4622733))))
        ans2 = 40076544269.0 + y1 * (745249964.8 + y1 * (7189466.438 + y1 * (47447.26470 + y1 * (226.1030244 + y1 * 1.0))))
        return (ans1 / ans2) + 0.63661977236758134308 * _bessel_j0(x) * math.log(x)
    z = 8.0 / x
    y2 = z * z
    xx = x - 0.78539816339744830962
    ans3 = 1.0 + y2 * (-0.1098628627e-2 + y2 * (0.2734510407e-4 + y2 * (-0.2073370639e-5 + y2 * 0.2093887211e-6)))
    ans4 = -0.1562499995e-1 + y2 * (0.1430488765e-3 + y2 * (-0.6911147651e-5 + y2 * (0.7621095161e-6 - y2 * 0.934935152e-7)))
    return math.sqrt(0.63661977236758134308 / x) * (math.sin(xx) * ans3 + z * math.cos(xx) * ans4)


def _bessel_y1(x: float) -> float:
    if x < 8.0:
        y1 = x * x
        ans1 = x * (-0.4900604943e13 + y1 * (0.1275274390e13 + y1 * (-0.5153438139e11 + y1 * (0.7349264551e9 + y1 * (-0.4237922726e7 + y1 * 0.8511937935e4)))))
        ans2 = 0.2499580570e14 + y1 * (0.4244419664e12 + y1 * (0.3733650367e10 + y1 * (0.2245904002e8 + y1 * (0.1020426050e6 + y1 * (0.3549632885e3 + y1)))))
        return (ans1 / ans2) + 0.63661977236758134308 * (_bessel_j1(x) * math.log(x) - 1.0 / x)
    z = 8.0 / x
    y2 = z * z
    xx = x - 2.35619449019234492885
    ans3 = 1.0 + y2 * (0.183105e-2 + y2 * (-0.3516396496e-4 + y2 * (0.2457520174e-5 + y2 * (-0.240337019e-6))))
    ans4 = 0.04687499995 + y2 * (-0.2002690873e-3 + y2 * (0.8449199096e-5 + y2 * (-0.88228987e-6 + y2 * 0.105787412e-6)))
    return math.sqrt(0.63661977236758134308 / x) * (math.sin(xx) * ans3 + z * math.cos(xx) * ans4)


def bessel_j(n: int, x: float) -> float:
    if x == 0:
        return 1.0 if n == 0 else 0.0
    if n < 0:
        n = -n
    ax = abs(x)
    if ax < 1e-10:
        return 1.0 if n == 0 else 0.0
    if n == 0:
        return _bessel_j0(x)
    if n == 1:
        return _bessel_j1(x)
    if ax > n:
        j0 = _bessel_j0(x)
        j1 = _bessel_j1(x)
        result = 0.0
        for k in range(1, n):
            result = (2.0 * k / x) * j1 - j0
            j0, j1 = j1, result
        return j1
    m = int(1.5 * n + 10)
    values = [0.0] * (m + 2)
    values[m] = 1.0
    for k in range(m, 0, -1):
        values[k - 1] = (2.0 * k / x) * values[k] - values[k + 1]
    total = 0.0
    for k in range(0, m + 1, 2):
        total += 2.0 * values[k]
    total -= values[0]
    return values[n] / total


def bessel_y(n: int, x: float) -> float:
    if n < 0:
        n = -n
    if n == 0:
        return _bessel_y0(x)
    if n == 1:
        return _bessel_y1(x)
    y0 = _bessel_y0(x)
    y1 = _bessel_y1(x)
    result = 0.0
    for k in range(1, n):
        result = (2.0 * k / x) * y1 - y0
        y0, y1 = y1, result
    return y1
