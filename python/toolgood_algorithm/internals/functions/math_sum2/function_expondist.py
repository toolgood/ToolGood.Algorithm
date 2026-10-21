"""Function_EXPONDIST (C#: Internals/Functions/MathSum2/Function_EXPONDIST.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_3 import Function_3
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float

from ._common import _expon_dist


@register("EXPONDIST")
class Function_EXPONDIST(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "ExpOnDist", 3, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "ExpOnDist"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        args3 = self._get_boolean_3(engine, tp)
        if args3.is_error_or_none():
            return args3

        n1 = args1.number_value()
        if n1 < 0:
            return self._parameter_error(1)
        rate = args2.number_value()
        if rate <= 0:
            return self._parameter_error(2)
        result = _expon_dist(float(n1), float(rate), args3.boolean_value())
        return Operand.create(from_float(result))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func3._get_parameter_types(none_engine, result, OperandType.BOOLEAN)
