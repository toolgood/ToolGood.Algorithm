"""Function_NPV (C#: Internals/Functions/Financial/Function_NPV.cs)."""
from __future__ import annotations
import math
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_n import Function_N
from toolgood_algorithm.internals.numeric import ONE, ZERO, divide
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float


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
