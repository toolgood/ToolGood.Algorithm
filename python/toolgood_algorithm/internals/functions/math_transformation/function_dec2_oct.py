"""Function_DEC2OCT (C#: Internals/Functions/MathTransformation/Function_DEC2OCT.cs)."""
from __future__ import annotations
from decimal import Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _MAX_PLACES, _OCT_MASK, _pad


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
