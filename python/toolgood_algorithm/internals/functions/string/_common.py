"""string: shared helpers and base classes (kept out of the per-class files)."""
from __future__ import annotations
import re
from decimal import ROUND_HALF_EVEN, Decimal, localcontext
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_1 import Function_1


class _Text1(Function_1):
    """One text parameter, TEXT result."""

    def __init__(self, funcs):
        if not isinstance(funcs, (list, tuple)):
            funcs = [funcs]
        _require_args(funcs, self.name(), 1, 1)
        super().__init__(funcs)

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)

_QUOTES = ("'", '"')

_TRIM_CHARS = "".join(chr(i) for i in range(33))

_SPACES = re.compile(" +")

_BIG_DECIMAL = re.compile(r"^[+-]?(\d+(\.\d*)?|\.\d+)([eE][+-]?\d+)?$")


def _upper_char(c: str) -> str:
    """``Character.toUpperCase(char)`` - never expands to several chars."""
    u = c.upper()
    return u if len(u) == 1 else c


def _lower_char(c: str) -> str:
    """``Character.toLowerCase(char)`` - never expands to several chars."""
    lo = c.lower()
    return lo if len(lo) == 1 else c


def _is_letter(c: str) -> bool:
    return c.isalpha()


def _to_dbc(text: str) -> str:
    """Full width -> half width (``Function_ASC``)."""
    if not any(ord(c) == 12288 or 65280 < ord(c) < 65375 for c in text):
        return text
    out: list[str] = []
    for c in text:
        o = ord(c)
        if o == 12288:
            out.append(" ")
        elif 65280 < o < 65375:
            out.append(chr(o - 65248))
        else:
            out.append(c)
    return "".join(out)


def _to_sbc(text: str) -> str:
    """Half width -> full width (``Function_JIS``)."""
    if not any(c == " " or 32 < ord(c) < 127 for c in text):
        return text
    out: list[str] = []
    for c in text:
        o = ord(c)
        if c == " ":
            out.append(chr(12288))
        elif 32 < o < 127:
            out.append(chr(o + 65248))
        else:
            out.append(c)
    return "".join(out)


def _java_trim(text: str) -> str:
    return text.strip(_TRIM_CHARS)


def _skip_quoted(pattern: str, i: int) -> int:
    quote = pattern[i]
    i += 1
    n = len(pattern)
    while i < n:
        if pattern[i] == quote:
            if i + 1 < n and pattern[i + 1] == quote:
                i += 2
                continue
            return i + 1
        i += 1
    raise ValueError(f"invalid pattern: {pattern}")


def _split_sub_patterns(pattern: str) -> tuple[str, str | None]:
    """Split ``positive[;negative]`` honouring quotes and escapes."""
    parts: list[str] = []
    buf: list[str] = []
    i, n = 0, len(pattern)
    while i < n:
        c = pattern[i]
        if c in _QUOTES:
            end = _skip_quoted(pattern, i)
            buf.append(pattern[i:end])
            i = end
            continue
        if c == "\\" and i + 1 < n:
            buf.append(pattern[i:i + 2])
            i += 2
            continue
        if c == ";":
            parts.append("".join(buf))
            buf = []
            i += 1
            continue
        buf.append(c)
        i += 1
    parts.append("".join(buf))
    if len(parts) > 2:
        raise ValueError(f"invalid pattern: {pattern}")
    return parts[0], (parts[1] if len(parts) == 2 else None)


def _unquoted(pattern: str) -> str:
    out: list[str] = []
    i, n = 0, len(pattern)
    while i < n:
        c = pattern[i]
        if c in _QUOTES:
            i = _skip_quoted(pattern, i)
            continue
        if c == "\\" and i + 1 < n:
            out.append(pattern[i + 1])
            i += 2
            continue
        out.append(c)
        i += 1
    return "".join(out)


def _literal_text(segment: str) -> str:
    """Turn quoted / escaped pattern text into its literal value."""
    out: list[str] = []
    i, n = 0, len(segment)
    while i < n:
        c = segment[i]
        if c in _QUOTES:
            quote = c
            i += 1
            closed = False
            while i < n:
                if segment[i] == quote:
                    if i + 1 < n and segment[i + 1] == quote:
                        out.append(quote)
                        i += 2
                        continue
                    i += 1
                    closed = True
                    break
                out.append(segment[i])
                i += 1
            if not closed:
                raise ValueError(f"invalid pattern: {segment}")
            continue
        if c == "\\" and i + 1 < n:
            out.append(segment[i + 1])
            i += 2
            continue
        out.append(c)
        i += 1
    return "".join(out)


def _split_number_part(sub: str) -> tuple[str, str, str]:
    """Return ``(prefix, number, suffix)`` for one sub-pattern."""
    i, n = 0, len(sub)
    start = -1
    while i < n:
        c = sub[i]
        if c in _QUOTES:
            i = _skip_quoted(sub, i)
            continue
        if c == "\\" and i + 1 < n:
            i += 2
            continue
        if c in ("0", "#"):
            start = i
            break
        i += 1
    if start < 0:
        raise ValueError(f"invalid pattern: {sub}")
    j = start
    while j < n and sub[j] in ("0", "#", ",", "."):
        j += 1
    return _literal_text(sub[:start]), sub[start:j], _literal_text(sub[j:])


def _format_number(value: Decimal, pattern: str) -> str:
    """``java.text.DecimalFormat`` (Locale.US) subset.

    Supports ``0`` / ``#`` digit masks, grouping, ``.``, ``%`` / ``‰``,
    literal text in quotes or after a backslash and an optional negative
    sub-pattern.  Raises :class:`ValueError` for an illegal pattern.
    """
    if not isinstance(pattern, str) or not pattern:
        raise ValueError("invalid pattern")
    positive, negative = _split_sub_patterns(pattern)
    is_negative = value < 0
    sub = negative if (is_negative and negative is not None) else positive

    raw_prefix, number, raw_suffix = _split_number_part(sub)
    scale_text = _unquoted(sub)
    multiplier = 1
    if "%" in scale_text:
        multiplier = 100
    elif "‰" in scale_text:
        multiplier = 1000

    int_mask, dot, frac_mask = number.partition(".")
    if "." in frac_mask:
        raise ValueError(f"invalid pattern: {pattern}")
    if not int_mask and not frac_mask:
        raise ValueError(f"invalid pattern: {pattern}")
    if "," in frac_mask:
        raise ValueError(f"invalid pattern: {pattern}")

    grouping = "," in int_mask
    group_size = 0
    if grouping:
        group_size = len(int_mask) - int_mask.rindex(",") - 1
        if group_size <= 0:
            raise ValueError(f"invalid pattern: {pattern}")

    min_int = int_mask.count("0")
    min_frac = frac_mask.count("0")
    max_frac = min_frac + frac_mask.count("#")

    with localcontext() as ctx:
        ctx.prec = 60
        number_value = value.copy_abs() if is_negative else value
        if multiplier != 1:
            number_value = number_value * multiplier
        quantum = Decimal(1).scaleb(-max_frac)
        number_value = number_value.quantize(quantum, rounding=ROUND_HALF_EVEN)
        text = format(number_value, "f")

    int_digits, _, frac_digits = text.partition(".")
    if max_frac == 0:
        frac_digits = ""
    else:
        frac_digits = frac_digits.rstrip("0")
        if len(frac_digits) < min_frac:
            frac_digits = frac_digits.ljust(min_frac, "0")
    if len(frac_digits) > max_frac:  # pragma: no cover - defensive
        frac_digits = frac_digits[:max_frac]

    if int_digits == "0" and min_int == 0:
        int_digits = "" if frac_digits else "0"
    else:
        int_digits = int_digits.rjust(min_int, "0")

    if grouping and int_digits:
        chunks: list[str] = []
        for end in range(len(int_digits), 0, -group_size):
            chunks.append(int_digits[max(0, end - group_size):end])
        int_digits = ",".join(reversed(chunks))

    if int_digits and frac_digits:
        body = int_digits + "." + frac_digits
    elif frac_digits:
        body = "." + frac_digits
    else:
        body = int_digits
    sign = "-" if (is_negative and negative is None) else ""
    return sign + raw_prefix + body + raw_suffix
