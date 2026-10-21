"""Function_ARABIC (C#: Internals/Functions/MathTransformation/Function_ARABIC.cs)."""
from __future__ import annotations
from decimal import Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_1 import Function_1
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _ROMAN_REGEX, _roman_to_arabic


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
