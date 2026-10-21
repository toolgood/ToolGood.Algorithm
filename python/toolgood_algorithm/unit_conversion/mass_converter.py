"""Mass unit conversion (``UnitConversion/MassConverter.cs``)."""
from __future__ import annotations

from decimal import Decimal, ROUND_HALF_UP, localcontext

from ._converter import convert


def _ratio(numerator: str, denominator: str) -> Decimal:
    with localcontext() as ctx:
        ctx.prec = 30
        ctx.rounding = ROUND_HALF_UP
        return Decimal(numerator) / Decimal(denominator)


_UNITS = {
    "kg": Decimal(1),
    "kilogram": Decimal(1),
    "千克": Decimal(1),
    "gram": Decimal(1000),
    "g": Decimal(1000),
    "克": Decimal(1000),
    "ton": _ratio("1", "1000"),
    "t": _ratio("1", "1000"),
    "吨": _ratio("1", "1000"),
    "lb": _ratio("100000000", "45359237"),
    "lbs": _ratio("100000000", "45359237"),
    "pound": _ratio("100000000", "45359237"),
    "pounds": _ratio("100000000", "45359237"),
    "磅": _ratio("100000000", "45359237"),
    "英磅": _ratio("100000000", "45359237"),
    "st": _ratio("50000000", "317514659"),
    "stone": _ratio("50000000", "317514659"),
    "石": _ratio("50000000", "317514659"),
    "oz": _ratio("1600000000", "45359237"),
    "ounce": _ratio("1600000000", "45359237"),
    "盎司": _ratio("1600000000", "45359237"),
    "quintal": Decimal("0.01"),
    "英担": Decimal("0.01"),
    "short ton": _ratio("50000", "45359237"),
    "net ton": _ratio("50000", "45359237"),
    "us ton": _ratio("50000", "45359237"),
    "短吨": _ratio("50000", "45359237"),
    "美吨": _ratio("50000", "45359237"),
    "long ton": _ratio("312500", "317514659"),
    "weight ton": _ratio("312500", "317514659"),
    "gross ton": _ratio("312500", "317514659"),
    "imperial ton": _ratio("312500", "317514659"),
    "长吨": _ratio("312500", "317514659"),
    "英吨": _ratio("312500", "317514659"),
    "mg": Decimal(1000000),
    "毫克": Decimal(1000000),
    "斤": Decimal(2),
    "两": Decimal(20),
}


def try_convert(source_unit: str, target_unit: str, value: Decimal):
    return convert(_UNITS, source_unit, target_unit, value)


class MassConverter:
    UNITS = _UNITS

    @staticmethod
    def try_convert(source_unit: str, target_unit: str, value: Decimal):
        return try_convert(source_unit, target_unit, value)

    TryConvert = try_convert
