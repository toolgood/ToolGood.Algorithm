"""Volume unit conversion (``UnitConversion/VolumeConverter.cs``)."""
from __future__ import annotations

from decimal import Decimal, ROUND_HALF_UP, localcontext

from ._converter import convert


def _ratio(numerator: str, denominator: str) -> Decimal:
    with localcontext() as ctx:
        ctx.prec = 30
        ctx.rounding = ROUND_HALF_UP
        return Decimal(numerator) / Decimal(denominator)


_UNITS = {
    "l": Decimal(1),
    "lt": Decimal(1),
    "ltr": Decimal(1),
    "liter": Decimal(1),
    "litre": Decimal(1),
    "dm³": Decimal(1),
    "dm3": Decimal(1),
    "cubic decimetre": Decimal(1),
    "cubic decimeter": Decimal(1),
    "升": Decimal(1),
    "立方分米": Decimal(1),
    "m³": Decimal("0.001"),
    "m3": Decimal("0.001"),
    "cubic metre": Decimal("0.001"),
    "cubic meter": Decimal("0.001"),
    "立方米": Decimal("0.001"),
    "km³": Decimal("0.000000000001"),
    "km3": Decimal("0.000000000001"),
    "cubic kilometre": Decimal("0.000000000001"),
    "cubic kilometer": Decimal("0.000000000001"),
    "立方千米": Decimal("0.000000000001"),
    "cm³": Decimal(1000),
    "cm3": Decimal(1000),
    "cubic centimetre": Decimal(1000),
    "cubic centimeter": Decimal(1000),
    "立方厘米": Decimal(1000),
    "毫升": Decimal(1000),
    "ml": Decimal(1000),
    "mm³": Decimal(1000000),
    "mm3": Decimal(1000000),
    "cubic millimetre": Decimal(1000000),
    "cubic millimeter": Decimal(1000000),
    "立方毫米": Decimal(1000000),
    "ft³": _ratio("1", "28.316846592"),
    "ft3": _ratio("1", "28.316846592"),
    "cubic foot": _ratio("1", "28.316846592"),
    "cubic feet": _ratio("1", "28.316846592"),
    "立方英尺": _ratio("1", "28.316846592"),
    "cu ft": _ratio("1", "28.316846592"),
    "in³": _ratio("125000000", "2048383"),
    "in3": _ratio("125000000", "2048383"),
    "cubic in": _ratio("125000000", "2048383"),
    "cubic inch": _ratio("125000000", "2048383"),
    "立方英寸": _ratio("125000000", "2048383"),
    "imperial pint": _ratio("800000", "454609"),
    "imperial pt": _ratio("800000", "454609"),
    "imperial p": _ratio("800000", "454609"),
    "imperial gallon": _ratio("100000", "454609"),
    "imperial gal": _ratio("100000", "454609"),
    "imperial quart": _ratio("400000", "454609"),
    "imperial qt": _ratio("400000", "454609"),
    "us pint": _ratio("1000000000", "473176473"),
    "us pt": _ratio("1000000000", "473176473"),
    "us p": _ratio("1000000000", "473176473"),
    "us gallon": _ratio("125000000", "473176473"),
    "us gal": _ratio("125000000", "473176473"),
    "us quart": _ratio("1000000000", "946352946"),
    "us qt": _ratio("1000000000", "946352946"),
}


def try_convert(source_unit: str, target_unit: str, value: Decimal):
    return convert(_UNITS, source_unit, target_unit, value)


class VolumeConverter:
    UNITS = _UNITS

    @staticmethod
    def try_convert(source_unit: str, target_unit: str, value: Decimal):
        return try_convert(source_unit, target_unit, value)

    TryConvert = try_convert
