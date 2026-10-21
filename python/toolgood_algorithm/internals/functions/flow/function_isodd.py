"""Function_ISODD (C#: Internals/Functions/Flow/Function_ISODD.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _IsBase


@register("ISODD")
class Function_ISODD(_IsBase):
    def name(self) -> str:
        return "ISODD"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self.func1.evaluate(engine, tp)
        if args1.is_number():
            return Operand.TRUE if args1.number_value().to_integral_value() % 2 != 0 else Operand.FALSE
        return Operand.FALSE

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
