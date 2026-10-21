"""Function_SYD (C#: Internals/Functions/Financial/Function_SYD.cs)."""
from __future__ import annotations
from decimal import Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_4 import Function_4
from toolgood_algorithm.internals.numeric import ONE, ZERO, divide
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("SYD")
class Function_SYD(Function_4):
    def __init__(self, funcs):
        _require_args(funcs, "SYD", 4, 4)
        super().__init__(funcs)

    def name(self) -> str:
        return "SYD"

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

        period_arg = self._get_number_4(engine, tp)
        if period_arg.is_error_or_none():
            return period_arg
        period = period_arg.number_value()

        if life == ZERO:
            return self._div0_error()
        if life <= ZERO:
            return self._parameter_error(3)
        if period < ONE or period > life:
            return self._parameter_error(4)

        numerator = (cost - salvage) * (life - period + ONE) * Decimal(2)
        denominator = life * (life + ONE)
        return Operand.create(divide(numerator, denominator))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func4._get_parameter_types(none_engine, result, OperandType.NUMBER)
