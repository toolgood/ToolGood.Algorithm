"""Function_GAMMALN (C#: Internals/Functions/MathSum2/Function_GAMMALN.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_1 import Function_1
from toolgood_algorithm.math_net.excel_functions import gammln
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float


@register("GAMMALN")
class Function_GAMMALN(Function_1):
    def __init__(self, funcs):
        _require_args(funcs, "GammaLn", 1, 1)
        super().__init__(funcs)

    def name(self) -> str:
        return "GammaLn"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1

        value = args1.number_value()
        if value <= 0:
            return self._parameter_error(1)
        return Operand.create(from_float(gammln(float(value))))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
