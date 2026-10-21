"""Function_FIXED (C#: Internals/Functions/MathBase/Function_FIXED.cs)."""
from __future__ import annotations
from decimal import ROUND_HALF_UP, Decimal
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_3 import Function_3
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


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
