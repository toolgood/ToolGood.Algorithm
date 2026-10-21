"""Function_MULTINOMIAL (C#: Internals/Functions/MathBase/Function_MULTINOMIAL.cs)."""
from __future__ import annotations
from decimal import Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_n import Function_N
from toolgood_algorithm.internals.functions.function_util import flatten_to_big_decimal_list, get_factorial
from toolgood_algorithm.internals.numeric import ONE, ZERO
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("MULTINOMIAL")
class Function_MULTINOMIAL(Function_N):
    def __init__(self, funcs):
        _require_args(funcs, "MULTINOMIAL", 1)
        super().__init__(funcs)

    def name(self) -> str:
        return "Multinomial"

    def evaluate(self, engine, tp) -> Operand:
        args: list[Operand] = []
        error = self.try_evaluate_all(engine, tp, args)
        if error is not None:
            return error
        values: list[Decimal] = []
        if not flatten_to_big_decimal_list(args, values):
            return self._function_error()
        total = ZERO
        denominator = ONE
        for i, value in enumerate(values):
            if value > 2147483647 or value < -2147483648:
                return self._parameter_error(i + 1)
            a = int(value)
            if a < 0:
                return self._parameter_error(i + 1)
            denominator *= get_factorial(Decimal(a))
            total += a
        return Operand.create(get_factorial(total) / denominator)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        for func in self.funcs:
            func._get_parameter_types(none_engine, result, OperandType.NUMBER)
