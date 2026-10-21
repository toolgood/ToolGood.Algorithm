"""Function_INDEXOF (C#: Internals/Functions/Csharp/Function_INDEXOF.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_4 import Function_4
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("INDEXOF")
class Function_INDEXOF(Function_4):
    def __init__(self, funcs):
        _require_args(funcs, "IndexOf", 2, 4)
        super().__init__(funcs)

    def name(self) -> str:
        return "IndexOf"

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
            return Operand.create(text.find(search_str) + excel_index)

        args3 = self._get_number_3(engine, tp)
        if args3.is_error_or_none():
            return args3
        start_index = args3.int_value() - excel_index
        if start_index < 0 or start_index > len(text):
            return self._parameter_error(3)

        if self.func4 is None:
            return Operand.create(text.find(search_str, start_index) + excel_index)

        args4 = self._get_number_4(engine, tp)
        if args4.is_error_or_none():
            return args4
        count = args4.int_value()
        if count < 0 or start_index + count > len(text):
            return self._parameter_error(4)

        # the match has to fit completely inside [startIndex, startIndex + count)
        local_index = text[start_index:start_index + count].find(search_str)
        index = -1 if local_index < 0 else local_index + start_index
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
