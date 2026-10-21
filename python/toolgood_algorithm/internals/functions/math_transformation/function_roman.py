"""Function_ROMAN (C#: Internals/Functions/MathTransformation/Function_ROMAN.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _arabic_to_roman


@register("ROMAN")
class Function_ROMAN(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "ROMAN", 1, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "ROMAN"

    def evaluate(self, engine, tp) -> Operand:
        num_arg = self._get_number_1(engine, tp)
        if num_arg.is_error_or_none():
            return num_arg
        num = num_arg.int_value()
        # Excel: number 为负或大于 3999 时返回 #VALUE!,number 为 0 返回空文本
        if num < 0 or num > 3999:
            return self._parameter_error(1)
        form = 0
        if self.func2 is not None:
            form_arg = self._get_number_2(engine, tp)
            if form_arg.is_error_or_none():
                return form_arg
            form = form_arg.int_value()
            # Excel: form 超出 0~4 范围返回 #VALUE!
            if form < 0 or form > 4:
                return self._parameter_error(2)
        return Operand.create(_arabic_to_roman(num, form))

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func2 is not None:
            self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
