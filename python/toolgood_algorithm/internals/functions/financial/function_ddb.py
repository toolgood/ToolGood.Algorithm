"""Function_DDB (C#: Internals/Functions/Financial/Function_DDB.cs)."""
from __future__ import annotations
from decimal import Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_5 import Function_5
from toolgood_algorithm.internals.numeric import ONE, ZERO, divide
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("DDB")
class Function_DDB(Function_5):
    def __init__(self, funcs):
        _require_args(funcs, "DDB", 4, 5)
        super().__init__(funcs)

    def name(self) -> str:
        return "DDB"

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

        factor = Decimal(2)
        if self.func5 is not None:
            factor_arg = self._get_number_5(engine, tp)
            if factor_arg.is_error_or_none():
                return factor_arg
            factor = factor_arg.number_value()

        if life == ZERO or factor == ZERO:
            return self._div0_error()
        if life <= ZERO:
            return self._parameter_error(3)
        if period < ONE or period > life:
            return self._parameter_error(4)

        remaining_cost = cost
        i = 1
        while Decimal(i) < period:
            ddb = divide(remaining_cost * factor, life)
            max_depreciation = remaining_cost - salvage
            if ddb > max_depreciation:
                ddb = max_depreciation
            remaining_cost = remaining_cost - ddb
            if remaining_cost <= salvage:
                break
            i += 1

        depreciation = divide(remaining_cost * factor, life)
        max_depreciation = remaining_cost - salvage
        if depreciation > max_depreciation:
            depreciation = max_depreciation
        return Operand.create(depreciation)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func4._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func5 is not None:
            self.func5._get_parameter_types(none_engine, result, OperandType.NUMBER)
