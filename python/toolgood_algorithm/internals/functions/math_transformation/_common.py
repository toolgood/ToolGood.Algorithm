"""math_transformation: shared helpers and base classes (kept out of the per-class files)."""
from __future__ import annotations
import re
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.operands.operand import Operand


class _FromBase(Function_2):
    """Common shell for BIN2*/OCT2*/HEX2* text-input functions."""

    def __init__(self, funcs):
        _require_args(funcs, self.name(), 1, 2)
        super().__init__(funcs)

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        if self.func2 is not None:
            self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)

    def _get_places(self, engine, tp):
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2, None
        places = args2.int_value()
        if places < 0 or places > _MAX_PLACES:
            return self._parameter_error(2), None
        return None, places

    def _apply_places(self, text: str, engine, tp) -> Operand:
        if self.func2 is None:
            return Operand.create(text)
        error, places = self._get_places(engine, tp)
        if error is not None:
            return error
        if len(text) > places:
            return self._parameter_error(2)
        return Operand.create(_pad(text, places))

_MAX_PLACES = 10

_BIN_HALF = 512

_BIN_MODULUS = 1024

_OCT_HALF = 536870912

_OCT_MODULUS = 1073741824

_OCT_MASK = 0x3FFFFFFF

_HEX_HALF = 0x8000000000

_HEX_MODULUS = 0x10000000000

_HEX_MASK = 0xFFFFFFFFFF


def _is_bin(value: str) -> bool:
    return bool(value) and all(ch in "01" for ch in value)


def _is_oct(value: str) -> bool:
    return bool(value) and all("0" <= ch <= "7" for ch in value)


def _is_hex(value: str) -> bool:
    return bool(value) and all("0" <= ch <= "9" or "A" <= ch <= "F" or "a" <= ch <= "f" for ch in value)


def _pad(text: str, width: int) -> str:
    """``String.format("%Ns", text).replace(' ', '0')`` equivalent."""
    return text.rjust(width, "0")


def _parse_complement(text: str, base: int, half: int, modulus: int) -> int:
    num = int(text, base)
    if num >= half:
        num -= modulus
    return num


def _format_decimal(value: int, places: int):
    """Pad a decimal string to ``places`` (the minus sign takes one position)."""
    text = str(value)
    if len(text) > places:
        return None
    if value < 0:
        return "-" + _pad(text[1:], places - 1)
    return _pad(text, places)

_ROMAN_REGEX = re.compile(r"^M{0,3}(CM|CD|D?C{0,3})(XC|XL|L?X{0,3})(IX|IV|V?I{0,3})$")

_ROMAN_VALUES = (
    (1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1),
    (1000, 950, 900, 500, 450, 400, 100, 95, 90, 50, 45, 40, 10, 9, 5, 4, 1),
    (1000, 990, 950, 900, 500, 490, 450, 400, 100, 99, 95, 90, 50, 49, 45, 40, 10, 9, 5, 4, 1),
    (1000, 995, 990, 950, 900, 500, 495, 490, 450, 400, 100, 99, 95, 90, 50, 49, 45, 40, 10, 9, 5, 4, 1),
    (1000, 999, 995, 990, 950, 900, 500, 499, 495, 490, 450, 400, 100, 99, 95, 90, 50, 49, 45, 40, 10, 9, 5, 4, 1),
)

_ROMAN_SYMBOLS = (
    ("M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I"),
    ("M", "LM", "CM", "D", "LD", "CD", "C", "VC", "XC", "L", "VL", "XL", "X", "IX", "V", "IV", "I"),
    ("M", "XM", "LM", "CM", "D", "XD", "LD", "CD", "C", "IC", "VC", "XC", "L", "IL", "VL", "XL", "X", "IX", "V", "IV", "I"),
    ("M", "VM", "XM", "LM", "CM", "D", "VD", "XD", "LD", "CD", "C", "IC", "VC", "XC", "L", "IL", "VL", "XL", "X", "IX", "V", "IV", "I"),
    ("M", "IM", "VM", "XM", "LM", "CM", "D", "ID", "VD", "XD", "LD", "CD", "C", "IC", "VC", "XC", "L", "IL", "VL", "XL", "X", "IX", "V", "IV", "I"),
)


def _roman_value(ch: str) -> int:
    if ch == "I":
        return 1
    if ch == "V":
        return 5
    if ch == "X":
        return 10
    if ch == "L":
        return 50
    if ch == "C":
        return 100
    if ch == "D":
        return 500
    if ch == "M":
        return 1000
    return -1


def _roman_to_arabic(roman: str) -> int:
    result = 0
    prev_value = 0
    for ch in reversed(roman):
        value = _roman_value(ch)
        if value < 0:
            return -1
        if value < prev_value:
            result -= value
        else:
            result += value
        prev_value = value
    return result


def _arabic_to_roman(num: int, form: int) -> str:
    if num == 0:
        return ""
    values = _ROMAN_VALUES[form]
    symbols = _ROMAN_SYMBOLS[form]
    out: list[str] = []
    for value, symbol in zip(values, symbols):
        while num >= value:
            out.append(symbol)
            num -= value
    return "".join(out)
