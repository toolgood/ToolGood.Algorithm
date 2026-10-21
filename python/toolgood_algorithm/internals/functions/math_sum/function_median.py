"""Function_MEDIAN (C#: Internals/Functions/MathSum/Function_MEDIAN.cs)."""
from __future__ import annotations
from decimal import Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_util import flatten_to_big_decimal_list, quick_select
from toolgood_algorithm.internals.numeric import divide
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _SumFunctionN, _TWO


@register("MEDIAN")
class Function_MEDIAN(_SumFunctionN):
    def __init__(self, funcs):
        super().__init__(funcs, "Median", 1)

    def name(self) -> str:
        return "Median"

    def evaluate(self, engine, tp) -> Operand:
        error, args = self._collect(engine, tp)
        if error is not None:
            return error

        values: list[Decimal] = []
        if not flatten_to_big_decimal_list(args, values):
            return self._function_error()
        if not values:
            return self._function_error()

        n = len(values)
        if n == 1:
            return Operand.create(values[0])
        mid = n // 2
        if n % 2 == 0:
            a = quick_select(values, mid - 1, False)
            b = quick_select(values, mid, False)
            return Operand.create(divide(a + b, _TWO))
        return Operand.create(quick_select(values, mid, False))

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        if len(self.funcs) == 1:
            self.funcs[0]._get_parameter_types(none_engine, result, OperandType.ARRAY)
        else:
            for func in self.funcs:
                func._get_parameter_types(none_engine, result, OperandType.NUMBER)
