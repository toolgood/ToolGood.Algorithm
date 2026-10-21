"""Function_COUNTIF (C#: Internals/Functions/MathSum/Function_COUNTIF.cs)."""
from __future__ import annotations
from decimal import Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.internals.functions.function_util import flatten_to_number_list, get_count_if, get_count_if_op, parse_sum_if_match
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _try_parse_decimal


@register("COUNTIF")
class Function_COUNTIF(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "CountIf", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "CountIf"

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

        if args2.is_number():
            count = get_count_if(values, args2.number_value())
        elif args2.is_text():
            text = args2.text_value().strip()
            number = _try_parse_decimal(text)
            if number is not None:
                count = get_count_if(values, number)
            else:
                match = parse_sum_if_match(text)
                if match is None:
                    return self._parameter_error(2)
                count = get_count_if_op(values, match[0], match[1])
        else:
            return self._parameter_error(2)
        return Operand.create(count)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.ARRAY)
        self.func2._get_parameter_types(none_engine, result, OperandType.NONE)
