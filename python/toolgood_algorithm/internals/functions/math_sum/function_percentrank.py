"""Function_PERCENTRANK (C#: Internals/Functions/MathSum/Function_PERCENTRANK.cs)."""
from __future__ import annotations
from decimal import Decimal, ROUND_HALF_UP, localcontext
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_3 import Function_3
from toolgood_algorithm.internals.functions.function_util import flatten_to_number_list
from toolgood_algorithm.math_net.excel_functions import percent_rank
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float


@register("PERCENTRANK")
class Function_PERCENTRANK(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "PercentRank", 2, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "PercentRank"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_array_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2

        values: list[Decimal] = []
        if not flatten_to_number_list(args1, values):
            return self._parameter_error(1)
        if not values:
            return self._parameter_error(1)

        value = percent_rank([float(v) for v in values], float(args2.number_value()))

        digits = 3
        if self.func3 is not None:
            args3 = self._get_number_3(engine, tp)
            if args3.is_error_or_none():
                return args3
            digits = args3.int_value()
            # aligned with Math.Round(decimal,int): more than 28 decimals is an error
            if digits < 0 or digits > 28:
                return self._parameter_error(3)

        with localcontext() as ctx:
            ctx.prec = 60
            result = from_float(value).quantize(Decimal(1).scaleb(-digits), rounding=ROUND_HALF_UP)
        return Operand.create(result)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.ARRAY)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func3 is not None:
            self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)
