"""Tests for ``AlgorithmEngineEx``.

Ported from ``src/test/java/toolgood/algorithm/algorithmEngineEx/AlgorithmEngineExTest.java``.
"""
from __future__ import annotations

from decimal import Decimal

import pytest

from toolgood_algorithm import AlgorithmEngineEx, MyDate, Operand


# region 构造函数测试


def test_constructor_default():
    # Java: AlgorithmEngineExTest.java:19
    engine = AlgorithmEngineEx()
    assert engine.IgnoreCase is False
    assert engine.UseTempDict is False


def test_constructor_ignore_case():
    # Java: AlgorithmEngineExTest.java:26
    engine = AlgorithmEngineEx(True)
    assert engine.IgnoreCase is True

    engine = AlgorithmEngineEx(False)
    assert engine.IgnoreCase is False


# endregion

# region AddParameter 测试


def test_add_parameter_bool():
    # Java: AlgorithmEngineExTest.java:39
    engine = AlgorithmEngineEx()
    engine.add_parameter("flag", True)
    assert engine.try_evaluate("flag", False) is True

    engine.add_parameter("flag", False)
    assert engine.try_evaluate("flag", True) is False


def test_add_parameter_int():
    # Java: AlgorithmEngineExTest.java:51
    engine = AlgorithmEngineEx()
    engine.add_parameter("num", 42)
    assert engine.try_evaluate("num", 0) == 42


def test_add_parameter_long():
    # Java: AlgorithmEngineExTest.java:59
    engine = AlgorithmEngineEx()
    engine.add_parameter("num", 123456789)
    assert engine.try_evaluate("num", 0) == 123456789


def test_add_parameter_double():
    # Java: AlgorithmEngineExTest.java:67
    engine = AlgorithmEngineEx()
    engine.add_parameter("num", 3.14159)
    assert engine.try_evaluate("num", 0.0) == pytest.approx(3.14159, abs=0.00001)


def test_add_parameter_decimal():
    # Java: AlgorithmEngineExTest.java:75
    engine = AlgorithmEngineEx()
    engine.add_parameter("num", Decimal("123.45"))
    result = engine.try_evaluate("num", Decimal("0"))
    assert Decimal("123.45").compare(result) == 0


def test_add_parameter_string():
    # Java: AlgorithmEngineExTest.java:83
    engine = AlgorithmEngineEx()
    engine.add_parameter("name", "ToolGood")
    assert engine.try_evaluate("name", "") == "ToolGood"


def test_add_parameter_date_time():
    # Java: AlgorithmEngineExTest.java:91
    engine = AlgorithmEngineEx()
    engine.add_parameter("dt", MyDate(2024, 6, 15, 10, 30, 0))
    assert engine.try_evaluate("year(dt)", 0) == 2024
    assert engine.try_evaluate("month(dt)", 0) == 6
    assert engine.try_evaluate("day(dt)", 0) == 15


def test_add_parameter_time_span():
    # Java: AlgorithmEngineExTest.java:104
    engine = AlgorithmEngineEx()
    engine.add_parameter("ti", MyDate(None, None, None, 10, 30, 0))
    assert engine.try_evaluate("hour(ti)", 0) == 10
    assert engine.try_evaluate("minute(ti)", 0) == 30


def test_add_parameter_list():
    # Java: AlgorithmEngineExTest.java:115
    engine = AlgorithmEngineEx()
    engine.add_parameter("arr", [Operand.create(1), Operand.create(2), Operand.create(3)])
    assert engine.try_evaluate("count(arr)", 0) == 3
    assert engine.try_evaluate("arr[1]", 0) == 1


def test_add_parameter_collection_string():
    # Java: AlgorithmEngineExTest.java:129
    engine = AlgorithmEngineEx()
    engine.add_parameter(
        "names", Operand.create([Operand.create("a"), Operand.create("b"), Operand.create("c")])
    )
    assert engine.try_evaluate("count(names)", 0) == 3


def test_add_parameter_collection_int():
    # Java: AlgorithmEngineExTest.java:141
    engine = AlgorithmEngineEx()
    engine.add_parameter(
        "nums",
        Operand.create([Operand.create(1), Operand.create(2), Operand.create(3),
                        Operand.create(4), Operand.create(5)]),
    )
    assert engine.try_evaluate("sum(nums)", 0) == 15


def test_add_parameter_collection_double():
    # Java: AlgorithmEngineExTest.java:155
    engine = AlgorithmEngineEx()
    engine.add_parameter(
        "nums",
        Operand.create([Operand.create(1.1), Operand.create(2.2), Operand.create(3.3)]),
    )
    assert engine.try_evaluate("sum(nums)", 0.0) == pytest.approx(6.6, abs=0.01)


def test_add_parameter_collection_bool():
    # Java: AlgorithmEngineExTest.java:167
    engine = AlgorithmEngineEx()
    engine.add_parameter(
        "flags",
        Operand.create([Operand.create(True), Operand.create(False), Operand.create(True)]),
    )
    assert engine.try_evaluate("count(flags)", 0) == 3


def test_add_parameter_operand():
    # Java: AlgorithmEngineExTest.java:179
    engine = AlgorithmEngineEx()
    engine.add_parameter("val", Operand.create(100))
    assert engine.try_evaluate("val", 0) == 100


# endregion

# region AddParameterFromJson 测试


def test_add_parameter_from_json():
    # Java: AlgorithmEngineExTest.java:191
    engine = AlgorithmEngineEx()
    engine.add_parameter_from_json('{"name":"test","age":25,"active":true}')

    assert engine.try_evaluate("name", "") == "test"
    assert engine.try_evaluate("age", 0) == 25
    assert engine.try_evaluate("active", False) is True


def test_add_parameter_from_json_complex():
    # Java: AlgorithmEngineExTest.java:206
    engine = AlgorithmEngineEx()
    engine.add_parameter_from_json('{"灰色":"L","canBookCount":905,"saleCount":91}')

    assert engine.try_evaluate("灰色", "") == "L"
    assert engine.try_evaluate("canBookCount", 0) == 905


def test_add_parameter_from_json_invalid():
    # Java: AlgorithmEngineExTest.java:218
    # Java 捕获任意 Exception；Python 移植抛出 ValueError。
    engine = AlgorithmEngineEx()
    with pytest.raises(ValueError):
        engine.add_parameter_from_json("not a json")


# endregion

# region ClearParameters 测试


def test_clear_parameters():
    # Java: AlgorithmEngineExTest.java:234
    engine = AlgorithmEngineEx()
    engine.add_parameter("num", 42)
    assert engine.try_evaluate("num", 0) == 42

    engine.clear_parameters()
    assert engine.try_evaluate("num", -1) == -1


# endregion

# region UseTempDict 测试


def test_use_temp_dict():
    # Java: AlgorithmEngineExTest.java:250
    engine = AlgorithmEngineEx()
    engine.UseTempDict = True
    engine.add_parameter("num", 42)

    assert engine.try_evaluate("num", 0) == 42


# endregion

# region IgnoreCase 测试


def test_ignore_case_true():
    # Java: AlgorithmEngineExTest.java:264
    engine = AlgorithmEngineEx(True)
    engine.add_parameter("Name", "Test")

    assert engine.try_evaluate("name", "") == "Test"
    assert engine.try_evaluate("NAME", "") == "Test"
    assert engine.try_evaluate("Name", "") == "Test"


def test_ignore_case_false():
    # Java: AlgorithmEngineExTest.java:279
    engine = AlgorithmEngineEx(False)
    engine.add_parameter("Name", "Test")

    assert engine.try_evaluate("Name", "") == "Test"
    assert engine.try_evaluate("name", "default") == "default"


# endregion

# region GetParameterEx 测试


def test_get_parameter_ex_missing():
    # Java: AlgorithmEngineExTest.java:295
    engine = AlgorithmEngineEx()
    assert engine.try_evaluate("missing_param", "default") == "default"


# endregion

# region 综合测试


def test_complex_expression():
    # Java: AlgorithmEngineExTest.java:306
    engine = AlgorithmEngineEx()
    engine.add_parameter("a", 10)
    engine.add_parameter("b", 20)
    engine.add_parameter("c", 30)

    assert engine.try_evaluate("a + b + c", 0) == 60
    assert engine.try_evaluate("a * b - c", 0) == 170
    assert engine.try_evaluate("if(a > 5, b, c)", 0) == 20


def test_nested_json():
    # Java: AlgorithmEngineExTest.java:323
    engine = AlgorithmEngineEx()
    engine.add_parameter_from_json('{"data":{"name":"test","value":123}}')

    assert engine.try_evaluate("json(data)['name']", "") == "test"


def test_array_operations():
    # Java: AlgorithmEngineExTest.java:332
    engine = AlgorithmEngineEx()
    engine.add_parameter(
        "arr",
        Operand.create([Operand.create(1), Operand.create(2), Operand.create(3),
                        Operand.create(4), Operand.create(5)]),
    )

    assert engine.try_evaluate("sum(arr)", 0) == 15
    assert engine.try_evaluate("average(arr)", 0.0) == pytest.approx(3.0, abs=0.01)
    assert engine.try_evaluate("max(arr)", 0) == 5
    assert engine.try_evaluate("min(arr)", 0) == 1
    assert engine.try_evaluate("count(arr)", 0) == 5


# endregion
