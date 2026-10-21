"""math_sum: shared helpers and base classes (kept out of the per-class files)."""
from __future__ import annotations
import math
from decimal import Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_n import Function_N
from toolgood_algorithm.internals.functions.function_util import flatten_to_number_list
from toolgood_algorithm.internals.numeric import ZERO, divide
from toolgood_algorithm.operands.operand import Operand


class _SumFunctionN(Function_N):
    """`Function_N` shell that validates arity and evaluates every argument."""

    def __init__(self, funcs, name: str, minimum=1, maximum=None):
        _require_args(funcs, name, minimum, maximum)
        super().__init__(funcs)

    def _collect(self, engine, tp):
        args: list[Operand] = []
        error = self.try_evaluate_all(engine, tp, args)
        if error is not None:
            return error, None
        return None, args

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER


class _SumXY(_SumFunctionN):
    """Shared shell for the SUMX2MY2/SUMX2PY2/SUMXMY2 pairwise sums."""

    def __init__(self, funcs, name: str):
        super().__init__(funcs, name, 2, 2)

    def _pair_values(self, engine, tp):
        x_arg = self._get_array(engine, tp, 0)
        if x_arg.is_error_or_none():
            return x_arg, None, None
        x_values: list[Decimal] = []
        for item in x_arg.array_value():
            if item.is_number():
                x_values.append(item.number_value())

        y_arg = self._get_array(engine, tp, 1)
        if y_arg.is_error_or_none():
            return y_arg, None, None
        y_values: list[Decimal] = []
        for item in y_arg.array_value():
            if item.is_number():
                y_values.append(item.number_value())
        return None, x_values, y_values

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.funcs[0]._get_parameter_types(none_engine, result, OperandType.ARRAY)
        self.funcs[1]._get_parameter_types(none_engine, result, OperandType.ARRAY)

_TWO = Decimal(2)


def _sum_values(values) -> Decimal:
    total = ZERO
    for value in values:
        total += value
    return total


def _welford(values):
    """Streaming mean/M2 accumulation, matching the ``BigDecimal`` sequence."""
    mean = ZERO
    m2 = ZERO
    count = 0
    for value in values:
        count += 1
        delta = value - mean
        mean = mean + divide(delta, Decimal(count))
        m2 = m2 + delta * (value - mean)
    return m2, count


def _flatten_numeric_args(args):
    """Collect numbers from the mixed argument shapes; ``None`` on error."""
    values: list[Decimal] = []
    for item in args:
        if item.is_array():
            for elem in item.array_value():
                if elem.is_number():
                    values.append(elem.number_value())
                elif elem.is_array() or elem.is_json():
                    sub: list[Decimal] = []
                    if not flatten_to_number_list(elem, sub):
                        return None
                    values.extend(sub)
                else:
                    converted = elem.to_number(None)
                    if converted.is_error():
                        return None
                    values.append(converted.number_value())
        elif item.is_number():
            values.append(item.number_value())
        elif item.is_json():
            sub = []
            if not flatten_to_number_list(item, sub):
                return None
            values.extend(sub)
        else:
            converted = item.to_number(None)
            if converted.is_error():
                return None
            values.append(converted.number_value())
    return values


def _extreme_value(args, maximum: bool):
    values = _flatten_numeric_args(args)
    if values is None or not values:
        return None
    return max(values) if maximum else min(values)


def _try_parse_decimal(text: str):
    try:
        value = Decimal(text)
    except Exception:  # noqa: BLE001 - mirrors the Java ``new BigDecimal`` catch
        return None
    return value if value.is_finite() else None


def _arg_array(self, engine, tp, index: int) -> Operand:
    """``GetArray_<i>`` for arities the framework base classes don't expose."""
    value = getattr(self, f"func{index}").evaluate(engine, tp)
    return value if value.is_array() else self._convert_to_array(value, index)


def _pow(base: float, exponent: float) -> float:
    """``Math.pow``: invalid input yields NaN, overflow yields Infinity."""
    try:
        return math.pow(base, exponent)
    except ValueError:
        return float("nan")
    except OverflowError:
        return float("inf")


def _extreme_parameter_types(func, none_engine, result) -> None:
    """Shared MAX/MIN parameter typing (single array vs. text/number pair)."""
    funcs = func.funcs
    if len(funcs) == 1:
        funcs[0]._get_parameter_types(none_engine, result, OperandType.ARRAY)
    elif len(funcs) == 2:
        t1 = funcs[0].get_result_type()
        t2 = funcs[1].get_result_type()
        if t1 == OperandType.NONE and t2 == OperandType.NUMBER:
            p = none_engine.evaluate(funcs[1]).to_text(None)
            if not p.is_error_or_none():
                funcs[0]._get_parameter_types(none_engine, result, t2, func.name(), p.text_value())
                funcs[1]._get_parameter_types(none_engine, result, t2)
                return
        elif t1 == OperandType.NUMBER and t2 == OperandType.NONE:
            p = none_engine.evaluate(funcs[0]).to_text(None)
            if not p.is_error_or_none():
                funcs[1]._get_parameter_types(none_engine, result, t1, func.name(), p.text_value())
                funcs[0]._get_parameter_types(none_engine, result, t1)
                return
        funcs[0]._get_parameter_types(none_engine, result, OperandType.NUMBER)
        funcs[1]._get_parameter_types(none_engine, result, OperandType.NUMBER)
    else:
        for item in funcs:
            item._get_parameter_types(none_engine, result, OperandType.NUMBER)
