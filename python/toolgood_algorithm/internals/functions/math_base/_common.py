"""math_base: shared helpers and base classes (kept out of the per-class files)."""
from __future__ import annotations
from decimal import Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_1 import Function_1
from toolgood_algorithm.internals.functions.function_n import Function_N
from toolgood_algorithm.operands.operand import Operand


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


class _AggregateNumbers(Function_N):
    def __init__(self, funcs, minimum=1):
        _require_args(funcs, "AGG", minimum)
        super().__init__(funcs)

    def _collect(self, engine, tp):
        args: list[Operand] = []
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


def from_float(value: float) -> Decimal:
    """``BigDecimal.valueOf(double)`` equivalent."""
    if value != value or value in (float("inf"), float("-inf")):
        raise ValueError("Number is NaN or Infinity!")
    return Decimal(str(value))
