"""Function_WEEKDAY (C#: Internals/Functions/DateTimes/Function_WEEKDAY.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("WEEKDAY")
class Function_WEEKDAY(Function_2):
    _MAPPINGS = {
        12: (6, 7, 1, 2, 3, 4, 5),
        13: (5, 6, 7, 1, 2, 3, 4),
        14: (4, 5, 6, 7, 1, 2, 3),
        15: (3, 4, 5, 6, 7, 1, 2),
        16: (2, 3, 4, 5, 6, 7, 1),
    }

    def __init__(self, funcs):
        _require_args(funcs, "Weekday", 1, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Weekday"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_date_1(engine, tp)
        if args1.is_error_or_none():
            return args1

        type_ = 1
        if self.func2 is not None:
            args2 = self._get_number_2(engine, tp)
            if args2.is_error_or_none():
                return args2
            type_ = args2.int_value()
            if type_ != 1 and type_ != 2 and type_ != 3 and (type_ < 11 or type_ > 17):
                return self._parameter_error(2)

        # 0 = Sunday ... 6 = Saturday (matches Joda ``dayOfWeek % 7``)
        day_of_week = args1.date_value().day_of_week() % 7

        if type_ == 1 or type_ == 17:
            return Operand.create(day_of_week + 1)
        if type_ == 2 or type_ == 11:
            return Operand.create(7 if day_of_week == 0 else day_of_week)
        if type_ == 3:
            return Operand.create(6 if day_of_week == 0 else day_of_week - 1)
        mapping = self._MAPPINGS.get(type_)
        if mapping is not None:
            return Operand.create(mapping[day_of_week])
        return self._parameter_error(2)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.DATE)
        if self.func2 is not None:
            self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
