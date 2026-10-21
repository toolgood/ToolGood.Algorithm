"""Function_DAYS (C#: Internals/Functions/DateTimes/Function_DAYS.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _start_of_day, _whole_days


@register("DAYS")
class Function_DAYS(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "Days", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Days"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_date_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_date_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        end = _start_of_day(args1.date_value().to_datetime())
        start = _start_of_day(args2.date_value().to_datetime())
        return Operand.create(_whole_days(start, end))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.DATE)
        self.func2._get_parameter_types(none_engine, result, OperandType.DATE)
