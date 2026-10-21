"""Function_Number2 (C#: Internals/Functions/Value/Function_Number2.cs)."""
from __future__ import annotations
from decimal import Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_0 import Function_0
from toolgood_algorithm.operands.operand import Operand

from ._common import _UNIT_DICT, transformation_unit


class Function_Number2(Function_0):
    def __init__(self, value: Decimal, unit: str):
        self.d = value
        self.unit = unit

    def name(self) -> str:
        return "Num"

    def evaluate(self, engine, tp) -> Operand:
        unit_type = _UNIT_DICT.get(self.unit.upper())
        value = transformation_unit(self.d, unit_type, engine.DistanceUnit, engine.AreaUnit, engine.VolumeUnit, engine.MassUnit)
        return Operand.create(value)

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append(str(self.d))
        sb.append(self.unit)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER
