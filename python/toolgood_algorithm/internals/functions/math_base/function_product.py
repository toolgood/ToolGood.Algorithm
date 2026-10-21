"""Function_PRODUCT (C#: Internals/Functions/MathBase/Function_PRODUCT.cs)."""
from __future__ import annotations
from decimal import Decimal
from toolgood_algorithm.internals.functions.function_util import flatten_to_big_decimal_list
from toolgood_algorithm.internals.numeric import ONE
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _AggregateNumbers


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
