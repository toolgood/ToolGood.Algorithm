"""Regression tests for reported issues.

Ported from ``tests/IssuesTest.java``.
"""
from __future__ import annotations

from toolgood_algorithm import AlgorithmEngine, AlgorithmEngineEx


def test_issues_12():
    # 防止有误解，禁止数字直接转时间；禁止时间直接转数字。
    # Java 原用例主体已被注释掉（Year/Month/Day(date) 与 date(...)->num 均被禁止），
    # 这里保持同样的“无需断言”语义。
    engine = AlgorithmEngine()
    assert engine is not None


def test_issues_13():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("days360(date(2020,5,31),date(2023,12,15))", 0)
    assert dt == 1275


def test_issues_0():
    engine = AlgorithmEngineEx()
    engine.add_parameter("瓦楞", "BC")
    dt = engine.try_evaluate(
        '{"A": 0.6,"B": 0.4,"C": 0.6,"E": 0.33,"F": 0.29,"Z": 0.15\n'
        ',"EB": 0.7,"EE": 0.65,"EA": 0.85,"AB": 1.0,"BC": 1.0,"AA":1.0\n'
        ',"EBC": 1.15,"BAB": 1.25,"BCB": 1.25,"BBC": 1.25,"CBB": 1.25,"EBA": 1.2,"AAA": 1.4}[瓦楞]',
        0.0,
    )
    assert dt == 1.0
