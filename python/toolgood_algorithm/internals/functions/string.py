"""String functions (ASC, CHAR, LEFT, MID, TEXT, RMB, SEARCH, ...).

Ported one to one from
``toolgood.algorithm.internals.functions.string`` (Java).
"""
from __future__ import annotations

import re
from decimal import ROUND_HALF_EVEN, ROUND_HALF_UP, Decimal, localcontext

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import Function_1, Function_2, Function_3, Function_4, Function_N, _require_args
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

_QUOTES = ("'", '"')
# Java ``String.trim()`` removes every char <= ' ' (0x20)
_TRIM_CHARS = "".join(chr(i) for i in range(33))
_SPACES = re.compile(" +")
_BIG_DECIMAL = re.compile(r"^[+-]?(\d+(\.\d*)?|\.\d+)([eE][+-]?\d+)?$")


# ---------------------------------------------------------------------------
# character helpers
# ---------------------------------------------------------------------------
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


# ---------------------------------------------------------------------------
# DecimalFormat subset (Locale.US) used by Function_TEXT
# ---------------------------------------------------------------------------
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


# ---------------------------------------------------------------------------
# single text argument functions
# ---------------------------------------------------------------------------
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


@register("ASC")
class Function_ASC(_Text1):
    def name(self) -> str:
        return "Asc"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        return Operand.create(_to_dbc(args1.text_value()))


@register("JIS")
class Function_JIS(_Text1):
    def name(self) -> str:
        return "Jis"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        return Operand.create(_to_sbc(args1.text_value()))


@register("CLEAN")
class Function_CLEAN(_Text1):
    def name(self) -> str:
        return "Clean"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        text = args1.text_value()
        if not any(ord(c) < 32 for c in text):
            return args1
        return Operand.create("".join(c for c in text if ord(c) >= 32))


@register("LOWER")
class Function_LOWER(_Text1):
    def name(self) -> str:
        return "Lower"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        text = args1.text_value()
        for c in text:
            if c.isupper():
                return Operand.create(text.lower())
        return args1


@register("UPPER")
class Function_UPPER(_Text1):
    def name(self) -> str:
        return "Upper"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        text = args1.text_value()
        for c in text:
            if c.islower():
                return Operand.create(text.upper())
        return args1


@register("PROPER")
class Function_PROPER(_Text1):
    def name(self) -> str:
        return "Proper"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        text = args1.text_value()
        if not text:
            return Operand.create(text)

        need_modify = False
        is_first = True
        for c in text:
            if not _is_letter(c):
                is_first = True
            else:
                if is_first:
                    if c.islower():
                        need_modify = True
                else:
                    if c.isupper():
                        need_modify = True
                if need_modify:
                    break
                is_first = False

        if not need_modify:
            return args1

        chars: list[str] = []
        is_first = True
        for c in text:
            if not _is_letter(c):
                chars.append(c)
                is_first = True
            else:
                chars.append(_upper_char(c) if is_first else _lower_char(c))
                is_first = False
        return Operand.create("".join(chars))


@register("TRIM")
class Function_TRIM(_Text1):
    def name(self) -> str:
        return "Trim"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        text = _java_trim(args1.text_value())
        text = _SPACES.sub(" ", text)
        if text == args1.text_value():
            return args1
        return Operand.create(text)


@register("T")
class Function_T(_Text1):
    def name(self) -> str:
        return "T"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self.func1.evaluate(engine, tp)
        if args1.is_text():
            return args1
        return Operand.create("")

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NONE)


@register("CONCATENATE")
class Function_CONCATENATE(Function_N):
    def __init__(self, funcs):
        _require_args(funcs, "Concatenate", 1)
        super().__init__(funcs)

    def name(self) -> str:
        return "Concatenate"

    def evaluate(self, engine, tp) -> Operand:
        if not self.funcs:
            return Operand.create("")
        if len(self.funcs) == 1:
            a = self._get_text(engine, tp, 0)
            if a.is_error_or_none():
                return a
            return a
        sb: list[str] = []
        for i in range(len(self.funcs)):
            a = self._get_text(engine, tp, i)
            if a.is_error_or_none():
                return a
            sb.append(a.text_value())
        return Operand.create("".join(sb))

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        for func in self.funcs:
            func._get_parameter_types(none_engine, result, OperandType.TEXT)


@register("EXACT")
class Function_EXACT(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "Exact", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Exact"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_text_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        return Operand.create(args1.text_value() == args2.text_value())

    def get_result_type(self) -> OperandType:
        return OperandType.BOOLEAN

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        t1 = self.func1.get_result_type()
        t2 = self.func2.get_result_type()
        if t1 == OperandType.NONE:
            p = none_engine.evaluate(self.func2).to_text(None)
            if t2 != OperandType.ERROR and not p.is_error_or_none():
                self.func1._get_parameter_types(none_engine, result, OperandType.TEXT, "==", p.text_value())
                self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)
                return
        elif t2 == OperandType.NONE:
            p = none_engine.evaluate(self.func1).to_text(None)
            if t1 != OperandType.ERROR and not p.is_error_or_none():
                self.func2._get_parameter_types(none_engine, result, OperandType.TEXT, "==", p.text_value())
                self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
                return
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)


@register("FIND")
class Function_FIND(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "Find", 2, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "Find"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_text_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        if self.func3 is None:
            index = args2.text_value().find(args1.text_value())
            if index < 0:
                # not found: Excel mode (1 based) is an error, C# mode returns -1
                return self._function_error() if engine.ExcelIndex == 1 else Operand.create(-1)
            return Operand.create(index + engine.ExcelIndex)
        count = self._get_number_3(engine, tp)
        if count.is_error_or_none():
            return count
        excel_index = engine.ExcelIndex
        start_index = count.int_value() - excel_index
        if start_index < 0 or start_index >= len(args2.text_value()):
            return self._parameter_error(3)
        p2 = args2.text_value().find(args1.text_value(), start_index)
        if p2 < 0:
            return self._function_error() if engine.ExcelIndex == 1 else Operand.create(-1)
        return Operand.create(p2 + excel_index)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)
        if self.func3 is not None:
            self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("SEARCH")
class Function_SEARCH(Function_3):
    LITERAL = 0
    ANY_ONE = 1
    ANY_SEQ = 2

    def __init__(self, funcs):
        _require_args(funcs, "Search", 2, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "Search"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_text_2(engine, tp)
        if args2.is_error_or_none():
            return args2

        excel_index = engine.ExcelIndex

        if self.func3 is None:
            index = self._wildcard_index_of(args2.text_value(), args1.text_value(), 0)
            if index < 0:
                # not found: Excel mode (1 based) is an error, C# mode returns -1
                return self._function_error() if engine.ExcelIndex == 1 else Operand.create(-1)
            return Operand.create(index + excel_index)
        args3 = self._get_number_3(engine, tp)
        if args3.is_error_or_none():
            return args3
        start_index = args3.int_value() - excel_index
        if start_index < 0 or start_index >= len(args2.text_value()):
            return self._parameter_error(3)
        p2 = self._wildcard_index_of(args2.text_value(), args1.text_value(), start_index)
        if p2 < 0:
            return self._function_error() if engine.ExcelIndex == 1 else Operand.create(-1)
        return Operand.create(p2 + excel_index)

    # case insensitive wildcard search supporting Excel ``?`` / ``*`` / ``~``
    @classmethod
    def _wildcard_index_of(cls, text: str, pattern: str, start_index: int) -> int:
        if start_index < 0:
            start_index = 0
        if start_index > len(text):
            return -1
        tokens = cls._parse_wildcard(pattern)
        if not tokens:
            return start_index
        for start in range(start_index, len(text) + 1):
            if cls._match_wildcard(text, tokens, start):
                return start
        return -1

    @classmethod
    def _parse_wildcard(cls, pattern: str) -> list[tuple[int, str]]:
        tokens: list[tuple[int, str]] = []
        i = 0
        n = len(pattern)
        while i < n:
            c = pattern[i]
            if c == "~":
                if i + 1 < n and pattern[i + 1] in ("?", "*", "~"):
                    tokens.append((cls.LITERAL, _upper_char(pattern[i + 1])))
                    i += 1
                else:
                    tokens.append((cls.LITERAL, "~"))
            elif c == "*":
                tokens.append((cls.ANY_SEQ, "\0"))
            elif c == "?":
                tokens.append((cls.ANY_ONE, "\0"))
            else:
                tokens.append((cls.LITERAL, _upper_char(c)))
            i += 1
        return tokens

    @classmethod
    def _match_wildcard(cls, text: str, tokens: list[tuple[int, str]], start: int) -> bool:
        ti = 0
        si = start
        star_token = -1
        star_match = 0
        size = len(tokens)

        while ti < size:
            if si < len(text):
                kind, value = tokens[ti]
                if kind == cls.LITERAL:
                    if _upper_char(text[si]) == value:
                        ti += 1
                        si += 1
                    elif star_token != -1:
                        ti = star_token + 1
                        star_match += 1
                        si = star_match
                    else:
                        return False
                elif kind == cls.ANY_ONE:
                    ti += 1
                    si += 1
                else:
                    star_token = ti
                    star_match = si
                    ti += 1
            else:
                break

        while ti < size and tokens[ti][0] == cls.ANY_SEQ:
            ti += 1
        return ti == size

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)
        if self.func3 is not None:
            self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("SUBSTITUTE")
class Function_SUBSTITUTE(Function_4):
    def __init__(self, funcs):
        _require_args(funcs, "Substitute", 3, 4)
        super().__init__(funcs)

    def name(self) -> str:
        return "Substitute"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_text_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        args3 = self._get_text_3(engine, tp)
        if args3.is_error_or_none():
            return args3
        if self.func4 is None:
            if not args2.text_value():
                return Operand.create(args1.text_value())
            return Operand.create(args1.text_value().replace(args2.text_value(), args3.text_value()))

        args4 = self._get_number_4(engine, tp)
        if args4.is_error_or_none():
            return args4
        text = args1.text_value()
        oldtext = args2.text_value()
        newtext = args3.text_value()
        replace_index = args4.int_value()

        if not oldtext:
            return Operand.create(text)
        if replace_index < 1:
            return self._parameter_error(4)

        sb: list[str] = []
        current_index = 0
        found_count = 0
        search_pos = 0

        while search_pos <= len(text) - len(oldtext):
            found_pos = text.find(oldtext, search_pos)
            if found_pos < 0:
                break
            found_count += 1
            if found_count == replace_index:
                sb.append(text[current_index:found_pos])
                sb.append(newtext)
                current_index = found_pos + len(oldtext)
                break
            search_pos = found_pos + len(oldtext)

        if current_index == 0:
            return Operand.create(text)
        sb.append(text[current_index:])
        return Operand.create("".join(sb))

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)
        self.func3._get_parameter_types(none_engine, result, OperandType.TEXT)
        if self.func4 is not None:
            self.func4._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("CHAR")
class Function_CHAR(Function_1):
    def __init__(self, funcs):
        if not isinstance(funcs, (list, tuple)):
            funcs = [funcs]
        _require_args(funcs, "Char", 1, 1)
        super().__init__(funcs)

    def name(self) -> str:
        return "Char"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        code = args1.int_value()
        if code < 0 or code > 65535:
            return self._parameter_error(1)
        return Operand.create(chr(code))

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("CODE")
class Function_CODE(Function_1):
    def __init__(self, funcs):
        if not isinstance(funcs, (list, tuple)):
            funcs = [funcs]
        _require_args(funcs, "Code", 1, 1)
        super().__init__(funcs)

    def name(self) -> str:
        return "Code"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        text = args1.text_value()
        if not text:
            return self._parameter_error(1)
        return Operand.create(ord(text[0]))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)


@register("LEN")
class Function_LEN(Function_1):
    def __init__(self, funcs):
        if not isinstance(funcs, (list, tuple)):
            funcs = [funcs]
        _require_args(funcs, "Len", 1, 1)
        super().__init__(funcs)

    def name(self) -> str:
        return "Len"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        return Operand.create(len(args1.text_value()))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)


@register("UNICHAR")
class Function_UNICHAR(Function_1):
    def __init__(self, funcs):
        if not isinstance(funcs, (list, tuple)):
            funcs = [funcs]
        _require_args(funcs, "UniChar", 1, 1)
        super().__init__(funcs)

    def name(self) -> str:
        return "UniChar"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        code = args1.int_value()
        if code < 0 or code > 0x10FFFF or 0xD800 <= code <= 0xDFFF:
            return self._parameter_error(1)
        try:
            return Operand.create(chr(code))
        except (ValueError, OverflowError):
            return self._parameter_error(1)

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("UNICODE")
class Function_UNICODE(Function_1):
    def __init__(self, funcs):
        if not isinstance(funcs, (list, tuple)):
            funcs = [funcs]
        _require_args(funcs, "Unicode", 1, 1)
        super().__init__(funcs)

    def name(self) -> str:
        return "Unicode"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        text = args1.text_value()
        if not text:
            return self._parameter_error(1)
        return Operand.create(ord(text[0]))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)


@register("VALUE")
class Function_VALUE(Function_1):
    def __init__(self, funcs):
        if not isinstance(funcs, (list, tuple)):
            funcs = [funcs]
        _require_args(funcs, "Value", 1, 1)
        super().__init__(funcs)

    def name(self) -> str:
        return "Value"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        text = args1.text_value()
        if not _BIG_DECIMAL.match(text):
            return self._parameter_error(1)
        try:
            return Operand.create(Decimal(text))
        except (ArithmeticError, ValueError):
            return self._parameter_error(1)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NONE)


@register("LEFT")
class Function_LEFT(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "Left", 1, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Left"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        text = args1.text_value()
        if not text:
            return Operand.create("")
        if self.func2 is None:
            return Operand.create(text[:1])
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        count = args2.int_value()
        if count < 0:
            return self._parameter_error(2)
        if count >= len(text):
            return args1
        return Operand.create(text[:count])

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        if self.func2 is not None:
            self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("RIGHT")
class Function_RIGHT(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "Right", 1, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Right"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        text = args1.text_value()
        if not text:
            return Operand.create("")
        if self.func2 is None:
            return Operand.create(text[-1:])
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        count = args2.int_value()
        if count < 0:
            return self._parameter_error(2)
        if count >= len(text):
            return args1
        start = len(text) - count
        return Operand.create(text[start:start + count])

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        if self.func2 is not None:
            self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("MID")
class Function_MID(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "Mid", 3, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "Mid"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        args3 = self._get_number_3(engine, tp)
        if args3.is_error_or_none():
            return args3

        text = args1.text_value()
        excel_index = engine.ExcelIndex
        start_index = args2.int_value() - excel_index
        length = args3.int_value()

        if start_index < 0:
            return self._parameter_error(2)
        if length < 0:
            return self._parameter_error(3)
        if start_index == 0 and length >= len(text):
            return args1
        if start_index >= len(text):
            return Operand.create("")
        if start_index + length > len(text):
            length = len(text) - start_index
        return Operand.create(text[start_index:start_index + length])

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("REPLACE")
class Function_REPLACE(Function_4):
    def __init__(self, funcs):
        _require_args(funcs, "Replace", 3, 4)
        super().__init__(funcs)

    def name(self) -> str:
        return "Replace"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        oldtext = args1.text_value()
        if self.func4 is None:
            args22 = self._get_text_2(engine, tp)
            if args22.is_error_or_none():
                return args22
            args32 = self._get_text_3(engine, tp)
            if args32.is_error_or_none():
                return args32

            old = args22.text_value()
            newstr = args32.text_value()
            if not old or old not in oldtext:
                return args1
            return Operand.create(oldtext.replace(old, newstr))

        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        args3 = self._get_number_3(engine, tp)
        if args3.is_error_or_none():
            return args3
        args4 = self._get_text_4(engine, tp)
        if args4.is_error_or_none():
            return args4

        excel_index = engine.ExcelIndex
        start = args2.int_value() - excel_index
        length = args3.int_value()
        newtext = args4.text_value()

        if start < 0:
            return self._parameter_error(2)
        if length < 0:
            return self._parameter_error(3)
        if start >= len(oldtext):
            return Operand.create(oldtext + newtext)
        # truncate an oversized length instead of failing on the slice
        if length > len(oldtext) - start:
            length = len(oldtext) - start

        sb: list[str] = [oldtext[:start], newtext]
        end_index = start + length
        if end_index < len(oldtext):
            sb.append(oldtext[end_index:])
        return Operand.create("".join(sb))

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        if self.func4 is None:
            self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)
            self.func3._get_parameter_types(none_engine, result, OperandType.TEXT)
        else:
            self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
            self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)
            self.func4._get_parameter_types(none_engine, result, OperandType.TEXT)


@register("REPT")
class Function_REPT(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "Rept", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Rept"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2

        newtext = args1.text_value()
        length = args2.int_value()
        if length < 0:
            return self._parameter_error(2)
        if length == 0:
            return Operand.create("")
        if not newtext:
            return args1
        if length > 32767 // len(newtext):
            return self._parameter_error(2)
        if length == 1:
            return args1
        return Operand.create(newtext * length)

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("TEXT")
class Function_TEXT(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "Text", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Text"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self.func1.evaluate(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_text_2(engine, tp)
        if args2.is_error_or_none():
            return args2

        if args1.is_text():
            return args1
        if args1.is_boolean():
            return Operand.create("TRUE" if args1.boolean_value() else "FALSE")
        if args1.is_number():
            # an illegal pattern raises: report it as a parameter error
            try:
                return Operand.create(_format_number(args1.number_value(), args2.text_value()))
            except ValueError:
                return self._parameter_error(2)
        if args1.is_date():
            try:
                return Operand.create(args1.date_value().to_string(args2.text_value()))
            except ValueError:
                return self._parameter_error(2)
        args1 = self._convert_to_text(args1, 1)
        if args1.is_error_or_none():
            return args1
        return Operand.create(args1.text_value())

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NONE)
        self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)


@register("RMB")
class Function_RMB(Function_1):
    CN_UPPER_NUMBER = ("零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖")
    CN_UPPER_MONETRAY_UNIT = (
        "分", "角", "元", "拾", "佰", "仟", "万", "拾", "佰", "仟", "亿", "拾", "佰",
        "仟", "兆", "拾", "佰", "仟",
    )
    CN_FULL = "整"
    CN_NEGATIVE = "负"
    MONEY_PRECISION = 2
    CN_ZEOR_FULL = "零元" + CN_FULL

    def __init__(self, funcs):
        if not isinstance(funcs, (list, tuple)):
            funcs = [funcs]
        _require_args(funcs, "Rmb", 1, 1)
        super().__init__(funcs)

    def name(self) -> str:
        return "Rmb"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        return Operand.create(self._to_chinese_rmb(args1.number_value()))

    def _to_chinese_rmb(self, number_of_money: Decimal) -> str:
        signum = 1 if number_of_money > 0 else (-1 if number_of_money < 0 else 0)
        if signum == 0:
            return self.CN_ZEOR_FULL
        with localcontext() as ctx:
            ctx.prec = 60
            scaled = (number_of_money * 100).quantize(Decimal(1), rounding=ROUND_HALF_UP)
        number = abs(int(scaled))
        sb: list[str] = []
        scale = number % 100
        num_index = 0
        get_zero = False
        if not scale > 0:
            num_index = 2
            number = number // 100
            get_zero = True
        if scale > 0 and not scale % 10 > 0:
            num_index = 1
            number = number // 10
            get_zero = True
        zero_size = 0
        while True:
            if number <= 0:
                break
            num_unit = number % 10
            if num_unit > 0:
                if num_index == 9 and zero_size >= 3:
                    sb.insert(0, self.CN_UPPER_MONETRAY_UNIT[6])
                if num_index == 13 and zero_size >= 3:
                    sb.insert(0, self.CN_UPPER_MONETRAY_UNIT[10])
                sb.insert(0, self.CN_UPPER_MONETRAY_UNIT[num_index])
                sb.insert(0, self.CN_UPPER_NUMBER[num_unit])
                get_zero = False
                zero_size = 0
            else:
                zero_size += 1
                if not get_zero:
                    sb.insert(0, self.CN_UPPER_NUMBER[num_unit])
                if num_index == 2:
                    if number > 0:
                        sb.insert(0, self.CN_UPPER_MONETRAY_UNIT[num_index])
                elif (num_index - 2) % 4 == 0 and number % 1000 > 0:
                    sb.insert(0, self.CN_UPPER_MONETRAY_UNIT[num_index])
                get_zero = True
            number = number // 10
            num_index += 1
        if signum == -1:
            sb.insert(0, self.CN_NEGATIVE)
        if not scale > 0:
            sb.append(self.CN_FULL)
        return "".join(sb)

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
