"""Function_SPLIT (C#: Internals/Functions/Csharp/Function_SPLIT.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("SPLIT")
class Function_SPLIT(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "Split", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Split"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_text_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        separator = args2.text_value()
        if not separator:
            return self._parameter_error(2)
        return Operand.create([Operand.create(s) for s in args1.text_value().split(separator)])

    def get_result_type(self) -> OperandType:
        return OperandType.ARRAY

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)
