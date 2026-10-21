"""Function_DEC2HEX (C#: Internals/Functions/MathTransformation/Function_DEC2HEX.cs)."""
from __future__ import annotations
from decimal import Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _HEX_MASK, _MAX_PLACES, _pad


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
