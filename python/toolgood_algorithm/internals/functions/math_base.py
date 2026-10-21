"""Math functions (ABS, ROUND, POWER, GCD, ...)."""
from __future__ import annotations

import math
import random
from decimal import ROUND_CEILING, ROUND_DOWN, ROUND_FLOOR, ROUND_HALF_UP, Decimal

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import Function_0, Function_1, Function_2, Function_3, Function_N, _require_args
from toolgood_algorithm.internals.functions.function_util import flatten_to_big_decimal_list, get_factorial, get_gcd, get_lcm
from toolgood_algorithm.internals.numeric import ONE, ZERO
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


def from_float(value: float) -> Decimal:
    """``BigDecimal.valueOf(double)`` equivalent."""
    if value != value or value in (float("inf"), float("-inf")):
        raise ValueError("Number is NaN or Infinity!")
    return Decimal(str(value))


class _Math1(Function_1):
    def __init__(self, funcs):
        if not isinstance(funcs, (list, tuple)):
            funcs = [funcs]
        _require_args(funcs, self.__class__.__name__, 1, 1)
        super().__init__(funcs)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("ABS")
class Function_ABS(_Math1):
    def name(self) -> str:
        return "Abs"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        return Operand.create(abs(a.number_value()))


@register("CEILING")
class Function_CEILING(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "CEILING", 1, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Ceiling"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        if self.func2 is None:
            return Operand.create(a.number_value().quantize(Decimal(1), rounding=ROUND_CEILING))
        b = self._get_number_2(engine, tp)
        if b.is_error_or_none():
            return b
        divisor = b.number_value()
        if divisor == ZERO:
            return Operand.ZERO
        value = a.number_value()
        if divisor > ZERO:
            return Operand.create(from_float(math.ceil(float(value / divisor)) * float(divisor)))
        if value > ZERO:
            return self._parameter_error(1)
        return Operand.create(from_float(math.ceil(float(value / divisor)) * float(divisor)))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func2 is not None:
            self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("COMBIN")
class Function_COMBIN(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "COMBIN", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Combin"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        b = self._get_number_2(engine, tp)
        if b.is_error_or_none():
            return b
        num1, num2 = a.number_value(), b.number_value()
        if num1 > 2147483647 or num1 < -2147483648:
            return self._parameter_error(1)
        if num2 > 2147483647 or num2 < -2147483648:
            return self._parameter_error(2)
        total, count = int(num1), int(num2)
        if total < 0:
            return self._parameter_error(1)
        if count < 0:
            return self._parameter_error(2)
        if total < count:
            return self._parameter_error(2)
        numerator = ONE
        denominator = ONE
        for i in range(count):
            numerator *= total - i
            denominator *= i + 1
        return Operand.create(numerator / denominator)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("DELTA")
class Function_DELTA(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "DELTA", 1, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Delta"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        num1 = a.number_value()
        num2 = ZERO
        if self.func2 is not None:
            b = self._get_number_2(engine, tp)
            if b.is_error_or_none():
                return b
            num2 = b.number_value()
        return Operand.create(1 if num1 == num2 else 0)


@register("EVEN")
class Function_EVEN(_Math1):
    def name(self) -> str:
        return "Even"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        z = a.number_value()
        if z == ZERO:
            return Operand.ZERO
        two = Decimal(2)
        if z > ZERO:
            if z % two == ZERO:
                return a
            z = z.quantize(Decimal(1), rounding=ROUND_CEILING)
            if z % two == ZERO:
                return Operand.create(z)
            return Operand.create(z + ONE)
        if z % two == ZERO:
            return a
        z = z.quantize(Decimal(1), rounding=ROUND_FLOOR)
        if z % two == ZERO:
            return Operand.create(z)
        return Operand.create(z - ONE)


@register("EXP")
class Function_EXP(_Math1):
    def name(self) -> str:
        return "Exp"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        if a.number_value() > 700:
            return self._parameter_error(1)
        return Operand.create(from_float(math.exp(float(a.number_value()))))


@register("FACT")
class Function_FACT(_Math1):
    def name(self) -> str:
        return "Fact"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        number = a.number_value()
        if number > 2147483647 or number < -2147483648:
            return self._parameter_error(1)
        z = int(number)
        if z < 0 or z > 27:
            return self._parameter_error(1)
        result = ONE
        for i in range(1, z + 1):
            result *= i
        return Operand.create(result)


@register("FACTDOUBLE")
class Function_FACTDOUBLE(_Math1):
    def name(self) -> str:
        return "FactDouble"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        number = a.number_value()
        if number > 2147483647 or number < -2147483648:
            return self._parameter_error(1)
        z = int(number)
        if z < 0 or z > 45:
            return self._parameter_error(1)
        result = ONE
        for i in range(z, 0, -2):
            result *= i
        return Operand.create(result)


@register("FIXED")
class Function_FIXED(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "FIXED", 1, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "Fixed"

    def evaluate(self, engine, tp) -> Operand:
        num = 2
        if self.func2 is not None:
            b = self._get_number_2(engine, tp)
            if b.is_error_or_none():
                return b
            num = b.int_value()
            if num < -15 or num > 15:
                return self._parameter_error(2)
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        scale = a.number_value().quantize(Decimal(1).scaleb(-num), rounding=ROUND_HALF_UP)
        no = False
        if self.func3 is not None:
            c = self._get_boolean_3(engine, tp)
            if c.is_error_or_none():
                return c
            no = c.boolean_value()
        if no is False:
            if num < 0:
                return Operand.create(f"{scale:,.0f}")
            return Operand.create(f"{scale:,.{num}f}")
        return Operand.create(format(scale, "f"))


@register("FLOOR")
class Function_FLOOR(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "FLOOR", 1, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Floor"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        if self.func2 is None:
            return Operand.create(a.number_value().quantize(Decimal(1), rounding=ROUND_FLOOR))
        b = self._get_number_2(engine, tp)
        if b.is_error_or_none():
            return b
        divisor = b.number_value()
        if divisor == ZERO:
            return Operand.ZERO
        value = a.number_value()
        if divisor > ZERO:
            return Operand.create(from_float(math.floor(float(value / divisor)) * float(divisor)))
        if value > ZERO:
            return self._parameter_error(1)
        return Operand.create(from_float(math.floor(float(value / divisor)) * float(divisor)))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func2 is not None:
            self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)


class _AggregateNumbers(Function_N):
    def __init__(self, funcs, minimum=1):
        _require_args(funcs, "AGG", minimum)
        super().__init__(funcs)

    def _collect(self, engine, tp):
        args: list[Operand] = []
        for i, func in enumerate(self.funcs):
            value = self._get_number(engine, tp, i)
            if value.is_error_or_none():
                return value, None
            args.append(value)
        return None, args

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        for func in self.funcs:
            func._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("GCD")
class Function_GCD(_AggregateNumbers):
    def name(self) -> str:
        return "Gcd"

    def evaluate(self, engine, tp) -> Operand:
        error, args = self._collect(engine, tp)
        if error is not None:
            return error
        values: list[Decimal] = []
        if not flatten_to_big_decimal_list(args, values):
            return self._function_error()
        for i, value in enumerate(values):
            if value < ZERO:
                return self._parameter_error(i + 1)
        return Operand.create(get_gcd(values))


@register("LCM")
class Function_LCM(_AggregateNumbers):
    def name(self) -> str:
        return "Lcm"

    def evaluate(self, engine, tp) -> Operand:
        error, args = self._collect(engine, tp)
        if error is not None:
            return error
        values: list[Decimal] = []
        if not flatten_to_big_decimal_list(args, values):
            return self._function_error()
        for i, value in enumerate(values):
            if value < ZERO:
                return self._parameter_error(i + 1)
        return Operand.create(get_lcm(values))


@register("GESTEP")
class Function_GESTEP(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "GESTEP", 1, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "GEStep"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        number = a.number_value()
        step = ZERO
        if self.func2 is not None:
            b = self._get_number_2(engine, tp)
            if b.is_error_or_none():
                return b
            step = b.number_value()
        return Operand.create(1 if number >= step else 0)


@register("INT")
class Function_INT(_Math1):
    def name(self) -> str:
        return "Int"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        return Operand.create(a.number_value().quantize(Decimal(1), rounding=ROUND_FLOOR))


@register("LN")
class Function_LN(_Math1):
    def name(self) -> str:
        return "Ln"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        z = a.number_value()
        if z <= ZERO:
            return self._parameter_error(1)
        return Operand.create(from_float(math.log(float(z))))


@register("LOG")
class Function_LOG(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "LOG", 1, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Log"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        z = a.number_value()
        if z <= ZERO:
            return self._parameter_error(1)
        if self.func2 is None:
            return Operand.create(from_float(math.log10(float(z))))
        b = self._get_number_2(engine, tp)
        if b.is_error_or_none():
            return b
        base = b.number_value()
        if base <= ZERO or base == ONE:
            return self._parameter_error(2)
        return Operand.create(from_float(math.log(float(z)) / math.log(float(base))))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func2 is not None:
            self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("LOG10")
class Function_LOG10(_Math1):
    def name(self) -> str:
        return "Log10"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        z = a.number_value()
        if z <= ZERO:
            return self._parameter_error(1)
        return Operand.create(from_float(math.log10(float(z))))


@register("MROUND")
class Function_MROUND(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "MROUND", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "MRound"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        b = self._get_number_2(engine, tp)
        if b.is_error_or_none():
            return b
        multiple = b.number_value()
        if multiple == ZERO:
            return self._parameter_error(2)
        number = a.number_value()
        if (number > ZERO and multiple < ZERO) or (number < ZERO and multiple > ZERO):
            return self._parameter_error(2)
        r = (number / multiple).quantize(Decimal(1), rounding=ROUND_HALF_UP) * multiple
        return Operand.create(r)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("MULTINOMIAL")
class Function_MULTINOMIAL(Function_N):
    def __init__(self, funcs):
        _require_args(funcs, "MULTINOMIAL", 1)
        super().__init__(funcs)

    def name(self) -> str:
        return "Multinomial"

    def evaluate(self, engine, tp) -> Operand:
        args: list[Operand] = []
        error = self.try_evaluate_all(engine, tp, args)
        if error is not None:
            return error
        values: list[Decimal] = []
        if not flatten_to_big_decimal_list(args, values):
            return self._function_error()
        total = ZERO
        denominator = ONE
        for i, value in enumerate(values):
            if value > 2147483647 or value < -2147483648:
                return self._parameter_error(i + 1)
            a = int(value)
            if a < 0:
                return self._parameter_error(i + 1)
            denominator *= get_factorial(Decimal(a))
            total += a
        return Operand.create(get_factorial(total) / denominator)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        for func in self.funcs:
            func._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("ODD")
class Function_ODD(_Math1):
    def name(self) -> str:
        return "Odd"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        z = a.number_value()
        if z == ZERO:
            return Operand.ONE
        two = Decimal(2)
        if z > ZERO:
            if abs(z % two) == ONE:
                return a
            z = z.quantize(Decimal(1), rounding=ROUND_CEILING)
            if z % two != ZERO:
                return Operand.create(z)
            return Operand.create(z + ONE)
        if abs(z % two) == ONE:
            return a
        z = z.quantize(Decimal(1), rounding=ROUND_FLOOR)
        if z % two != ZERO:
            return Operand.create(z)
        return Operand.create(z - ONE)


@register("PERCENTAGE")
class Function_Percentage(_Math1):
    def name(self) -> str:
        return "%"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        return Operand.create(from_float(float(a.number_value()) / 100.0))

    def to_string(self, sb: list, add_brackets: bool) -> None:
        if add_brackets:
            sb.append("(")
        self.func1.to_string(sb, False)
        sb.append("%")
        if add_brackets:
            sb.append(")")


@register("PERMUT")
class Function_PERMUT(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "PERMUT", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Permut"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        b = self._get_number_2(engine, tp)
        if b.is_error_or_none():
            return b
        num1, num2 = a.number_value(), b.number_value()
        if num1 > 2147483647 or num1 < -2147483648:
            return self._parameter_error(1)
        if num2 > 2147483647 or num2 < -2147483648:
            return self._parameter_error(2)
        total, count = int(num1), int(num2)
        if total < 0:
            return self._parameter_error(1)
        if count < 0:
            return self._parameter_error(2)
        if total < count:
            return self._parameter_error(2)
        result = ONE
        for i in range(count):
            result *= total - i
        return Operand.create(result)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("POWER")
class Function_POWER(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "POWER", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Power"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        b = self._get_number_2(engine, tp)
        if b.is_error_or_none():
            return b
        base, exponent = a.number_value(), b.number_value()
        if base == ZERO and exponent < ZERO:
            return self._div0_error()
        if base < ZERO and exponent % ONE != ZERO:
            return self._parameter_error(1)
        result = math.pow(float(base), float(exponent))
        if result != result or result in (float("inf"), float("-inf")):
            return self._function_error()
        return Operand.create(from_float(result))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("PRODUCT")
class Function_PRODUCT(_AggregateNumbers):
    def name(self) -> str:
        return "Product"

    def evaluate(self, engine, tp) -> Operand:
        error, args = self._collect(engine, tp)
        if error is not None:
            return error
        values: list[Decimal] = []
        if not flatten_to_big_decimal_list(args, values):
            return self._function_error()
        result = ONE
        for value in values:
            result *= value
        return Operand.create(result)


@register("QUOTIENT")
class Function_QUOTIENT(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "QUOTIENT", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Quotient"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        b = self._get_number_2(engine, tp)
        if b.is_error_or_none():
            return b
        if b.number_value() == ZERO:
            return self._div0_error()
        result = float(a.number_value()) / float(b.number_value())
        if result != result or result in (float("inf"), float("-inf")) or result < -2147483648 or result > 2147483647:
            return self._function_error()
        return Operand.create(int(result))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("RANDBETWEEN")
class Function_RANDBETWEEN(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "RANDBETWEEN", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "RandBetween"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        b = self._get_number_2(engine, tp)
        if b.is_error_or_none():
            return b
        bottom, top = a.number_value(), b.number_value()
        if bottom > top:
            return self._parameter_error(1)
        r = math.floor(random.random() * (float(top) - float(bottom) + 1) + float(bottom))
        return Operand.create(from_float(r))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("ROUND")
class Function_ROUND(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "ROUND", 1, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Round"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        if self.func2 is None:
            return Operand.create(a.number_value().quantize(Decimal(1), rounding=ROUND_HALF_UP))
        b = self._get_number_2(engine, tp)
        if b.is_error_or_none():
            return b
        digits = b.int_value()
        if digits < -15 or digits > 15:
            return self._parameter_error(2)
        return Operand.create(a.number_value().quantize(Decimal(1).scaleb(-digits), rounding=ROUND_HALF_UP))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func2 is not None:
            self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("ROUNDDOWN")
class Function_ROUNDDOWN(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "ROUNDDOWN", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "RoundDown"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        b = self._get_number_2(engine, tp)
        if b.is_error_or_none():
            return b
        digits = b.int_value()
        if digits < -15 or digits > 15:
            return self._parameter_error(2)
        num = a.number_value()
        if num == ZERO:
            return a
        factor = 10.0 ** digits
        if num > ZERO:
            result = math.floor(float(num) * factor) / factor
        else:
            result = math.ceil(float(num) * factor) / factor
        return Operand.create(from_float(result))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("ROUNDUP")
class Function_ROUNDUP(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "ROUNDUP", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "RoundUp"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        b = self._get_number_2(engine, tp)
        if b.is_error_or_none():
            return b
        digits = b.int_value()
        if digits < -15 or digits > 15:
            return self._parameter_error(2)
        num = a.number_value()
        if num == ZERO:
            return a
        factor = 10.0 ** digits
        t = math.ceil(abs(float(num)) * factor) / factor
        return Operand.create(from_float(t if num > ZERO else -t))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("SIGN")
class Function_SIGN(_Math1):
    def name(self) -> str:
        return "Sign"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        value = a.number_value()
        return Operand.create(-1 if value < 0 else (1 if value > 0 else 0))


@register("SQRT")
class Function_SQRT(_Math1):
    def name(self) -> str:
        return "Sqrt"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        if a.number_value() < ZERO:
            return self._parameter_error(1)
        return Operand.create(from_float(math.sqrt(float(a.number_value()))))


@register("SQRTPI")
class Function_SQRTPI(_Math1):
    def name(self) -> str:
        return "SqrtPi"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        if a.number_value() < ZERO:
            return self._parameter_error(1)
        return Operand.create(from_float(math.sqrt(float(a.number_value()) * math.pi)))


@register("TRUNC")
class Function_TRUNC(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "TRUNC", 1, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Trunc"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        digits = 0
        if self.func2 is not None:
            b = self._get_number_2(engine, tp)
            if b.is_error_or_none():
                return b
            digits = b.int_value()
            if digits < -15 or digits > 15:
                return self._parameter_error(2)
        num = a.number_value()
        if num == ZERO:
            return a
        factor = 10.0 ** digits
        if num > ZERO:
            result = math.floor(float(num) * factor) / factor
        else:
            result = math.ceil(float(num) * factor) / factor
        return Operand.create(from_float(result))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func2 is not None:
            self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)


# RAND lives in the value module in this port (it is a constant-like node)
from toolgood_algorithm.internals.functions.value import Function_RAND  # noqa: E402,F401
