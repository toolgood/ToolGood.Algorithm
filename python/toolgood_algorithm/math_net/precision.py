"""Decimal precision helpers (``MathNet/Precision.cs``)."""
from __future__ import annotations

from decimal import Decimal

DECIMAL_PRECISION = Decimal("0.0000000000000000000000000001")
POSITIVE_DECIMAL_PRECISION = 2 * DECIMAL_PRECISION
_DEFAULT_DECIMAL_ACCURACY = DECIMAL_PRECISION * 10


def almost_equal_norm_relative(a: Decimal, b: Decimal, diff: Decimal, maximum_error: Decimal) -> bool:
    if abs(a) < DECIMAL_PRECISION or abs(b) < DECIMAL_PRECISION:
        return abs(diff) < maximum_error
    if (a == 0 and abs(b) < maximum_error) or (b == 0 and abs(a) < maximum_error):
        return True
    return abs(diff) < maximum_error * max(abs(a), abs(b))


def almost_equal_relative(a: Decimal, b: Decimal) -> bool:
    return almost_equal_norm_relative(a, b, a - b, _DEFAULT_DECIMAL_ACCURACY)


def almost_equal(a: Decimal, b: Decimal) -> bool:
    return almost_equal_norm(a, b, a - b, _DEFAULT_DECIMAL_ACCURACY)


def almost_equal_norm(a: Decimal, b: Decimal, diff: Decimal, maximum_absolute_error: Decimal) -> bool:
    return abs(diff) < maximum_absolute_error
