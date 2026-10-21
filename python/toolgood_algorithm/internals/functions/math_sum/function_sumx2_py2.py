"""Function_SUMX2PY2 (C#: Internals/Functions/MathSum/Function_SUMX2PY2.cs)."""
from __future__ import annotations
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float

from ._common import _SumXY


@register("SUMX2PY2")
class Function_SUMX2PY2(_SumXY):
    def __init__(self, funcs):
        super().__init__(funcs, "SUMX2PY2")

    def name(self) -> str:
        return "SUMX2PY2"

    def evaluate(self, engine, tp) -> Operand:
        error, x_values, y_values = self._pair_values(engine, tp)
        if error is not None:
            return error

        result = 0.0
        for i in range(min(len(x_values), len(y_values))):
            xv = float(x_values[i])
            yv = float(y_values[i])
            result += xv * xv + yv * yv
        return Operand.create(from_float(result))
