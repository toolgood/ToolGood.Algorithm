"""Enumerations used by the algorithm engine.

Ported from ``toolgood.algorithm.enums`` (C# / Java).
"""
from __future__ import annotations

from enum import IntEnum


class OperandType(IntEnum):
    """Operation result types."""

    NONE = 0
    NULL = 1
    ERROR = 2
    DATE = 3
    ARRAY = 4
    NUMBER = 5
    BOOLEAN = 6
    TEXT = 7
    JSON = 8
    ARRAYJSON = 9


class NumberUnitType(IntEnum):
    MM = 1
    CM = 2
    DM = 3
    M = 4
    KM = 5

    MM2 = 11
    CM2 = 12
    DM2 = 13
    M2 = 14
    KM2 = 15

    MM3 = 21
    CM3 = 22
    DM3 = 23
    M3 = 24
    KM3 = 25

    G = 31
    KG = 32
    T = 33


class DistanceUnitType(IntEnum):
    MM = 1
    CM = 2
    DM = 3
    M = 4
    KM = 5


class AreaUnitType(IntEnum):
    MM2 = 11
    CM2 = 12
    DM2 = 13
    M2 = 14
    KM2 = 15


class VolumeUnitType(IntEnum):
    MM3 = 21
    CM3 = 22
    DM3 = 23
    M3 = 24
    KM3 = 25


class MassUnitType(IntEnum):
    G = 31
    KG = 32
    T = 33


class CalculateTreeType(IntEnum):
    String = 0
    Add = 1
    Sub = 2
    Mul = 3
    Div = 4
    Mod = 5
    Connect = 6
    And = 7
    Or = 8
    OpGt = 9
    OpLt = 10
    OpGe = 11
    OpLe = 12
    OpEq = 13
    OpNe = 14
    Error = 15

    @staticmethod
    def int_to_enum(value: int) -> "CalculateTreeType | None":
        try:
            return CalculateTreeType(value)
        except ValueError:
            return None


class ConditionTreeType(IntEnum):
    String = 0
    And = 1
    Or = 2
    Error = 3

    @staticmethod
    def int_to_enum(value: int) -> "ConditionTreeType | None":
        try:
            return ConditionTreeType(value)
        except ValueError:
            return None


class CombineCalculateType(IntEnum):
    Add = 1
    Sub = 2
    Mul = 3
    Div = 4
    Mod = 5
    Connect = 6
    And = 7
    Or = 8
    OpGt = 9
    OpLt = 10
    OpGe = 11
    OpLe = 12
    OpEq = 13
    OpNe = 14

    @staticmethod
    def int_to_enum(value: int) -> "CombineCalculateType | None":
        try:
            return CombineCalculateType(value)
        except ValueError:
            return None
