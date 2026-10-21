"""Function_SUBSTITUTE (C#: Internals/Functions/String/Function_SUBSTITUTE.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_4 import Function_4
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("SUBSTITUTE")
class Function_SUBSTITUTE(Function_4):
    def __init__(self, funcs):
        _require_args(funcs, "Substitute", 3, 4)
        super().__init__(funcs)

    def name(self) -> str:
        return "Substitute"

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
        if self.func4 is None:
            if not args2.text_value():
                return Operand.create(args1.text_value())
            return Operand.create(args1.text_value().replace(args2.text_value(), args3.text_value()))

        args4 = self._get_number_4(engine, tp)
        if args4.is_error_or_none():
            return args4
        text = args1.text_value()
        oldtext = args2.text_value()
        newtext = args3.text_value()
        replace_index = args4.int_value()

        if not oldtext:
            return Operand.create(text)
        if replace_index < 1:
            return self._parameter_error(4)

        sb: list[str] = []
        current_index = 0
        found_count = 0
        search_pos = 0

        while search_pos <= len(text) - len(oldtext):
            found_pos = text.find(oldtext, search_pos)
            if found_pos < 0:
                break
            found_count += 1
            if found_count == replace_index:
                sb.append(text[current_index:found_pos])
                sb.append(newtext)
                current_index = found_pos + len(oldtext)
                break
            search_pos = found_pos + len(oldtext)

        if current_index == 0:
            return Operand.create(text)
        sb.append(text[current_index:])
        return Operand.create("".join(sb))

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)
        self.func3._get_parameter_types(none_engine, result, OperandType.TEXT)
        if self.func4 is not None:
            self.func4._get_parameter_types(none_engine, result, OperandType.NUMBER)
