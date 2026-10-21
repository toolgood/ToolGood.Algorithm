"""Function_AVERAGEIF (C#: Internals/Functions/MathSum/Function_AVERAGEIF.cs)."""
from __future__ import annotations
from decimal import Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_3 import Function_3
from toolgood_algorithm.internals.functions.function_util import flatten_to_number_list, get_count_if, get_count_if_op, get_sum_if, get_sum_if_op, parse_sum_if_match
from toolgood_algorithm.internals.numeric import ZERO, divide
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _arg_array, _try_parse_decimal


@register("AVERAGEIF")
class Function_AVERAGEIF(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "AverageIf", 2, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "AverageIf"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_array_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self.func2.evaluate(engine, tp)
        if args2.is_error_or_none():
            return args2

        values: list[Decimal] = []
        if not flatten_to_number_list(args1, values):
            return self._parameter_error(1)

        if self.func3 is not None:
            args3 = _arg_array(self, engine, tp, 3)
            if args3.is_error_or_none():
                return args3
            sums: list[Decimal] = []
            if not flatten_to_number_list(args3, sums):
                return self._parameter_error(3)
        else:
            sums = values

        count = 0
        total = ZERO
        if args2.is_number():
            number = args2.number_value()
            count = get_count_if(values, number)
            total = get_sum_if(values, number, sums)
        elif args2.is_text():
            text = args2.text_value().strip()
            number = _try_parse_decimal(text)
            if number is not None:
                count = get_count_if(values, number)
                total = get_sum_if(values, number, sums)
            else:
                match = parse_sum_if_match(text)
                if match is None:
                    return self._parameter_error(2)
                count = get_count_if_op(values, match[0], match[1])
                total = get_sum_if_op(values, match[0], match[1], sums)
        else:
            return self._parameter_error(2)

        if count == 0:
            return self._div0_error()
        return Operand.create(divide(total, Decimal(count)))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.ARRAY)
        self.func2._get_parameter_types(none_engine, result, OperandType.NONE)
        if self.func3 is not None:
            self.func3._get_parameter_types(none_engine, result, OperandType.ARRAY)
