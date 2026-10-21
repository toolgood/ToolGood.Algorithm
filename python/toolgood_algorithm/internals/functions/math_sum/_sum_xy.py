"""``_SumXY`` - shared shell of SUMX2MY2 / SUMX2PY2 / SUMXMY2 (Python base)."""
from __future__ import annotations

from decimal import Decimal

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.math_sum._sum_function_n import _SumFunctionN


class _SumXY(_SumFunctionN):
    """Shared shell for the SUMX2MY2/SUMX2PY2/SUMXMY2 pairwise sums."""

    def __init__(self, funcs, name: str):
        super().__init__(funcs, name, 2, 2)

    def _pair_values(self, engine, tp):
        x_arg = self._get_array(engine, tp, 0)
        if x_arg.is_error_or_none():
            return x_arg, None, None
        x_values = []
        for item in x_arg.array_value():
            if item.is_number():
                x_values.append(item.number_value())

        y_arg = self._get_array(engine, tp, 1)
        if y_arg.is_error_or_none():
            return y_arg, None, None
        y_values = []
        for item in y_arg.array_value():
            if item.is_number():
                y_values.append(item.number_value())
        return None, x_values, y_values

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.funcs[0]._get_parameter_types(none_engine, result, OperandType.ARRAY)
        self.funcs[1]._get_parameter_types(none_engine, result, OperandType.ARRAY)


__all__ = ["_SumXY"]
