"""Function_TRUNC (C#: Internals/Functions/MathBase/Function_TRUNC.cs)."""
from __future__ import annotations
import math
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.internals.numeric import ZERO
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import from_float


@register("TRUNC")
class Function_TRUNC(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "TRUNC", 1, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Trunc"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        digits = 0
        if self.func2 is not None:
            b = self._get_number_2(engine, tp)
            if b.is_error_or_none():
                return b
            digits = b.int_value()
            if digits < -15 or digits > 15:
                return self._parameter_error(2)
        num = a.number_value()
        if num == ZERO:
            return a
        factor = 10.0 ** digits
        if num > ZERO:
            result = math.floor(float(num) * factor) / factor
        else:
            result = math.ceil(float(num) * factor) / factor
        return Operand.create(from_float(result))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func2 is not None:
            self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
