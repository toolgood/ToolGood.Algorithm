"""``_AggregateNumbers`` - variadic numeric shell (Python shared base)."""
from __future__ import annotations

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_n import Function_N


class _AggregateNumbers(Function_N):
    def __init__(self, funcs, minimum=1):
        _require_args(funcs, "AGG", minimum)
        super().__init__(funcs)

    def _collect(self, engine, tp):
        args = []
        for i, func in enumerate(self.funcs):
            value = self._get_number(engine, tp, i)
            if value.is_error_or_none():
                return value, None
            args.append(value)
        return None, args

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        for func in self.funcs:
            func._get_parameter_types(none_engine, result, OperandType.NUMBER)


__all__ = ["_AggregateNumbers"]
