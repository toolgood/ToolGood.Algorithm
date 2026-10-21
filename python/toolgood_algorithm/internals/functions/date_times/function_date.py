"""Function_DATE (C#: Internals/Functions/DateTimes/Function_DATE.cs)."""
from __future__ import annotations
import datetime as _dt
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_6 import Function_6
from toolgood_algorithm.operands.my_date import MyDate
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _add_months


@register("DATE")
class Function_DATE(Function_6):
    def __init__(self, funcs):
        _require_args(funcs, "Date", 3, 6)
        super().__init__(funcs)

    def name(self) -> str:
        return "Date"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        args3 = self._get_number_3(engine, tp)
        if args3.is_error_or_none():
            return args3

        year = args1.int_value()
        month = args2.int_value()
        day = args3.int_value()

        try:
            base = _add_months(_dt.datetime(year, 1, 1), month - 1)
            base = base + _dt.timedelta(days=day - 1)
        except (ValueError, OverflowError):
            return self._parameter_error(1)

        hour = 0
        minute = 0
        second = 0
        if self.func4 is not None:
            args4 = self._get_number_4(engine, tp)
            if args4.is_error_or_none():
                return args4
            hour = args4.int_value()
            if hour < 0 or hour > 23:
                return self._parameter_error(4)
        if self.func5 is not None:
            args5 = self._get_number_5(engine, tp)
            if args5.is_error_or_none():
                return args5
            minute = args5.int_value()
            if minute < 0 or minute > 59:
                return self._parameter_error(5)
        if self.func6 is not None:
            args6 = self._get_number_6(engine, tp)
            if args6.is_error_or_none():
                return args6
            second = args6.int_value()
            if second < 0 or second > 59:
                return self._parameter_error(6)

        return Operand.create(
            MyDate(base.year, base.month, base.day, hour, minute, second)
        )

    def get_result_type(self) -> OperandType:
        return OperandType.DATE

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func4 is not None:
            self.func4._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func5 is not None:
            self.func5._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func6 is not None:
            self.func6._get_parameter_types(none_engine, result, OperandType.NUMBER)
