"""Tests ported from ``java/.../AlgorithmEngine/EngineTest.java``.

包含 ``Cylinder.java`` 自定义引擎的 Python 版本（覆写
``get_parameter_ex`` / ``execute_diy_function``）。
"""
from __future__ import annotations

import math

import pytest

from toolgood_algorithm import AlgorithmEngine, AlgorithmEngineEx, Operand


class Cylinder(AlgorithmEngineEx):
    """Java: Cylinder.java - 圆柱体自定义引擎。"""

    def __init__(self, radius: int, height: int):
        super().__init__()
        self._radius = radius
        self._height = height

    def get_parameter_ex(self, parameter: str) -> Operand:
        if parameter == "半径":
            return Operand.create(self._radius)
        if parameter == "直径":
            return Operand.create(self._radius * 2)
        if parameter == "高":
            return Operand.create(self._height)
        return super().get_parameter_ex(parameter)

    def execute_diy_function(self, parameter: str, args) -> Operand:
        if parameter == "求面积":
            if len(args) == 1:
                r = float(args[0].to_number("It can't be converted to number!").number_value())
                return Operand.create(r * r * math.pi)
        return super().execute_diy_function(parameter, args)


def test_base_test2():
    # Java: EngineTest.java:11
    engine = AlgorithmEngine()
    engine.try_evaluate_text("1+(3*2+2)/2 & '11' & '11:20'*9 & isnumber(22)*3", "")


def test_base_test3():
    # Java: EngineTest.java:17
    engine = AlgorithmEngine()

    c = engine.try_evaluate("(2)+/*123456*/3", 0)
    assert c == 5

    c = engine.try_evaluate("2+3//eee", 0)
    assert c == 5

    c = engine.try_evaluate("(2)+/*123456*/3 ee22+22", 0)
    assert c == 0


def test_base_test4():
    # Java: EngineTest.java:31
    engine = AlgorithmEngine()

    c = engine.try_evaluate_text("'4dd'&'55' rr", "")
    assert c == ""


def test_base_test5():
    # Java: EngineTest.java:39
    engine = AlgorithmEngine()

    c = engine.try_evaluate_text("'4dd'&'55'.left(1)", "")
    assert c == "4dd5"


def test_cylinder():
    # Java: EngineTest.java:47
    c = Cylinder(3, 10)
    t2 = c.try_evaluate("半径*半径*pi()", 0.0)

    t = c.try_evaluate("直径*pi()", 0.0)
    t = c.try_evaluate("半径*半径*pi()*高", 0.0)

    t = c.try_evaluate("'半径'*半径*pi()*高", 0.0)

    t = c.try_evaluate("求面积（10）", 0.0)
    assert t == pytest.approx(10 * 10 * math.pi, abs=10.0)

    json = "{'灰色':'L','canBookCount':905,'saleCount':91,'specId':'43b0e72e98731aed69e1f0cc7d64bf4d'}"
    c.add_parameter_from_json(json)

    tt = c.try_evaluate_text("灰色", "")
    assert tt == "L"
