"""Function_1 (C#: Internals/Functions/Function_1.cs)."""
from __future__ import annotations

from decimal import Decimal
from typing import Callable, List

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.parameter_type import ParameterType
from toolgood_algorithm.internals.functions.function_base import FunctionBase, _require_args


class Function_1(FunctionBase):
    def __init__(self, funcs):
        self.func1 = funcs[0] if isinstance(funcs, (list, tuple)) else funcs

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append(self.name())
        sb.append("(")
        self.func1.to_string(sb, False)
        sb.append(")")

    def _add_function(self, sb: list, function_name: str) -> None:
        sb.append(function_name + "(")
        self.func1.to_string(sb, False)
        sb.append(")")

    def _get_text_1(self, engine, tp) -> Operand:
        a = self.func1.evaluate(engine, tp)
        return a if a.is_text() else self._convert_to_text(a, 1)

    def _get_number_1(self, engine, tp) -> Operand:
        a = self.func1.evaluate(engine, tp)
        return a if a.is_number() else self._convert_to_number(a, 1)

    def _get_date_1(self, engine, tp) -> Operand:
        a = self.func1.evaluate(engine, tp)
        return a if a.is_date() else self._convert_to_date(a, 1)

    def _get_boolean_1(self, engine, tp) -> Operand:
        a = self.func1.evaluate(engine, tp)
        return a if a.is_boolean() else self._convert_to_boolean(a, 1)

    def _get_array_1(self, engine, tp) -> Operand:
        a = self.func1.evaluate(engine, tp)
        return a if a.is_array() else self._convert_to_array(a, 1)

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, operand_type, op, val)
