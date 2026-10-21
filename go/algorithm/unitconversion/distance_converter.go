package unitconversion

import "github.com/toolgood/ToolGood.Algorithm/go/algorithm/decimal"

// 长度换算系数，值为「1 m 等于多少该单位」（与 C# DistanceConverter 一致）。
var (
	distanceFt   = ratio(1250, 381)
	distanceYd   = ratio(1250, 1143)
	distanceMile = ratio(125, 201168)
	distanceIn   = ratio(5000, 127)
	distanceAU   = decimal.DecimalOne.Div(dec("149600000000"))
)

// distanceUnits 长度单位换算表，键均为小写。
var distanceUnits = map[string]Decimal{
	"m": dec("1"), "metre": dec("1"), "meter": dec("1"), "米": dec("1"),
	"km": dec("0.001"), "kilometre": dec("0.001"), "kilometer": dec("0.001"), "千米": dec("0.001"),
	"dm": dec("10"), "decimetre": dec("10"), "decimeter": dec("10"), "分米": dec("10"),
	"cm": dec("100"), "centimetre": dec("100"), "centimeter": dec("100"), "厘米": dec("100"),
	"mm": dec("1000"), "millimetre": dec("1000"), "millimeter": dec("1000"), "毫米": dec("1000"),
	"ft": distanceFt, "foot": distanceFt, "feet": distanceFt, "英尺": distanceFt,
	"yd": distanceYd, "yard": distanceYd, "码": distanceYd,
	"mile": distanceMile, "英里": distanceMile,
	"in": distanceIn, "inch": distanceIn, "英寸": distanceIn,
	"au": distanceAU,
	"尺":  dec("3"), "寸": dec("30"),
}

// TryConvertDistance 长度单位换算。
//
// 对应 C# DistanceConverter.TryConvert；单位名大小写不敏感，任一单位不存在时返回 (0, false)。
func TryConvertDistance(leftSynonym, rightSynonym string, left Decimal) (Decimal, bool) {
	return tryConvert(distanceUnits, leftSynonym, rightSynonym, left)
}
