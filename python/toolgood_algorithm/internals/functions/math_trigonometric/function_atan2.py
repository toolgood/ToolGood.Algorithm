"""Function_ATAN2 (C#: Internals/Functions/MathTrigonometric/Function_ATAN2.cs)."""
from __future__ import annotations
import math
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.internals.numeric import ZERO
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float


@register("ATAN2")
class Function_ATAN2(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "Atan2", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Atan2"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        if args1.number_value() == ZERO and args2.number_value() == ZERO:
            return self._div0_error()
        return Operand.create(from_float(math.atan2(float(args2.number_value()), float(args1.number_value()))))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
