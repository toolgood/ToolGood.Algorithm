"""Function_MIN (C#: Internals/Functions/MathSum/Function_MIN.cs)."""
from __future__ import annotations
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _SumFunctionN, _extreme_parameter_types, _extreme_value


@register("MIN")
class Function_MIN(_SumFunctionN):
    def __init__(self, funcs):
        super().__init__(funcs, "Min", 1)

    def name(self) -> str:
        return "Min"

    def evaluate(self, engine, tp) -> Operand:
        error, args = self._collect(engine, tp)
        if error is not None:
            return error
        value = _extreme_value(args, False)
        if value is None:
            return self._function_error()
        return Operand.create(value)

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        _extreme_parameter_types(self, none_engine, result)
