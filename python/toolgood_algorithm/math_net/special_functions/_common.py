"""Shared numeric constants of the special functions."""
from __future__ import annotations

import math

SQRT2 = math.sqrt(2.0)
SQRT2PI = math.sqrt(2.0 * math.pi)

#: convergence tolerance of the continued fractions / series
_EPS = 1e-16
#: smallest representable divisor (guards the Lentz algorithm)
_FPMIN = 1e-300

EPS = _EPS
FPMIN = _FPMIN

__all__ = ["SQRT2", "SQRT2PI", "EPS", "FPMIN", "_EPS", "_FPMIN"]
