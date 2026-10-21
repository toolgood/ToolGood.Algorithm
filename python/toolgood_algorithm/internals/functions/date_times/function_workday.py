"""Function_WORKDAY (C#: Internals/Functions/DateTimes/Function_WORKDAY.cs)."""
from __future__ import annotations
import datetime as _dt
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_n import Function_N
from toolgood_algorithm.operands.my_date import MyDate
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("WORKDAY")
class Function_WORKDAY(Function_N):
    def __init__(self, funcs):
        _require_args(funcs, "Workday", 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Workday"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_date(engine, tp, 0)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number(engine, tp, 1)
        if args2.is_error_or_none():
            return args2

        start = args1.date_value().to_datetime()
        days = args2.int_value()
        holidays = set()
        for i in range(2, len(self.funcs)):
            ar = self._get_date(engine, tp, i)
            if ar.is_error_or_none():
                return ar
            holidays.add(ar.date_value().to_datetime())

        def is_weekend(dt: _dt.datetime) -> bool:
            return dt.isoweekday() in (6, 7)

        if days > 0:
            # walk day by day until Monday, consuming working days
            while start.isoweekday() != 1 and days > 0:
                start = start + _dt.timedelta(days=1)
                if is_weekend(start):
                    continue
                if start in holidays:
                    continue
                days -= 1
            if days == 0:
                return Operand.create(MyDate(start))

            # coarse jump: from a Monday every 5 working days is 7 days
            after_jump = start + _dt.timedelta(days=(days // 5) * 7)
            extra = 0
            for holiday in holidays:
                if start < holiday <= after_jump and not is_weekend(holiday):
                    extra += 1
            start = after_jump
            days = days % 5 + extra
            while days > 0:
                start = start + _dt.timedelta(days=1)
                if is_weekend(start):
                    continue
                if start in holidays:
                    continue
                days -= 1
        elif days < 0:
            # walk backwards until Friday, consuming working days
            while start.isoweekday() != 5 and days < 0:
                start = start - _dt.timedelta(days=1)
                if is_weekend(start):
                    continue
                if start in holidays:
                    continue
                days += 1
            if days == 0:
                return Operand.create(MyDate(start))

            # coarse jump: from a Friday every 5 working days is 7 days
            after_jump = start + _dt.timedelta(days=(-days // 5) * -7)
            extra = 0
            for holiday in holidays:
                if after_jump <= holiday < start and not is_weekend(holiday):
                    extra += 1
            start = after_jump
            days = -((-days) % 5) - extra
            while days < 0:
                start = start - _dt.timedelta(days=1)
                if is_weekend(start):
                    continue
                if start in holidays:
                    continue
                days += 1
        return Operand.create(MyDate(start))

    def get_result_type(self) -> OperandType:
        return OperandType.DATE

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.funcs[0]._get_parameter_types(none_engine, result, OperandType.DATE)
        self.funcs[1]._get_parameter_types(none_engine, result, OperandType.NUMBER)
        for i in range(2, len(self.funcs)):
            self.funcs[i]._get_parameter_types(none_engine, result, OperandType.DATE)
