"""Function_IFERROR (C#: Internals/Functions/Flow/Function_IFERROR.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_3 import Function_3
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("IFERROR")
class Function_IFERROR(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "IFERROR", 2, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "IFERROR"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self.func1.evaluate(engine, tp)
        if args1.is_none():
            return args1
        if args1.is_error():
            return self.func2.evaluate(engine, tp)
        if self.func3 is None:
            return args1
        return self.func3.evaluate(engine, tp)

    def get_result_type(self) -> OperandType:
        t2 = self.func2.get_result_type()
        if t2 != OperandType.NONE:
            return t2
        if self.func3 is not None:
            t3 = self.func3.get_result_type()
            if t3 != OperandType.NONE:
                return t3
        return OperandType.NONE

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NONE)
        self.func2._get_parameter_types(none_engine, result, OperandType.NONE)
        if self.func3 is not None:
            self.func3._get_parameter_types(none_engine, result, OperandType.NONE)
