"""Function_SLN (C#: Internals/Functions/Financial/Function_SLN.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_3 import Function_3
from toolgood_algorithm.internals.numeric import ZERO, divide
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("SLN")
class Function_SLN(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "SLN", 3, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "SLN"

    def evaluate(self, engine, tp) -> Operand:
        cost_arg = self._get_number_1(engine, tp)
        if cost_arg.is_error_or_none():
            return cost_arg
        cost = cost_arg.number_value()

        salvage_arg = self._get_number_2(engine, tp)
        if salvage_arg.is_error_or_none():
            return salvage_arg
        salvage = salvage_arg.number_value()

        life_arg = self._get_number_3(engine, tp)
        if life_arg.is_error_or_none():
            return life_arg
        life = life_arg.number_value()

        if life == ZERO:
            return self._div0_error()
        return Operand.create(divide(cost - salvage, life))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)
