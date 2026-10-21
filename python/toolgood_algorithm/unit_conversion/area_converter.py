"""Area unit conversion (``UnitConversion/AreaConverter.cs``)."""
from __future__ import annotations

from decimal import Decimal, ROUND_HALF_UP, localcontext

from ._converter import convert


def _ratio(numerator: str, denominator: str) -> Decimal:
    with localcontext() as ctx:
        ctx.prec = 30
        ctx.rounding = ROUND_HALF_UP
        return Decimal(numerator) / Decimal(denominator)


def _square_of_inverse(factor: str) -> Decimal:
    with localcontext() as ctx:
        ctx.prec = 30
        ctx.rounding = ROUND_HALF_UP
        return (Decimal(1) / Decimal(factor)) / Decimal(factor)


_UNITS = {
    "m²": Decimal(1),
    "m2": Decimal(1),
    "square metre": Decimal(1),
    "square meter": Decimal(1),
    "centiare": Decimal(1),
    "平方米": Decimal(1),
    "平方公尺": Decimal(1),
    "km²": Decimal("0.000001"),
    "km2": Decimal("0.000001"),
    "square kilometre": Decimal("0.000001"),
    "square kilometer": Decimal("0.000001"),
    "平方千米": Decimal("0.000001"),
    "dm²": Decimal(100),
    "dm2": Decimal(100),
    "square decimetre": Decimal(100),
    "square decimeter": Decimal(100),
    "平方分米": Decimal(100),
    "cm²": Decimal(10000),
    "cm2": Decimal(10000),
    "square centimetre": Decimal(10000),
    "square centimeter": Decimal(10000),
    "平方厘米": Decimal(10000),
    "mm²": Decimal(1000000),
    "mm2": Decimal(1000000),
    "square millimetre": Decimal(1000000),
    "square millimeter": Decimal(1000000),
    "平方毫米": Decimal(1000000),
    "ft²": _square_of_inverse("0.3048"),
    "ft2": _square_of_inverse("0.3048"),
    "square foot": _square_of_inverse("0.3048"),
    "square feet": _square_of_inverse("0.3048"),
    "sq ft": _square_of_inverse("0.3048"),
    "平方英尺": _square_of_inverse("0.3048"),
    "yd²": _square_of_inverse("0.9144"),
    "yd2": _square_of_inverse("0.9144"),
    "sq yd": _square_of_inverse("0.9144"),
    "square yard": _square_of_inverse("0.9144"),
    "平方码": _square_of_inverse("0.9144"),
    "a": Decimal("0.01"),
    "are": Decimal("0.01"),
    "ha": Decimal("0.0001"),
    "hectare": Decimal("0.0001"),
    "公顷": Decimal("0.0001"),
    "in²": _ratio("1", "0.00064516"),
    "in2": _ratio("1", "0.00064516"),
    "sq in": _ratio("1", "0.00064516"),
    "square inch": _ratio("1", "0.00064516"),
    "平方英寸": _ratio("1", "0.00064516"),
    "mi²": _ratio("1", "2589988.110336"),
    "mi2": _ratio("1", "2589988.110336"),
    "sq mi": _ratio("1", "2589988.110336"),
    "square mile": _ratio("1", "2589988.110336"),
    "平方英里": _ratio("1", "2589988.110336"),
    "亩": _ratio("3", "2000"),
}


def try_convert(source_unit: str, target_unit: str, value: Decimal):
    return convert(_UNITS, source_unit, target_unit, value)


class AreaConverter:
    UNITS = _UNITS

    @staticmethod
    def try_convert(source_unit: str, target_unit: str, value: Decimal):
        return try_convert(source_unit, target_unit, value)

    TryConvert = try_convert
