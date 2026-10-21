"""``_DateTime2`` - two argument date shell (Python shared base)."""
from __future__ import annotations

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.internals.functions.function_base import _require_args


class _DateTime2(Function_2):
    """Two parameter shell whose first argument is a date."""

    def __init__(self, funcs, display_name: str):
        if not isinstance(funcs, (list, tuple)):
            funcs = [funcs]
        _require_args(funcs, display_name, 2, 2)
        super().__init__(funcs)

    def get_result_type(self) -> OperandType:
        return OperandType.DATE

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.DATE)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)


__all__ = ["_DateTime2"]
