"""Trigonometric functions (SIN, ACOS, DEGREES, ...)."""
from __future__ import annotations

import math
from decimal import Decimal

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import Function_1, Function_2, _require_args
from toolgood_algorithm.internals.numeric import ONE, ZERO
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float


class _Trig1(Function_1):
    def __init__(self, funcs):
        if not isinstance(funcs, (list, tuple)):
            funcs = [funcs]
        _require_args(funcs, self.name(), 1, 1)
        super().__init__(funcs)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("ACOS")
class Function_ACOS(_Trig1):
    def name(self) -> str:
        return "Acos"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        x = args1.number_value()
        if x < Decimal(-1) or x > ONE:
            return self._parameter_error(1)
        return Operand.create(from_float(math.acos(float(x))))


@register("ACOSH")
class Function_ACOSH(_Trig1):
    def name(self) -> str:
        return "Acosh"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        z = args1.number_value()
        if z < ONE:
            return self._parameter_error(1)
        dz = float(z)
        return Operand.create(from_float(math.log(dz + math.sqrt(dz * dz - 1))))


@register("ACOT")
class Function_ACOT(_Trig1):
    def name(self) -> str:
        return "Acot"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        dz = float(args1.number_value())
        return Operand.create(from_float(math.pi / 2 - math.atan(dz)))


@register("ACOTH")
class Function_ACOTH(_Trig1):
    def name(self) -> str:
        return "Acoth"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        d = args1.number_value()
        if d >= Decimal(-1) and d <= ONE:
            return self._parameter_error(1)
        dd = float(d)
        return Operand.create(from_float(0.5 * math.log((dd + 1) / (dd - 1))))


@register("ASIN")
class Function_ASIN(_Trig1):
    def name(self) -> str:
        return "Asin"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        x = args1.number_value()
        if x < Decimal(-1) or x > ONE:
            return self._parameter_error(1)
        return Operand.create(from_float(math.asin(float(x))))


@register("ASINH")
class Function_ASINH(_Trig1):
    def name(self) -> str:
        return "Asinh"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        dz = float(args1.number_value())
        return Operand.create(from_float(math.log(dz + math.sqrt(dz * dz + 1))))


@register("ATAN")
class Function_ATAN(_Trig1):
    def name(self) -> str:
        return "Atan"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        return Operand.create(from_float(math.atan(float(args1.number_value()))))


@register("ATAN2")
class Function_ATAN2(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "Atan2", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Atan2"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        if args1.number_value() == ZERO and args2.number_value() == ZERO:
            return self._div0_error()
        return Operand.create(from_float(math.atan2(float(args2.number_value()), float(args1.number_value()))))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("ATANH")
class Function_ATANH(_Trig1):
    def name(self) -> str:
        return "Atanh"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        x = args1.number_value()
        if x <= Decimal(-1) or x >= ONE:
            return self._parameter_error(1)
        dx = float(x)
        return Operand.create(from_float(0.5 * math.log((1 + dx) / (1 - dx))))


@register("COS")
class Function_COS(_Trig1):
    def name(self) -> str:
        return "Cos"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        return Operand.create(from_float(math.cos(float(args1.number_value()))))


@register("COSH")
class Function_COSH(_Trig1):
    def name(self) -> str:
        return "Cosh"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        x = float(args1.number_value())
        # double 上限约 1.8e308,|x|>=709 时 Math.cosh 返回 Infinity
        if x >= 709 or x <= -709:
            return self._parameter_error(1)
        return Operand.create(from_float(math.cosh(x)))


@register("COT")
class Function_COT(_Trig1):
    def name(self) -> str:
        return "Cot"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        d = math.tan(float(args1.number_value()))
        if d == 0:
            return self._div0_error()
        return Operand.create(from_float(1.0 / d))


@register("COTH")
class Function_COTH(_Trig1):
    def name(self) -> str:
        return "Coth"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        x = float(args1.number_value())
        # coth 在 |x| 较大时趋近 ±1,提前返回避免 Infinity/NaN
        if x >= 709:
            return Operand.create(from_float(1.0))
        if x <= -709:
            return Operand.create(from_float(-1.0))
        d = math.sinh(x)
        if d == 0:
            return self._div0_error()
        return Operand.create(from_float(math.cosh(x) / d))


@register("CSC")
class Function_CSC(_Trig1):
    def name(self) -> str:
        return "Csc"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        d = math.sin(float(args1.number_value()))
        if d == 0:
            return self._div0_error()
        return Operand.create(from_float(1.0 / d))


@register("CSCH")
class Function_CSCH(_Trig1):
    def name(self) -> str:
        return "Csch"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        x = float(args1.number_value())
        # csch 在 |x| 较大时趋近 0,提前返回避免 Infinity
        if x >= 709 or x <= -709:
            return Operand.create(Decimal(0))
        d = math.sinh(x)
        if d == 0:
            return self._div0_error()
        return Operand.create(from_float(1.0 / d))


@register("DEGREES")
class Function_DEGREES(_Trig1):
    def name(self) -> str:
        return "Degrees"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        return Operand.create(from_float(float(args1.number_value()) / math.pi * 180))


@register("RADIANS")
class Function_RADIANS(_Trig1):
    def name(self) -> str:
        return "Radians"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        return Operand.create(from_float(float(args1.number_value()) * math.pi / 180))


@register("SEC")
class Function_SEC(_Trig1):
    def name(self) -> str:
        return "Sec"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        d = math.cos(float(args1.number_value()))
        if d == 0:
            return self._div0_error()
        return Operand.create(from_float(1.0 / d))


@register("SECH")
class Function_SECH(_Trig1):
    def name(self) -> str:
        return "Sech"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        x = float(args1.number_value())
        # sech 在 |x| 较大时趋近 0,提前返回避免 Infinity
        if x >= 709 or x <= -709:
            return Operand.create(Decimal(0))
        return Operand.create(from_float(1.0 / math.cosh(x)))


@register("SIN")
class Function_SIN(_Trig1):
    def name(self) -> str:
        return "Sin"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        return Operand.create(from_float(math.sin(float(args1.number_value()))))


@register("SINH")
class Function_SINH(_Trig1):
    def name(self) -> str:
        return "Sinh"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        x = float(args1.number_value())
        # double 上限约 1.8e308,|x|>=709 时 Math.sinh 返回 Infinity
        if x >= 709 or x <= -709:
            return self._parameter_error(1)
        return Operand.create(from_float(math.sinh(x)))


@register("TAN")
class Function_TAN(_Trig1):
    def name(self) -> str:
        return "Tan"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        cos = math.cos(float(args1.number_value()))
        if cos == 0:
            return self._div0_error()
        return Operand.create(from_float(math.sin(float(args1.number_value())) / cos))


@register("TANH")
class Function_TANH(_Trig1):
    def name(self) -> str:
        return "Tanh"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        x = float(args1.number_value())
        # tanh 在 |x| 较大时趋近 ±1,提前返回避免 Infinity
        if x >= 709:
            return Operand.create(from_float(1.0))
        if x <= -709:
            return Operand.create(from_float(-1.0))
        return Operand.create(from_float(math.tanh(x)))
