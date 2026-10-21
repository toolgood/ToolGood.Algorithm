"""Function_DAYS360 (C#: Internals/Functions/DateTimes/Function_DAYS360.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_3 import Function_3
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _month_days


@register("DAYS360")
class Function_DAYS360(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "Days360", 2, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "Days360"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_date_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_date_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        start = args1.date_value().to_datetime()
        end = args2.date_value().to_datetime()

        method = False
        if self.func3 is not None:
            args3 = self._get_boolean_3(engine, tp)
            if args3.is_error_or_none():
                return args3
            method = args3.boolean_value()

        days = (
            end.year * 360
            + (end.month - 1) * 30
            - start.year * 360
            - (start.month - 1) * 30
        )
        if method:
            # European method: only 31 becomes 30
            days += min(end.day, 30) - min(start.day, 30)
        else:
            # US (NASD) method
            start_day = start.day
            end_day = end.day
            if start_day == _month_days(start.year, start.month):
                start_day = 30
            if end_day == _month_days(end.year, end.month):
                end_day = 31 if start_day < 30 else 30
            days += end_day - start_day
        return Operand.create(days)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.DATE)
        self.func2._get_parameter_types(none_engine, result, OperandType.DATE)
        if self.func3 is not None:
            self.func3._get_parameter_types(none_engine, result, OperandType.BOOLEAN)
