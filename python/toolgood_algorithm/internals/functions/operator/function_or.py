"""Function_OR (C#: Internals/Functions/Operator/Function_OR.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.operands.operand import Operand


class Function_OR(Function_2):
    def name(self) -> str:
        return "Or"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_boolean_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        if args1.boolean_value():
            if engine.UseStrictMode:
                args2 = self._get_boolean_2(engine, tp)
                if args2.is_error_or_none():
                    return args2
            return Operand.TRUE
        return self._get_boolean_2(engine, tp)

    def to_string(self, sb: list, add_brackets: bool) -> None:
        if add_brackets:
            sb.append("(")
        self.func1.to_string(sb, False)
        sb.append(" || ")
        self.func2.to_string(sb, False)
        if add_brackets:
            sb.append(")")

    def get_result_type(self) -> OperandType:
        return OperandType.BOOLEAN

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.BOOLEAN)
        self.func2._get_parameter_types(none_engine, result, OperandType.BOOLEAN)
