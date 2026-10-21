"""Unit conversion tests.

Ported from the Java JUnit suites:

* ``unitConversion/UnitConversionTest.java``
* ``unitConversion/UnitConversionPrecisionTest.java``
"""
from __future__ import annotations

from decimal import Decimal

import pytest

from toolgood_algorithm import AlgorithmEngineHelper

convert = AlgorithmEngineHelper.unit_conversion


def _assert_close(src: str, tar: str, value: Decimal, expected: float) -> None:
    """Relative error below 1e-9 (``UnitConversionPrecisionTest.assertClose``)."""
    result = convert(value, src, tar, "精度测试")
    d = float(result)
    rel = abs(d - expected) / max(abs(expected), 1e-30)
    assert rel < 1e-9, f"{value} {src} -> {tar} = {d}, expected {expected}, rel={rel}"


# ---------------------------------------------------------------------------
# 距离单位转换测试
# ---------------------------------------------------------------------------
def test_distance_converter_meter_to_kilometer():
    result = convert(Decimal("1000"), "m", "km", "测试")
    assert result == Decimal("1")


def test_distance_converter_kilometer_to_meter():
    result = convert(Decimal("1"), "km", "m", "测试")
    assert result == Decimal("1000")


def test_distance_converter_meter_to_centimeter():
    result = convert(Decimal("1"), "m", "cm", "测试")
    assert result == Decimal("100")


def test_distance_converter_meter_to_millimeter():
    result = convert(Decimal("1"), "m", "mm", "测试")
    assert result == Decimal("1000")


def test_distance_converter_meter_to_decimeter():
    result = convert(Decimal("1"), "m", "dm", "测试")
    assert result == Decimal("10")


def test_distance_converter_centimeter_to_meter():
    result = convert(Decimal("100"), "cm", "m", "测试")
    assert result == Decimal("1")


def test_distance_converter_millimeter_to_meter():
    result = convert(Decimal("1000"), "mm", "m", "测试")
    assert result == Decimal("1")


def test_distance_converter_chinese_unit():
    result = convert(Decimal("1"), "米", "千米", "测试")
    assert result == Decimal("0.001")

    result = convert(Decimal("1"), "米", "厘米", "测试")
    assert result == Decimal("100")

    result = convert(Decimal("1"), "米", "毫米", "测试")
    assert result == Decimal("1000")


def test_distance_converter_chi_cun():
    result = convert(Decimal("1"), "m", "尺", "测试")
    assert result == Decimal("3")

    result = convert(Decimal("1"), "m", "寸", "测试")
    assert result == Decimal("30")

    result = convert(Decimal("6"), "尺", "m", "测试")
    assert result == Decimal("2")

    result = convert(Decimal("3"), "尺", "cm", "测试")
    assert result == Decimal("100")


def test_distance_converter_foot_to_meter():
    result = convert(Decimal("1"), "ft", "m", "测试")
    assert float(result) == pytest.approx(0.3048, abs=0.0001)


def test_distance_converter_inch_to_meter():
    result = convert(Decimal("1"), "in", "m", "测试")
    assert float(result) == pytest.approx(0.0254, abs=0.0001)


def test_distance_converter_yard_to_meter():
    result = convert(Decimal("1"), "yd", "m", "测试")
    assert float(result) == pytest.approx(0.9144, abs=0.0001)


def test_distance_converter_mile_to_meter():
    result = convert(Decimal("1"), "mile", "m", "测试")
    assert float(result) == pytest.approx(1609.344, abs=0.1)


# ---------------------------------------------------------------------------
# 面积单位转换测试
# ---------------------------------------------------------------------------
def test_area_converter_square_meter_to_square_kilometer():
    result = convert(Decimal("1000000"), "m2", "km2", "测试")
    assert result == Decimal("1")


def test_area_converter_square_meter_to_square_centimeter():
    result = convert(Decimal("1"), "m2", "cm2", "测试")
    assert result == Decimal("10000")


def test_area_converter_square_meter_to_square_millimeter():
    result = convert(Decimal("1"), "m2", "mm2", "测试")
    assert result == Decimal("1000000")


def test_area_converter_square_meter_to_square_decimeter():
    result = convert(Decimal("1"), "m2", "dm2", "测试")
    assert result == Decimal("100")


def test_area_converter_chinese_unit():
    result = convert(Decimal("1"), "平方米", "平方厘米", "测试")
    assert result == Decimal("10000")

    result = convert(Decimal("1"), "平方米", "平方毫米", "测试")
    assert result == Decimal("1000000")


def test_area_converter_hectare_to_square_meter():
    result = convert(Decimal("1"), "ha", "m2", "测试")
    assert result == Decimal("10000")


def test_area_converter_mu_to_square_meter():
    result = convert(Decimal("1"), "亩", "m2", "测试")
    assert float(result) == pytest.approx(666.667, abs=0.1)


def test_area_converter_square_foot_to_square_meter():
    result = convert(Decimal("1"), "ft2", "m2", "测试")
    assert float(result) == pytest.approx(0.092903, abs=0.0001)


# ---------------------------------------------------------------------------
# 体积单位转换测试
# ---------------------------------------------------------------------------
def test_volume_converter_cubic_meter_to_liter():
    result = convert(Decimal("1"), "m3", "L", "测试")
    assert result == Decimal("1000")


def test_volume_converter_liter_to_milliliter():
    result = convert(Decimal("1"), "L", "cm3", "测试")
    assert result == Decimal("1000")


def test_volume_converter_cubic_meter_to_cubic_centimeter():
    result = convert(Decimal("1"), "m3", "cm3", "测试")
    assert result == Decimal("1000000")


def test_volume_converter_cubic_meter_to_cubic_millimeter():
    result = convert(Decimal("1"), "m3", "mm3", "测试")
    assert result == Decimal("1000000000")


def test_volume_converter_chinese_unit():
    result = convert(Decimal("1"), "立方米", "升", "测试")
    assert result == Decimal("1000")

    result = convert(Decimal("1"), "升", "毫升", "测试")
    assert result == Decimal("1000")


def test_volume_converter_cubic_foot_to_liter():
    result = convert(Decimal("1"), "ft3", "L", "测试")
    assert float(result) == pytest.approx(28.3168, abs=0.1)


def test_volume_converter_us_gallon_to_liter():
    result = convert(Decimal("1"), "US gallon", "L", "测试")
    assert float(result) == pytest.approx(3.78541, abs=0.1)


# ---------------------------------------------------------------------------
# 质量单位转换测试
# ---------------------------------------------------------------------------
def test_mass_converter_kilogram_to_gram():
    result = convert(Decimal("1"), "kg", "g", "测试")
    assert result == Decimal("1000")


def test_mass_converter_kilogram_to_ton():
    result = convert(Decimal("1000"), "kg", "t", "测试")
    assert result == Decimal("1")


def test_mass_converter_ton_to_kilogram():
    result = convert(Decimal("1"), "t", "kg", "测试")
    assert result == Decimal("1000")


def test_mass_converter_gram_to_kilogram():
    result = convert(Decimal("1000"), "g", "kg", "测试")
    assert result == Decimal("1")


def test_mass_converter_chinese_unit():
    result = convert(Decimal("1"), "千克", "克", "测试")
    assert result == Decimal("1000")

    result = convert(Decimal("1"), "吨", "千克", "测试")
    assert result == Decimal("1000")


def test_mass_converter_milligram():
    result = convert(Decimal("1"), "kg", "mg", "测试")
    assert result == Decimal("1000000")

    result = convert(Decimal("1"), "kg", "毫克", "测试")
    assert result == Decimal("1000000")

    result = convert(Decimal("500"), "mg", "kg", "测试")
    assert result == Decimal("0.0005")


def test_mass_converter_jin_liang():
    result = convert(Decimal("1"), "kg", "斤", "测试")
    assert result == Decimal("2")

    result = convert(Decimal("1"), "kg", "两", "测试")
    assert result == Decimal("20")

    result = convert(Decimal("5"), "斤", "kg", "测试")
    assert result == Decimal("2.5")

    result = convert(Decimal("2"), "两", "g", "测试")
    assert result == Decimal("100")


def test_mass_converter_pound_to_kilogram():
    result = convert(Decimal("1"), "lb", "kg", "测试")
    assert float(result) == pytest.approx(0.453592, abs=0.0001)


def test_mass_converter_ounce_to_kilogram():
    result = convert(Decimal("1"), "oz", "kg", "测试")
    assert float(result) == pytest.approx(0.0283495, abs=0.0001)


# ---------------------------------------------------------------------------
# 无效单位测试
# ---------------------------------------------------------------------------
def test_unit_conversion_invalid_unit():
    with pytest.raises(Exception):
        convert(Decimal("1"), "invalid", "m", "测试")

    with pytest.raises(Exception):
        convert(Decimal("1"), "m", "invalid", "测试")


# ---------------------------------------------------------------------------
# Ported from ``UnitConversionPrecisionTest.java``
# 精确分数系数防回归测试：反向换算误差应远小于 1e-9。
# ---------------------------------------------------------------------------
def test_mass_short_ton_exact():
    _assert_close("short ton", "kg", Decimal(1), 907.18474)
    _assert_close("kg", "short ton", Decimal(1), 1.0 / 907.18474)
    _assert_close("short ton", "lb", Decimal(1), 2000)


def test_mass_long_ton_exact():
    _assert_close("long ton", "kg", Decimal(1), 1016.0469088)
    _assert_close("kg", "long ton", Decimal(1), 1.0 / 1016.0469088)
    _assert_close("long ton", "lb", Decimal(1), 2240)


def test_area_mu_exact():
    _assert_close("亩", "m²", Decimal(1), 666.6666666666667)
    _assert_close("m²", "亩", Decimal(1), 0.0015)


def test_volume_cubic_foot_exact():
    _assert_close("ft³", "l", Decimal(1), 28.316846592)
    _assert_close("l", "ft³", Decimal(1), 0.03531466672148859)
    _assert_close("ft³", "in³", Decimal(1), 1728)


def test_volume_cubic_inch_exact():
    _assert_close("in³", "l", Decimal(1), 0.016387064)
    _assert_close("l", "in³", Decimal(1), 61.0237440947323)


def test_volume_imperial_pint_exact():
    _assert_close("imperial pint", "l", Decimal(1), 0.56826125)
    _assert_close("l", "imperial pint", Decimal(1), 1.7597539863927023)


def test_volume_imperial_gallon_exact():
    _assert_close("imperial gallon", "l", Decimal(1), 4.54609)
    _assert_close("l", "imperial gallon", Decimal(1), 0.2199692482990878)
    _assert_close("imperial gallon", "imperial pint", Decimal(1), 8)
    _assert_close("imperial gallon", "imperial quart", Decimal(1), 4)


def test_volume_imperial_quart_exact():
    _assert_close("imperial quart", "l", Decimal(1), 1.1365225)
    _assert_close("l", "imperial quart", Decimal(1), 0.87987699319635115)


def test_volume_us_pint_exact():
    _assert_close("US pint", "l", Decimal(1), 0.473176473)
    _assert_close("l", "US pint", Decimal(1), 2.1133764188651873)


def test_volume_us_gallon_exact():
    _assert_close("US gallon", "l", Decimal(1), 3.785411784)
    _assert_close("l", "US gallon", Decimal(1), 0.2641720523581484)
    _assert_close("US gallon", "US quart", Decimal(1), 4)
