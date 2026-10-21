"""Function_SMALL (C#: Internals/Functions/MathSum/Function_SMALL.cs)."""
from __future__ import annotations
from decimal import Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.internals.functions.function_util import flatten_to_number_list, quick_select
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("SMALL")
class Function_SMALL(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "Small", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Small"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._convert_to_array(self.func1.evaluate(engine, tp), 1)
        if args1.is_error_or_none():
            return args1
        args2 = self._convert_to_number(self.func2.evaluate(engine, tp), 2)
        if args2.is_error_or_none():
            return args2

        values: list[Decimal] = []
        if not flatten_to_number_list(args1, values):
            return self._parameter_error(1)
        if not values:
            return self._parameter_error(1)

        k = args2.int_value() - engine.ExcelIndex
        if k < 0 or k >= len(values):
            return self._parameter_error(2)
        return Operand.create(quick_select(values, k, False))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.ARRAY)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
