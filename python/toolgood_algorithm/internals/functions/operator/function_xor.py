"""Function_XOR (C#: Internals/Functions/Operator/Function_XOR.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_n import Function_N
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("XOR")
class Function_XOR(Function_N):
    def name(self) -> str:
        return "Xor"

    def evaluate(self, engine, tp) -> Operand:
        true_count = 0
        for i in range(len(self.funcs)):
            arg = self._get_boolean(engine, tp, i)
            if arg.is_error_or_none():
                return arg
            if arg.boolean_value():
                true_count += 1
        return Operand.TRUE if true_count % 2 == 1 else Operand.FALSE

    def get_result_type(self) -> OperandType:
        return OperandType.BOOLEAN

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        for func in self.funcs:
            func._get_parameter_types(none_engine, result, OperandType.BOOLEAN)
