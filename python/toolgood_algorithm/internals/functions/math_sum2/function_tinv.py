"""Function_TINV (C#: Internals/Functions/MathSum2/Function_TINV.cs)."""
from __future__ import annotations
from decimal import Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.math_net.excel_functions import t_inv
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float


@register("TINV")
class Function_TINV(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "TInv", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "TInv"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2

        p = args1.number_value()
        if p <= 0 or p >= Decimal(1):
            return self._parameter_error(1)
        degrees_freedom = args2.int_value()
        if degrees_freedom <= 0:
            return self._parameter_error(2)
        try:
            return Operand.create(from_float(t_inv(float(p), degrees_freedom)))
        except Exception:  # noqa: BLE001 - mirrors the Java catch
            return self._function_error()

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
