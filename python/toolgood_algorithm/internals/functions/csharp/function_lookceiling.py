"""Function_LOOKCEILING (C#: Internals/Functions/Csharp/Function_LOOKCEILING.cs)."""
from __future__ import annotations
import bisect
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.internals.functions.function_util import flatten_to_number_list
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("LOOKCEILING")
class Function_LOOKCEILING(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "LookCeiling", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "LookCeiling"

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
        if index == len(values):
            # every element is smaller than value: return the biggest one
            return Operand.create(values[-1])
        return Operand.create(values[index])

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.ARRAY)
