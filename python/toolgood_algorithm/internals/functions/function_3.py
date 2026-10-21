"""Function_3 (C#: Internals/Functions/Function_3.cs)."""
from __future__ import annotations

from decimal import Decimal
from typing import Callable, List

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.parameter_type import ParameterType
from toolgood_algorithm.internals.functions.function_base import FunctionBase, _require_args
from toolgood_algorithm.internals.functions.function_2 import Function_2


class Function_3(Function_2):
    def __init__(self, funcs):
        super().__init__(funcs)
        self.func3 = funcs[2] if len(funcs) > 2 else None

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append(self.name() + "(")
        self.func1.to_string(sb, False)
        for f in (self.func2, self.func3):
            if f is not None:
                sb.append(", ")
                f.to_string(sb, False)
        sb.append(")")

    def _get_text_3(self, engine, tp) -> Operand:
        a = self.func3.evaluate(engine, tp)
        return a if a.is_text() else self._convert_to_text(a, 3)

    def _get_number_3(self, engine, tp) -> Operand:
        a = self.func3.evaluate(engine, tp)
        return a if a.is_number() else self._convert_to_number(a, 3)

    def _get_date_3(self, engine, tp) -> Operand:
        a = self.func3.evaluate(engine, tp)
        return a if a.is_date() else self._convert_to_date(a, 3)

    def _get_boolean_3(self, engine, tp) -> Operand:
        a = self.func3.evaluate(engine, tp)
        return a if a.is_boolean() else self._convert_to_boolean(a, 3)

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        super()._get_parameter_types(none_engine, result, operand_type, op, val)
        if self.func3 is not None:
            self.func3._get_parameter_types(none_engine, result, operand_type, op, val)
