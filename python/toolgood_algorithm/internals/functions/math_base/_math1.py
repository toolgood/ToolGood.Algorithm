"""``_Math1`` - one argument shell returning a number (Python shared base)."""
from __future__ import annotations

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_1 import Function_1
from toolgood_algorithm.internals.functions.function_base import _require_args


class _Math1(Function_1):
    def __init__(self, funcs):
        if not isinstance(funcs, (list, tuple)):
            funcs = [funcs]
        _require_args(funcs, self.__class__.__name__, 1, 1)
        super().__init__(funcs)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)


__all__ = ["_Math1"]
