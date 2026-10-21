"""Distance unit conversion (``UnitConversion/DistanceConverter.cs``)."""
from __future__ import annotations

from decimal import Decimal, ROUND_HALF_UP, localcontext

from ._converter import convert


def _ratio(numerator: str, denominator: str) -> Decimal:
    with localcontext() as ctx:
        ctx.prec = 30
        ctx.rounding = ROUND_HALF_UP
        return Decimal(numerator) / Decimal(denominator)


UNITS = {
    "m": Decimal(1),
    "metre": Decimal(1),
    "meter": Decimal(1),
    "米": Decimal(1),
    "km": Decimal("0.001"),
    "kilometre": Decimal("0.001"),
    "kilometer": Decimal("0.001"),
    "千米": Decimal("0.001"),
    "dm": Decimal(10),
    "decimetre": Decimal(10),
    "decimeter": Decimal(10),
    "分米": Decimal(10),
    "cm": Decimal(100),
    "centimetre": Decimal(100),
    "centimeter": Decimal(100),
    "厘米": Decimal(100),
    "mm": Decimal(1000),
    "millimetre": Decimal(1000),
    "millimeter": Decimal(1000),
    "毫米": Decimal(1000),
    "ft": _ratio("1250", "381"),
    "foot": _ratio("1250", "381"),
    "feet": _ratio("1250", "381"),
    "英尺": _ratio("1250", "381"),
    "yd": _ratio("1250", "1143"),
    "yard": _ratio("1250", "1143"),
    "码": _ratio("1250", "1143"),
    "mile": _ratio("125", "201168"),
    "英里": _ratio("125", "201168"),
    "in": _ratio("5000", "127"),
    "inch": _ratio("5000", "127"),
    "英寸": _ratio("5000", "127"),
    "au": _ratio("1", "149600000000"),
    "尺": Decimal(3),
    "寸": Decimal(30),
}


def try_convert(source_unit: str, target_unit: str, value: Decimal):
    return convert(UNITS, source_unit, target_unit, value)


class DistanceConverter:
    UNITS = UNITS

    @staticmethod
    def try_convert(source_unit: str, target_unit: str, value: Decimal):
        return try_convert(source_unit, target_unit, value)

    TryConvert = try_convert
