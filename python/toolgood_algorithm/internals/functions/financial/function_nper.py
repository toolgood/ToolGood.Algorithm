"""Function_NPER (C#: Internals/Functions/Financial/Function_NPER.cs)."""
from __future__ import annotations
import math
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_5 import Function_5
from toolgood_algorithm.internals.numeric import ONE, ZERO, divide
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float


@register("NPER")
class Function_NPER(Function_5):
    def __init__(self, funcs):
        _require_args(funcs, "NPER", 3, 5)
        super().__init__(funcs)

    def name(self) -> str:
        return "NPER"

    def evaluate(self, engine, tp) -> Operand:
        rate_arg = self._get_number_1(engine, tp)
        if rate_arg.is_error_or_none():
            return rate_arg
        rate = rate_arg.number_value()

        pmt_arg = self._get_number_2(engine, tp)
        if pmt_arg.is_error_or_none():
            return pmt_arg
        pmt = pmt_arg.number_value()

        pv_arg = self._get_number_3(engine, tp)
        if pv_arg.is_error_or_none():
            return pv_arg
        pv = pv_arg.number_value()

        fv = ZERO
        if self.func4 is not None:
            fv_arg = self._get_number_4(engine, tp)
            if fv_arg.is_error_or_none():
                return fv_arg
            fv = fv_arg.number_value()

        type_ = 0
        if self.func5 is not None:
            type_arg = self._get_number_5(engine, tp)
            if type_arg.is_error_or_none():
                return type_arg
            type_ = type_arg.int_value()

        if rate == ZERO:
            if pmt == ZERO:
                return self._div0_error()
            return Operand.create(divide((pv + fv).copy_negate(), pmt))
        if rate == ONE.copy_negate():
            return self._div0_error()

        factor = pmt
        if type_ == 1:
            factor = pmt * (ONE + rate)

        numerator = (fv * rate).copy_negate() + factor
        denominator = pv * rate + factor
        try:
            nper = math.log(float(divide(numerator, denominator))) / math.log(
                float(ONE + rate)
            )
        except (ValueError, ZeroDivisionError, ArithmeticError):
            return self._num_error()
        return Operand.create(from_float(nper))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func4 is not None:
            self.func4._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func5 is not None:
            self.func5._get_parameter_types(none_engine, result, OperandType.NUMBER)
