"""Function_ROUND (C#: Internals/Functions/MathBase/Function_ROUND.cs)."""
from __future__ import annotations
from decimal import ROUND_HALF_UP, Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("ROUND")
class Function_ROUND(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "ROUND", 1, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Round"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        if self.func2 is None:
            return Operand.create(a.number_value().quantize(Decimal(1), rounding=ROUND_HALF_UP))
        b = self._get_number_2(engine, tp)
        if b.is_error_or_none():
            return b
        digits = b.int_value()
        if digits < -15 or digits > 15:
            return self._parameter_error(2)
        return Operand.create(a.number_value().quantize(Decimal(1).scaleb(-digits), rounding=ROUND_HALF_UP))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func2 is not None:
            self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
