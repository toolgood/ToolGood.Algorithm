"""financial: shared helpers and base classes (kept out of the per-class files)."""
from __future__ import annotations
from toolgood_algorithm.operands.my_date import MyDate
from toolgood_algorithm.internals.numeric import ONE, ZERO, divide
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.math_base import from_float

_EPS_STEP = from_float(1e-10)

_EPS_SLOPE = from_float(1e-12)

_MAX_ITERATIONS = 100

_DAYS_IN_YEAR = from_float(365.0)


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
