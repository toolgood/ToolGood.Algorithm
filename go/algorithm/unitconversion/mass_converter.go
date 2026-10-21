package unitconversion

import "github.com/toolgood/ToolGood.Algorithm/go/algorithm/decimal"

// 质量换算系数，值为「1 kg 等于多少该单位」（与 C# MassConverter 一致）。
var (
	massTon      = decimal.DecimalOne.Div(dec("1000"))
	massLb       = ratio(100000000, 45359237)
	massStone    = ratio(50000000, 317514659)
	massOunce    = ratio(1600000000, 45359237)
	massShortTon = ratio(50000, 45359237)
	massLongTon  = ratio(312500, 317514659)
)

// massUnits 质量单位换算表，键均为小写。
var massUnits = map[string]Decimal{
	"kg": dec("1"), "kilogram": dec("1"), "千克": dec("1"),
	"gram": dec("1000"), "g": dec("1000"), "克": dec("1000"),
	"ton": massTon, "t": massTon, "吨": massTon,
	"lb": massLb, "lbs": massLb, "pound": massLb, "pounds": massLb, "磅": massLb, "英磅": massLb,
	"st": massStone, "stone": massStone, "石": massStone,
	"oz": massOunce, "ounce": massOunce, "盎司": massOunce,
	"quintal": dec("0.01"), "英担": dec("0.01"),
	"short ton": massShortTon, "net ton": massShortTon, "us ton": massShortTon, "短吨": massShortTon, "美吨": massShortTon,
	"long ton": massLongTon, "weight ton": massLongTon, "gross ton": massLongTon, "imperial ton": massLongTon, "长吨": massLongTon, "英吨": massLongTon,
	"mg": dec("1000000"), "毫克": dec("1000000"),
	"斤": dec("2"), "两": dec("20"),
}

// TryConvertMass 质量单位换算。
//
// 对应 C# MassConverter.TryConvert；单位名大小写不敏感，任一单位不存在时返回 (0, false)。
func TryConvertMass(leftSynonym, rightSynonym string, left Decimal) (Decimal, bool) {
	return tryConvert(massUnits, leftSynonym, rightSynonym, left)
}
