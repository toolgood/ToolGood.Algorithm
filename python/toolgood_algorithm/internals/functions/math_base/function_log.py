"""Function_LOG (C#: Internals/Functions/MathBase/Function_LOG.cs)."""
from __future__ import annotations
import math
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.internals.numeric import ONE, ZERO
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import from_float


@register("LOG")
class Function_LOG(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "LOG", 1, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Log"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        z = a.number_value()
        if z <= ZERO:
            return self._parameter_error(1)
        if self.func2 is None:
            return Operand.create(from_float(math.log10(float(z))))
        b = self._get_number_2(engine, tp)
        if b.is_error_or_none():
            return b
        base = b.number_value()
        if base <= ZERO or base == ONE:
            return self._parameter_error(2)
        return Operand.create(from_float(math.log(float(z)) / math.log(float(base))))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func2 is not None:
            self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
