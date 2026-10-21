"""Function_6 (C#: Internals/Functions/Function_6.cs)."""
from __future__ import annotations

from decimal import Decimal
from typing import Callable, List

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.parameter_type import ParameterType
from toolgood_algorithm.internals.functions.function_base import FunctionBase, _require_args
from toolgood_algorithm.internals.functions.function_5 import Function_5


class Function_6(Function_5):
    def __init__(self, funcs):
        super().__init__(funcs)
        self.func6 = funcs[5] if len(funcs) > 5 else None

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append(self.name() + "(")
        self.func1.to_string(sb, False)
        for f in (self.func2, self.func3, self.func4, self.func5, self.func6):
            if f is not None:
                sb.append(", ")
                f.to_string(sb, False)
        sb.append(")")

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        super()._get_parameter_types(none_engine, result, operand_type, op, val)
        if self.func6 is not None:
            self.func6._get_parameter_types(none_engine, result, operand_type, op, val)
