"""Function_DiyFunction (C#: Internals/Functions/Value/Function_DiyFunction.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_n import Function_N
from toolgood_algorithm.operands.operand import Operand


class Function_DiyFunction(Function_N):
    def __init__(self, fun_name: str, funcs):
        super().__init__(funcs)
        self.fun_name = fun_name

    def name(self) -> str:
        return "DiyFunction"

    def evaluate(self, engine, tp) -> Operand:
        args = [func.evaluate(engine, tp) for func in self.funcs]
        return engine.execute_diy_function(self.fun_name, args)

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append(self.fun_name + "(")
        for i, func in enumerate(self.funcs):
            if i:
                sb.append(", ")
            func.to_string(sb, False)
        sb.append(")")

    def get_result_type(self) -> OperandType:
        return OperandType.NONE

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        for func in self.funcs:
            func._get_parameter_types(none_engine, result, OperandType.NONE)
