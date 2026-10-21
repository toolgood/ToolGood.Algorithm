"""Function_DEVSQ (C#: Internals/Functions/MathSum/Function_DEVSQ.cs)."""
from __future__ import annotations
from decimal import Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_util import flatten_to_big_decimal_list
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _SumFunctionN, _welford


@register("DEVSQ")
class Function_DEVSQ(_SumFunctionN):
    def __init__(self, funcs):
        super().__init__(funcs, "DevSq", 1)

    def name(self) -> str:
        return "DevSq"

    def evaluate(self, engine, tp) -> Operand:
        error, args = self._collect(engine, tp)
        if error is not None:
            return error

        values: list[Decimal] = []
        if not flatten_to_big_decimal_list(args, values):
            return self._function_error()
        if not values:
            return self._function_error()

        m2, _ = _welford(values)
        return Operand.create(m2)

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        if len(self.funcs) == 1:
            self.funcs[0]._get_parameter_types(none_engine, result, OperandType.ARRAY)
        else:
            for func in self.funcs:
                func._get_parameter_types(none_engine, result, OperandType.NUMBER)
