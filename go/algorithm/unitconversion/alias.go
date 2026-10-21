package unitconversion

import (
	"strings"

	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/decimal"
)

// 便于移植代码阅读的类型别名（与 C# 的 decimal 对应）。

// Decimal 十进制数值类型。
type Decimal = decimal.Decimal

// dec 解析静态换算系数；仅用于包初始化，写错直接 panic。
func dec(s string) Decimal { return decimal.MustDecimal(s) }

// ratio 计算 numerator / denominator，对应 C# 中的 "1250m / 381" 写法。
func ratio(numerator, denominator int64) Decimal {
	return decimal.DecFromInt(numerator).Div(decimal.DecFromInt(denominator))
}

// tryConvert 单位换算：right = left / units[leftSynonym] * units[rightSynonym]。
//
// 单位名大小写不敏感（对应 C# 的 StringComparer.OrdinalIgnoreCase）；
// 任一单位不存在时返回 (0, false)。
func tryConvert(units map[string]Decimal, leftSynonym, rightSynonym string, left Decimal) (Decimal, bool) {
	l, ok := units[strings.ToLower(leftSynonym)]
	if !ok {
		return decimal.DecimalZero, false
	}
	r, ok := units[strings.ToLower(rightSynonym)]
	if !ok {
		return decimal.DecimalZero, false
	}
	return left.Div(l).Mul(r), true
}
