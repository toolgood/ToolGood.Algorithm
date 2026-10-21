"""Function_RATE (C#: Internals/Functions/Financial/Function_RATE.cs)."""
from __future__ import annotations
import math
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_6 import Function_6
from toolgood_algorithm.internals.numeric import ONE, ZERO, divide
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float

from ._common import _EPS_SLOPE, _EPS_STEP, _MAX_ITERATIONS, _arg_number


@register("RATE")
class Function_RATE(Function_6):
    def __init__(self, funcs):
        _require_args(funcs, "RATE", 3, 6)
        super().__init__(funcs)

    def name(self) -> str:
        return "RATE"

    def evaluate(self, engine, tp) -> Operand:
        nper_arg = self._get_number_1(engine, tp)
        if nper_arg.is_error_or_none():
            return nper_arg
        nper = nper_arg.number_value()

        pmt_arg = self._get_number_2(engine, tp)
        if pmt_arg.is_error_or_none():
            return pmt_arg
        pmt = pmt_arg.number_value()

        pv_arg = self._get_number_3(engine, tp)
        if pv_arg.is_error_or_none():
            return pv_arg
        pv = pv_arg.number_value()

        if nper <= ZERO:
            return self._parameter_error(1)

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
            if type_ != 0 and type_ != 1:
                return self._parameter_error(5)

        guess = from_float(0.1)
        if self.func6 is not None:
            guess_arg = _arg_number(self, engine, tp, 6)
            if guess_arg.is_error_or_none():
                return guess_arg
            guess = guess_arg.number_value()

        try:
            return Operand.create(self._newton_raphson(nper, pmt, pv, fv, type_, guess))
        except (ArithmeticError, ValueError, OverflowError):
            return self._function_error()

    @staticmethod
    def _newton_raphson(n, p, v, f, type_, rate):
        for _ in range(_MAX_ITERATIONS):
            one_plus_rate = ONE + rate
            factor = from_float(math.pow(float(one_plus_rate), float(n)))
            pow_n_minus_1 = from_float(math.pow(float(one_plus_rate), float(n - ONE)))

            if type_ == 1:
                fn = (
                    v * factor
                    + divide(p * one_plus_rate * (factor - ONE), rate)
                    + f
                )
            else:
                fn = v * factor + divide(p * (factor - ONE), rate) + f

            term1 = v * n * pow_n_minus_1
            inner = n * rate * pow_n_minus_1 - (factor - ONE)
            if type_ == 1:
                term2 = divide(p * one_plus_rate * inner, rate * rate)
                term3 = divide(p * (factor - ONE), rate)
                dfn = term1 + term2 + term3
            else:
                term2 = divide(p * inner, rate * rate)
                dfn = term1 + term2

            if abs(dfn) < _EPS_SLOPE:
                break
            new_rate = rate - divide(fn, dfn)
            if abs(new_rate - rate) < _EPS_STEP:
                return new_rate
            rate = new_rate
        raise ValueError("RATE calculation did not converge")

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
        if self.func6 is not None:
            self.func6._get_parameter_types(none_engine, result, OperandType.NUMBER)
