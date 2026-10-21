"""Function_PERMUT (C#: Internals/Functions/MathBase/Function_PERMUT.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.internals.numeric import ONE
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("PERMUT")
class Function_PERMUT(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "PERMUT", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Permut"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        b = self._get_number_2(engine, tp)
        if b.is_error_or_none():
            return b
        num1, num2 = a.number_value(), b.number_value()
        if num1 > 2147483647 or num1 < -2147483648:
            return self._parameter_error(1)
        if num2 > 2147483647 or num2 < -2147483648:
            return self._parameter_error(2)
        total, count = int(num1), int(num2)
        if total < 0:
            return self._parameter_error(1)
        if count < 0:
            return self._parameter_error(2)
        if total < count:
            return self._parameter_error(2)
        result = ONE
        for i in range(count):
            result *= total - i
        return Operand.create(result)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
