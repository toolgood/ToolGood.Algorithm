"""Function_NEGBINOMDIST (C#: Internals/Functions/MathSum2/Function_NEGBINOMDIST.cs)."""
from __future__ import annotations
from decimal import Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_3 import Function_3
from toolgood_algorithm.math_net.excel_functions import negbinom_dist
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float


@register("NEGBINOMDIST")
class Function_NEGBINOMDIST(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "NegBinomDist", 3, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "NegBinomDist"

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

        k = args1.int_value()
        if k < 0:
            return self._parameter_error(1)
        r = args2.number_value()
        if r <= 0:
            return self._parameter_error(2)
        p = args3.number_value()
        if p < 0 or p > Decimal(1):
            return self._parameter_error(3)
        return Operand.create(from_float(negbinom_dist(k, float(r), float(p))))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)
