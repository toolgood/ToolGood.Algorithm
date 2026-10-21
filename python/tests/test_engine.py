"""Public API tests for ``AlgorithmEngine``."""
from __future__ import annotations

from decimal import Decimal

import pytest

from toolgood_algorithm import AlgorithmEngine, AlgorithmEngineEx, Operand, OperandType


@pytest.fixture()
def engine():
    return AlgorithmEngine()


def test_arithmetic(engine):
    assert engine.try_evaluate("1+2*3", 0) == 7
    assert engine.try_evaluate("(8-3)*(3+2)", 0) == 25
    assert engine.try_evaluate("2+3*2+10/2*4", 0) == 28
    assert engine.try_evaluate("2.1e3 + 10", 0) == 2110
    assert engine.try_evaluate("2.1e-3 + 10", 0.0) == pytest.approx(10.0021)
    assert engine.try_evaluate("10/4", 0.0) == pytest.approx(2.5)


def test_operators_and_compare(engine):
    assert engine.try_evaluate("2>1 && 3>2", False) is True
    assert engine.try_evaluate("1>2 || 3>2", False) is True
    assert engine.try_evaluate("'aa'&'bb'", "") == "aabb"
    assert engine.try_evaluate("'3'+2", "") == "5"
    assert engine.try_evaluate("-7 < -2 ? 1 : 2", 0) == 1
    assert engine.try_evaluate("1！=2？1：2", 0) == 1  # full width punctuation
    assert engine.try_evaluate("1=1m", False) is True  # unit literal folded at parse time


def test_text_conversion(engine):
    assert engine.try_evaluate("TEXT(123,'0.00')", "") == "123.00"
    assert engine.try_evaluate("123.456.TEXT('0.00')", "") == "123.46"
    assert engine.try_evaluate("LEFT('abcd',2)", "") == "ab"
    assert engine.try_evaluate("MID('中文测试',1,2)", "") == "中文"
    assert engine.try_evaluate("'abc'.UPPER()", "") == "ABC"


def test_typed_try_evaluate(engine):
    assert engine.try_evaluate_int("1+1", -1) == 2
    assert engine.try_evaluate_double("1/4", 0.0) == pytest.approx(0.25)
    assert engine.try_evaluate_number("1/8", None) == Decimal("0.125")
    assert engine.try_evaluate_text("123", None) == "123"
    assert engine.try_evaluate_bool("1=1", False) is True
    # failures fall back to the default and record LastError
    assert engine.try_evaluate_int("1/0", -1) == -1
    assert engine.LastError


def test_errors(engine):
    assert engine.try_evaluate("1/0", -1) == -1
    assert "Div 0" in engine.LastError
    assert engine.try_evaluate("missing", -1) == -1
    assert "missing" in engine.LastError
    assert engine.try_evaluate("1+", -1) == -1
    assert engine.LastError


def test_strict_mode():
    engine = AlgorithmEngine()
    engine.UseStrictMode = False
    assert engine.try_evaluate("(2>1) || ERROR('x')", False) is True
    engine.UseStrictMode = True
    assert engine.try_evaluate("(2>1) || ERROR('x')", None) is None
    assert "x" in (engine.LastError or "")


def test_excel_index(engine):
    assert engine.try_evaluate("IndexOf('abcd','cd')", -1) == 3
    assert engine.try_evaluate("['a','b','c'][1]", "") == "a"
    engine.SetUseExcelIndex(False)
    assert engine.try_evaluate("IndexOf('abcd','cd')", -1) == 2
    assert engine.try_evaluate("['a','b','c'][1]", "") == "b"


def test_parse_and_tostring(engine):
    function = engine.parse("1+2*3")
    assert engine.evaluate(function) == engine.evaluate(engine.parse("7"))
    assert str(function) == "1 + 2 * 3"
    assert function.get_result_type() == OperandType.NUMBER


def test_parse_cache():
    engine = AlgorithmEngine()
    engine.UseParseCache = True
    first = engine.parse("1+1")
    second = engine.parse("1+1")
    assert first is second


def test_custom_parameter_and_function():
    class Custom(AlgorithmEngine):
        def get_parameter(self, parameter):
            return Operand.create(10) if parameter == "半径" else super().get_parameter(parameter)

        def execute_diy_function(self, parameter, args):
            if parameter == "double":
                return Operand.create(args[0].number_value() * 2)
            return super().execute_diy_function(parameter, args)

    engine = Custom()
    assert engine.try_evaluate("半径*2", 0) == 20
    assert engine.try_evaluate("double(半径)", 0) == 20


def test_engine_ex_parameters():
    engine = AlgorithmEngineEx()
    engine.AddParameter("num", 42)
    assert engine.try_evaluate("num", 0) == 42
    engine.AddParameter("name", "Test")
    assert engine.try_evaluate("name & 1", "") == "Test1"
    engine.ClearParameters()
    assert engine.try_evaluate("num", -1) == -1

    case_engine = AlgorithmEngineEx(True)
    case_engine.AddParameter("Name", "Test")
    assert case_engine.try_evaluate("name", "") == "Test"

    json_engine = AlgorithmEngineEx()
    json_engine.AddParameterFromJson('{"a":1,"b":"x"}')
    assert json_engine.try_evaluate("a", 0) == 1
    assert json_engine.try_evaluate("b", "") == "x"


def test_array_and_json(engine):
    assert str(engine.try_evaluate("[1,2,3]", None)) == "[1,2,3]"
    assert engine.try_evaluate("[1,2,3][2]", 0) == 2
    assert engine.try_evaluate("json('{\"a\":1}')[\"a\"]", 0) == 1
    assert engine.try_evaluate("json('{\"a\":{\"b\":5}}').a.b", 0) == 5
    assert engine.try_evaluate("{name:'toolgood'}['name']", "") == "toolgood"
    assert engine.try_evaluate("json(\"{'x':1}\")['x']", 0) == 1  # lenient single quotes


def test_parameter_types(engine):
    function = engine.parse("test+1")
    types = function.get_parameter_types(engine)
    assert [p.name for p in types] == ["test"]
    assert types[0].type == OperandType.NUMBER

    function = engine.parse("if(test,2,3)")
    types = function.get_parameter_types(engine)
    assert types[0].name == "test"
    assert types[0].type == OperandType.BOOLEAN

    function = engine.parse("left(test,ww)")
    types = function.get_parameter_types(engine)
    assert [p.name for p in types] == ["test", "ww"]
    assert types[0].type == OperandType.TEXT
