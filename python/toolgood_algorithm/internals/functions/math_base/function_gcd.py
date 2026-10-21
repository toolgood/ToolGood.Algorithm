"""Function_GCD (C#: Internals/Functions/MathBase/Function_GCD.cs)."""
from __future__ import annotations
from decimal import Decimal
from toolgood_algorithm.internals.functions.function_util import flatten_to_big_decimal_list, get_gcd
from toolgood_algorithm.internals.numeric import ZERO
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _AggregateNumbers


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
