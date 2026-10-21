"""Function_PARAMETER (C#: Internals/Functions/Value/Function_PARAMETER.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_0 import Function_0
from toolgood_algorithm.operands.operand import Operand


class Function_PARAMETER(Function_0):
    def __init__(self, name: str):
        self.name_text = name

    def name(self) -> str:
        return "Parameter"

    def evaluate(self, engine, tp) -> Operand:
        if tp is not None:
            result = tp(engine, self.name_text)
            if result is not None:
                return result
        return Operand.error(f"Parameter '{self.name_text}' is missing.")

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append(self.name_text)

    def get_result_type(self) -> OperandType:
        return OperandType.NONE

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        from toolgood_algorithm.internals.parameter_type import ParameterType

        result.append(ParameterType(self.name_text, operand_type, op, val))
