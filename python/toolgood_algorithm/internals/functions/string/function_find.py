"""Function_FIND (C#: Internals/Functions/String/Function_FIND.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_3 import Function_3
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("FIND")
class Function_FIND(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "Find", 2, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "Find"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_text_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        if self.func3 is None:
            index = args2.text_value().find(args1.text_value())
            if index < 0:
                # not found: Excel mode (1 based) is an error, C# mode returns -1
                return self._function_error() if engine.ExcelIndex == 1 else Operand.create(-1)
            return Operand.create(index + engine.ExcelIndex)
        count = self._get_number_3(engine, tp)
        if count.is_error_or_none():
            return count
        excel_index = engine.ExcelIndex
        start_index = count.int_value() - excel_index
        if start_index < 0 or start_index >= len(args2.text_value()):
            return self._parameter_error(3)
        p2 = args2.text_value().find(args1.text_value(), start_index)
        if p2 < 0:
            return self._function_error() if engine.ExcelIndex == 1 else Operand.create(-1)
        return Operand.create(p2 + excel_index)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)
        if self.func3 is not None:
            self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)
