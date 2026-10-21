"""Function_FISHERINV (C#: Internals/Functions/MathSum2/Function_FISHERINV.cs)."""
from __future__ import annotations
import math
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_1 import Function_1
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float


@register("FISHERINV")
class Function_FISHERINV(Function_1):
    def __init__(self, funcs):
        _require_args(funcs, "FisherInv", 1, 1)
        super().__init__(funcs)

    def name(self) -> str:
        return "FisherInv"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1

        x = float(args1.number_value())
        try:
            exp2x = math.exp(2 * x)
        except OverflowError:
            exp2x = float("inf")
        return Operand.create(from_float((exp2x - 1) / (exp2x + 1)))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
