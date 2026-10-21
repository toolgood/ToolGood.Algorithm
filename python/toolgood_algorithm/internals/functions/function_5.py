"""Function_5 (C#: Internals/Functions/Function_5.cs)."""
from __future__ import annotations

from decimal import Decimal
from typing import Callable, List

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.parameter_type import ParameterType
from toolgood_algorithm.internals.functions.function_base import FunctionBase, _require_args
from toolgood_algorithm.internals.functions.function_4 import Function_4


class Function_5(Function_4):
    def __init__(self, funcs):
        super().__init__(funcs)
        self.func5 = funcs[4] if len(funcs) > 4 else None

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append(self.name() + "(")
        self.func1.to_string(sb, False)
        for f in (self.func2, self.func3, self.func4, self.func5):
            if f is not None:
                sb.append(", ")
                f.to_string(sb, False)
        sb.append(")")

    def _get_number_5(self, engine, tp) -> Operand:
        a = self.func5.evaluate(engine, tp)
        return a if a.is_number() else self._convert_to_number(a, 5)

    def _get_text_5(self, engine, tp) -> Operand:
        a = self.func5.evaluate(engine, tp)
        return a if a.is_text() else self._convert_to_text(a, 5)

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        super()._get_parameter_types(none_engine, result, operand_type, op, val)
        if self.func5 is not None:
            self.func5._get_parameter_types(none_engine, result, operand_type, op, val)
