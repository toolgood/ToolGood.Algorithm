"""``Bessel`` - Bessel functions of the first/second kind (MathNet/SpecialFunctions/Bessel.cs)."""
from __future__ import annotations

import math


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
