"""Function_RANDBETWEEN (C#: Internals/Functions/MathBase/Function_RANDBETWEEN.cs)."""
from __future__ import annotations
import math
import random
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import from_float


@register("RANDBETWEEN")
class Function_RANDBETWEEN(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "RANDBETWEEN", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "RandBetween"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        b = self._get_number_2(engine, tp)
        if b.is_error_or_none():
            return b
        bottom, top = a.number_value(), b.number_value()
        if bottom > top:
            return self._parameter_error(1)
        r = math.floor(random.random() * (float(top) - float(bottom) + 1) + float(bottom))
        return Operand.create(from_float(r))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
