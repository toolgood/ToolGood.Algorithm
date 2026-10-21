package unitconversion

import "github.com/toolgood/ToolGood.Algorithm/go/algorithm/decimal"

// 面积换算系数，值为「1 m² 等于多少该单位」（与 C# AreaConverter 一致）。
var (
	areaFt2 = decimal.DecimalOne.Div(dec("0.3048")).Div(dec("0.3048"))
	areaYd2 = decimal.DecimalOne.Div(dec("0.9144")).Div(dec("0.9144"))
	areaIn2 = decimal.DecimalOne.Div(dec("0.00064516"))
	areaMi2 = decimal.DecimalOne.Div(dec("2589988.110336"))
)

// areaUnits 面积单位换算表，键均为小写。
var areaUnits = map[string]Decimal{
	"m²": dec("1"), "m2": dec("1"), "square metre": dec("1"), "square meter": dec("1"), "centiare": dec("1"), "平方米": dec("1"), "平方公尺": dec("1"),
	"km²": dec("0.000001"), "km2": dec("0.000001"), "square kilometre": dec("0.000001"), "square kilometer": dec("0.000001"), "平方千米": dec("0.000001"),
	"dm²": dec("100"), "dm2": dec("100"), "square decimetre": dec("100"), "square decimeter": dec("100"), "平方分米": dec("100"),
	"cm²": dec("10000"), "cm2": dec("10000"), "square centimetre": dec("10000"), "square centimeter": dec("10000"), "平方厘米": dec("10000"),
	"mm²": dec("1000000"), "mm2": dec("1000000"), "square millimetre": dec("1000000"), "square millimeter": dec("1000000"), "平方毫米": dec("1000000"),
	"ft²": areaFt2, "ft2": areaFt2, "square foot": areaFt2, "square feet": areaFt2, "sq ft": areaFt2, "平方英尺": areaFt2,
	"yd²": areaYd2, "yd2": areaYd2, "sq yd": areaYd2, "square yard": areaYd2, "平方码": areaYd2,
	"a": dec("0.01"), "are": dec("0.01"),
	"ha": dec("0.0001"), "hectare": dec("0.0001"), "公顷": dec("0.0001"),
	"in²": areaIn2, "in2": areaIn2, "sq in": areaIn2, "square inch": areaIn2, "平方英寸": areaIn2,
	"mi²": areaMi2, "mi2": areaMi2, "sq mi": areaMi2, "square mile": areaMi2, "平方英里": areaMi2,
	"亩": ratio(3, 2000),
}

// TryConvertArea 面积单位换算。right = left / areaUnits[leftSynonym] * areaUnits[rightSynonym]。
//
// 对应 C# AreaConverter.TryConvert；单位名大小写不敏感，任一单位不存在时返回 (0, false)。
func TryConvertArea(leftSynonym, rightSynonym string, left Decimal) (Decimal, bool) {
	return tryConvert(areaUnits, leftSynonym, rightSynonym, left)
}
