"""Function_RANK (C#: Internals/Functions/MathSum/Function_RANK.cs)."""
from __future__ import annotations
from decimal import Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_util import get_rank
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _SumFunctionN


@register("RANK")
class Function_RANK(_SumFunctionN):
    def __init__(self, funcs):
        super().__init__(funcs, "RANK", 2, 3)

    def name(self) -> str:
        return "RANK"

    def evaluate(self, engine, tp) -> Operand:
        if len(self.funcs) < 2:
            return self._parameter_error(1)

        num_arg = self._get_number(engine, tp, 0)
        if num_arg.is_error_or_none():
            return num_arg
        num = num_arg.number_value()

        array_arg = self._get_array(engine, tp, 1)
        if array_arg.is_error_or_none():
            return array_arg

        order = 0
        if len(self.funcs) > 2:
            order_arg = self._get_number(engine, tp, 2)
            if order_arg.is_error_or_none():
                return order_arg
            order = order_arg.int_value()

        values: list[Decimal] = []
        for item in array_arg.array_value():
            if item.is_number():
                values.append(item.number_value())
        if not values:
            return self._parameter_error(2)

        rank = get_rank(values, num, order == 0)
        if rank == 0:
            return self._parameter_error(1)
        return Operand.create(rank)

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.funcs[0]._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.funcs[1]._get_parameter_types(none_engine, result, OperandType.ARRAY)
        if len(self.funcs) > 2:
            self.funcs[2]._get_parameter_types(none_engine, result, OperandType.NUMBER)
