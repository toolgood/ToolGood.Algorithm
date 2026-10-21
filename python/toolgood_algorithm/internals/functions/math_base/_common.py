"""math_base: shared helpers (the private base classes live next to this file).

* ``_math1.py``              -> :class:`_Math1`
* ``_aggregate_numbers.py``  -> :class:`_AggregateNumbers`
* ``from_float``             -> ``BigDecimal.valueOf(double)`` equivalent

The classes are re-exported here so every function module can keep importing
them from ``_common``.
"""
from __future__ import annotations

from decimal import Decimal

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.math_base._aggregate_numbers import _AggregateNumbers
from toolgood_algorithm.internals.functions.math_base._math1 import _Math1


def from_float(value: float) -> Decimal:
    """``BigDecimal.valueOf(double)`` equivalent."""
    if value != value or value in (float("inf"), float("-inf")):
        raise ValueError("Number is NaN or Infinity!")
    return Decimal(str(value))


__all__ = ["_Math1", "_AggregateNumbers", "from_float"]
