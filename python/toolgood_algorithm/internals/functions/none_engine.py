"""NoneEngine (C#: Internals/Functions/NoneEngine.cs).

Engine used while inspecting parameter types: every parameter resolves to NULL.
"""
from __future__ import annotations

from toolgood_algorithm.algorithm_engine import AlgorithmEngine
from toolgood_algorithm.operands.operand import Operand


class NoneEngine(AlgorithmEngine):
    """Engine used while inspecting parameter types (all values are ``NULL``)."""

    def __init__(self, engine: AlgorithmEngine):
        super().__init__(engine.IgnoreCase)
        self.AreaUnit = engine.AreaUnit
        self.DistanceUnit = engine.DistanceUnit
        self.LastError = engine.LastError
        self.MassUnit = engine.MassUnit
        self.UseLocalTime = engine.UseLocalTime
        self.ExcelIndex = engine.ExcelIndex

    def get_parameter(self, parameter: str) -> Operand:
        return Operand.NULL

    def execute_diy_function(self, parameter: str, args) -> Operand:
        return Operand.NULL
