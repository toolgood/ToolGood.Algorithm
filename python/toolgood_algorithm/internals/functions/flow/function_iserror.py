"""Function_ISERROR (C#: Internals/Functions/Flow/Function_ISERROR.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("ISERROR")
class Function_ISERROR(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "ISERROR", 1, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "ISERROR"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self.func1.evaluate(engine, tp)
        if args1.is_none():
            return args1
        if self.func2 is not None:
            if args1.is_error():
                return self.func2.evaluate(engine, tp)
            return args1
        return Operand.TRUE if args1.is_error() else Operand.FALSE

    def get_result_type(self) -> OperandType:
        return self.func2.get_result_type() if self.func2 is not None else OperandType.BOOLEAN

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NONE)
        if self.func2 is not None:
            self.func2._get_parameter_types(none_engine, result, OperandType.NONE)
