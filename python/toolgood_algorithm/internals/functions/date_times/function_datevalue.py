"""Function_DATEVALUE (C#: Internals/Functions/DateTimes/Function_DATEVALUE.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.operands.my_date import MyDate
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _MAX_SECONDS, _MAX_SERIAL_DAY, _create_utc, _from_ms


@register("DATEVALUE")
class Function_DATEVALUE(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "DateValue", 1, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "DateValue"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self.func1.evaluate(engine, tp)
        if args1.is_error_or_none():
            return args1
        if args1.is_date():
            return args1

        type_ = 0
        if self.func2 is not None:
            args2 = self._get_number_2(engine, tp)
            if args2.is_error_or_none():
                return args2
            type_ = args2.int_value()

        if type_ == 0:
            if args1.is_text():
                md = MyDate.parse(args1.text_value())
                if md is not None:
                    return Operand.create(md)
            arg1 = self._convert_to_number(args1, 1)
            if arg1.is_error_or_none():
                return arg1
            if arg1.long_value() <= _MAX_SERIAL_DAY:
                return arg1.to_my_date(None)
            if arg1.long_value() <= _MAX_SECONDS:
                return _create_utc(_from_ms(arg1.long_value() * 1000), engine)
            return _create_utc(_from_ms(arg1.long_value()), engine)
        if type_ == 1:
            arg1 = self._convert_to_text(args1, 1)
            if arg1.is_error_or_none():
                return arg1
            md = MyDate.parse(arg1.text_value())
            if md is not None:
                return Operand.create(md)
        elif type_ == 2:
            arg1 = self._convert_to_number(args1, 1)
            if arg1.is_error_or_none():
                return arg1
            return arg1.to_my_date(None)
        elif type_ == 3:
            arg1 = self._convert_to_number(args1, 1)
            if arg1.is_error_or_none():
                return arg1
            return _create_utc(_from_ms(arg1.long_value()), engine)
        elif type_ == 4:
            arg1 = self._convert_to_number(args1, 1)
            if arg1.is_error_or_none():
                return arg1
            return _create_utc(_from_ms(arg1.long_value() * 1000), engine)
        return self._parameter_error(1)

    def get_result_type(self) -> OperandType:
        return OperandType.DATE

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NONE)
        if self.func2 is not None:
            self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
