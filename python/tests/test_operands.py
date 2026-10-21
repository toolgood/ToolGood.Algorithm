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
    from toolgood_algorithm.operands.operand import KeyValue, OperandKeyValueList

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
