"""Function_PERCENTILE (C#: Internals/Functions/MathSum/Function_PERCENTILE.cs)."""
from __future__ import annotations
from decimal import Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.internals.functions.function_util import flatten_to_number_list
from toolgood_algorithm.math_net.excel_functions import percentile
from toolgood_algorithm.internals.numeric import ZERO
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float


@register("PERCENTILE")
class Function_PERCENTILE(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "Percentile", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Percentile"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_array_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2

        values: list[Decimal] = []
        if not flatten_to_number_list(args1, values):
            return self._parameter_error(1)
        if not values:
            return self._parameter_error(1)

        k = args2.number_value()
        if k < ZERO or k > Decimal(1):
            return self._parameter_error(2)
        try:
            return Operand.create(from_float(percentile([float(v) for v in values], float(k))))
        except Exception:  # noqa: BLE001 - mirrors the Java catch
            return self._parameter_error(2)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.ARRAY)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
