"""Tests ported from ``java/.../AlgorithmEngine/ValueTest.java``.

One Java ``@Test`` maps to one pytest function (snake_case).
"""
from __future__ import annotations

import math

import pytest

from toolgood_algorithm import (
    AlgorithmEngine,
    AlgorithmEngineEx,
    AreaUnitType,
    DistanceUnitType,
    MassUnitType,
    Operand,
    VolumeUnitType,
)


class Cylinder(AlgorithmEngineEx):
    """Python port of ``java/.../AlgorithmEngine/Cylinder.java``."""

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
        return super().get_parameter(parameter)

    def execute_diy_function(self, func_name: str, operands) -> Operand:
        if func_name == "求面积" and len(operands) == 1:
            value = operands[0].to_number("It can't be converted to number!").double_value()
            return Operand.create(value * value * math.pi)
        return super().execute_diy_function(func_name, operands)


def test_constant():
    engine = AlgorithmEngine()
    e = engine.try_evaluate("e", 0.0)
    assert e == pytest.approx(math.e, abs=10)
    e = engine.try_evaluate("pi", 0.0)
    assert e == pytest.approx(math.pi, abs=10)

    b = engine.try_evaluate("true", False)
    assert b is True
    b = engine.try_evaluate("false", True)
    assert b is False


def test_boolean():
    engine = AlgorithmEngine()
    b1 = engine.try_evaluate("if(true,1,2)", 0)
    assert b1 == 1

    b1 = engine.try_evaluate("if(false,1,2)", 0)
    assert b1 == 2


def test_array():
    engine = AlgorithmEngine()
    r = engine.try_evaluate("count(Array(1,2,3,4))", 0)
    assert r == 4

    r = engine.try_evaluate("(1=1)*9+2", 0)
    assert r == 11
    r = engine.try_evaluate("(1=2)*9+2", 0)
    assert r == 2


def test_version():
    engine = AlgorithmEngine()
    t25 = engine.try_evaluate("Engineversion", "")
    assert t25 == "ToolGood.Algorithm 6.3"
    t26 = engine.try_evaluate("Algorithmversion", "")
    assert t26 == "ToolGood.Algorithm 6.3"


def test_json_value():
    engine = AlgorithmEngine()
    t = engine.parse("{'灰色':'L','canBookCount':905,'saleCount':91,'specId':'43b0e72e98731aed69e1f0cc7d64bf4d'}")
    c = str(engine.evaluate(t))
    assert c == (
        '{"灰色":"L","canBookCount":905,"saleCount":91,'
        '"specId":"43b0e72e98731aed69e1f0cc7d64bf4d"}'
    )


def test_param():
    engine = Cylinder(10, 15)
    num = engine.try_evaluate("PARAM('半径')", 0)
    assert num == 10
    num = engine.try_evaluate("PARAMETER('半径')", 0)
    assert num == 10
    num = engine.try_evaluate("GETPARAMETER('半径')", 0)
    assert num == 10

    num = engine.try_evaluate("半径", 0)
    assert num == 10


def test_error():
    engine = AlgorithmEngine()
    num = engine.try_evaluate("Error('出错了')", "")
    assert num == ""
    assert engine.LastError == "出错了"


def test_json_access():
    engine = AlgorithmEngine()
    str_value = engine.try_evaluate("{name:'toolgood', age:'12',}['name']", "")
    assert str_value == "toolgood"

    str_value = engine.try_evaluate("{name:'toolgood', age:'12',other:{work:'IT'}}['other']['work']", "")
    assert str_value == "IT"

    str_value = engine.try_evaluate(
        "json(\"{'name':'toolgood', 'age':'12','other':{'work':'IT'}}\")['name']", ""
    )
    assert str_value == "toolgood"

    str_value = engine.try_evaluate(
        "json(\"{'name':'toolgood', 'age':'12','other':{'work':'IT'}}\")['other']['work']", ""
    )
    assert str_value == "IT"

    b = engine.try_evaluate("{name:'toolgood', age:'12',other:{work:'IT'}}.has('age')", False)
    assert b is True
    b = engine.try_evaluate("{name:'toolgood', age:'12',other:{work:'IT'}}.hasKey('age')", False)
    assert b is True

    b = engine.try_evaluate("{e:'toolgood', pi:'12',other:{work:'IT'}}.hasKey('e')", False)
    assert b is True

    b = engine.try_evaluate(
        "json(\"{'name':'toolgood', 'age':'12','other':{'work':'IT'}}\").has('age')", False
    )
    assert b is True

    b = engine.try_evaluate("{name:'toolgood', age:'12',other:{work:'IT'}}.has('work')", True)
    assert b is False

    b = engine.try_evaluate("{name:'toolgood', age:'12',other:{work:'IT'}}.hasValue('toolgood')", False)
    assert b is True

    b = engine.try_evaluate(
        "json(\"{'name':'toolgood', 'age':'12','other':{'work':'IT'}}\").hasValue('toolgood')", False
    )
    assert b is True


def test_json_backslash_escape():
    engine = AlgorithmEngine()
    # 序列化字符串应转义反斜杠：引擎字面量 {"a":"C:\\temp"} → 值 C:\temp，输出需转义
    dt = engine.try_evaluate(r'{"a":"C:\\temp"}', "")
    assert dt == r'{"a":"C:\\temp"}'

    # 反斜杠与引号同时转义
    dt = engine.try_evaluate(r'{"a":"a\"b\\c"}', "")
    assert dt == r'{"a":"a\"b\\c"}'


def test_array2():
    engine = AlgorithmEngine()
    engine.set_use_excel_index(True)
    num = engine.try_evaluate("[1,2,3,4,][2]", 0)
    assert num == 2

    str_value = engine.try_evaluate("[1,2,3,4,'555'][5]", "")
    assert str_value == "555"

    b = engine.try_evaluate("[1,2,3,4,].has('1')", False)
    assert b is True
    b = engine.try_evaluate("['abc','age'].hasKey('age')", False)
    assert b is True
    b = engine.try_evaluate("['abc','age'].hasValue('age')", False)
    assert b is True

    b = engine.try_evaluate("json(\"['abc','age']\").has('age')", False)
    assert b is True

    b = engine.try_evaluate("json(\"['abc','age']\").hasValue('age')", False)
    assert b is True


def test_distance_m():
    engine = AlgorithmEngine()
    b = engine.try_evaluate("1=1m", False)
    assert b is True

    b = engine.try_evaluate("1=10dm", False)
    assert b is True

    b = engine.try_evaluate("1=100cm", False)
    assert b is True

    b = engine.try_evaluate("1=1000mm", False)
    assert b is True

    b = engine.try_evaluate("1=0.001km", False)
    assert b is True


def test_distance_dm():
    engine = AlgorithmEngine()
    engine.DistanceUnit = DistanceUnitType.DM
    b = engine.try_evaluate("1=0.1m", False)
    assert b is True

    b = engine.try_evaluate("1=1dm", False)
    assert b is True

    b = engine.try_evaluate("1=10cm", False)
    assert b is True

    b = engine.try_evaluate("1=100mm", False)
    assert b is True

    b = engine.try_evaluate("1=0.0001km", False)
    assert b is True


def test_distance_cm():
    engine = AlgorithmEngine()
    engine.DistanceUnit = DistanceUnitType.CM
    b = engine.try_evaluate("1=0.01m", False)
    assert b is True

    b = engine.try_evaluate("1=0.1dm", False)
    assert b is True

    b = engine.try_evaluate("1=1cm", False)
    assert b is True

    b = engine.try_evaluate("1=10mm", False)
    assert b is True

    b = engine.try_evaluate("1=0.00001km", False)
    assert b is True


def test_distance_mm():
    engine = AlgorithmEngine()
    engine.DistanceUnit = DistanceUnitType.MM
    b = engine.try_evaluate("1=0.001m", False)
    assert b is True

    b = engine.try_evaluate("1=0.01dm", False)
    assert b is True

    b = engine.try_evaluate("1=0.1cm", False)
    assert b is True

    b = engine.try_evaluate("1=1mm", False)
    assert b is True

    b = engine.try_evaluate("1=0.000001km", False)
    assert b is True


def test_distance_km():
    engine = AlgorithmEngine()
    engine.DistanceUnit = DistanceUnitType.KM

    b = engine.try_evaluate("1=1m*1000", False)
    assert b is True

    b = engine.try_evaluate("1=10dm*1000", False)
    assert b is True

    b = engine.try_evaluate("1=100cm*1000", False)
    assert b is True

    b = engine.try_evaluate("1=1000mm*1000", False)
    assert b is True

    b = engine.try_evaluate("1=0.001km*1000", False)
    assert b is True


def test_area_m2():
    engine = AlgorithmEngine()
    b = engine.try_evaluate("1=1m*1m", False)
    assert b is True

    b = engine.try_evaluate("1m2=1m*1m", False)
    assert b is True

    b = engine.try_evaluate("1m2=10dm*10dm", False)
    assert b is True

    b = engine.try_evaluate("1m2=100cm*100cm", False)
    assert b is True

    b = engine.try_evaluate("1m2=1000mm*1000mm", False)
    assert b is True

    b = engine.try_evaluate("1m2=0.001km*0.001km", False)
    assert b is True

    b = engine.try_evaluate("1=1m2", False)
    assert b is True

    b = engine.try_evaluate("1=100dm2", False)
    assert b is True

    b = engine.try_evaluate("1=10000cm2", False)
    assert b is True

    b = engine.try_evaluate("1=1000000mm2", False)
    assert b is True

    b = engine.try_evaluate("1=0.000001km2", False)
    assert b is True

    b = engine.try_evaluate("1km2=1km*1km", False)
    assert b is True


def test_area_dm2():
    engine = AlgorithmEngine()
    engine.AreaUnit = AreaUnitType.DM2

    b = engine.try_evaluate("1=1m2/100", False)
    assert b is True

    b = engine.try_evaluate("1=100dm2/100", False)
    assert b is True

    b = engine.try_evaluate("1=10000cm2/100", False)
    assert b is True

    b = engine.try_evaluate("1=1000000mm2/100", False)
    assert b is True

    b = engine.try_evaluate("1=0.000001km2/100", False)
    assert b is True


def test_area_cm2():
    engine = AlgorithmEngine()
    engine.AreaUnit = AreaUnitType.CM2

    b = engine.try_evaluate("1=1m2/100/100", False)
    assert b is True

    b = engine.try_evaluate("1=100dm2/100/100", False)
    assert b is True

    b = engine.try_evaluate("1=10000cm2/100/100", False)
    assert b is True

    b = engine.try_evaluate("1=1000000mm2/100/100", False)
    assert b is True

    b = engine.try_evaluate("1=0.000001km2/100/100", False)
    assert b is True


def test_area_mm2():
    engine = AlgorithmEngine()
    engine.AreaUnit = AreaUnitType.MM2

    b = engine.try_evaluate("1=1m2/100/100/100", False)
    assert b is True

    b = engine.try_evaluate("1=100dm2/100/100/100", False)
    assert b is True

    b = engine.try_evaluate("1=10000cm2/100/100/100", False)
    assert b is True

    b = engine.try_evaluate("1=1000000mm2/100/100/100", False)
    assert b is True

    b = engine.try_evaluate("1=0.000001km2/100/100/100", False)
    assert b is True


def test_area_km2():
    engine = AlgorithmEngine()
    engine.AreaUnit = AreaUnitType.KM2

    b = engine.try_evaluate("1=1m2*1000*1000", False)
    assert b is True

    b = engine.try_evaluate("1=100dm2*1000*1000", False)
    assert b is True

    b = engine.try_evaluate("1=10000cm2*1000*1000", False)
    assert b is True

    b = engine.try_evaluate("1=1000000mm2*1000*1000", False)
    assert b is True

    b = engine.try_evaluate("1=0.000001km2*1000*1000", False)
    assert b is True


def test_volume_m3():
    engine = AlgorithmEngine()
    b = engine.try_evaluate("1=1m*1m*1m", False)
    assert b is True

    b = engine.try_evaluate("1m3=1m*1m*1m", False)
    assert b is True

    b = engine.try_evaluate("1m3=1m2*1m", False)
    assert b is True

    b = engine.try_evaluate("1m3=1000L", False)
    assert b is True

    b = engine.try_evaluate("1L=1000ml", False)
    assert b is True

    b = engine.try_evaluate("1m3=1000ml*1000", False)
    assert b is True

    b = engine.try_evaluate("1m3=10dm*10dm*10dm", False)
    assert b is True

    b = engine.try_evaluate("1m3=100cm*100cm*100cm", False)
    assert b is True

    b = engine.try_evaluate("1m3=1000mm*1000mm*1000mm", False)
    assert b is True

    b = engine.try_evaluate("1m3=0.001km*0.001km*0.001km", False)
    assert b is True

    b = engine.try_evaluate("1=1m3", False)
    assert b is True

    b = engine.try_evaluate("1=1000dm3", False)
    assert b is True

    b = engine.try_evaluate("1=1000000cm3", False)
    assert b is True

    b = engine.try_evaluate("1=1000000000mm3", False)
    assert b is True

    b = engine.try_evaluate("1=1km3/1000/1000/1000", False)
    assert b is True


def test_volume_dm3():
    engine = AlgorithmEngine()
    engine.VolumeUnit = VolumeUnitType.DM3
    b = engine.try_evaluate("1=1m3/1000", False)
    assert b is True

    b = engine.try_evaluate("1=1000dm3/1000", False)
    assert b is True

    b = engine.try_evaluate("1=1000000cm3/1000", False)
    assert b is True

    b = engine.try_evaluate("1=1000000000mm3/1000", False)
    assert b is True

    b = engine.try_evaluate("1=1km3/1000/1000/1000/1000", False)
    assert b is True


def test_volume_cm3():
    engine = AlgorithmEngine()
    engine.VolumeUnit = VolumeUnitType.CM3
    b = engine.try_evaluate("1=1m3/1000/1000", False)
    assert b is True

    b = engine.try_evaluate("1=1000dm3/1000/1000", False)
    assert b is True

    b = engine.try_evaluate("1=1000000cm3/1000/1000", False)
    assert b is True

    b = engine.try_evaluate("1=1000000000mm3/1000/1000", False)
    assert b is True

    b = engine.try_evaluate("1=1km3/1000/1000/1000/1000/1000", False)
    assert b is True


def test_volume_mm3():
    engine = AlgorithmEngine()
    engine.VolumeUnit = VolumeUnitType.MM3
    b = engine.try_evaluate("1=1m3/1000/1000/1000", False)
    assert b is True

    b = engine.try_evaluate("1=1000dm3/1000/1000/1000", False)
    assert b is True

    b = engine.try_evaluate("1=1000000cm3/1000/1000/1000", False)
    assert b is True

    b = engine.try_evaluate("1=1000000000mm3/1000/1000/1000", False)
    assert b is True

    b = engine.try_evaluate("1=1km3/1000/1000/1000/1000/1000/1000", False)
    assert b is True


def test_volume_km3():
    engine = AlgorithmEngine()
    engine.VolumeUnit = VolumeUnitType.KM3
    b = engine.try_evaluate("1=1m3*1000*1000*1000", False)
    assert b is True
    b = engine.try_evaluate("1=1000dm3*1000*1000*1000", False)
    assert b is True
    b = engine.try_evaluate("1=1000000cm3*1000*1000*1000", False)
    assert b is True
    b = engine.try_evaluate("1=1000000000mm3*1000*1000*1000", False)
    assert b is True
    b = engine.try_evaluate("1=1km3", False)
    assert b is True


def test_mass_kg():
    engine = AlgorithmEngine()
    b = engine.try_evaluate("1=1kg", False)
    assert b is True

    b = engine.try_evaluate("1=1000g", False)
    assert b is True

    b = engine.try_evaluate("1=0.001t", False)
    assert b is True


def test_mass_g():
    engine = AlgorithmEngine()
    engine.MassUnit = MassUnitType.G

    b = engine.try_evaluate("1=1kg/1000", False)
    assert b is True

    b = engine.try_evaluate("1=1000g/1000", False)
    assert b is True

    b = engine.try_evaluate("1=0.001t/1000", False)
    assert b is True


def test_mass_t():
    engine = AlgorithmEngine()
    engine.MassUnit = MassUnitType.T

    b = engine.try_evaluate("1=1kg*1000", False)
    assert b is True

    b = engine.try_evaluate("1=1000g*1000", False)
    assert b is True

    b = engine.try_evaluate("1=0.001t*1000", False)
    assert b is True


def test_unit_error():
    engine = AlgorithmEngine()
    b = engine.try_evaluate("1m=1kg", False)
    assert b is True
    b = engine.try_evaluate("1m=1m2", False)
    assert b is True
    b = engine.try_evaluate("1m=1m3", False)
    assert b is True
