"""Function_BINOMDIST (C#: Internals/Functions/MathSum2/Function_BINOMDIST.cs)."""
from __future__ import annotations
from decimal import Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_4 import Function_4
from toolgood_algorithm.math_net.excel_functions import binom_dist
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float


@register("BINOMDIST")
class Function_BINOMDIST(Function_4):
    def __init__(self, funcs):
        _require_args(funcs, "BinomDist", 4, 4)
        super().__init__(funcs)

    def name(self) -> str:
        return "BinomDist"

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
        args4 = self._get_boolean_4(engine, tp)
        if args4.is_error_or_none():
            return args4

        n2 = args2.int_value()
        if n2 < 0:
            return self._parameter_error(2)
        k = args1.int_value()
        if k < 0 or k > n2:
            return self._parameter_error(1)
        n3 = args3.number_value()
        if n3 < 0 or n3 > Decimal(1):
            return self._parameter_error(3)
        return Operand.create(from_float(binom_dist(k, n2, float(n3), args4.boolean_value())))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func4._get_parameter_types(none_engine, result, OperandType.BOOLEAN)
