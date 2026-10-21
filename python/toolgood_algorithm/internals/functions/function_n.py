"""Function_N (C#: Internals/Functions/Function_N.cs)."""
from __future__ import annotations

from decimal import Decimal
from typing import Callable, List

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.parameter_type import ParameterType
from toolgood_algorithm.internals.functions.function_base import FunctionBase, _require_args


class Function_N(FunctionBase):
    def __init__(self, funcs):
        self.funcs = list(funcs)

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append(self.name() + "(")
        for i, f in enumerate(self.funcs):
            if i:
                sb.append(", ")
            f.to_string(sb, False)
        sb.append(")")

    def try_evaluate_all(self, engine, tp, args: list) -> Operand | None:
        for func in self.funcs:
            value = func.evaluate(engine, tp)
            if value.is_error_or_none():
                return value
            args.append(value)
        return None

    def _get_text(self, engine, tp, idx: int) -> Operand:
        a = self.funcs[idx].evaluate(engine, tp)
        return a if a.is_text() else self._convert_to_text(a, idx)

    def _get_number(self, engine, tp, idx: int) -> Operand:
        a = self.funcs[idx].evaluate(engine, tp)
        return a if a.is_number() else self._convert_to_number(a, idx)

    def _get_date(self, engine, tp, idx: int) -> Operand:
        a = self.funcs[idx].evaluate(engine, tp)
        return a if a.is_date() else self._convert_to_date(a, idx)

    def _get_boolean(self, engine, tp, idx: int) -> Operand:
        a = self.funcs[idx].evaluate(engine, tp)
        return a if a.is_boolean() else self._convert_to_boolean(a, idx)

    def _get_array(self, engine, tp, idx: int) -> Operand:
        a = self.funcs[idx].evaluate(engine, tp)
        return a if a.is_array() else self._convert_to_array(a, idx)

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        for func in self.funcs:
            func._get_parameter_types(none_engine, result, operand_type, op, val)
