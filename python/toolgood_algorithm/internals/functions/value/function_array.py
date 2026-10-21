"""Function_ARRAY (C#: Internals/Functions/Value/Function_ARRAY.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_n import Function_N
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("ARRAY")
class Function_ARRAY(Function_N):
    def __init__(self, funcs):
        _require_args(funcs, "Array", 1)
        super().__init__(funcs)

    def name(self) -> str:
        return "Array"

    def evaluate(self, engine, tp) -> Operand:
        args: list[Operand] = []
        error = self.try_evaluate_all(engine, tp, args)
        if error is not None:
            return error
        return Operand.create(args)

    def get_result_type(self) -> OperandType:
        return OperandType.ARRAY

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        for func in self.funcs:
            func._get_parameter_types(none_engine, result, OperandType.NONE)
