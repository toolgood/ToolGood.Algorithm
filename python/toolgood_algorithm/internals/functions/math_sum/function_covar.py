"""Function_COVAR (C#: Internals/Functions/MathSum/Function_COVAR.cs)."""
from __future__ import annotations
from decimal import Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.internals.functions.function_util import flatten_to_number_list
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float


@register("COVAR")
class Function_COVAR(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "Covar", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Covar"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self.func1.evaluate(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self.func2.evaluate(engine, tp)
        if args2.is_error_or_none():
            return args2

        list1: list[Decimal] = []
        list2: list[Decimal] = []
        if not flatten_to_number_list(args1, list1):
            return self._parameter_error(1)
        if not flatten_to_number_list(args2, list2):
            return self._parameter_error(2)
        if len(list1) != len(list2) or len(list1) == 0:
            return Operand.error(f"Function '{self.name()}' parameter's count error!")

        mean1 = 0.0
        mean2 = 0.0
        c = 0.0
        for i in range(len(list1)):
            delta1 = float(list1[i]) - mean1
            delta2 = float(list2[i]) - mean2
            mean1 += delta1 / (i + 1)
            mean2 += delta2 / (i + 1)
            c += delta1 * (float(list2[i]) - mean2)
        return Operand.create(from_float(c / len(list1)))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.ARRAY)
        self.func2._get_parameter_types(none_engine, result, OperandType.ARRAY)
