"""Function_TEXT (C#: Internals/Functions/String/Function_TEXT.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _format_number


@register("TEXT")
class Function_TEXT(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "Text", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Text"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self.func1.evaluate(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_text_2(engine, tp)
        if args2.is_error_or_none():
            return args2

        if args1.is_text():
            return args1
        if args1.is_boolean():
            return Operand.create("TRUE" if args1.boolean_value() else "FALSE")
        if args1.is_number():
            # an illegal pattern raises: report it as a parameter error
            try:
                return Operand.create(_format_number(args1.number_value(), args2.text_value()))
            except ValueError:
                return self._parameter_error(2)
        if args1.is_date():
            try:
                return Operand.create(args1.date_value().to_string(args2.text_value()))
            except ValueError:
                return self._parameter_error(2)
        args1 = self._convert_to_text(args1, 1)
        if args1.is_error_or_none():
            return args1
        return Operand.create(args1.text_value())

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NONE)
        self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)
