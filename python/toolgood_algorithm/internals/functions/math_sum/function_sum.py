"""Function_SUM (C#: Internals/Functions/MathSum/Function_SUM.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _SumFunctionN, _flatten_numeric_args, _sum_values


@register("SUM")
class Function_SUM(_SumFunctionN):
    def __init__(self, funcs):
        super().__init__(funcs, "Sum", 1)

    def name(self) -> str:
        return "Sum"

    def evaluate(self, engine, tp) -> Operand:
        error, args = self._collect(engine, tp)
        if error is not None:
            return error
        values = _flatten_numeric_args(args)
        if values is None:
            return self._function_error()
        return Operand.create(_sum_values(values))

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        if len(self.funcs) == 1:
            self.funcs[0]._get_parameter_types(none_engine, result, OperandType.ARRAY)
        else:
            for func in self.funcs:
                func._get_parameter_types(none_engine, result, OperandType.NUMBER)
