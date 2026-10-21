"""Function_CEILING (C#: Internals/Functions/MathBase/Function_CEILING.cs)."""
from __future__ import annotations
import math
from decimal import ROUND_CEILING, Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.internals.numeric import ZERO
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import from_float


@register("CEILING")
class Function_CEILING(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "CEILING", 1, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Ceiling"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        if self.func2 is None:
            return Operand.create(a.number_value().quantize(Decimal(1), rounding=ROUND_CEILING))
        b = self._get_number_2(engine, tp)
        if b.is_error_or_none():
            return b
        divisor = b.number_value()
        if divisor == ZERO:
            return Operand.ZERO
        value = a.number_value()
        if divisor > ZERO:
            return Operand.create(from_float(math.ceil(float(value / divisor)) * float(divisor)))
        if value > ZERO:
            return self._parameter_error(1)
        return Operand.create(from_float(math.ceil(float(value / divisor)) * float(divisor)))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func2 is not None:
            self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
