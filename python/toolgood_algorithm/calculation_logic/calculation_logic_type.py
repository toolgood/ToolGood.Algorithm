"""Calculation logic type (``CalculationLogicType.java``)."""
from __future__ import annotations

from enum import IntEnum


class CalculationLogicType(IntEnum):
    """Kind of a recorded :class:`CalculationLogicInfo` entry."""

    Scene = 0
    """场景"""
    InitValue = 1
    """值"""
    Condition = 2
    """条件"""
    SetFormula = 3
    """用公式赋值"""
    SetValue = 4
    """赋值"""
    Error = 5
    """错误"""
    BlankLine = 6
    """空行"""
