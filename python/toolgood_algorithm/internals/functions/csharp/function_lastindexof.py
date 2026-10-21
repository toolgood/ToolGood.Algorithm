"""Function_LASTINDEXOF (C#: Internals/Functions/Csharp/Function_LASTINDEXOF.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_4 import Function_4
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("LASTINDEXOF")
class Function_LASTINDEXOF(Function_4):
    def __init__(self, funcs):
        _require_args(funcs, "LastIndexOf", 2, 4)
        super().__init__(funcs)

    def name(self) -> str:
        return "LastIndexOf"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_text_2(engine, tp)
        if args2.is_error_or_none():
            return args2

        text = args1.text_value()
        search_str = args2.text_value()
        excel_index = engine.ExcelIndex

        if self.func3 is None:
            return Operand.create(text.rfind(search_str) + excel_index)

        args3 = self._get_number_3(engine, tp)
        if args3.is_error_or_none():
            return args3
        start_index = args3.int_value() - excel_index
        if start_index < 0 or start_index > len(text):
            return self._parameter_error(3)

        if self.func4 is None:
            # the last matching character has to be at position <= startIndex
            from_index = start_index - len(search_str) + 1
            if from_index < 0:
                return Operand.create(-1 + excel_index)
            index = text.rfind(search_str, 0, from_index + len(search_str))
            return Operand.create(index + excel_index)

        args4 = self._get_number_4(engine, tp)
        if args4.is_error_or_none():
            return args4
        count = args4.int_value()
        if count < 0 or count > start_index + 1:
            return self._parameter_error(4)

        # search inside the [startIndex - count + 1, startIndex] window
        offset = start_index - count + 1
        end_pos = min(start_index + 1, len(text))
        window = text[offset:end_pos] if offset <= end_pos else ""
        local_index = window.rfind(search_str)
        index = -1 if local_index < 0 else local_index + offset
        return Operand.create(index + excel_index)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)
        if self.func3 is not None:
            self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func4 is not None:
            self.func4._get_parameter_types(none_engine, result, OperandType.NUMBER)
