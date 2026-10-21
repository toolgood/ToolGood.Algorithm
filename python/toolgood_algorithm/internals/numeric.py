"""Numeric helpers.

The original library is built on ``decimal`` (C#) / ``BigDecimal`` (Java).
The Python port keeps :class:`decimal.Decimal` everywhere so that results stay
comparable with the original implementations, and mirrors the ``MathContext``
choices of the Java port (``DECIMAL128`` for divisions).
"""
from __future__ import annotations

from decimal import Context, Decimal, InvalidOperation, ROUND_DOWN, ROUND_HALF_UP, localcontext

DECIMAL128 = Context(prec=34)
DECIMAL32 = Context(prec=7)

ZERO = Decimal(0)
ONE = Decimal(1)

# C# decimal.MaxValue
DECIMAL_MAX = Decimal("79228162514264337593543950335")

# Excel / .NET epoch used by the original MyDate serial conversion.
EXCEL_EPOCH_DAYS = 2


def to_decimal(value) -> Decimal | None:
    """Best effort conversion used when parsing user supplied numbers."""
    if isinstance(value, Decimal):
        return value
    if isinstance(value, bool):
        return ONE if value else ZERO
    if isinstance(value, int):
        return Decimal(value)
    if isinstance(value, float):
        if value != value or value in (float("inf"), float("-inf")):
            return None
        return Decimal(str(value))
    if isinstance(value, str):
        try:
            return Decimal(value.strip())
        except (InvalidOperation, ValueError):
            return None
    return None


def divide(a: Decimal, b: Decimal, ctx: Context = DECIMAL128) -> Decimal:
    with localcontext(ctx):
        return a / b


def decimal_divide32(a: Decimal, b: Decimal) -> Decimal:
    with localcontext(DECIMAL32):
        return a / b


def trunc(value: Decimal) -> int:
    """Truncate towards zero (C# ``(int)`` cast / Java ``intValue()``)."""
    return int(value.to_integral_value(rounding=ROUND_DOWN))


def round_half_away(value: Decimal, digits: int = 0) -> Decimal:
    """``Math.Round(..., MidpointRounding.AwayFromZero)`` semantics."""
    if digits >= 0:
        quant = Decimal(1).scaleb(-digits)
    else:
        quant = Decimal(1).scaleb(-digits)
    with localcontext() as ctx:
        ctx.prec = max(28, len(value.as_tuple().digits) + abs(digits) + 4)
        return value.quantize(quant, rounding=ROUND_HALF_UP)


def is_nan_or_inf(value: float) -> bool:
    return value != value or value in (float("inf"), float("-inf"))


def plain(value: Decimal) -> str:
    """``BigDecimal.ToPlainString`` equivalent (keeps trailing zeros)."""
    return format(value, "f")


def plain_strip(value: Decimal) -> str:
    """Plain string with trailing zeros removed, as used by ``Operand.ToText``."""
    text = format(value, "f")
    if "." in text:
        text = text.rstrip("0").rstrip(".")
    return text or "0"
