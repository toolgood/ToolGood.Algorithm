"""Number-base and roman-numeral conversion functions (DEC2BIN, HEX2OCT, ROMAN, ...)."""
from __future__ import annotations

import re
from decimal import Decimal

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import Function_1, Function_2, _require_args
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

# places parameter upper bound (matches the 10-digit two's complement notation)
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


@register("BIN2DEC")
class Function_BIN2DEC(_FromBase):
    def name(self) -> str:
        return "Bin2Dec"

    def get_result_type(self) -> OperandType:
        if self.func2 is not None:
            return OperandType.TEXT
        return OperandType.NUMBER

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        if not _is_bin(args1.text_value()):
            return self._parameter_error(1)
        text = args1.text_value()
        if len(text) > _MAX_PLACES:
            return self._parameter_error(1)
        # 10 位二进制补码解析:最高位为 1 时表示负数
        num = _parse_complement(text, 2, _BIN_HALF, _BIN_MODULUS)
        if self.func2 is not None:
            args2 = self._get_number_2(engine, tp)
            if args2.is_error_or_none():
                return args2
            places = args2.int_value()
            if places < 0:
                return self._parameter_error(2)
            n = str(num)
            if len(n) <= places:
                return Operand.create(_format_decimal(num, places))
            return self._parameter_error(2)
        return Operand.create(Decimal(num))


@register("BIN2HEX")
class Function_BIN2HEX(_FromBase):
    def name(self) -> str:
        return "Bin2Hex"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        if not _is_bin(args1.text_value()):
            return self._parameter_error(1)
        text = args1.text_value()
        if len(text) > _MAX_PLACES:
            return self._parameter_error(1)
        # 10 位二进制补码解析
        num = _parse_complement(text, 2, _BIN_HALF, _BIN_MODULUS)
        if num < 0:
            # 负数:10 位十六进制补码
            hex_text = _pad(format(num + _HEX_MODULUS, "X"), _MAX_PLACES)
        else:
            hex_text = format(num, "X")
        return self._apply_places(hex_text, engine, tp)


@register("BIN2OCT")
class Function_BIN2OCT(_FromBase):
    def name(self) -> str:
        return "Bin2Oct"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        if not _is_bin(args1.text_value()):
            return self._parameter_error(1)
        text = args1.text_value()
        if len(text) > _MAX_PLACES:
            return self._parameter_error(1)
        # 10 位二进制补码解析
        num = _parse_complement(text, 2, _BIN_HALF, _BIN_MODULUS)
        if num < 0:
            # 负数:10 位八进制补码
            oct_text = _pad(format(num & _OCT_MASK, "o"), _MAX_PLACES)
        else:
            oct_text = format(num, "o")
        return self._apply_places(oct_text, engine, tp)


@register("DEC2BIN")
class Function_DEC2BIN(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "Dec2Bin", 1, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Dec2Bin"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        num = args1.int_value()
        if num < -512 or num > 511:
            return self._parameter_error(1)
        if num < 0:
            # 负数:返回 10 位二进制补码,按 Excel 语义忽略 places
            return Operand.create(_pad(format(num & 1023, "b"), _MAX_PLACES))
        binary_str = format(num, "b")
        if self.func2 is not None:
            args2 = self._get_number_2(engine, tp)
            if args2.is_error_or_none():
                return args2
            places = args2.int_value()
            if places < 0 or places > _MAX_PLACES:
                return self._parameter_error(2)
            if len(binary_str) > places:
                return self._parameter_error(2)
            return Operand.create(_pad(binary_str, places))
        return Operand.create(binary_str)

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func2 is not None:
            self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("DEC2HEX")
class Function_DEC2HEX(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "Dec2Hex", 1, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Dec2Hex"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        # Excel 范围:-549755813888 ~ 549755813887,超出 int 范围,用 Decimal 校验后转 int
        num_value = args1.number_value()
        if num_value < Decimal(-549755813888) or num_value > Decimal(549755813887):
            return self._parameter_error(1)
        num = int(num_value)
        if num < 0:
            # 负数:返回 10 位十六进制补码,按 Excel 语义忽略 places
            return Operand.create(_pad(format(num & _HEX_MASK, "X"), _MAX_PLACES))
        hex_text = format(num, "X")
        if self.func2 is not None:
            args2 = self._get_number_2(engine, tp)
            if args2.is_error_or_none():
                return args2
            places = args2.int_value()
            if places < 0 or places > _MAX_PLACES:
                return self._parameter_error(2)
            if len(hex_text) > places:
                return self._parameter_error(2)
            return Operand.create(_pad(hex_text, places))
        return Operand.create(hex_text)

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func2 is not None:
            self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("DEC2OCT")
class Function_DEC2OCT(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "Dec2Oct", 1, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Dec2Oct"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        # Excel 范围:-536870912 ~ 536870911
        num_value = args1.number_value()
        if num_value < Decimal(-536870912) or num_value > Decimal(536870911):
            return self._parameter_error(1)
        num = int(num_value)
        if num < 0:
            # 负数:返回 10 位八进制补码,按 Excel 语义忽略 places
            return Operand.create(_pad(format(num & _OCT_MASK, "o"), _MAX_PLACES))
        oct_text = format(num, "o")
        if self.func2 is not None:
            args2 = self._get_number_2(engine, tp)
            if args2.is_error_or_none():
                return args2
            places = args2.int_value()
            if places < 0 or places > _MAX_PLACES:
                return self._parameter_error(2)
            if len(oct_text) > places:
                return self._parameter_error(2)
            return Operand.create(_pad(oct_text, places))
        return Operand.create(oct_text)

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func2 is not None:
            self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("HEX2BIN")
class Function_HEX2BIN(_FromBase):
    def name(self) -> str:
        return "Hex2Bin"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        if not _is_hex(args1.text_value()):
            return self._parameter_error(1)
        text = args1.text_value()
        if len(text) > _MAX_PLACES:
            return self._parameter_error(1)
        # 10 位十六进制补码解析
        num = _parse_complement(text, 16, _HEX_HALF, _HEX_MODULUS)
        # Excel HEX2BIN 结果范围为 -512~511
        if num < -512 or num > 511:
            return self._parameter_error(1)
        if num < 0:
            # 负数:10 位二进制补码
            bin_text = _pad(format(num & 1023, "b"), _MAX_PLACES)
        else:
            bin_text = format(num, "b")
        return self._apply_places(bin_text, engine, tp)


@register("HEX2DEC")
class Function_HEX2DEC(_FromBase):
    def name(self) -> str:
        return "Hex2Dec"

    def get_result_type(self) -> OperandType:
        if self.func2 is not None:
            return OperandType.TEXT
        return OperandType.NUMBER

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        if not _is_hex(args1.text_value()):
            return self._parameter_error(1)
        text = args1.text_value()
        if len(text) > _MAX_PLACES:
            return self._parameter_error(1)
        # 10 位十六进制补码解析:最高位(bit39)为 1 时表示负数
        num = _parse_complement(text, 16, _HEX_HALF, _HEX_MODULUS)
        if self.func2 is not None:
            args2 = self._get_number_2(engine, tp)
            if args2.is_error_or_none():
                return args2
            places = args2.int_value()
            if places < 0:
                return self._parameter_error(2)
            if len(str(num)) <= places:
                return Operand.create(_format_decimal(num, places))
            return self._parameter_error(2)
        return Operand.create(Decimal(num))


@register("HEX2OCT")
class Function_HEX2OCT(_FromBase):
    def name(self) -> str:
        return "Hex2Oct"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        if not _is_hex(args1.text_value()):
            return self._parameter_error(1)
        text = args1.text_value()
        if len(text) > _MAX_PLACES:
            return self._parameter_error(1)
        # 10 位十六进制补码解析
        num = _parse_complement(text, 16, _HEX_HALF, _HEX_MODULUS)
        # Excel HEX2OCT 结果范围为 -536870912~536870911
        if num < -536870912 or num > 536870911:
            return self._parameter_error(1)
        if num < 0:
            # 负数:10 位八进制补码
            oct_text = _pad(format(num & _OCT_MASK, "o"), _MAX_PLACES)
        else:
            oct_text = format(num, "o")
        return self._apply_places(oct_text, engine, tp)


@register("OCT2BIN")
class Function_OCT2BIN(_FromBase):
    def name(self) -> str:
        return "Oct2Bin"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        if not _is_oct(args1.text_value()):
            return self._parameter_error(1)
        text = args1.text_value()
        if len(text) > _MAX_PLACES:
            return self._parameter_error(1)
        # 10 位八进制补码解析
        num = _parse_complement(text, 8, _OCT_HALF, _OCT_MODULUS)
        # Excel OCT2BIN 结果范围为 -512~511
        if num < -512 or num > 511:
            return self._parameter_error(1)
        if num < 0:
            # 负数:10 位二进制补码
            bin_text = _pad(format(num & 1023, "b"), _MAX_PLACES)
        else:
            bin_text = format(num, "b")
        return self._apply_places(bin_text, engine, tp)


@register("OCT2DEC")
class Function_OCT2DEC(_FromBase):
    def name(self) -> str:
        return "Oct2Dec"

    def get_result_type(self) -> OperandType:
        if self.func2 is not None:
            return OperandType.TEXT
        return OperandType.NUMBER

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        if not _is_oct(args1.text_value()):
            return self._parameter_error(1)
        text = args1.text_value()
        if len(text) > _MAX_PLACES:
            return self._parameter_error(1)
        # 10 位八进制补码解析:最高位(bit29)为 1 时表示负数
        num = _parse_complement(text, 8, _OCT_HALF, _OCT_MODULUS)
        if self.func2 is not None:
            args2 = self._get_number_2(engine, tp)
            if args2.is_error_or_none():
                return args2
            places = args2.int_value()
            if places < 0:
                return self._parameter_error(2)
            if len(str(num)) <= places:
                return Operand.create(_format_decimal(num, places))
            return self._parameter_error(2)
        return Operand.create(Decimal(num))


@register("OCT2HEX")
class Function_OCT2HEX(_FromBase):
    def name(self) -> str:
        return "Oct2Hex"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        if not _is_oct(args1.text_value()):
            return self._parameter_error(1)
        text = args1.text_value()
        if len(text) > _MAX_PLACES:
            return self._parameter_error(1)
        # 10 位八进制补码解析
        num = _parse_complement(text, 8, _OCT_HALF, _OCT_MODULUS)
        if num < 0:
            # 负数:10 位十六进制补码
            hex_text = _pad(format(num + _HEX_MODULUS, "X"), _MAX_PLACES)
        else:
            hex_text = format(num, "X")
        return self._apply_places(hex_text, engine, tp)


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


@register("ARABIC")
class Function_ARABIC(Function_1):
    def __init__(self, funcs):
        _require_args(funcs, "ARABIC", 1, 1)
        super().__init__(funcs)

    def name(self) -> str:
        return "ARABIC"

    def evaluate(self, engine, tp) -> Operand:
        arg = self._get_text_1(engine, tp)
        if arg.is_error_or_none():
            return arg
        text = arg.text_value().upper()
        # Excel: 空文本或不符合罗马数字语法(非法字符/非法减法/非法重复)时返回 #VALUE!
        if len(text) == 0 or _ROMAN_REGEX.fullmatch(text) is None:
            return self._parameter_error(1)
        result = _roman_to_arabic(text)
        if result < 0:
            return self._parameter_error(1)
        return Operand.create(Decimal(result))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)


@register("ROMAN")
class Function_ROMAN(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "ROMAN", 1, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "ROMAN"

    def evaluate(self, engine, tp) -> Operand:
        num_arg = self._get_number_1(engine, tp)
        if num_arg.is_error_or_none():
            return num_arg
        num = num_arg.int_value()
        # Excel: number 为负或大于 3999 时返回 #VALUE!,number 为 0 返回空文本
        if num < 0 or num > 3999:
            return self._parameter_error(1)
        form = 0
        if self.func2 is not None:
            form_arg = self._get_number_2(engine, tp)
            if form_arg.is_error_or_none():
                return form_arg
            form = form_arg.int_value()
            # Excel: form 超出 0~4 范围返回 #VALUE!
            if form < 0 or form > 4:
                return self._parameter_error(2)
        return Operand.create(_arabic_to_roman(num, form))

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func2 is not None:
            self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
