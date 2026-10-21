"""Function_POWER (C#: Internals/Functions/MathBase/Function_POWER.cs)."""
from __future__ import annotations
import math
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.internals.numeric import ONE, ZERO
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import from_float


@register("POWER")
class Function_POWER(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "POWER", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Power"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        b = self._get_number_2(engine, tp)
        if b.is_error_or_none():
            return b
        base, exponent = a.number_value(), b.number_value()
        if base == ZERO and exponent < ZERO:
            return self._div0_error()
        if base < ZERO and exponent % ONE != ZERO:
            return self._parameter_error(1)
        result = math.pow(float(base), float(exponent))
        if result != result or result in (float("inf"), float("-inf")):
            return self._function_error()
        return Operand.create(from_float(result))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
