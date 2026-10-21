"""Shared conversion helper for the ``UnitConversion`` converters."""
from __future__ import annotations

from decimal import Decimal, ROUND_HALF_UP, localcontext


def convert(units: dict, source_unit: str, target_unit: str, value: Decimal):
    """``value / units[source] * units[target]`` with 30 digit precision.

    Lookups are case insensitive, matching the ``TreeMap`` with
    ``CASE_INSENSITIVE_ORDER`` used by the original implementations.
    """
    if source_unit is None or target_unit is None:
        return None
    left = units.get(source_unit.lower())
    if left is None:
        return None
    right = units.get(target_unit.lower())
    if right is None:
        return None
    with localcontext() as ctx:
        ctx.prec = 30
        ctx.rounding = ROUND_HALF_UP
        return value / left * right
