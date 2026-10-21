"""Function_ArrayJsonItem (C#: Internals/Functions/Value/Function_ArrayJsonItem.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_1 import Function_1
from toolgood_algorithm.operands.key_value import KeyValue
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.operands.operand_key_value import OperandKeyValue


class Function_ArrayJsonItem(Function_1):
    def __init__(self, key: str, func1):
        super().__init__([func1])
        self.key = key

    def name(self) -> str:
        return "ArrayJsonItem"

    def evaluate(self, engine, tp) -> Operand:
        return OperandKeyValue(KeyValue(self.key, self.func1.evaluate(engine, tp)))

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append(str(self.key))
        sb.append(":")
        self.func1.to_string(sb, False)

    def get_result_type(self) -> OperandType:
        return OperandType.ARRAYJSON

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NONE)
