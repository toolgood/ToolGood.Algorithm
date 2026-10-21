"""Function_TRIMSTART (C#: Internals/Functions/Csharp/Function_TRIMSTART.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _trim_start


@register("TRIMSTART")
class Function_TRIMSTART(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "TrimStart", 1, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "TrimStart"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        text = args1.text_value()

        if self.func2 is None:
            return Operand.create(_trim_start(text))

        args2 = self._get_text_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        trim_chars = args2.text_value()
        start = 0
        while start < len(text) and text[start] in trim_chars:
            start += 1
        return Operand.create(text[start:])

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        if self.func2 is not None:
            self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)
