"""Function_REGEXREPLACE (C#: Internals/Functions/Csharp/Function_REGEXREPLACE.cs)."""
from __future__ import annotations
import re
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_3 import Function_3
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _to_unicode_replacement


@register("REGEXREPLACE")
class Function_REGEXREPLACE(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "RegexReplace", 3, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "RegexReplace"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_text_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        args3 = self._get_text_3(engine, tp)
        if args3.is_error_or_none():
            return args3
        try:
            result = re.sub(args2.text_value(), _to_unicode_replacement(args3.text_value()), args1.text_value())
        except (re.error, IndexError):
            return self._parameter_error(2)
        return Operand.create(result)

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)
        self.func3._get_parameter_types(none_engine, result, OperandType.TEXT)
