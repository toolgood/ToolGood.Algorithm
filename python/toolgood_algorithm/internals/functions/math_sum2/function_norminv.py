"""Function_NORMINV (C#: Internals/Functions/MathSum2/Function_NORMINV.cs)."""
from __future__ import annotations
from decimal import Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_3 import Function_3
from toolgood_algorithm.math_net.excel_functions import norm_inv
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float


@register("NORMINV")
class Function_NORMINV(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "NormInv", 3, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "NormInv"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        args3 = self._get_number_3(engine, tp)
        if args3.is_error_or_none():
            return args3

        p = args1.number_value()
        if p <= 0 or p >= Decimal(1):
            return self._parameter_error(1)
        avg = args2.number_value()
        stdev = args3.number_value()
        if stdev <= 0:
            return self._parameter_error(3)
        return Operand.create(from_float(norm_inv(float(p), float(avg), float(stdev))))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)
