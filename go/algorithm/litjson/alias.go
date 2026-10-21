package litjson

import "github.com/toolgood/ToolGood.Algorithm/go/algorithm/decimal"

// Decimal 是 decimal.Decimal 的类型别名（LitJson 数字节点使用）。
type Decimal = decimal.Decimal

var (
	// DecFromInt 创建整数 Decimal。
	DecFromInt = decimal.DecFromInt
	// DecFromFloat 创建浮点 Decimal。
	DecFromFloat = decimal.DecFromFloat
	// DecFromString 解析 Decimal。
	DecFromString = decimal.DecFromString
	// TryParseDecimal 尝试解析 Decimal。
	TryParseDecimal = decimal.TryParseDecimal
)
