"""Function_REMOVESTART (C#: Internals/Functions/Csharp/Function_REMOVESTART.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_3 import Function_3
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("REMOVESTART")
class Function_REMOVESTART(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "RemoveStart", 2, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "RemoveStart"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_text_2(engine, tp)
        if args2.is_error_or_none():
            return args2

        ignore_case = False
        if self.func3 is not None:
            args3 = self._get_boolean_3(engine, tp)
            if args3.is_error_or_none():
                return args3
            ignore_case = args3.boolean_value()

        text = args1.text_value()
        remove_text = args2.text_value()
        if ignore_case:
            if text.lower().startswith(remove_text.lower()):
                return Operand.create(text[len(remove_text):])
        elif text.startswith(remove_text):
            return Operand.create(text[len(remove_text):])
        return args1

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)
        if self.func3 is not None:
            self.func3._get_parameter_types(none_engine, result, OperandType.BOOLEAN)
