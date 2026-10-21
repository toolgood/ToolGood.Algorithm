package unitconversion

import "github.com/toolgood/ToolGood.Algorithm/go/algorithm/decimal"

// 体积换算系数，值为「1 L 等于多少该单位」（与 C# VolumeConverter 一致）。
var (
	volumeKm3            = dec("0.001").Mul(dec("0.001")).Mul(dec("0.001")).Mul(dec("0.001"))
	volumeFt3            = decimal.DecimalOne.Div(dec("28.316846592"))
	volumeIn3            = ratio(125000000, 2048383)
	volumeImperialPint   = ratio(800000, 454609)
	volumeImperialGallon = ratio(100000, 454609)
	volumeImperialQuart  = ratio(400000, 454609)
	volumeUSPint         = ratio(1000000000, 473176473)
	volumeUSGallon       = ratio(125000000, 473176473)
	volumeUSQuart        = ratio(1000000000, 946352946)
)

// volumeUnits 体积单位换算表，键均为小写。
var volumeUnits = map[string]Decimal{
	"l": dec("1"), "lt": dec("1"), "ltr": dec("1"), "liter": dec("1"), "litre": dec("1"),
	"dm³": dec("1"), "dm3": dec("1"), "cubic decimetre": dec("1"), "cubic decimeter": dec("1"), "升": dec("1"), "立方分米": dec("1"),
	"m³": dec("0.001"), "m3": dec("0.001"), "cubic metre": dec("0.001"), "cubic meter": dec("0.001"), "立方米": dec("0.001"),
	"km³": volumeKm3, "km3": volumeKm3, "cubic kilometre": volumeKm3, "cubic kilometer": volumeKm3, "立方千米": volumeKm3,
	"cm³": dec("1000"), "cm3": dec("1000"), "cubic centimetre": dec("1000"), "cubic centimeter": dec("1000"), "立方厘米": dec("1000"), "毫升": dec("1000"), "ml": dec("1000"),
	"mm³": dec("1000000"), "mm3": dec("1000000"), "cubic millimetre": dec("1000000"), "cubic millimeter": dec("1000000"), "立方毫米": dec("1000000"),
	"ft³": volumeFt3, "ft3": volumeFt3, "cubic foot": volumeFt3, "cubic feet": volumeFt3, "立方英尺": volumeFt3, "cu ft": volumeFt3,
	"in³": volumeIn3, "in3": volumeIn3, "cubic in": volumeIn3, "cubic inch": volumeIn3, "立方英寸": volumeIn3,

	"imperial pint": volumeImperialPint, "imperial pt": volumeImperialPint, "imperial p": volumeImperialPint,
	"imperial gallon": volumeImperialGallon, "imperial gal": volumeImperialGallon,
	"imperial quart": volumeImperialQuart, "imperial qt": volumeImperialQuart,
	"us pint": volumeUSPint, "us pt": volumeUSPint, "us p": volumeUSPint,
	"us gallon": volumeUSGallon, "us gal": volumeUSGallon,
	"us quart": volumeUSQuart, "us qt": volumeUSQuart,
}

// TryConvertVolume 体积单位换算。
//
// 对应 C# VolumeConverter.TryConvert；单位名大小写不敏感，任一单位不存在时返回 (0, false)。
func TryConvertVolume(leftSynonym, rightSynonym string, left Decimal) (Decimal, bool) {
	return tryConvert(volumeUnits, leftSynonym, rightSynonym, left)
}
