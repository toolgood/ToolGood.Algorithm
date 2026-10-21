"""Root finding used by the inverse distribution functions (``Brent`` port)."""
from __future__ import annotations

from typing import Callable


def find_root(f: Callable[[float], float], lower: float, upper: float, accuracy: float = 1e-12, max_iter: int = 300) -> float:
    """Bisection root finder with the same contract as ``Brent.FindRoot``.

    Returns a root of ``f`` inside ``[lower, upper]``; raises ``ValueError`` when
    the interval does not bracket a sign change.
    """
    f_low = f(lower)
    f_high = f(upper)
    if f_low == 0.0:
        return lower
    if f_high == 0.0:
        return upper
    if f_low * f_high > 0.0:
        raise ValueError("root is not bracketed")

    for _ in range(max_iter):
        mid = 0.5 * (lower + upper)
        f_mid = f(mid)
        if f_mid == 0.0 or (upper - lower) <= accuracy:
            return mid
        if (f_low < 0.0) != (f_mid < 0.0):
            upper = mid
            f_high = f_mid
        else:
            lower = mid
            f_low = f_mid
    return 0.5 * (lower + upper)
