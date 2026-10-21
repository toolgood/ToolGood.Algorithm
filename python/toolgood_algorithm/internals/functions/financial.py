"""Financial functions (PMT, NPV, IRR, XIRR, DB, ...).

Direct port of the Java ``internals/functions/financial`` package, including the
shared ``BigDecimal`` helpers (``bigPow`` / ``calculatePMT`` / ``calculateIPMT``
and the Newton-Raphson solvers) which live at module level here.
"""
from __future__ import annotations

import math
from decimal import ROUND_HALF_UP, Decimal

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import (
    Function_3,
    Function_4,
    Function_5,
    Function_6,
    Function_N,
    _require_args,
)
from toolgood_algorithm.operands.my_date import MyDate
from toolgood_algorithm.internals.numeric import ONE, ZERO, divide
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float

# solver tolerances (mirrors the Java constants)
_EPS_STEP = from_float(1e-10)
_EPS_SLOPE = from_float(1e-12)
_MAX_ITERATIONS = 100

_DAYS_IN_YEAR = from_float(365.0)


# ---------------------------------------------------------------------------
# shared helpers (``FinancialMath``)
# ---------------------------------------------------------------------------


def big_pow(base, exponent):
    """``FinancialMath.BigPow`` - integer power computed by repeated squaring."""
    n = int(exponent)
    result = ONE
    current = base
    while n > 0:
        if n % 2 == 1:
            result = result * current
        current = current * current
        n //= 2
    return result


def calculate_pmt(rate, nper, pv, fv, type_):
    if rate == ZERO:
        return divide(pv + fv, nper).copy_negate()
    factor = big_pow(ONE + rate, nper)
    pmt = divide((pv * factor + fv).copy_negate() * rate, factor - ONE)
    if type_ == 1:
        pmt = divide(pmt, ONE + rate)
    return pmt


def calculate_ipmt(rate, per, nper, pv, fv, type_):
    if rate == ZERO:
        return ZERO
    pmt = calculate_pmt(rate, nper, pv, fv, type_)
    factor = big_pow(ONE + rate, per - ONE)
    term1 = pv * factor
    term2 = divide(pmt * (factor - ONE), rate)
    ipmt = (term1 + term2).copy_negate() * rate
    if type_ == 1 and per == ONE:
        ipmt = ZERO
    return ipmt


def _numbers_from_array(array_operand, name: str, index: int):
    """Convert every element of an array operand to a number."""
    values = []
    for value in array_operand.array_value():
        if value.is_number():
            values.append(value.number_value())
        else:
            converted = value.to_number(f"Function '{name}' parameter {index} is error!")
            if converted.is_error_or_none():
                return None, converted
            values.append(converted.number_value())
    return values, None


def _dates_from_array(array_operand, name: str, index: int):
    """Convert every element of an array operand to a ``datetime``."""
    dates = []
    for value in array_operand.array_value():
        if value.is_date():
            dates.append(value.date_value().to_datetime())
        elif value.is_text():
            my_date = MyDate.parse(value.text_value())
            if my_date is None:
                return None, f"Function '{name}' parameter {index} is error!"
            dates.append(my_date.to_datetime())
        else:
            return None, f"Function '{name}' parameter {index} is error!"
    return dates, None


def _whole_days(start, end) -> int:
    """``Days.daysBetween(start, end).getDays()`` - truncated towards zero."""
    delta = end - start
    seconds = delta.days * 86400 + delta.seconds
    if seconds >= 0:
        return seconds // 86400
    return -((-seconds) // 86400)


def _arg_number(function, engine, tp, index: int) -> Operand:
    """``GetNumber_N`` for arities the base shells do not provide (e.g. the sixth)."""
    value = getattr(function, f"func{index}").evaluate(engine, tp)
    return value if value.is_number() else function._convert_to_number(value, index)


def _arg_array(function, engine, tp, index: int) -> Operand:
    """``GetArray_N`` for arities the base shells do not provide (e.g. the third)."""
    value = getattr(function, f"func{index}").evaluate(engine, tp)
    return value if value.is_array() else function._convert_to_array(value, index)


# ---------------------------------------------------------------------------
# time value of money
# ---------------------------------------------------------------------------


@register("FV")
class Function_FV(Function_5):
    def __init__(self, funcs):
        _require_args(funcs, "FV", 3, 5)
        super().__init__(funcs)

    def name(self) -> str:
        return "FV"

    def evaluate(self, engine, tp) -> Operand:
        rate_arg = self._get_number_1(engine, tp)
        if rate_arg.is_error_or_none():
            return rate_arg
        rate = rate_arg.number_value()

        nper_arg = self._get_number_2(engine, tp)
        if nper_arg.is_error_or_none():
            return nper_arg
        nper = nper_arg.number_value()

        pmt_arg = self._get_number_3(engine, tp)
        if pmt_arg.is_error_or_none():
            return pmt_arg
        pmt = pmt_arg.number_value()

        pv = ZERO
        if self.func4 is not None:
            pv_arg = self._get_number_4(engine, tp)
            if pv_arg.is_error_or_none():
                return pv_arg
            pv = pv_arg.number_value()

        type_ = 0
        if self.func5 is not None:
            type_arg = self._get_number_5(engine, tp)
            if type_arg.is_error_or_none():
                return type_arg
            type_ = type_arg.int_value()
            if type_ != 0 and type_ != 1:
                return self._parameter_error(5)

        if rate == ZERO:
            return Operand.create((pmt * nper).copy_negate() - pv)

        factor = big_pow(ONE + rate, nper)
        if type_ == 1:
            fv = (pv * factor).copy_negate() - divide(
                pmt * (ONE + rate) * (factor - ONE), rate
            )
        else:
            fv = (pv * factor).copy_negate() - divide(pmt * (factor - ONE), rate)
        return Operand.create(fv)

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


@register("PV")
class Function_PV(Function_5):
    def __init__(self, funcs):
        _require_args(funcs, "PV", 3, 5)
        super().__init__(funcs)

    def name(self) -> str:
        return "PV"

    def evaluate(self, engine, tp) -> Operand:
        rate_arg = self._get_number_1(engine, tp)
        if rate_arg.is_error_or_none():
            return rate_arg
        rate = rate_arg.number_value()

        nper_arg = self._get_number_2(engine, tp)
        if nper_arg.is_error_or_none():
            return nper_arg
        nper = nper_arg.number_value()

        pmt_arg = self._get_number_3(engine, tp)
        if pmt_arg.is_error_or_none():
            return pmt_arg
        pmt = pmt_arg.number_value()

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

        if rate == ZERO:
            return Operand.create((pmt * nper).copy_negate() - fv)

        factor = big_pow(ONE + rate, nper)
        if type_ == 1:
            pv = divide(
                fv + divide(pmt * (ONE + rate) * (factor - ONE), rate), factor
            ).copy_negate()
        else:
            pv = divide(fv + divide(pmt * (factor - ONE), rate), factor).copy_negate()
        return Operand.create(pv)

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


@register("PMT")
class Function_PMT(Function_5):
    def __init__(self, funcs):
        _require_args(funcs, "PMT", 3, 5)
        super().__init__(funcs)

    def name(self) -> str:
        return "PMT"

    def evaluate(self, engine, tp) -> Operand:
        rate_arg = self._get_number_1(engine, tp)
        if rate_arg.is_error_or_none():
            return rate_arg
        rate = rate_arg.number_value()

        nper_arg = self._get_number_2(engine, tp)
        if nper_arg.is_error_or_none():
            return nper_arg
        nper = nper_arg.number_value()

        pv_arg = self._get_number_3(engine, tp)
        if pv_arg.is_error_or_none():
            return pv_arg
        pv = pv_arg.number_value()

        if nper == ZERO:
            return self._div0_error()

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
            return Operand.create(divide((pv + fv).copy_negate(), nper))

        return Operand.create(calculate_pmt(rate, nper, pv, fv, type_))

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


@register("IPMT")
class Function_IPMT(Function_6):
    def __init__(self, funcs):
        _require_args(funcs, "IPMT", 4, 6)
        super().__init__(funcs)

    def name(self) -> str:
        return "IPMT"

    def evaluate(self, engine, tp) -> Operand:
        rate_arg = self._get_number_1(engine, tp)
        if rate_arg.is_error_or_none():
            return rate_arg
        rate = rate_arg.number_value()

        per_arg = self._get_number_2(engine, tp)
        if per_arg.is_error_or_none():
            return per_arg
        per = per_arg.number_value()

        nper_arg = self._get_number_3(engine, tp)
        if nper_arg.is_error_or_none():
            return nper_arg
        nper = nper_arg.number_value()

        pv_arg = self._get_number_4(engine, tp)
        if pv_arg.is_error_or_none():
            return pv_arg
        pv = pv_arg.number_value()

        fv = ZERO
        if self.func5 is not None:
            fv_arg = self._get_number_5(engine, tp)
            if fv_arg.is_error_or_none():
                return fv_arg
            fv = fv_arg.number_value()

        type_ = 0
        if self.func6 is not None:
            type_arg = self._get_number_6(engine, tp)
            if type_arg.is_error_or_none():
                return type_arg
            type_ = type_arg.int_value()
            if type_ != 0 and type_ != 1:
                return self._parameter_error(6)

        if per < ONE or per > nper:
            return self._parameter_error(2)

        if rate == ZERO:
            return Operand.create(ZERO)

        return Operand.create(calculate_ipmt(rate, per, nper, pv, fv, type_))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func4._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func5 is not None:
            self.func5._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func6 is not None:
            self.func6._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("PPMT")
class Function_PPMT(Function_6):
    def __init__(self, funcs):
        _require_args(funcs, "PPMT", 4, 6)
        super().__init__(funcs)

    def name(self) -> str:
        return "PPMT"

    def evaluate(self, engine, tp) -> Operand:
        rate_arg = self._get_number_1(engine, tp)
        if rate_arg.is_error_or_none():
            return rate_arg
        rate = rate_arg.number_value()

        per_arg = self._get_number_2(engine, tp)
        if per_arg.is_error_or_none():
            return per_arg
        per = per_arg.number_value()

        nper_arg = self._get_number_3(engine, tp)
        if nper_arg.is_error_or_none():
            return nper_arg
        nper = nper_arg.number_value()

        pv_arg = self._get_number_4(engine, tp)
        if pv_arg.is_error_or_none():
            return pv_arg
        pv = pv_arg.number_value()

        fv = ZERO
        if self.func5 is not None:
            fv_arg = self._get_number_5(engine, tp)
            if fv_arg.is_error_or_none():
                return fv_arg
            fv = fv_arg.number_value()

        type_ = 0
        if self.func6 is not None:
            type_arg = self._get_number_6(engine, tp)
            if type_arg.is_error_or_none():
                return type_arg
            type_ = type_arg.int_value()
            if type_ != 0 and type_ != 1:
                return self._parameter_error(6)

        if per < ONE or per > nper:
            return self._parameter_error(2)

        pmt = calculate_pmt(rate, nper, pv, fv, type_)
        ipmt = calculate_ipmt(rate, per, nper, pv, fv, type_)
        return Operand.create(pmt - ipmt)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func4._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func5 is not None:
            self.func5._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func6 is not None:
            self.func6._get_parameter_types(none_engine, result, OperandType.NUMBER)


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


# ---------------------------------------------------------------------------
# cash-flow analysis
# ---------------------------------------------------------------------------


@register("NPV")
class Function_NPV(Function_N):
    def __init__(self, funcs):
        _require_args(funcs, "NPV", 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "NPV"

    def evaluate(self, engine, tp) -> Operand:
        rate_arg = self._get_number(engine, tp, 0)
        if rate_arg.is_error_or_none():
            return rate_arg
        rate = rate_arg.number_value()
        if rate == ONE.copy_negate():
            return self._div0_error()

        values = []
        for i in range(1, len(self.funcs)):
            arg = self._get_number(engine, tp, i)
            if arg.is_error_or_none():
                return arg
            values.append(arg.number_value())

        npv = ZERO
        for i, value in enumerate(values):
            factor = from_float(math.pow(float(ONE + rate), i + 1))
            npv += divide(value, factor)
        return Operand.create(npv)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        for func in self.funcs:
            func._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("IRR")
class Function_IRR(Function_N):
    def __init__(self, funcs):
        _require_args(funcs, "IRR", 1, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "IRR"

    def evaluate(self, engine, tp) -> Operand:
        values_arg = self._get_array(engine, tp, 0)
        if values_arg.is_error_or_none():
            return values_arg
        values, error = _numbers_from_array(values_arg, "IRR", 1)
        if error is not None:
            return error

        if not values:
            return self._parameter_error(1)

        has_positive = any(v > ZERO for v in values)
        has_negative = any(v < ZERO for v in values)
        if not has_positive or not has_negative:
            return self._parameter_error(1)

        guess = from_float(0.1)
        if len(self.funcs) > 1:
            guess_arg = self._get_number(engine, tp, 1)
            if guess_arg.is_error_or_none():
                return guess_arg
            guess = guess_arg.number_value()

        try:
            return Operand.create(self._newton_raphson(values, guess))
        except (ArithmeticError, ValueError, OverflowError):
            return self._function_error()

    @staticmethod
    def _newton_raphson(values, guess):
        rate = guess
        for _ in range(_MAX_ITERATIONS):
            npv = ZERO
            dnpv = ZERO
            one_plus_rate = ONE + rate
            factor = ONE
            for i, value in enumerate(values):
                npv += divide(value, factor)
                dnpv -= divide(Decimal(i) * value, factor * one_plus_rate)
                factor = factor * one_plus_rate

            if abs(dnpv) < _EPS_SLOPE:
                break
            new_rate = rate - divide(npv, dnpv)
            if abs(new_rate - rate) < _EPS_STEP:
                return new_rate
            rate = new_rate
        raise ValueError("IRR calculation did not converge")

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.funcs[0]._get_parameter_types(none_engine, result, OperandType.ARRAY)
        if len(self.funcs) > 1:
            self.funcs[1]._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("MIRR")
class Function_MIRR(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "MIRR", 3, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "MIRR"

    def evaluate(self, engine, tp) -> Operand:
        values_arg = self._get_array_1(engine, tp)
        if values_arg.is_error_or_none():
            return values_arg
        values, error = _numbers_from_array(values_arg, "MIRR", 1)
        if error is not None:
            return error

        finance_rate_arg = self._get_number_2(engine, tp)
        if finance_rate_arg.is_error_or_none():
            return finance_rate_arg
        finance_rate = finance_rate_arg.number_value()

        reinvest_rate_arg = self._get_number_3(engine, tp)
        if reinvest_rate_arg.is_error_or_none():
            return reinvest_rate_arg
        reinvest_rate = reinvest_rate_arg.number_value()

        n = len(values)
        if n == 0:
            return self._parameter_error(1)
        if n == 1:
            return self._div0_error()

        npv_negative = ZERO
        npv_positive = ZERO
        for i, value in enumerate(values):
            if value < ZERO:
                factor = from_float(math.pow(float(ONE + finance_rate), i))
                npv_negative += divide(value, factor)
            else:
                factor = from_float(math.pow(float(ONE + reinvest_rate), n - 1 - i))
                npv_positive += value * factor

        if npv_negative == ZERO or npv_positive == ZERO:
            return self._div0_error()

        base = divide(npv_positive.copy_negate(), npv_negative)
        exponent = divide(ONE, Decimal(n - 1))
        mirr = from_float(math.pow(float(base), float(exponent))) - ONE
        return Operand.create(mirr)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.ARRAY)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("XIRR")
class Function_XIRR(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "XIRR", 2, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "XIRR"

    def evaluate(self, engine, tp) -> Operand:
        values_arg = self._get_array_1(engine, tp)
        if values_arg.is_error_or_none():
            return values_arg
        values, error = _numbers_from_array(values_arg, "XIRR", 1)
        if error is not None:
            return error

        dates_arg = self._get_array_2(engine, tp)
        if dates_arg.is_error_or_none():
            return dates_arg
        dates, error = _dates_from_array(dates_arg, "XIRR", 2)
        if error is not None:
            return Operand.error(error)

        if len(values) != len(dates) or len(values) < 2:
            return self._function_error()

        guess = from_float(0.1)
        if self.func3 is not None:
            guess_arg = self._get_number_3(engine, tp)
            if guess_arg.is_error_or_none():
                return guess_arg
            guess = guess_arg.number_value()

        try:
            return Operand.create(self._newton_raphson(values, dates, guess))
        except (ArithmeticError, ValueError, OverflowError):
            return self._function_error()

    @staticmethod
    def _newton_raphson(values, dates, rate):
        base_date = dates[0]
        exps = [divide(Decimal(_whole_days(base_date, d)), _DAYS_IN_YEAR) for d in dates]

        for _ in range(_MAX_ITERATIONS):
            npv = ZERO
            dnpv = ZERO
            one_plus_rate = ONE + rate
            log_base = math.log(float(one_plus_rate))
            for i, value in enumerate(values):
                factor = from_float(math.exp(float(exps[i]) * log_base))
                npv += divide(value, factor)
                dnpv -= divide(value * exps[i], factor * one_plus_rate)

            if abs(dnpv) < _EPS_SLOPE:
                break
            new_rate = rate - divide(npv, dnpv)
            if abs(new_rate - rate) < _EPS_STEP:
                return new_rate
            rate = new_rate
        raise ValueError("XIRR calculation did not converge")

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.ARRAY)
        self.func2._get_parameter_types(none_engine, result, OperandType.ARRAY)
        if self.func3 is not None:
            self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("XNPV")
class Function_XNPV(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "XNPV", 3, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "XNPV"

    def evaluate(self, engine, tp) -> Operand:
        rate_arg = self._get_number_1(engine, tp)
        if rate_arg.is_error_or_none():
            return rate_arg
        rate = rate_arg.number_value()
        if rate == ONE.copy_negate():
            return self._div0_error()

        values_arg = self._get_array_2(engine, tp)
        if values_arg.is_error_or_none():
            return values_arg
        values, error = _numbers_from_array(values_arg, "XNPV", 2)
        if error is not None:
            return error

        dates_arg = _arg_array(self, engine, tp, 3)
        if dates_arg.is_error_or_none():
            return dates_arg
        dates, error = _dates_from_array(dates_arg, "XNPV", 3)
        if error is not None:
            return Operand.error(error)

        if len(values) != len(dates):
            return self._function_error()
        if len(values) == 0:
            return self._parameter_error(2)

        base_date = dates[0]
        xnpv = ZERO
        for i, value in enumerate(values):
            exponent = divide(Decimal(_whole_days(base_date, dates[i])), _DAYS_IN_YEAR)
            factor = from_float(math.pow(float(ONE + rate), float(exponent)))
            xnpv += divide(value, factor)
        return Operand.create(xnpv)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.ARRAY)
        self.func3._get_parameter_types(none_engine, result, OperandType.ARRAY)


# ---------------------------------------------------------------------------
# depreciation
# ---------------------------------------------------------------------------


@register("SLN")
class Function_SLN(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "SLN", 3, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "SLN"

    def evaluate(self, engine, tp) -> Operand:
        cost_arg = self._get_number_1(engine, tp)
        if cost_arg.is_error_or_none():
            return cost_arg
        cost = cost_arg.number_value()

        salvage_arg = self._get_number_2(engine, tp)
        if salvage_arg.is_error_or_none():
            return salvage_arg
        salvage = salvage_arg.number_value()

        life_arg = self._get_number_3(engine, tp)
        if life_arg.is_error_or_none():
            return life_arg
        life = life_arg.number_value()

        if life == ZERO:
            return self._div0_error()
        return Operand.create(divide(cost - salvage, life))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("SYD")
class Function_SYD(Function_4):
    def __init__(self, funcs):
        _require_args(funcs, "SYD", 4, 4)
        super().__init__(funcs)

    def name(self) -> str:
        return "SYD"

    def evaluate(self, engine, tp) -> Operand:
        cost_arg = self._get_number_1(engine, tp)
        if cost_arg.is_error_or_none():
            return cost_arg
        cost = cost_arg.number_value()

        salvage_arg = self._get_number_2(engine, tp)
        if salvage_arg.is_error_or_none():
            return salvage_arg
        salvage = salvage_arg.number_value()

        life_arg = self._get_number_3(engine, tp)
        if life_arg.is_error_or_none():
            return life_arg
        life = life_arg.number_value()

        period_arg = self._get_number_4(engine, tp)
        if period_arg.is_error_or_none():
            return period_arg
        period = period_arg.number_value()

        if life == ZERO:
            return self._div0_error()
        if life <= ZERO:
            return self._parameter_error(3)
        if period < ONE or period > life:
            return self._parameter_error(4)

        numerator = (cost - salvage) * (life - period + ONE) * Decimal(2)
        denominator = life * (life + ONE)
        return Operand.create(divide(numerator, denominator))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func4._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("DB")
class Function_DB(Function_5):
    def __init__(self, funcs):
        _require_args(funcs, "DB", 4, 5)
        super().__init__(funcs)

    def name(self) -> str:
        return "DB"

    def evaluate(self, engine, tp) -> Operand:
        cost_arg = self._get_number_1(engine, tp)
        if cost_arg.is_error_or_none():
            return cost_arg
        cost = cost_arg.number_value()

        salvage_arg = self._get_number_2(engine, tp)
        if salvage_arg.is_error_or_none():
            return salvage_arg
        salvage = salvage_arg.number_value()

        life_arg = self._get_number_3(engine, tp)
        if life_arg.is_error_or_none():
            return life_arg
        life = life_arg.number_value()

        period_arg = self._get_number_4(engine, tp)
        if period_arg.is_error_or_none():
            return period_arg
        period = period_arg.number_value()

        month = 12
        if self.func5 is not None:
            month_arg = self._get_number_5(engine, tp)
            if month_arg.is_error_or_none():
                return month_arg
            month = month_arg.int_value()
            if month < 1 or month > 12:
                return self._parameter_error(5)

        if life == ZERO or cost == ZERO:
            return self._div0_error()

        total_periods = int(life) if month == 12 else int(life) + 1
        if period < ONE or period > Decimal(total_periods):
            return self._parameter_error(4)
        if life < ONE:
            return self._parameter_error(3)

        rate = ONE - from_float(
            math.pow(
                float(divide(salvage, cost)),
                float(divide(ONE, life)),
            )
        )
        rate = rate.quantize(Decimal("0.001"), rounding=ROUND_HALF_UP)

        remaining_cost = cost
        depreciation = ZERO
        for i in range(1, int(period) + 1):
            if i == 1:
                depreciation = divide(cost * rate * Decimal(month), Decimal(12))
            elif i == total_periods and month != 12:
                depreciation = divide(
                    remaining_cost * rate * Decimal(12 - month), Decimal(12)
                )
            else:
                depreciation = remaining_cost * rate
            remaining_cost = remaining_cost - depreciation
        return Operand.create(depreciation)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func4._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func5 is not None:
            self.func5._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("DDB")
class Function_DDB(Function_5):
    def __init__(self, funcs):
        _require_args(funcs, "DDB", 4, 5)
        super().__init__(funcs)

    def name(self) -> str:
        return "DDB"

    def evaluate(self, engine, tp) -> Operand:
        cost_arg = self._get_number_1(engine, tp)
        if cost_arg.is_error_or_none():
            return cost_arg
        cost = cost_arg.number_value()

        salvage_arg = self._get_number_2(engine, tp)
        if salvage_arg.is_error_or_none():
            return salvage_arg
        salvage = salvage_arg.number_value()

        life_arg = self._get_number_3(engine, tp)
        if life_arg.is_error_or_none():
            return life_arg
        life = life_arg.number_value()

        period_arg = self._get_number_4(engine, tp)
        if period_arg.is_error_or_none():
            return period_arg
        period = period_arg.number_value()

        factor = Decimal(2)
        if self.func5 is not None:
            factor_arg = self._get_number_5(engine, tp)
            if factor_arg.is_error_or_none():
                return factor_arg
            factor = factor_arg.number_value()

        if life == ZERO or factor == ZERO:
            return self._div0_error()
        if life <= ZERO:
            return self._parameter_error(3)
        if period < ONE or period > life:
            return self._parameter_error(4)

        remaining_cost = cost
        i = 1
        while Decimal(i) < period:
            ddb = divide(remaining_cost * factor, life)
            max_depreciation = remaining_cost - salvage
            if ddb > max_depreciation:
                ddb = max_depreciation
            remaining_cost = remaining_cost - ddb
            if remaining_cost <= salvage:
                break
            i += 1

        depreciation = divide(remaining_cost * factor, life)
        max_depreciation = remaining_cost - salvage
        if depreciation > max_depreciation:
            depreciation = max_depreciation
        return Operand.create(depreciation)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func4._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func5 is not None:
            self.func5._get_parameter_types(none_engine, result, OperandType.NUMBER)
