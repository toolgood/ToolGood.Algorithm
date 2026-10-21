"""Statistical aggregation functions (SUM, AVERAGE, STDEV, RANK, ...)."""
from __future__ import annotations

import math
from decimal import Decimal, ROUND_HALF_UP, localcontext

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import Function_2, Function_3, Function_N, _require_args
from toolgood_algorithm.internals.functions.function_util import (
    flatten_to_big_decimal_list,
    flatten_to_list,
    flatten_to_number_list,
    get_count_if,
    get_count_if_op,
    get_rank,
    get_sum_if,
    get_sum_if_op,
    parse_sum_if_match,
    quick_select,
)
from toolgood_algorithm.math_net.excel_functions import percentile, percent_rank, poisson, quartile
from toolgood_algorithm.internals.numeric import ZERO, divide
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float

_TWO = Decimal(2)


# ---------------------------------------------------------------------------
# helpers
# ---------------------------------------------------------------------------
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


# ---------------------------------------------------------------------------
# simple statistics
# ---------------------------------------------------------------------------
@register("AVEDEV")
class Function_AVEDEV(_SumFunctionN):
    def __init__(self, funcs):
        super().__init__(funcs, "AveDev", 1)

    def name(self) -> str:
        return "AveDev"

    def evaluate(self, engine, tp) -> Operand:
        error, args = self._collect(engine, tp)
        if error is not None:
            return error

        values: list[Decimal] = []
        if not flatten_to_big_decimal_list(args, values):
            return self._function_error()
        if not values:
            return Operand.ZERO

        avg = divide(_sum_values(values), Decimal(len(values)))
        total = ZERO
        for value in values:
            total += abs(value - avg)
        return Operand.create(divide(total, Decimal(len(values))))

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        if len(self.funcs) == 1:
            self.funcs[0]._get_parameter_types(none_engine, result, OperandType.ARRAY)
        else:
            for func in self.funcs:
                func._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("AVERAGE")
class Function_AVERAGE(_SumFunctionN):
    def __init__(self, funcs):
        super().__init__(funcs, "Average", 1)

    def name(self) -> str:
        return "Average"

    def evaluate(self, engine, tp) -> Operand:
        error, args = self._collect(engine, tp)
        if error is not None:
            return error

        values = _flatten_numeric_args(args)
        if values is None:
            return self._function_error()
        if not values:
            return self._div0_error()
        return Operand.create(divide(_sum_values(values), Decimal(len(values))))

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        if len(self.funcs) == 1:
            self.funcs[0]._get_parameter_types(none_engine, result, OperandType.ARRAY)
        else:
            for func in self.funcs:
                func._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("AVERAGEIF")
class Function_AVERAGEIF(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "AverageIf", 2, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "AverageIf"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_array_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self.func2.evaluate(engine, tp)
        if args2.is_error_or_none():
            return args2

        values: list[Decimal] = []
        if not flatten_to_number_list(args1, values):
            return self._parameter_error(1)

        if self.func3 is not None:
            args3 = _arg_array(self, engine, tp, 3)
            if args3.is_error_or_none():
                return args3
            sums: list[Decimal] = []
            if not flatten_to_number_list(args3, sums):
                return self._parameter_error(3)
        else:
            sums = values

        count = 0
        total = ZERO
        if args2.is_number():
            number = args2.number_value()
            count = get_count_if(values, number)
            total = get_sum_if(values, number, sums)
        elif args2.is_text():
            text = args2.text_value().strip()
            number = _try_parse_decimal(text)
            if number is not None:
                count = get_count_if(values, number)
                total = get_sum_if(values, number, sums)
            else:
                match = parse_sum_if_match(text)
                if match is None:
                    return self._parameter_error(2)
                count = get_count_if_op(values, match[0], match[1])
                total = get_sum_if_op(values, match[0], match[1], sums)
        else:
            return self._parameter_error(2)

        if count == 0:
            return self._div0_error()
        return Operand.create(divide(total, Decimal(count)))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.ARRAY)
        self.func2._get_parameter_types(none_engine, result, OperandType.NONE)
        if self.func3 is not None:
            self.func3._get_parameter_types(none_engine, result, OperandType.ARRAY)


@register("CORREL")
class Function_CORREL(_SumFunctionN):
    def __init__(self, funcs):
        super().__init__(funcs, "CORREL", 2, 2)

    def name(self) -> str:
        return "CORREL"

    def evaluate(self, engine, tp) -> Operand:
        array1 = self._get_array(engine, tp, 0)
        if array1.is_error_or_none():
            return array1
        array2 = self._get_array(engine, tp, 1)
        if array2.is_error_or_none():
            return array2

        x_values: list[Decimal] = []
        for item in array1.array_value():
            if item.is_number():
                x_values.append(item.number_value())
            else:
                return self._parameter_error(1)

        y_values: list[Decimal] = []
        for item in array2.array_value():
            if item.is_number():
                y_values.append(item.number_value())
            else:
                return self._parameter_error(2)

        if len(x_values) != len(y_values) or len(x_values) < 2:
            return self._function_error()

        n = len(x_values)
        sum_x = 0.0
        sum_y = 0.0
        for i in range(n):
            sum_x += float(x_values[i])
            sum_y += float(y_values[i])

        mean_x = sum_x / n
        mean_y = sum_y / n

        numerator = 0.0
        denom_x = 0.0
        denom_y = 0.0
        for i in range(n):
            dx = float(x_values[i]) - mean_x
            dy = float(y_values[i]) - mean_y
            numerator += dx * dy
            denom_x += dx * dx
            denom_y += dy * dy

        if denom_x == 0 or denom_y == 0:
            return self._div0_error()
        return Operand.create(from_float(numerator / math.sqrt(denom_x * denom_y)))

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.funcs[0]._get_parameter_types(none_engine, result, OperandType.ARRAY)
        self.funcs[1]._get_parameter_types(none_engine, result, OperandType.ARRAY)


@register("COUNT")
class Function_COUNT(_SumFunctionN):
    def __init__(self, funcs):
        super().__init__(funcs, "Count", 1)

    def name(self) -> str:
        return "Count"

    def evaluate(self, engine, tp) -> Operand:
        error, args = self._collect(engine, tp)
        if error is not None:
            return error

        items: list[Operand] = []
        if not flatten_to_list(args, items):
            return self._function_error()
        return Operand.create(len(items))

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        if len(self.funcs) == 1:
            self.funcs[0]._get_parameter_types(none_engine, result, OperandType.ARRAY)
        else:
            for func in self.funcs:
                func._get_parameter_types(none_engine, result, OperandType.NONE)


@register("COUNTIF")
class Function_COUNTIF(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "CountIf", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "CountIf"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_array_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self.func2.evaluate(engine, tp)
        if args2.is_error_or_none():
            return args2

        values: list[Decimal] = []
        if not flatten_to_number_list(args1, values):
            return self._parameter_error(1)

        if args2.is_number():
            count = get_count_if(values, args2.number_value())
        elif args2.is_text():
            text = args2.text_value().strip()
            number = _try_parse_decimal(text)
            if number is not None:
                count = get_count_if(values, number)
            else:
                match = parse_sum_if_match(text)
                if match is None:
                    return self._parameter_error(2)
                count = get_count_if_op(values, match[0], match[1])
        else:
            return self._parameter_error(2)
        return Operand.create(count)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.ARRAY)
        self.func2._get_parameter_types(none_engine, result, OperandType.NONE)


@register("COVAR")
class Function_COVAR(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "Covar", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Covar"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self.func1.evaluate(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self.func2.evaluate(engine, tp)
        if args2.is_error_or_none():
            return args2

        list1: list[Decimal] = []
        list2: list[Decimal] = []
        if not flatten_to_number_list(args1, list1):
            return self._parameter_error(1)
        if not flatten_to_number_list(args2, list2):
            return self._parameter_error(2)
        if len(list1) != len(list2) or len(list1) == 0:
            return Operand.error(f"Function '{self.name()}' parameter's count error!")

        mean1 = 0.0
        mean2 = 0.0
        c = 0.0
        for i in range(len(list1)):
            delta1 = float(list1[i]) - mean1
            delta2 = float(list2[i]) - mean2
            mean1 += delta1 / (i + 1)
            mean2 += delta2 / (i + 1)
            c += delta1 * (float(list2[i]) - mean2)
        return Operand.create(from_float(c / len(list1)))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.ARRAY)
        self.func2._get_parameter_types(none_engine, result, OperandType.ARRAY)


@register("COVARIANCES")
class Function_COVARIANCES(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "Covariances", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Covariances"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self.func1.evaluate(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self.func2.evaluate(engine, tp)
        if args2.is_error_or_none():
            return args2

        list1: list[Decimal] = []
        list2: list[Decimal] = []
        if not flatten_to_number_list(args1, list1):
            return self._parameter_error(1)
        if not flatten_to_number_list(args2, list2):
            return self._parameter_error(2)
        if len(list1) != len(list2) or len(list1) < 2:
            return Operand.error(f"Function '{self.name()}' parameter's count error!")

        mean1 = 0.0
        mean2 = 0.0
        c = 0.0
        for i in range(len(list1)):
            delta1 = float(list1[i]) - mean1
            delta2 = float(list2[i]) - mean2
            mean1 += delta1 / (i + 1)
            mean2 += delta2 / (i + 1)
            c += delta1 * (float(list2[i]) - mean2)
        return Operand.create(from_float(c / (len(list1) - 1)))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.ARRAY)
        self.func2._get_parameter_types(none_engine, result, OperandType.ARRAY)


@register("DEVSQ")
class Function_DEVSQ(_SumFunctionN):
    def __init__(self, funcs):
        super().__init__(funcs, "DevSq", 1)

    def name(self) -> str:
        return "DevSq"

    def evaluate(self, engine, tp) -> Operand:
        error, args = self._collect(engine, tp)
        if error is not None:
            return error

        values: list[Decimal] = []
        if not flatten_to_big_decimal_list(args, values):
            return self._function_error()
        if not values:
            return self._function_error()

        m2, _ = _welford(values)
        return Operand.create(m2)

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        if len(self.funcs) == 1:
            self.funcs[0]._get_parameter_types(none_engine, result, OperandType.ARRAY)
        else:
            for func in self.funcs:
                func._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("FORECAST")
class Function_FORECAST(_SumFunctionN):
    def __init__(self, funcs):
        super().__init__(funcs, "FORECAST", 3, 3)

    def name(self) -> str:
        return "FORECAST"

    def evaluate(self, engine, tp) -> Operand:
        x_arg = self._get_number(engine, tp, 0)
        if x_arg.is_error_or_none():
            return x_arg
        x = x_arg.number_value()

        y_arg = self._get_array(engine, tp, 1)
        if y_arg.is_error_or_none():
            return y_arg
        x_array_arg = self._get_array(engine, tp, 2)
        if x_array_arg.is_error_or_none():
            return x_array_arg

        y_values: list[Decimal] = []
        for item in y_arg.array_value():
            if item.is_number():
                y_values.append(item.number_value())

        x_values: list[Decimal] = []
        for item in x_array_arg.array_value():
            if item.is_number():
                x_values.append(item.number_value())

        if len(y_values) != len(x_values) or len(y_values) < 2:
            return self._function_error()

        n = len(y_values)
        sum_x = 0.0
        sum_y = 0.0
        sum_xy = 0.0
        sum_x2 = 0.0
        for i in range(n):
            xv = float(x_values[i])
            yv = float(y_values[i])
            sum_x += xv
            sum_y += yv
            sum_xy += xv * yv
            sum_x2 += xv * xv

        mean_x = sum_x / n
        mean_y = sum_y / n

        denominator = n * sum_x2 - sum_x * sum_x
        if denominator == 0:
            return self._div0_error()
        slope = (n * sum_xy - sum_x * sum_y) / denominator
        intercept = mean_y - slope * mean_x
        return Operand.create(from_float(intercept + slope * float(x)))

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.funcs[0]._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.funcs[1]._get_parameter_types(none_engine, result, OperandType.ARRAY)
        self.funcs[2]._get_parameter_types(none_engine, result, OperandType.ARRAY)


@register("GEOMEAN")
class Function_GEOMEAN(_SumFunctionN):
    def __init__(self, funcs):
        super().__init__(funcs, "GeoMean", 1)

    def name(self) -> str:
        return "GeoMean"

    def evaluate(self, engine, tp) -> Operand:
        error, args = self._collect(engine, tp)
        if error is not None:
            return error

        values: list[Decimal] = []
        if not flatten_to_big_decimal_list(args, values):
            return self._parameter_error(1)
        if not values:
            return self._parameter_error(1)

        product = 1.0
        for value in values:
            number = float(value)
            if number <= 0:
                return self._parameter_error(1)
            product *= number
        if product != product or product in (float("inf"), float("-inf")):
            return self._function_error()

        result = math.pow(product, 1.0 / len(values))
        if result != result or result in (float("inf"), float("-inf")):
            return self._function_error()
        return Operand.create(from_float(result))

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        if len(self.funcs) == 1:
            self.funcs[0]._get_parameter_types(none_engine, result, OperandType.ARRAY)
        else:
            for func in self.funcs:
                func._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("HARMEAN")
class Function_HARMEAN(_SumFunctionN):
    def __init__(self, funcs):
        super().__init__(funcs, "HarMean", 1)

    def name(self) -> str:
        return "HarMean"

    def evaluate(self, engine, tp) -> Operand:
        error, args = self._collect(engine, tp)
        if error is not None:
            return error

        values: list[Decimal] = []
        if not flatten_to_big_decimal_list(args, values):
            return self._function_error()
        if not values:
            return self._function_error()

        total = 0.0
        for value in values:
            if value == ZERO:
                return self._function_error()
            total += 1.0 / float(value)
        if total == 0:
            return self._function_error()
        return Operand.create(from_float(len(values) / total))

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        if len(self.funcs) == 1:
            self.funcs[0]._get_parameter_types(none_engine, result, OperandType.ARRAY)
        else:
            for func in self.funcs:
                func._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("INTERCEPT")
class Function_INTERCEPT(_SumFunctionN):
    def __init__(self, funcs):
        super().__init__(funcs, "INTERCEPT", 2, 2)

    def name(self) -> str:
        return "INTERCEPT"

    def evaluate(self, engine, tp) -> Operand:
        y_arg = self._get_array(engine, tp, 0)
        if y_arg.is_error_or_none():
            return y_arg
        x_arg = self._get_array(engine, tp, 1)
        if x_arg.is_error_or_none():
            return x_arg

        y_values: list[Decimal] = []
        for item in y_arg.array_value():
            if item.is_number():
                y_values.append(item.number_value())

        x_values: list[Decimal] = []
        for item in x_arg.array_value():
            if item.is_number():
                x_values.append(item.number_value())

        if len(y_values) != len(x_values) or len(y_values) < 2:
            return self._function_error()

        n = len(y_values)
        sum_x = 0.0
        sum_y = 0.0
        sum_xy = 0.0
        sum_x2 = 0.0
        for i in range(n):
            xv = float(x_values[i])
            yv = float(y_values[i])
            sum_x += xv
            sum_y += yv
            sum_xy += xv * yv
            sum_x2 += xv * xv

        mean_x = sum_x / n
        mean_y = sum_y / n

        denominator = n * sum_x2 - sum_x * sum_x
        if denominator == 0:
            return self._div0_error()
        slope = (n * sum_xy - sum_x * sum_y) / denominator
        return Operand.create(from_float(mean_y - slope * mean_x))

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.funcs[0]._get_parameter_types(none_engine, result, OperandType.ARRAY)
        self.funcs[1]._get_parameter_types(none_engine, result, OperandType.ARRAY)


@register("LARGE")
class Function_LARGE(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "Large", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Large"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._convert_to_array(self.func1.evaluate(engine, tp), 1)
        if args1.is_error_or_none():
            return args1
        args2 = self._convert_to_number(self.func2.evaluate(engine, tp), 2)
        if args2.is_error_or_none():
            return args2

        values: list[Decimal] = []
        if not flatten_to_number_list(args1, values):
            return self._parameter_error(1)
        if not values:
            return self._parameter_error(1)

        k = args2.int_value() - engine.ExcelIndex
        if k < 0 or k >= len(values):
            return self._parameter_error(2)
        return Operand.create(quick_select(values, k, True))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.ARRAY)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("MAX")
class Function_MAX(_SumFunctionN):
    def __init__(self, funcs):
        super().__init__(funcs, "Max", 1)

    def name(self) -> str:
        return "Max"

    def evaluate(self, engine, tp) -> Operand:
        error, args = self._collect(engine, tp)
        if error is not None:
            return error
        value = _extreme_value(args, True)
        if value is None:
            return self._function_error()
        return Operand.create(value)

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        _extreme_parameter_types(self, none_engine, result)


@register("MEDIAN")
class Function_MEDIAN(_SumFunctionN):
    def __init__(self, funcs):
        super().__init__(funcs, "Median", 1)

    def name(self) -> str:
        return "Median"

    def evaluate(self, engine, tp) -> Operand:
        error, args = self._collect(engine, tp)
        if error is not None:
            return error

        values: list[Decimal] = []
        if not flatten_to_big_decimal_list(args, values):
            return self._function_error()
        if not values:
            return self._function_error()

        n = len(values)
        if n == 1:
            return Operand.create(values[0])
        mid = n // 2
        if n % 2 == 0:
            a = quick_select(values, mid - 1, False)
            b = quick_select(values, mid, False)
            return Operand.create(divide(a + b, _TWO))
        return Operand.create(quick_select(values, mid, False))

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        if len(self.funcs) == 1:
            self.funcs[0]._get_parameter_types(none_engine, result, OperandType.ARRAY)
        else:
            for func in self.funcs:
                func._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("MIN")
class Function_MIN(_SumFunctionN):
    def __init__(self, funcs):
        super().__init__(funcs, "Min", 1)

    def name(self) -> str:
        return "Min"

    def evaluate(self, engine, tp) -> Operand:
        error, args = self._collect(engine, tp)
        if error is not None:
            return error
        value = _extreme_value(args, False)
        if value is None:
            return self._function_error()
        return Operand.create(value)

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        _extreme_parameter_types(self, none_engine, result)


@register("MODE")
class Function_MODE(_SumFunctionN):
    def __init__(self, funcs):
        super().__init__(funcs, "Mode", 1)

    def name(self) -> str:
        return "Mode"

    def evaluate(self, engine, tp) -> Operand:
        error, args = self._collect(engine, tp)
        if error is not None:
            return error

        values: list[Decimal] = []
        if not flatten_to_big_decimal_list(args, values):
            return self._function_error()
        if not values:
            return self._function_error()

        counts: dict[Decimal, int] = {}
        for value in values:
            counts[value] = counts.get(value, 0) + 1

        mode_key = ZERO
        max_count = 1
        for key, count in counts.items():
            if count > max_count:
                max_count = count
                mode_key = key
        if max_count == 1:
            return self._function_error()
        return Operand.create(mode_key)

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        if len(self.funcs) == 1:
            self.funcs[0]._get_parameter_types(none_engine, result, OperandType.ARRAY)
        else:
            for func in self.funcs:
                func._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("PEARSON")
class Function_PEARSON(_SumFunctionN):
    def __init__(self, funcs):
        super().__init__(funcs, "PEARSON", 2, 2)

    def name(self) -> str:
        return "PEARSON"

    def evaluate(self, engine, tp) -> Operand:
        array1 = self._get_array(engine, tp, 0)
        if array1.is_error_or_none():
            return array1
        array2 = self._get_array(engine, tp, 1)
        if array2.is_error_or_none():
            return array2

        x_values: list[Decimal] = []
        for item in array1.array_value():
            if item.is_number():
                x_values.append(item.number_value())

        y_values: list[Decimal] = []
        for item in array2.array_value():
            if item.is_number():
                y_values.append(item.number_value())

        if len(x_values) != len(y_values) or len(x_values) < 2:
            return self._function_error()

        n = len(x_values)
        sum_x = 0.0
        sum_y = 0.0
        for i in range(n):
            sum_x += float(x_values[i])
            sum_y += float(y_values[i])

        mean_x = sum_x / n
        mean_y = sum_y / n

        numerator = 0.0
        denom_x = 0.0
        denom_y = 0.0
        for i in range(n):
            dx = float(x_values[i]) - mean_x
            dy = float(y_values[i]) - mean_y
            numerator += dx * dy
            denom_x += dx * dx
            denom_y += dy * dy

        if denom_x == 0 or denom_y == 0:
            return self._div0_error()
        return Operand.create(from_float(numerator / math.sqrt(denom_x * denom_y)))

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.funcs[0]._get_parameter_types(none_engine, result, OperandType.ARRAY)
        self.funcs[1]._get_parameter_types(none_engine, result, OperandType.ARRAY)


@register("PERCENTILE")
class Function_PERCENTILE(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "Percentile", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Percentile"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_array_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2

        values: list[Decimal] = []
        if not flatten_to_number_list(args1, values):
            return self._parameter_error(1)
        if not values:
            return self._parameter_error(1)

        k = args2.number_value()
        if k < ZERO or k > Decimal(1):
            return self._parameter_error(2)
        try:
            return Operand.create(from_float(percentile([float(v) for v in values], float(k))))
        except Exception:  # noqa: BLE001 - mirrors the Java catch
            return self._parameter_error(2)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.ARRAY)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("PERCENTRANK")
class Function_PERCENTRANK(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "PercentRank", 2, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "PercentRank"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_array_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2

        values: list[Decimal] = []
        if not flatten_to_number_list(args1, values):
            return self._parameter_error(1)
        if not values:
            return self._parameter_error(1)

        value = percent_rank([float(v) for v in values], float(args2.number_value()))

        digits = 3
        if self.func3 is not None:
            args3 = self._get_number_3(engine, tp)
            if args3.is_error_or_none():
                return args3
            digits = args3.int_value()
            # aligned with Math.Round(decimal,int): more than 28 decimals is an error
            if digits < 0 or digits > 28:
                return self._parameter_error(3)

        with localcontext() as ctx:
            ctx.prec = 60
            result = from_float(value).quantize(Decimal(1).scaleb(-digits), rounding=ROUND_HALF_UP)
        return Operand.create(result)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.ARRAY)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func3 is not None:
            self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("POISSON")
class Function_POISSON(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "Poisson", 3, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "Poisson"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        args3 = self._get_boolean_3(engine, tp)
        if args3.is_error_or_none():
            return args3

        k = args1.int_value()
        if k < 0:
            return self._parameter_error(1)
        lam = args2.number_value()
        if not lam > ZERO:
            return self._parameter_error(2)
        return Operand.create(from_float(poisson(k, float(lam), args3.boolean_value())))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func3._get_parameter_types(none_engine, result, OperandType.BOOLEAN)


@register("QUARTILE")
class Function_QUARTILE(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "Quartile", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Quartile"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_array_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2

        values: list[Decimal] = []
        if not flatten_to_number_list(args1, values):
            return self._parameter_error(1)
        if not values:
            return self._parameter_error(1)

        quant = args2.int_value()
        if quant < 0 or quant > 4:
            return self._parameter_error(2)
        try:
            return Operand.create(from_float(quartile([float(v) for v in values], quant)))
        except Exception:  # noqa: BLE001 - mirrors the Java catch
            return self._parameter_error(2)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.ARRAY)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("RANK")
class Function_RANK(_SumFunctionN):
    def __init__(self, funcs):
        super().__init__(funcs, "RANK", 2, 3)

    def name(self) -> str:
        return "RANK"

    def evaluate(self, engine, tp) -> Operand:
        if len(self.funcs) < 2:
            return self._parameter_error(1)

        num_arg = self._get_number(engine, tp, 0)
        if num_arg.is_error_or_none():
            return num_arg
        num = num_arg.number_value()

        array_arg = self._get_array(engine, tp, 1)
        if array_arg.is_error_or_none():
            return array_arg

        order = 0
        if len(self.funcs) > 2:
            order_arg = self._get_number(engine, tp, 2)
            if order_arg.is_error_or_none():
                return order_arg
            order = order_arg.int_value()

        values: list[Decimal] = []
        for item in array_arg.array_value():
            if item.is_number():
                values.append(item.number_value())
        if not values:
            return self._parameter_error(2)

        rank = get_rank(values, num, order == 0)
        if rank == 0:
            return self._parameter_error(1)
        return Operand.create(rank)

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.funcs[0]._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.funcs[1]._get_parameter_types(none_engine, result, OperandType.ARRAY)
        if len(self.funcs) > 2:
            self.funcs[2]._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("SERIESSUM")
class Function_SERIESSUM(_SumFunctionN):
    def __init__(self, funcs):
        super().__init__(funcs, "SERIESSUM", 4, 4)

    def name(self) -> str:
        return "SERIESSUM"

    def evaluate(self, engine, tp) -> Operand:
        x_arg = self._get_number(engine, tp, 0)
        if x_arg.is_error_or_none():
            return x_arg
        n_arg = self._get_number(engine, tp, 1)
        if n_arg.is_error_or_none():
            return n_arg
        m_arg = self._get_number(engine, tp, 2)
        if m_arg.is_error_or_none():
            return m_arg
        coefficients = self._get_array(engine, tp, 3)
        if coefficients.is_error_or_none():
            return coefficients

        xd = float(x_arg.number_value())
        nd = float(n_arg.number_value())
        md = float(m_arg.number_value())

        result = 0.0
        for i, coef in enumerate(coefficients.array_value()):
            if coef.is_number():
                power = nd + i * md
                term = float(coef.number_value()) * _pow(xd, power)
                # NaN (negative base, fractional exponent) / Infinity fail like C#
                if term != term or term in (float("inf"), float("-inf")):
                    return self._function_error()
                result += term
        return Operand.create(from_float(result))

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.funcs[0]._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.funcs[1]._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.funcs[2]._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.funcs[3]._get_parameter_types(none_engine, result, OperandType.ARRAY)


@register("SLOPE")
class Function_SLOPE(_SumFunctionN):
    def __init__(self, funcs):
        super().__init__(funcs, "SLOPE", 2, 2)

    def name(self) -> str:
        return "SLOPE"

    def evaluate(self, engine, tp) -> Operand:
        y_arg = self._get_array(engine, tp, 0)
        if y_arg.is_error_or_none():
            return y_arg
        x_arg = self._get_array(engine, tp, 1)
        if x_arg.is_error_or_none():
            return x_arg

        y_values: list[Decimal] = []
        for item in y_arg.array_value():
            if item.is_number():
                y_values.append(item.number_value())

        x_values: list[Decimal] = []
        for item in x_arg.array_value():
            if item.is_number():
                x_values.append(item.number_value())

        if len(y_values) != len(x_values) or len(y_values) < 2:
            return self._function_error()

        n = len(y_values)
        sum_x = 0.0
        sum_y = 0.0
        sum_xy = 0.0
        sum_x2 = 0.0
        for i in range(n):
            xv = float(x_values[i])
            yv = float(y_values[i])
            sum_x += xv
            sum_y += yv
            sum_xy += xv * yv
            sum_x2 += xv * xv

        denominator = n * sum_x2 - sum_x * sum_x
        if denominator == 0:
            return self._div0_error()
        return Operand.create(from_float((n * sum_xy - sum_x * sum_y) / denominator))

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.funcs[0]._get_parameter_types(none_engine, result, OperandType.ARRAY)
        self.funcs[1]._get_parameter_types(none_engine, result, OperandType.ARRAY)


@register("SMALL")
class Function_SMALL(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "Small", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Small"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._convert_to_array(self.func1.evaluate(engine, tp), 1)
        if args1.is_error_or_none():
            return args1
        args2 = self._convert_to_number(self.func2.evaluate(engine, tp), 2)
        if args2.is_error_or_none():
            return args2

        values: list[Decimal] = []
        if not flatten_to_number_list(args1, values):
            return self._parameter_error(1)
        if not values:
            return self._parameter_error(1)

        k = args2.int_value() - engine.ExcelIndex
        if k < 0 or k >= len(values):
            return self._parameter_error(2)
        return Operand.create(quick_select(values, k, False))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.ARRAY)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("STDEV")
class Function_STDEV(_SumFunctionN):
    def __init__(self, funcs):
        super().__init__(funcs, "StDev", 1)

    def name(self) -> str:
        return "StDev"

    def evaluate(self, engine, tp) -> Operand:
        error, args = self._collect(engine, tp)
        if error is not None:
            return error

        values = _flatten_numeric_args(args)
        if values is None:
            return self._function_error()

        m2, count = _welford(values)
        if count <= 1:
            return self._function_error()
        variance = divide(m2, Decimal(count - 1))
        return Operand.create(from_float(math.sqrt(float(variance))))

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        if len(self.funcs) == 1:
            self.funcs[0]._get_parameter_types(none_engine, result, OperandType.ARRAY)
        else:
            for func in self.funcs:
                func._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("STDEVP")
class Function_STDEVP(_SumFunctionN):
    def __init__(self, funcs):
        super().__init__(funcs, "StDevP", 1)

    def name(self) -> str:
        return "StDevP"

    def evaluate(self, engine, tp) -> Operand:
        error, args = self._collect(engine, tp)
        if error is not None:
            return error

        values: list[Decimal] = []
        if not flatten_to_big_decimal_list(args, values):
            return self._function_error()
        if not values:
            return self._function_error()

        m2, _ = _welford(values)
        variance = divide(m2, Decimal(len(values)))
        return Operand.create(from_float(math.sqrt(float(variance))))

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        if len(self.funcs) == 1:
            self.funcs[0]._get_parameter_types(none_engine, result, OperandType.ARRAY)
        else:
            for func in self.funcs:
                func._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("SUM")
class Function_SUM(_SumFunctionN):
    def __init__(self, funcs):
        super().__init__(funcs, "Sum", 1)

    def name(self) -> str:
        return "Sum"

    def evaluate(self, engine, tp) -> Operand:
        error, args = self._collect(engine, tp)
        if error is not None:
            return error
        values = _flatten_numeric_args(args)
        if values is None:
            return self._function_error()
        return Operand.create(_sum_values(values))

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        if len(self.funcs) == 1:
            self.funcs[0]._get_parameter_types(none_engine, result, OperandType.ARRAY)
        else:
            for func in self.funcs:
                func._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("SUMIF")
class Function_SUMIF(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "SumIf", 2, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "SumIf"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_array_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self.func2.evaluate(engine, tp)
        if args2.is_error_or_none():
            return args2

        values: list[Decimal] = []
        if not flatten_to_number_list(args1, values):
            return self._parameter_error(1)

        if self.func3 is not None:
            args3 = _arg_array(self, engine, tp, 3)
            if args3.is_error_or_none():
                return args3
            sums: list[Decimal] = []
            if not flatten_to_number_list(args3, sums):
                return self._parameter_error(3)
        else:
            sums = values

        if args2.is_number():
            total = get_sum_if(values, args2.number_value(), sums)
        elif args2.is_text():
            text = args2.text_value().strip()
            number = _try_parse_decimal(text)
            if number is not None:
                total = get_sum_if(values, number, sums)
            else:
                match = parse_sum_if_match(text)
                if match is None:
                    return self._parameter_error(2)
                total = get_sum_if_op(values, match[0], match[1], sums)
        else:
            return self._parameter_error(2)
        return Operand.create(total)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.ARRAY)
        self.func2._get_parameter_types(none_engine, result, OperandType.NONE)
        if self.func3 is not None:
            self.func3._get_parameter_types(none_engine, result, OperandType.ARRAY)


@register("SUMPRODUCT")
class Function_SUMPRODUCT(_SumFunctionN):
    def __init__(self, funcs):
        super().__init__(funcs, "SUMPRODUCT", 1)

    def name(self) -> str:
        return "SUMPRODUCT"

    def evaluate(self, engine, tp) -> Operand:
        arrays: list[list[Decimal]] = []
        for i in range(len(self.funcs)):
            arg = self._get_array(engine, tp, i)
            if arg.is_error_or_none():
                return arg
            values: list[Decimal] = []
            for item in arg.array_value():
                if item.is_number():
                    values.append(item.number_value())
            arrays.append(values)

        min_length = len(arrays[0])
        for i in range(1, len(arrays)):
            if len(arrays[i]) < min_length:
                min_length = len(arrays[i])
        if min_length == 0:
            return Operand.ZERO

        result = 0.0
        for i in range(min_length):
            product = 1.0
            for values in arrays:
                product *= float(values[i])
            result += product
        return Operand.create(from_float(result))

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        for func in self.funcs:
            func._get_parameter_types(none_engine, result, OperandType.ARRAY)


@register("SUMSQ")
class Function_SUMSQ(_SumFunctionN):
    def __init__(self, funcs):
        super().__init__(funcs, "SumSq", 1)

    def name(self) -> str:
        return "SumSq"

    def evaluate(self, engine, tp) -> Operand:
        error, args = self._collect(engine, tp)
        if error is not None:
            return error

        values: list[Decimal] = []
        if not flatten_to_big_decimal_list(args, values):
            return self._function_error()

        total = ZERO
        for value in values:
            total += value * value
        return Operand.create(total)

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        if len(self.funcs) == 1:
            self.funcs[0]._get_parameter_types(none_engine, result, OperandType.ARRAY)
        else:
            for func in self.funcs:
                func._get_parameter_types(none_engine, result, OperandType.NUMBER)


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


@register("SUMX2MY2")
class Function_SUMX2MY2(_SumXY):
    def __init__(self, funcs):
        super().__init__(funcs, "SUMX2MY2")

    def name(self) -> str:
        return "SUMX2MY2"

    def evaluate(self, engine, tp) -> Operand:
        error, x_values, y_values = self._pair_values(engine, tp)
        if error is not None:
            return error

        result = 0.0
        for i in range(min(len(x_values), len(y_values))):
            xv = float(x_values[i])
            yv = float(y_values[i])
            result += xv * xv - yv * yv
        return Operand.create(from_float(result))


@register("SUMX2PY2")
class Function_SUMX2PY2(_SumXY):
    def __init__(self, funcs):
        super().__init__(funcs, "SUMX2PY2")

    def name(self) -> str:
        return "SUMX2PY2"

    def evaluate(self, engine, tp) -> Operand:
        error, x_values, y_values = self._pair_values(engine, tp)
        if error is not None:
            return error

        result = 0.0
        for i in range(min(len(x_values), len(y_values))):
            xv = float(x_values[i])
            yv = float(y_values[i])
            result += xv * xv + yv * yv
        return Operand.create(from_float(result))


@register("SUMXMY2")
class Function_SUMXMY2(_SumXY):
    def __init__(self, funcs):
        super().__init__(funcs, "SUMXMY2")

    def name(self) -> str:
        return "SUMXMY2"

    def evaluate(self, engine, tp) -> Operand:
        error, x_values, y_values = self._pair_values(engine, tp)
        if error is not None:
            return error

        result = 0.0
        for i in range(min(len(x_values), len(y_values))):
            diff = float(x_values[i]) - float(y_values[i])
            result += diff * diff
        return Operand.create(from_float(result))


@register("VAR")
class Function_VAR(_SumFunctionN):
    def __init__(self, funcs):
        super().__init__(funcs, "Var", 1)

    def name(self) -> str:
        return "Var"

    def evaluate(self, engine, tp) -> Operand:
        error, args = self._collect(engine, tp)
        if error is not None:
            return error

        values = _flatten_numeric_args(args)
        if values is None:
            return self._function_error()

        m2, count = _welford(values)
        if count <= 1:
            return self._function_error()
        return Operand.create(divide(m2, Decimal(count - 1)))

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        if len(self.funcs) == 1:
            self.funcs[0]._get_parameter_types(none_engine, result, OperandType.ARRAY)
        else:
            for func in self.funcs:
                func._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("VARP")
class Function_VARP(_SumFunctionN):
    def __init__(self, funcs):
        super().__init__(funcs, "VarP", 1)

    def name(self) -> str:
        return "VarP"

    def evaluate(self, engine, tp) -> Operand:
        error, args = self._collect(engine, tp)
        if error is not None:
            return error

        values: list[Decimal] = []
        if not flatten_to_big_decimal_list(args, values):
            return self._function_error()
        if not values:
            return self._function_error()
        if len(values) == 1:
            return Operand.ZERO

        m2, _ = _welford(values)
        return Operand.create(divide(m2, Decimal(len(values))))

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        if len(self.funcs) == 1:
            self.funcs[0]._get_parameter_types(none_engine, result, OperandType.ARRAY)
        else:
            for func in self.funcs:
                func._get_parameter_types(none_engine, result, OperandType.NUMBER)
