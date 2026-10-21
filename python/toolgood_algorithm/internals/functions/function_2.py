"""Function_2 (C#: Internals/Functions/Function_2.cs)."""
from __future__ import annotations

from decimal import Decimal
from typing import Callable, List

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.parameter_type import ParameterType
from toolgood_algorithm.internals.functions.function_base import FunctionBase, _require_args


class Function_2(FunctionBase):
    def __init__(self, funcs):
        if isinstance(funcs, (list, tuple)):
            self.func1 = funcs[0]
            self.func2 = funcs[1] if len(funcs) > 1 else None
        else:
            self.func1 = funcs
            self.func2 = None

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append(self.name() + "(")
        self.func1.to_string(sb, False)
        if self.func2 is not None:
            sb.append(", ")
            self.func2.to_string(sb, False)
        sb.append(")")

    def _args(self):
        return [f for f in (self.func1, self.func2) if f is not None]

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

    def _get_text_2(self, engine, tp) -> Operand:
        a = self.func2.evaluate(engine, tp)
        return a if a.is_text() else self._convert_to_text(a, 2)

    def _get_number_2(self, engine, tp) -> Operand:
        a = self.func2.evaluate(engine, tp)
        return a if a.is_number() else self._convert_to_number(a, 2)

    def _get_date_2(self, engine, tp) -> Operand:
        a = self.func2.evaluate(engine, tp)
        return a if a.is_date() else self._convert_to_date(a, 2)

    def _get_boolean_2(self, engine, tp) -> Operand:
        a = self.func2.evaluate(engine, tp)
        return a if a.is_boolean() else self._convert_to_boolean(a, 2)

    def _get_array_2(self, engine, tp) -> Operand:
        a = self.func2.evaluate(engine, tp)
        return a if a.is_array() else self._convert_to_array(a, 2)

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, operand_type, op, val)
        if self.func2 is not None:
            self.func2._get_parameter_types(none_engine, result, operand_type, op, val)
