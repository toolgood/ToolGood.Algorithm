"""Function_NORMSINV (C#: Internals/Functions/MathSum2/Function_NORMSINV.cs)."""
from __future__ import annotations
from decimal import Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_1 import Function_1
from toolgood_algorithm.math_net.excel_functions import norm_s_inv
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float


@register("NORMSINV")
class Function_NORMSINV(Function_1):
    def __init__(self, funcs):
        _require_args(funcs, "NormSInv", 1, 1)
        super().__init__(funcs)

    def name(self) -> str:
        return "NormSInv"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1

        p = args1.number_value()
        if p <= 0 or p >= Decimal(1):
            return self._parameter_error(1)
        return Operand.create(from_float(norm_s_inv(float(p))))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
