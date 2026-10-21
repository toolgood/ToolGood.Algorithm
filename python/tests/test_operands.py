"""Tests for the ``Operand`` value types."""
from __future__ import annotations

from decimal import Decimal

import pytest

from toolgood_algorithm import JsonMapper, MyDate, Operand, OperandType


def test_create_and_type():
    assert Operand.create(True) is Operand.TRUE
    assert Operand.create(None) is Operand.NULL
    assert Operand.create(5).is_number()
    assert Operand.create("5").is_text()
    assert Operand.create(Decimal("1.5")).is_number()
    assert Operand.create([Operand.create(1)]).is_array()
    assert Operand.create(MyDate(2024, 1, 2)).is_date()
    assert Operand.create(5).type() == OperandType.NUMBER


def test_number_text_conversion():
    assert Operand.create("12").to_number().number_value() == Decimal(12)
    assert Operand.create(" 1.25 ").to_number().number_value() == Decimal("1.25")
    assert Operand.create("abc").to_number().is_error()
    assert Operand.create(Decimal("1.50")).to_text().text_value() == "1.5"
    assert Operand.create(True).to_text().text_value() == "TRUE"
    assert Operand.create(0).to_boolean().boolean_value() is False
    assert Operand.create(1).to_boolean().boolean_value() is True
    assert Operand.create(2).to_boolean().is_error()


def test_text_boolean_rules():
    for text in ("true", "TRUE", "1", "是", "有"):
        assert Operand.create(text).to_boolean().boolean_value() is True
    for text in ("false", "FALSE", "0", "否", "不是", "无", "没有"):
        assert Operand.create(text).to_boolean().boolean_value() is False
    assert Operand.create("maybe").to_boolean().is_error()


def test_camel_case_api_is_polymorphic():
    value = Operand.create(5)
    assert value.IsNumber() is True
    assert value.IsText() is False
    assert value.IntValue() == 5
    assert value.ToText().TextValue() == "5"
    assert Operand.Create("x").TextValue() == "x"


def test_error_formatting():
    error = Operand.error("bad {0}", 3)
    assert error.is_error()
    assert error.error_msg() == "bad 3"
    assert error.to_number().is_error()


def test_json_array_conversion():
    engine = Operand.create(JsonMapper.to_object("[1,'a',true]"))
    array = engine.to_array()
    assert array.is_array()
    assert [str(v) for v in array.array_value()] == ["1", '"a"', "true"]

    obj = Operand.create(JsonMapper.to_object('{"a":1}'))
    assert obj.is_json()
    assert obj.to_text().text_value() == '{"a":1}'


def test_lenient_json_parser():
    data = JsonMapper.to_object("{'a': 1, \"b\": [1,2,], 'c': 'x',}")
    assert data.is_object()
    assert data.get_child("a").number_value() == Decimal(1)
    assert data.get_child("b").count() == 2
    assert data.get_child("c").string_value() == "x"


def test_key_value_list():
    from toolgood_algorithm.operands import KeyValue, OperandKeyValueList

    values = OperandKeyValueList()
    values.add_value(KeyValue("a", Operand.create(1)))
    values.add_value(KeyValue("b", Operand.create("x")))
    assert values.is_array_json()
    assert values.get_value("a").number_value() == Decimal(1)
    assert values.contains_key(Operand.create("b"))
    assert values.contains_value(Operand.create("x"))
    assert str(values) == '{"a":1,"b":"x"}'


def test_my_date_operand():
    date = Operand.create(MyDate(2024, 1, 2, 3, 4, 5))
    assert date.is_date()
    assert date.to_text().text_value() == "2024-01-02 03:04:05"
    assert str(date) == '"2024-01-02 03:04:05"'
    assert date.date_value().to_long() == 20240102030405


def test_numeric_precision():
    assert Operand.create(0.1).number_value() == Decimal("0.1")
    assert (Operand.create(Decimal("0.1")).number_value() + Decimal("0.2")) == Decimal("0.3")
    big = Operand.create(10 ** 20)
    assert big.is_number()
    assert big.number_value() == Decimal(10 ** 20)


# ---------------------------------------------------------------------------
# Ported from java/.../operands/OperandsTest.java
# ---------------------------------------------------------------------------

def test_operand_static_properties():
    assert Operand.VERSION.text_value() == "ToolGood.Algorithm 6.3"
    assert Operand.TRUE.boolean_value() is True
    assert Operand.FALSE.boolean_value() is False
    assert Operand.ONE.int_value() == 1
    assert Operand.ZERO.int_value() == 0
    assert Operand.NULL.is_null()


def test_operand_create_bool():
    op = Operand.create(True)
    assert op.is_boolean()
    assert op.boolean_value() is True

    op = Operand.create(False)
    assert op.is_boolean()
    assert op.boolean_value() is False


def test_operand_create_int():
    op = Operand.create(100)
    assert op.is_number()
    assert op.int_value() == 100
    assert op.number_value() == Decimal("100")

    op = Operand.create(-100)
    assert op.is_number()
    assert op.int_value() == -100

    op = Operand.create(0)
    assert op.is_number()
    assert op.int_value() == 0

    op = Operand.create(1000)
    assert op.is_number()
    assert op.int_value() == 1000

    op = Operand.create(-1000)
    assert op.is_number()
    assert op.int_value() == -1000


def test_operand_create_long():
    op = Operand.create(100000)
    assert op.is_number()
    assert op.long_value() == 100000


def test_operand_create_double():
    op = Operand.create(3.14159)
    assert op.is_number()
    assert op.double_value() == pytest.approx(3.14159, abs=0.00001)


def test_operand_create_decimal():
    op = Operand.create(Decimal("3.14159"))
    assert op.is_number()
    assert op.number_value() == Decimal("3.14159")


def test_operand_create_string():
    op = Operand.create("hello")
    assert op.is_text()
    assert op.text_value() == "hello"

    op = Operand.create("")
    assert op.is_text()
    assert op.text_value() == ""

    op = Operand.create(None)
    assert op.is_null()


def test_operand_create_date_time():
    dt = MyDate(2024, 1, 15, 10, 30, 45)
    op = Operand.create(dt)
    assert op.is_date()
    assert op.date_value().Year == 2024
    assert op.date_value().Month == 1
    assert op.date_value().Day == 15
    assert op.date_value().Hour == 10
    assert op.date_value().Minute == 30
    assert op.date_value().Second == 45


def test_operand_create_time_span():
    ts = MyDate(None, None, 1, 2, 3, 4)
    op = Operand.create(ts)
    assert op.is_date()
    assert op.date_value().Day == 1
    assert op.date_value().Hour == 2
    assert op.date_value().Minute == 3
    assert op.date_value().Second == 4


def test_operand_create_list():
    op = Operand.create([Operand.create(1), Operand.create(2), Operand.create(3)])
    assert op.is_array()
    assert len(op.array_value()) == 3
    assert op.array_value()[0].int_value() == 1
    assert op.array_value()[1].int_value() == 2
    assert op.array_value()[2].int_value() == 3


def test_operand_create_collection_string():
    op = Operand.create([Operand.create("a"), Operand.create("b"), Operand.create("c")])
    assert op.is_array()
    assert len(op.array_value()) == 3
    assert op.array_value()[0].text_value() == "a"
    assert op.array_value()[1].text_value() == "b"
    assert op.array_value()[2].text_value() == "c"


def test_operand_create_collection_int():
    op = Operand.create([Operand.create(1), Operand.create(2), Operand.create(3)])
    assert op.is_array()
    assert len(op.array_value()) == 3
    assert op.array_value()[0].int_value() == 1
    assert op.array_value()[1].int_value() == 2
    assert op.array_value()[2].int_value() == 3


def test_operand_create_collection_double():
    op = Operand.create([Operand.create(1.1), Operand.create(2.2), Operand.create(3.3)])
    assert op.is_array()
    assert len(op.array_value()) == 3
    assert op.array_value()[0].double_value() == pytest.approx(1.1, abs=0.001)
    assert op.array_value()[1].double_value() == pytest.approx(2.2, abs=0.001)
    assert op.array_value()[2].double_value() == pytest.approx(3.3, abs=0.001)


def test_operand_create_collection_bool():
    op = Operand.create([Operand.create(True), Operand.create(False), Operand.create(True)])
    assert op.is_array()
    assert len(op.array_value()) == 3
    assert op.array_value()[0].boolean_value() is True
    assert op.array_value()[1].boolean_value() is False
    assert op.array_value()[2].boolean_value() is True


def test_operand_create_json():
    op = Operand.create_json('{"name":"test","value":123}')
    assert op.is_json()

    op = Operand.create_json("[1,2,3]")
    assert op.is_json()

    op = Operand.create_json("invalid json")
    assert op.is_error()


def test_operand_error():
    op = Operand.error("test error")
    assert op.is_error()
    assert op.error_msg() == "test error"

    op = Operand.error("error {0}", "param")
    assert op.is_error()
    assert op.error_msg() == "error param"


def test_operand_create_null():
    op = Operand.NULL
    assert op.is_null()


def test_operand_int_properties():
    op = Operand.create(42)
    assert op.type() == OperandType.NUMBER
    assert op.is_number()
    assert not op.is_null()
    assert not op.is_text()
    assert not op.is_boolean()
    assert not op.is_array()
    assert not op.is_date()
    assert not op.is_json()
    assert not op.is_error()


def test_operand_int_to_number():
    op = Operand.create(42)
    result = op.to_number(None)
    assert result.is_number()
    assert result.int_value() == 42


def test_operand_int_to_boolean():
    op = Operand.create(0)
    result = op.to_boolean(None)
    assert result.is_boolean()
    assert result.boolean_value() is False

    op = Operand.create(1)
    result = op.to_boolean(None)
    assert result.is_boolean()
    assert result.boolean_value() is True

    op = Operand.create(2)
    result = op.to_boolean(None)
    assert result.is_error()


def test_operand_int_to_text():
    op = Operand.create(42)
    result = op.to_text(None)
    assert result.is_text()
    assert result.text_value() == "42"


def test_operand_int_to_string():
    op = Operand.create(42)
    assert str(op) == "42"


def test_operand_double_properties():
    op = Operand.create(3.14)
    assert op.type() == OperandType.NUMBER
    assert op.is_number()
    assert op.double_value() == pytest.approx(3.14, abs=0.001)
    assert op.number_value() == Decimal("3.14")
    assert op.int_value() == 3
    assert op.long_value() == 3


def test_operand_double_to_boolean():
    op = Operand.create(0.0)
    result = op.to_boolean(None)
    assert result.is_boolean()
    assert result.boolean_value() is False

    op = Operand.create(1.0)
    result = op.to_boolean(None)
    assert result.is_boolean()
    assert result.boolean_value() is True

    op = Operand.create(2.5)
    result = op.to_boolean(None)
    assert result.is_error()


def test_operand_double_to_text():
    op = Operand.create(3.14)
    result = op.to_text(None)
    assert result.is_text()
    assert result.text_value() == "3.14"


def test_operand_decimal_properties():
    op = Operand.create(Decimal("123.456"))
    assert op.type() == OperandType.NUMBER
    assert op.is_number()
    assert op.number_value() == Decimal("123.456")
    assert op.double_value() == pytest.approx(123.456, abs=0.001)
    assert op.int_value() == 123


def test_operand_decimal_to_boolean():
    op = Operand.create(Decimal("0"))
    result = op.to_boolean(None)
    assert result.is_boolean()
    assert result.boolean_value() is False

    op = Operand.create(Decimal("1"))
    result = op.to_boolean(None)
    assert result.is_boolean()
    assert result.boolean_value() is True


def test_operand_decimal_to_text():
    op = Operand.create(Decimal("123.45"))
    result = op.to_text(None)
    assert result.is_text()
    assert result.text_value() == "123.45"


def test_operand_boolean_properties():
    op = Operand.create(True)
    assert op.type() == OperandType.BOOLEAN
    assert op.is_boolean()
    assert not op.is_number()
    assert op.boolean_value() is True

    op = Operand.create(False)
    assert op.type() == OperandType.BOOLEAN
    assert op.is_boolean()
    assert op.boolean_value() is False


def test_operand_boolean_to_number():
    op = Operand.create(True)
    result = op.to_number(None)
    assert result.is_number()
    assert result.int_value() == 1

    op = Operand.create(False)
    result = op.to_number(None)
    assert result.is_number()
    assert result.int_value() == 0


def test_operand_boolean_to_text():
    op = Operand.create(True)
    result = op.to_text(None)
    assert result.is_text()
    assert result.text_value() == "TRUE"

    op = Operand.create(False)
    result = op.to_text(None)
    assert result.is_text()
    assert result.text_value() == "FALSE"


def test_operand_boolean_to_string():
    op = Operand.create(True)
    assert str(op) == "true"

    op = Operand.create(False)
    assert str(op) == "false"


def test_operand_string_properties():
    op = Operand.create("hello world")
    assert op.type() == OperandType.TEXT
    assert op.is_text()
    assert not op.is_number()
    assert not op.is_boolean()
    assert op.text_value() == "hello world"


def test_operand_string_to_number():
    op = Operand.create("42")
    result = op.to_number(None)
    assert result.is_number()
    assert result.int_value() == 42

    op = Operand.create("3.14")
    result = op.to_number(None)
    assert result.is_number()
    assert result.double_value() == pytest.approx(3.14, abs=0.001)

    op = Operand.create("not a number")
    result = op.to_number(None)
    assert result.is_error()


def test_operand_string_to_boolean():
    op = Operand.create("true")
    result = op.to_boolean(None)
    assert result.is_boolean()
    assert result.boolean_value() is True

    op = Operand.create("false")
    result = op.to_boolean(None)
    assert result.is_boolean()
    assert result.boolean_value() is False

    op = Operand.create("TRUE")
    result = op.to_boolean(None)
    assert result.is_boolean()
    assert result.boolean_value() is True

    op = Operand.create("not bool")
    result = op.to_boolean(None)
    assert result.is_error()


def test_operand_string_to_my_date():
    op = Operand.create("2024-01-15")
    result = op.to_my_date(None)
    assert result.is_date()
    assert result.date_value().Year == 2024
    assert result.date_value().Month == 1
    assert result.date_value().Day == 15

    op = Operand.create("10:30:45")
    result = op.to_my_date(None)
    assert result.is_date()
    assert result.date_value().Hour == 10
    assert result.date_value().Minute == 30
    assert result.date_value().Second == 45

    op = Operand.create("not a date")
    result = op.to_my_date(None)
    assert result.is_error()


def test_operand_string_to_json():
    op = Operand.create('{"name":"test"}')
    result = op.to_json(None)
    assert result.is_json()

    op = Operand.create("[1,2,3]")
    result = op.to_json(None)
    assert result.is_json()

    op = Operand.create("not json")
    result = op.to_json(None)
    assert result.is_error()


def test_operand_string_to_array():
    op = Operand.create("test")
    result = op.to_array(None)
    assert result.is_error()


def test_operand_string_to_string():
    op = Operand.create("hello")
    assert str(op) == '"hello"'

    op = Operand.create('hello"world')
    assert str(op) == '"hello\\"world"'

    op = Operand.create("hello\nworld")
    assert str(op) == '"hello\\nworld"'

    op = Operand.create("hello\tworld")
    assert str(op) == '"hello\\tworld"'


def test_operand_null_properties():
    op = Operand.NULL
    assert op.type() == OperandType.NULL
    assert op.is_null()
    assert not op.is_number()
    assert not op.is_text()
    assert not op.is_boolean()
    assert not op.is_error()


def test_operand_null_to_string():
    op = Operand.NULL
    assert str(op) == "null"


def test_operand_error_properties():
    op = Operand.error("test error message")
    assert op.type() == OperandType.ERROR
    assert op.is_error()
    assert not op.is_null()
    assert not op.is_number()
    assert not op.is_text()
    assert op.error_msg() == "test error message"


def test_operand_error_conversions():
    op = Operand.error("error")

    assert op.to_number(None).is_error()
    assert op.to_boolean(None).is_error()
    assert op.to_text(None).is_error()
    assert op.to_array(None).is_error()
    assert op.to_my_date(None).is_error()
    assert op.to_json(None).is_error()


def test_operand_array_properties():
    op = Operand.create([Operand.create(1), Operand.create(2), Operand.create(3)])
    assert op.type() == OperandType.ARRAY
    assert op.is_array()
    assert not op.is_number()
    assert not op.is_text()
    assert len(op.array_value()) == 3


def test_operand_array_to_text():
    op = Operand.create([Operand.create(1), Operand.create(2), Operand.create(3)])
    result = op.to_text(None)
    assert result.is_text()
    assert result.text_value() == "[1,2,3]"


def test_operand_array_to_array():
    op = Operand.create([Operand.create(1), Operand.create(2)])
    result = op.to_array("")
    assert result.is_array()
    assert len(result.array_value()) == 2


def test_operand_array_to_json():
    op = Operand.create([Operand.create(1), Operand.create(2)])
    result = op.to_json(None)
    assert result.is_json()


def test_operand_array_to_string():
    op = Operand.create([Operand.create(1), Operand.create("test"), Operand.create(True)])
    assert str(op) == '[1,"test",true]'


def test_operand_my_date_properties():
    dt = MyDate(2024, 6, 15, 10, 30, 45)
    op = Operand.create(dt)
    assert op.type() == OperandType.DATE
    assert op.is_date()
    assert not op.is_number()
    assert not op.is_text()
    assert op.date_value().Year == 2024
    assert op.date_value().Month == 6
    assert op.date_value().Day == 15
    assert op.date_value().Hour == 10
    assert op.date_value().Minute == 30
    assert op.date_value().Second == 45


def test_operand_my_date_to_text():
    dt = MyDate(2024, 6, 15, 10, 30, 45)
    op = Operand.create(dt)
    result = op.to_text(None)
    assert result.is_text()


def test_operand_my_date_to_my_date():
    dt = MyDate(2024, 6, 15, 10, 30, 45)
    op = Operand.create(dt)
    result = op.to_my_date(None)
    assert result.is_date()
    assert result.date_value().Year == 2024


def test_operand_my_date_to_string():
    dt = MyDate(2024, 6, 15, 10, 30, 45)
    op = Operand.create(dt)
    assert str(op).startswith('"2024-06-15')
