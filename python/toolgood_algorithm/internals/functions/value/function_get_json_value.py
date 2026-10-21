"""Function_GetJsonValue (C#: Internals/Functions/Value/Function_GetJsonValue.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.operands.operand import Operand, json_to_operand


class Function_GetJsonValue(Function_2):
    def __init__(self, func1, func2):
        super().__init__([func1, func2])

    def name(self) -> str:
        return "GetJsonValue"

    def evaluate(self, engine, tp) -> Operand:
        obj = self.func1.evaluate(engine, tp)
        if obj.is_error_or_none():
            return obj
        op = self.func2.evaluate(engine, tp)
        if op.is_error_or_none():
            return op

        if obj.is_array():
            op = self._convert_to_number(op, 2)
            if op.is_error_or_none():
                return op
            index = op.int_value() - engine.ExcelIndex
            array = obj.array_value()
            if 0 <= index < len(array):
                return array[index]
            return Operand.error(f"Function 'GetJsonValue' ARRAY index {index} out of range!")

        if obj.is_array_json():
            if op.is_number():
                key = op.number_value()
                result = obj.get_value(format(key, "f"))
                if result is not None:
                    return result
                return Operand.error(f"Function 'GetJsonValue' Parameter name '{format(key, 'f')}' is missing!")
            if op.is_text():
                result = obj.get_value(op.text_value())
                if result is not None:
                    return result
                return Operand.error(f"Function 'GetJsonValue' Parameter name '{op.text_value()}' is missing!")
            return Operand.error("Function 'GetJsonValue' Parameter name is missing!")

        if obj.is_json():
            json = obj.json_value()
            if json.is_array():
                op = self._convert_to_number(op, 2)
                if op.is_error_or_none():
                    return op
                index = op.int_value() - engine.ExcelIndex
                if 0 <= index < json.count():
                    return json_to_operand(json.get_child(index))
                return Operand.error(f"Function 'GetJsonValue' JSON index {index} out of range!")
            op = self._convert_to_text(op, 2)
            if op.is_error_or_none():
                return op
            value = json.get_child(op.text_value())
            if value is not None:
                return json_to_operand(value)
        return Operand.error("Function 'GetJsonValue' Operator is error!")

    def to_string(self, sb: list, add_brackets: bool) -> None:
        self.func1.to_string(sb, False)
        sb.append("[")
        self.func2.to_string(sb, False)
        sb.append("]")

    def get_result_type(self) -> OperandType:
        return OperandType.NONE

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.JSON)
        self.func2._get_parameter_types(none_engine, result, OperandType.NONE)
