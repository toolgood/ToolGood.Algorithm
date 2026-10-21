"""Function_GAMMADIST (C#: Internals/Functions/MathSum2/Function_GAMMADIST.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_4 import Function_4
from toolgood_algorithm.math_net.excel_functions import gamma_dist
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float


@register("GAMMADIST")
class Function_GAMMADIST(Function_4):
    def __init__(self, funcs):
        _require_args(funcs, "GammaDist", 4, 4)
        super().__init__(funcs)

    def name(self) -> str:
        return "GammaDist"

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

        x = args1.number_value()
        if x < 0:
            return self._parameter_error(1)
        alpha = args2.number_value()
        if alpha <= 0:
            return self._parameter_error(2)
        beta = args3.number_value()
        if beta <= 0:
            return self._parameter_error(3)
        return Operand.create(
            from_float(gamma_dist(float(x), float(alpha), float(beta), args4.boolean_value()))
        )

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func4._get_parameter_types(none_engine, result, OperandType.BOOLEAN)
