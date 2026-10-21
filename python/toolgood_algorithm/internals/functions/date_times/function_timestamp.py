"""Function_TIMESTAMP (C#: Internals/Functions/DateTimes/Function_TIMESTAMP.cs)."""
from __future__ import annotations
from decimal import Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _EPOCH, _START_DATE_UTC_MS, _UTC


@register("TIMESTAMP")
class Function_TIMESTAMP(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "Timestamp", 1, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Timestamp"

    def evaluate(self, engine, tp) -> Operand:
        type_ = 0
        if self.func2 is not None:
            args2 = self._get_number_2(engine, tp)
            if args2.is_error_or_none():
                return args2
            type_ = args2.int_value()

        args0 = self._get_date_1(engine, tp)
        if args0.is_error_or_none():
            return args0
        value = args0.date_value().to_datetime()
        if engine.UseLocalTime:
            # ``ToDateTime(default).withZone(UTC)``: the wall clock is local
            value = value.astimezone(_UTC).replace(tzinfo=None)
        delta = value - _EPOCH
        ms = (delta.days * 86400 + delta.seconds) * 1000 - _START_DATE_UTC_MS
        if type_ == 0:
            return Operand.create(Decimal(ms))
        if type_ == 1:
            return Operand.create(Decimal(ms) / Decimal(1000))
        return self._parameter_error(2)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.DATE)
        if self.func2 is not None:
            self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
