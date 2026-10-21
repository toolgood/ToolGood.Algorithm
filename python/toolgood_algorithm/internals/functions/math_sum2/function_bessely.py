"""Function_BESSELY (C#: Internals/Functions/MathSum2/Function_BESSELY.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.math_net.special_functions import bessel_y
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float

from ._common import _is_nan_or_inf


@register("BESSELY")
class Function_BESSELY(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "BesselY", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "BesselY"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2

        x = float(args1.number_value())
        n = int(float(args2.number_value()))
        if x <= 0:
            return self._parameter_error(1)
        if n < 0:
            return self._parameter_error(2)

        try:
            result = bessel_y(n, x)
        except (ValueError, OverflowError, ZeroDivisionError):
            return self._function_error()
        # tiny x overflows to Infinity/NaN
        if _is_nan_or_inf(result):
            return self._function_error()
        return Operand.create(from_float(result))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
