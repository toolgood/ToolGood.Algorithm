"""Function_LOOKFLOOR (C#: Internals/Functions/Csharp/Function_LOOKFLOOR.cs)."""
from __future__ import annotations
import bisect
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.internals.functions.function_util import flatten_to_number_list
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("LOOKFLOOR")
class Function_LOOKFLOOR(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "LookFloor", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "LookFloor"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_array_2(engine, tp)
        if args2.is_error_or_none():
            return args2

        values: list = []
        if not flatten_to_number_list(args2, values):
            return self._parameter_error(2)
        if not values:
            return self._parameter_error(2)
        values.sort()

        value = args1.number_value()
        index = bisect.bisect_left(values, value)
        if index < len(values) and values[index] == value:
            return args1
        if index == 0:
            # every element is greater than value: return the smallest one
            return Operand.create(values[0])
        return Operand.create(values[index - 1])

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.ARRAY)
