"""Function_TODAY (C#: Internals/Functions/Value/Function_TODAY.cs)."""
from __future__ import annotations
import datetime as _dt
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_0 import Function_0
from toolgood_algorithm.operands.my_date import MyDate
from toolgood_algorithm.operands.operand import Operand


class Function_TODAY(Function_0):
    def name(self) -> str:
        return "Today"

    def evaluate(self, engine, tp) -> Operand:
        now = _dt.datetime.now() if engine.UseLocalTime else _dt.datetime.utcnow()
        return Operand.create(MyDate(now.year, now.month, now.day, 0, 0, 0))

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append("Today()")

    def get_result_type(self) -> OperandType:
        return OperandType.DATE
