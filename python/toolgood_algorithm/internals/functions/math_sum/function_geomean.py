"""Function_GEOMEAN (C#: Internals/Functions/MathSum/Function_GEOMEAN.cs)."""
from __future__ import annotations
import math
from decimal import Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_util import flatten_to_big_decimal_list
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float

from ._common import _SumFunctionN


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
