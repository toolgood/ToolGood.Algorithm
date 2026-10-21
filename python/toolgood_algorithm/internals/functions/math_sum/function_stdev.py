"""Function_STDEV (C#: Internals/Functions/MathSum/Function_STDEV.cs)."""
from __future__ import annotations
import math
from decimal import Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.numeric import divide
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float

from ._common import _SumFunctionN, _flatten_numeric_args, _welford


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
