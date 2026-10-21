"""Function_DB (C#: Internals/Functions/Financial/Function_DB.cs)."""
from __future__ import annotations
import math
from decimal import ROUND_HALF_UP, Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_5 import Function_5
from toolgood_algorithm.internals.numeric import ONE, ZERO, divide
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float


@register("DB")
class Function_DB(Function_5):
    def __init__(self, funcs):
        _require_args(funcs, "DB", 4, 5)
        super().__init__(funcs)

    def name(self) -> str:
        return "DB"

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

        month = 12
        if self.func5 is not None:
            month_arg = self._get_number_5(engine, tp)
            if month_arg.is_error_or_none():
                return month_arg
            month = month_arg.int_value()
            if month < 1 or month > 12:
                return self._parameter_error(5)

        if life == ZERO or cost == ZERO:
            return self._div0_error()

        total_periods = int(life) if month == 12 else int(life) + 1
        if period < ONE or period > Decimal(total_periods):
            return self._parameter_error(4)
        if life < ONE:
            return self._parameter_error(3)

        rate = ONE - from_float(
            math.pow(
                float(divide(salvage, cost)),
                float(divide(ONE, life)),
            )
        )
        rate = rate.quantize(Decimal("0.001"), rounding=ROUND_HALF_UP)

        remaining_cost = cost
        depreciation = ZERO
        for i in range(1, int(period) + 1):
            if i == 1:
                depreciation = divide(cost * rate * Decimal(month), Decimal(12))
            elif i == total_periods and month != 12:
                depreciation = divide(
                    remaining_cost * rate * Decimal(12 - month), Decimal(12)
                )
            else:
                depreciation = remaining_cost * rate
            remaining_cost = remaining_cost - depreciation
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
