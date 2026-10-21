"""Function_TIME (C#: Internals/Functions/DateTimes/Function_TIME.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_3 import Function_3
from toolgood_algorithm.operands.my_date import MyDate
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("TIME")
class Function_TIME(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "Time", 2, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "Time"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2

        hour = args1.int_value()
        minute = args2.int_value()
        if hour < 0 or hour > 23:
            return self._parameter_error(1)
        if minute < 0 or minute > 59:
            return self._parameter_error(2)

        second = 0
        if self.func3 is not None:
            args3 = self._get_number_3(engine, tp)
            if args3.is_error_or_none():
                return args3
            second = args3.int_value()
            if second < 0 or second > 59:
                return self._parameter_error(3)
        return Operand.create(MyDate(0, 0, 0, hour, minute, second))

    def get_result_type(self) -> OperandType:
        return OperandType.DATE

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func3 is not None:
            self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)
