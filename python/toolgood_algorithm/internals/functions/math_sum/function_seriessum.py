"""Function_SERIESSUM (C#: Internals/Functions/MathSum/Function_SERIESSUM.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float

from ._common import _SumFunctionN, _pow


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
