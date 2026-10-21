"""Function_0 (C#: Internals/Functions/Function_0.cs)."""
from __future__ import annotations

from decimal import Decimal
from typing import Callable, List

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.parameter_type import ParameterType
from toolgood_algorithm.internals.functions.function_base import FunctionBase, _require_args


class Function_0(FunctionBase):
    def __init__(self, funcs=None):
        pass

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append(self.name() + "()")

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        pass
