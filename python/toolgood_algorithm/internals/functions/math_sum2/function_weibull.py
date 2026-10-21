"""Function_WEIBULL (C#: Internals/Functions/MathSum2/Function_WEIBULL.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_4 import Function_4
from toolgood_algorithm.math_net.excel_functions import weibull
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float

from ._common import _is_nan_or_inf


@register("WEIBULL")
class Function_WEIBULL(Function_4):
    def __init__(self, funcs):
        _require_args(funcs, "Weibull", 4, 4)
        super().__init__(funcs)

    def name(self) -> str:
        return "Weibull"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        args3 = self._get_number_3(engine, tp)
        if args3.is_error_or_none():
            return args3
        args4 = self._get_boolean_4(engine, tp)
        if args4.is_error_or_none():
            return args4

        x = args1.number_value()
        shape = args2.number_value()
        if shape <= 0:
            return self._parameter_error(2)
        scale = args3.number_value()
        if scale <= 0:
            return self._parameter_error(3)

        try:
            result = weibull(float(x), float(shape), float(scale), args4.boolean_value())
        except (ValueError, OverflowError, ZeroDivisionError):
            return self._function_error()
        # overflowing Pow/Exp yields Infinity, 0^negative yields NaN
        if _is_nan_or_inf(result):
            return self._function_error()
        return Operand.create(from_float(result))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func4._get_parameter_types(none_engine, result, OperandType.BOOLEAN)
