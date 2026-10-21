"""Calculation logic engine (规则 / 公式编排层).

Ported from ``csharp/ToolGood.Algorithm/CalculationLogic`` /
``java/.../calculationlogic``::

    CalculationLogicEngine logic = new CalculationLogicEngine(new FunctionCache(), true);
    logic.InitValue("a", 1);
    logic.InitValue("b", 2);
    logic.CheckCondition("a<b");
    logic.SetFormula("x", "a*3+b");
    print(logic.ToInfoString())
"""
from __future__ import annotations

from .calculation_logic_type import CalculationLogicType
from .engine import CalculationLogicEngine
from .info import CalculationLogicInfo

__all__ = [
    "CalculationLogicEngine",
    "CalculationLogicInfo",
    "CalculationLogicType",
]
