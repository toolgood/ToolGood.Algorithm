"""Function_VALUE (C#: Internals/Functions/String/Function_VALUE.cs)."""
from __future__ import annotations
from decimal import Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_1 import Function_1
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _BIG_DECIMAL


@register("VALUE")
class Function_VALUE(Function_1):
    def __init__(self, funcs):
        if not isinstance(funcs, (list, tuple)):
            funcs = [funcs]
        _require_args(funcs, "Value", 1, 1)
        super().__init__(funcs)

    def name(self) -> str:
        return "Value"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        text = args1.text_value()
        if not _BIG_DECIMAL.match(text):
            return self._parameter_error(1)
        try:
            return Operand.create(Decimal(text))
        except (ArithmeticError, ValueError):
            return self._parameter_error(1)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NONE)
