"""flow: shared helpers and base classes (kept out of the per-class files)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_1 import Function_1
from toolgood_algorithm.operands.operand import Operand


class _IsBase(Function_1):
    def __init__(self, funcs):
        _require_args(funcs, "IS", 1, 1)
        super().__init__(funcs)

    def get_result_type(self) -> OperandType:
        return OperandType.BOOLEAN

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NONE)


def _equals_operand(a: Operand, b: Operand) -> bool:
    if a.is_number() and b.is_number():
        return a.number_value() == b.number_value()
    if a.is_text() and b.is_text():
        return a.text_value() == b.text_value()
    if a.is_boolean() and b.is_boolean():
        return a.boolean_value() == b.boolean_value()
    if a.is_date() and b.is_date():
        return a.date_value().to_long() == b.date_value().to_long()
    if a.is_null() and b.is_null():
        return True
    return False
