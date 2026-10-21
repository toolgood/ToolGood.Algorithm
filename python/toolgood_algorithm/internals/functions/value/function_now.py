"""Function_NOW (C#: Internals/Functions/Value/Function_NOW.cs)."""
from __future__ import annotations
import datetime as _dt
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_0 import Function_0
from toolgood_algorithm.operands.operand import Operand


class Function_NOW(Function_0):
    def name(self) -> str:
        return "Now"

    def evaluate(self, engine, tp) -> Operand:
        if engine.UseLocalTime:
            return Operand.create(_dt.datetime.now())
        return Operand.create(_dt.datetime.utcnow())

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append("Now()")

    def get_result_type(self) -> OperandType:
        return OperandType.DATE
