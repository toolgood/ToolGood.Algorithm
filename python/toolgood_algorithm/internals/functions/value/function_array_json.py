"""Function_ArrayJson (C#: Internals/Functions/Value/Function_ArrayJson.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_n import Function_N
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.operands.operand_key_value import OperandKeyValue
from toolgood_algorithm.operands.operand_key_value_list import OperandKeyValueList
from toolgood_algorithm.internals.functions.registry import register


@register("ARRAYJSON")
class Function_ArrayJson(Function_N):
    def name(self) -> str:
        return "ArrayJson"

    def evaluate(self, engine, tp) -> Operand:
        result = OperandKeyValueList()
        for func in self.funcs:
            value = func.evaluate(engine, tp)
            if value.is_error_or_none():
                return value
            if isinstance(value, OperandKeyValue):
                result.add_value(value.value())
            else:
                return self._parameter_error(1)
        return result

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append("{")
        for i, func in enumerate(self.funcs):
            if i:
                sb.append(", ")
            func.to_string(sb, False)
        sb.append("}")

    def get_result_type(self) -> OperandType:
        return OperandType.ARRAYJSON

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        for func in self.funcs:
            func._get_parameter_types(none_engine, result, OperandType.NONE)
