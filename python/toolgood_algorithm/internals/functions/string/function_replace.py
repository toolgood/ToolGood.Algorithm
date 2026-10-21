"""Function_REPLACE (C#: Internals/Functions/String/Function_REPLACE.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_4 import Function_4
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("REPLACE")
class Function_REPLACE(Function_4):
    def __init__(self, funcs):
        _require_args(funcs, "Replace", 3, 4)
        super().__init__(funcs)

    def name(self) -> str:
        return "Replace"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        oldtext = args1.text_value()
        if self.func4 is None:
            args22 = self._get_text_2(engine, tp)
            if args22.is_error_or_none():
                return args22
            args32 = self._get_text_3(engine, tp)
            if args32.is_error_or_none():
                return args32

            old = args22.text_value()
            newstr = args32.text_value()
            if not old or old not in oldtext:
                return args1
            return Operand.create(oldtext.replace(old, newstr))

        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        args3 = self._get_number_3(engine, tp)
        if args3.is_error_or_none():
            return args3
        args4 = self._get_text_4(engine, tp)
        if args4.is_error_or_none():
            return args4

        excel_index = engine.ExcelIndex
        start = args2.int_value() - excel_index
        length = args3.int_value()
        newtext = args4.text_value()

        if start < 0:
            return self._parameter_error(2)
        if length < 0:
            return self._parameter_error(3)
        if start >= len(oldtext):
            return Operand.create(oldtext + newtext)
        # truncate an oversized length instead of failing on the slice
        if length > len(oldtext) - start:
            length = len(oldtext) - start

        sb: list[str] = [oldtext[:start], newtext]
        end_index = start + length
        if end_index < len(oldtext):
            sb.append(oldtext[end_index:])
        return Operand.create("".join(sb))

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        if self.func4 is None:
            self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)
            self.func3._get_parameter_types(none_engine, result, OperandType.TEXT)
        else:
            self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
            self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)
            self.func4._get_parameter_types(none_engine, result, OperandType.TEXT)
