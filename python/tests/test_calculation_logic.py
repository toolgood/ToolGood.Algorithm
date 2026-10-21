"""Tests for ``CalculationLogicEngine``.

Ported from ``src/test/java/toolgood/algorithm/calculationlogic/CalculationLogicTest.java``.

Java 的 ``IllegalArgumentException`` 在 Python 移植中对应 ``ValueError``。
"""
from __future__ import annotations

import datetime as _dt
from decimal import Decimal

import pytest

from toolgood_algorithm import (
    CalculationLogicEngine,
    FunctionCache,
    OperandType,
)


def _create_engine(use_calculation_logic_info: bool = True) -> CalculationLogicEngine:
    return CalculationLogicEngine(FunctionCache(), use_calculation_logic_info)


def _utc_2026_08_31() -> _dt.datetime:
    """``new DateTime(2026, 8, 31, 12, 0, 0, DateTimeZone.UTC)``."""
    return _dt.datetime(2026, 8, 31, 12, 0, 0, tzinfo=_dt.timezone.utc)


# region SetSceneName 测试


def test_set_scene_name():
    # Java: CalculationLogicTest.java:26
    logic = _create_engine()
    logic.set_scene_name("场景1")
    assert logic.to_info_string() == "===== 场景1 =====\n"


def test_set_scene_name_no_info():
    # Java: CalculationLogicTest.java:33
    logic = _create_engine(False)
    logic.set_scene_name("场景1")
    assert logic.to_info_string() == ""


# endregion

# region InitValue 测试


def test_init_value_int():
    # Java: CalculationLogicTest.java:44
    logic = _create_engine()
    logic.init_value("a", 1)
    logic.init_value("b", 2)
    assert logic.to_info_string() == "[初始] a=1;b=2;\n"


def test_init_value_string():
    # Java: CalculationLogicTest.java:52
    logic = _create_engine()
    logic.init_value("s", "hello")
    assert logic.to_info_string() == "[初始] s=hello;\n"


def test_init_value_decimal():
    # Java: CalculationLogicTest.java:59
    logic = _create_engine()
    logic.init_value("d", Decimal("1.5"))
    assert logic.to_info_string() == "[初始] d=1.5;\n"


def test_init_value_double():
    # Java: CalculationLogicTest.java:66
    logic = _create_engine()
    logic.init_value("dd", 1.5)
    assert logic.to_info_string() == "[初始] dd=1.5;\n"


def test_init_value_bool():
    # Java: CalculationLogicTest.java:73
    logic = _create_engine()
    logic.init_value("flag", True)
    assert logic.to_info_string() == "[初始] flag=True;\n"


def test_init_value_layer_remark():
    # Java: CalculationLogicTest.java:80
    # InitValue 的层级与备注不参与信息输出
    logic = _create_engine()
    logic.init_value("i", 100, 2, "备注")
    assert logic.to_info_string() == "[初始] i=100;\n"


def test_init_value_can_be_used_in_condition():
    # Java: CalculationLogicTest.java:88
    logic = _create_engine()
    logic.init_value("a", 1)
    logic.init_value("b", 2)
    assert logic.check_condition("a<b") is True
    assert logic.check_condition("a>b") is False


def test_init_value_date_time():
    # Java: CalculationLogicTest.java:97
    logic = _create_engine()
    logic.init_value("dt", _utc_2026_08_31())
    assert logic.to_info_string() == '[初始] dt="2026-08-31 12:00:00";\n'


def test_init_value_date_time_layer_remark():
    # Java: CalculationLogicTest.java:104
    # InitValue 的层级与备注不参与信息输出
    logic = _create_engine()
    logic.init_value("dt", _utc_2026_08_31(), 2, "备注")
    assert logic.to_info_string() == '[初始] dt="2026-08-31 12:00:00";\n'


def test_init_value_date_time_can_be_used_in_condition():
    # Java: CalculationLogicTest.java:112
    logic = _create_engine()
    logic.init_value("dt", _utc_2026_08_31())
    assert logic.check_condition("YEAR(dt)=2026") is True
    assert logic.check_condition("MONTH(dt)=8") is True
    assert logic.check_condition("dt>DATEVALUE('2026-08-30')") is True


# endregion

# region CheckCondition 测试


def test_check_condition_true():
    # Java: CalculationLogicTest.java:125
    logic = _create_engine()
    logic.init_value("a", 1)
    logic.init_value("b", 2)
    assert logic.check_condition("a<b") is True
    assert logic.to_info_string() == "[初始] a=1;b=2;\n[成功] if a<b: // 1<2\n"


def test_check_condition_false():
    # Java: CalculationLogicTest.java:134
    logic = _create_engine()
    logic.init_value("a", 1)
    logic.init_value("b", 2)
    assert logic.check_condition("a>b") is False
    assert logic.to_info_string() == "[初始] a=1;b=2;\n[失败] if a>b: // 1>2\n"


def test_check_condition_layer_remark():
    # Java: CalculationLogicTest.java:143
    logic = _create_engine()
    logic.init_value("a", 1)
    logic.init_value("b", 2)
    assert logic.check_condition("a<b", 1, "备注") is True
    assert logic.to_info_string() == "[初始] a=1;b=2;\n[成功]    if a<b: // 1<2 // 备注\n"


def test_check_condition_string_compare():
    # Java: CalculationLogicTest.java:152
    logic = _create_engine()
    logic.init_value("s", "abc")
    assert logic.check_condition("s='abc'") is True
    assert logic.check_condition("s='ab'") is False
    assert logic.to_info_string() == (
        '[初始] s=abc;\n'
        '[成功] if s=\'abc\': // "abc"=\'abc\'\n'
        '[失败] if s=\'ab\': // "abc"=\'ab\'\n'
    )


def test_check_condition_non_boolean_throws():
    # Java: CalculationLogicTest.java:161
    logic = _create_engine()
    logic.init_value("a", 1)
    logic.init_value("b", 2)
    with pytest.raises(ValueError) as ex:
        logic.check_condition("a+b")
    assert str(ex.value) == "The condition must be a boolean value!"
    assert logic.to_info_string() == (
        "[初始] a=1;b=2;\n"
        "[条件][错误] if a+b: // 1+2 = The condition must be a boolean value!\n"
    )


def test_check_condition_error_throws():
    # Java: CalculationLogicTest.java:171
    logic = _create_engine()
    with pytest.raises(ValueError) as ex:
        logic.check_condition("1/0=0")
    assert str(ex.value) == "Function '/' Div 0 error!"
    assert logic.to_info_string() == (
        "[条件][错误] if 1/0=0: // 1/0=0 = Function '/' Div 0 error!\n"
    )


# endregion

# region SetFormula 测试


def test_set_formula():
    # Java: CalculationLogicTest.java:183
    logic = _create_engine()
    logic.init_value("a", 1)
    logic.init_value("b", 2)
    logic.set_formula("x", "a*3+b", 1, "备注")
    assert logic.to_info_string() == (
        "[初始] a=1;b=2;\n[赋值]    x = a*3+b = 1*3+2 = 5 // 备注\n"
    )
    # 公式结果应已写入参数
    assert logic.check_condition("x=5") is True


def test_set_formula_string_concat():
    # Java: CalculationLogicTest.java:194
    logic = _create_engine()
    logic.init_value("s", "hello")
    logic.set_formula("y", "s&'!'")
    assert logic.to_info_string() == (
        '[初始] s=hello;\n[赋值] y = s&\'!\' = "hello"&\'!\' = hello!\n'
    )
    assert logic.check_condition("y='hello!'") is True


def test_set_formula_text_and_number():
    # Java: CalculationLogicTest.java:203
    logic = _create_engine()
    logic.init_value("a", 1)
    logic.set_formula("z", "'text'&a")
    assert logic.to_info_string() == (
        "[初始] a=1;\n[赋值] z = 'text'&a = 'text'&1 = text1\n"
    )


def test_set_formula_error_throws():
    # Java: CalculationLogicTest.java:211
    logic = _create_engine()
    with pytest.raises(ValueError) as ex:
        logic.set_formula("x", "1/0")
    assert str(ex.value) == "Function '/' Div 0 error!"
    assert logic.to_info_string() == (
        "[赋值][错误] x = 1/0 = 1/0 // Function '/' Div 0 error!\n"
    )


# endregion

# region SetValue 测试


def test_set_value_string():
    # Java: CalculationLogicTest.java:223
    logic = _create_engine()
    logic.set_value("s", "文本")
    assert logic.to_info_string() == '[赋值] s = "文本"\n'
    assert logic.check_condition("s='文本'") is True


def test_set_value_string_escape():
    # Java: CalculationLogicTest.java:231
    logic = _create_engine()
    logic.set_value("sq", 'a"b')
    assert logic.to_info_string() == '[赋值] sq = "a\\"b"\n'


def test_set_value_decimal():
    # Java: CalculationLogicTest.java:238
    logic = _create_engine()
    logic.set_value("d", Decimal("1.5"))
    assert logic.to_info_string() == "[赋值] d = 1.5\n"


def test_set_value_double():
    # Java: CalculationLogicTest.java:245
    logic = _create_engine()
    logic.set_value("dd", 1.5)
    assert logic.to_info_string() == "[赋值] dd = 1.5\n"


def test_set_value_bool():
    # Java: CalculationLogicTest.java:252
    logic = _create_engine()
    logic.set_value("flag", True)
    assert logic.to_info_string() == "[赋值] flag = True\n"


def test_set_value_layer_remark():
    # Java: CalculationLogicTest.java:259
    logic = _create_engine()
    logic.set_value("i", 100, 1, "备注")
    assert logic.to_info_string() == "[赋值]    i = 100 // 备注\n"


def test_set_value_date_time():
    # Java: CalculationLogicTest.java:266
    logic = _create_engine()
    logic.set_value("dt", _utc_2026_08_31())
    assert logic.to_info_string() == '[赋值] dt = "2026-08-31 12:00:00"\n'


def test_set_value_date_time_layer_remark():
    # Java: CalculationLogicTest.java:273
    logic = _create_engine()
    logic.set_value("dt", _utc_2026_08_31(), 1, "备注")
    assert logic.to_info_string() == '[赋值]    dt = "2026-08-31 12:00:00" // 备注\n'


def test_set_value_date_time_can_be_used_in_condition():
    # Java: CalculationLogicTest.java:280
    logic = _create_engine()
    logic.set_value("dt", _utc_2026_08_31())
    assert logic.check_condition("YEAR(dt)=2026") is True
    assert logic.check_condition("MONTH(dt)=8") is True
    assert logic.check_condition("dt>DATEVALUE('2026-08-30')") is True


# endregion

# region GetValue 测试


def test_get_value_init_value_int():
    # Java: CalculationLogicTest.java:293
    logic = _create_engine()
    logic.init_value("a", 1)
    operand = logic.get_value("a")
    assert operand.is_number() is True
    assert operand.type() == OperandType.NUMBER
    assert operand.number_value().compare(Decimal(1)) == 0


def test_get_value_init_value_string():
    # Java: CalculationLogicTest.java:303
    logic = _create_engine()
    logic.init_value("s", "hello")
    operand = logic.get_value("s")
    assert operand.is_text() is True
    assert operand.type() == OperandType.TEXT
    assert operand.text_value() == "hello"


def test_get_value_init_value_decimal():
    # Java: CalculationLogicTest.java:313
    logic = _create_engine()
    logic.init_value("d", Decimal("1.5"))
    operand = logic.get_value("d")
    assert operand.is_number() is True
    assert operand.number_value().compare(Decimal("1.5")) == 0


def test_get_value_init_value_double():
    # Java: CalculationLogicTest.java:322
    logic = _create_engine()
    logic.init_value("dd", 1.5)
    operand = logic.get_value("dd")
    assert operand.is_number() is True
    assert operand.number_value().compare(Decimal("1.5")) == 0


def test_get_value_init_value_bool():
    # Java: CalculationLogicTest.java:331
    logic = _create_engine()
    logic.init_value("flag", True)
    operand = logic.get_value("flag")
    assert operand.is_boolean() is True
    assert operand.type() == OperandType.BOOLEAN
    assert operand.boolean_value() is True


def test_get_value_init_value_date_time():
    # Java: CalculationLogicTest.java:341
    logic = _create_engine()
    logic.init_value("dt", _utc_2026_08_31())
    operand = logic.get_value("dt")
    assert operand.is_date() is True
    assert operand.type() == OperandType.DATE
    assert str(operand.date_value()) == "2026-08-31 12:00:00"


def test_get_value_set_value_date_time():
    # Java: CalculationLogicTest.java:351
    logic = _create_engine()
    logic.set_value("dt", _utc_2026_08_31())
    operand = logic.get_value("dt")
    assert operand.is_date() is True
    assert operand.type() == OperandType.DATE
    assert str(operand.date_value()) == "2026-08-31 12:00:00"


def test_get_value_set_value():
    # Java: CalculationLogicTest.java:361
    logic = _create_engine()
    logic.set_value("v", "text")
    operand = logic.get_value("v")
    assert operand.is_text() is True
    assert operand.text_value() == "text"


def test_get_value_set_formula():
    # Java: CalculationLogicTest.java:370
    logic = _create_engine()
    logic.init_value("a", 1)
    logic.init_value("b", 2)
    logic.set_formula("x", "a*3+b")
    operand = logic.get_value("x")
    assert operand.is_number() is True
    assert operand.number_value().compare(Decimal(5)) == 0


def test_get_value_after_set_formula_overwrite():
    # Java: CalculationLogicTest.java:381
    logic = _create_engine()
    logic.init_value("a", 1)
    logic.set_formula("a", "a+10")
    operand = logic.get_value("a")
    assert operand.is_number() is True
    assert operand.number_value().compare(Decimal(11)) == 0


def test_get_value_not_found_returns_error():
    # Java: CalculationLogicTest.java:391
    logic = _create_engine()
    operand = logic.get_value("missing")
    assert operand.is_error() is True
    assert operand.error_msg() == "Parameter [missing] is missing."


def test_get_value_no_info():
    # Java: CalculationLogicTest.java:399
    # 关闭信息记录不影响取值
    logic = _create_engine(False)
    logic.init_value("a", 1)
    assert logic.get_value("a").number_value().compare(Decimal(1)) == 0


# endregion

# region BlankLine 测试


def test_blank_line():
    # Java: CalculationLogicTest.java:411
    logic = _create_engine()
    logic.blank_line()
    assert logic.to_info_string() == "\n"

    logic.blank_line()
    logic.blank_line()
    assert logic.to_info_string() == "\n\n\n"


# endregion

# region ToInfoString 测试


def test_to_info_string_empty():
    # Java: CalculationLogicTest.java:426
    logic = _create_engine()
    assert logic.to_info_string() == ""


def test_to_info_string_disabled():
    # Java: CalculationLogicTest.java:432
    # 关闭信息记录后，所有操作都不产生信息
    logic = _create_engine(False)
    logic.set_scene_name("场景1")
    logic.init_value("a", 1)
    logic.check_condition("a>0")
    logic.set_value("b", 2)
    logic.set_formula("c", "a+1")
    logic.blank_line()
    assert logic.to_info_string() == ""


def test_to_info_string_full_flow():
    # Java: CalculationLogicTest.java:445
    logic = _create_engine()
    logic.set_scene_name("场景1")
    logic.init_value("a", 1)
    logic.init_value("b", 2)
    if logic.check_condition("a>b"):
        logic.set_value("c", 3, 1)
        logic.set_formula("c", "a*5+b", 1)
    elif logic.check_condition("a<b"):
        logic.set_value("c", 4, 1)
        logic.set_formula("c", "a*3+b", 1)
    logic.blank_line()
    logic.set_value("e", 5)

    expected = (
        "[初始] a=1;b=2;\n"
        "===== 场景1 =====\n"
        "[失败] if a>b: // 1>2\n"
        "[成功] if a<b: // 1<2\n"
        "[赋值]    c = 4\n"
        "[赋值]    c = a*3+b = 1*3+2 = 5\n"
        "\n"
        "[赋值] e = 5\n"
    )
    assert logic.to_info_string() == expected


# endregion
