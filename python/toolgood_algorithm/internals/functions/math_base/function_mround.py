"""Function_MROUND (C#: Internals/Functions/MathBase/Function_MROUND.cs)."""
from __future__ import annotations
from decimal import ROUND_HALF_UP, Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.internals.numeric import ZERO
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("MROUND")
class Function_MROUND(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "MROUND", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "MRound"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        b = self._get_number_2(engine, tp)
        if b.is_error_or_none():
            return b
        multiple = b.number_value()
        if multiple == ZERO:
            return self._parameter_error(2)
        number = a.number_value()
        if (number > ZERO and multiple < ZERO) or (number < ZERO and multiple > ZERO):
            return self._parameter_error(2)
        r = (number / multiple).quantize(Decimal(1), rounding=ROUND_HALF_UP) * multiple
        return Operand.create(r)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
