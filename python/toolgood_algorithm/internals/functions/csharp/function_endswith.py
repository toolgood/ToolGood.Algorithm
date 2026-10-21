"""Function_ENDSWITH (C#: Internals/Functions/Csharp/Function_ENDSWITH.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_3 import Function_3
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("ENDSWITH")
class Function_ENDSWITH(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "EndsWith", 2, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "EndsWith"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_text_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        text = args1.text_value()
        if self.func3 is None:
            return Operand.create(text.endswith(args2.text_value()))
        args3 = self._get_boolean_3(engine, tp)
        if args3.is_error_or_none():
            return args3
        if args3.boolean_value():
            return Operand.create(text.lower().endswith(args2.text_value().lower()))
        return Operand.create(text.endswith(args2.text_value()))

    def get_result_type(self) -> OperandType:
        return OperandType.BOOLEAN

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)
        if self.func3 is not None:
            self.func3._get_parameter_types(none_engine, result, OperandType.BOOLEAN)
