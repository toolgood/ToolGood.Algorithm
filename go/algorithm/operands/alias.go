package operands

import (
	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/decimal"
	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/enums"
	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/litjson"
)

// 便于移植代码阅读的类型别名（与 C# / Java 版本的短名保持一致）。

// Decimal 十进制数值类型。
type Decimal = decimal.Decimal

// OperandType 操作数类型。
type OperandType = enums.OperandType

// JsonData JSON 节点类型。
type JsonData = litjson.JsonData

// JsonType JSON 节点类型枚举。
type JsonType = litjson.JsonType

const (
	// OperandTypeNone 未指定。
	OperandTypeNone = enums.OperandTypeNone
	// OperandTypeNull 空值。
	OperandTypeNull = enums.OperandTypeNull
	// OperandTypeError 错误。
	OperandTypeError = enums.OperandTypeError
	// OperandTypeDate 日期。
	OperandTypeDate = enums.OperandTypeDate
	// OperandTypeArray 数组。
	OperandTypeArray = enums.OperandTypeArray
	// OperandTypeNumber 数字。
	OperandTypeNumber = enums.OperandTypeNumber
	// OperandTypeBoolean 布尔。
	OperandTypeBoolean = enums.OperandTypeBoolean
	// OperandTypeText 文本。
	OperandTypeText = enums.OperandTypeText
	// OperandTypeJson JSON。
	OperandTypeJson = enums.OperandTypeJson
	// OperandTypeArrayJson 键值集合。
	OperandTypeArrayJson = enums.OperandTypeArrayJson
)

var (
	// DecimalZero 常量 0。
	DecimalZero = decimal.DecimalZero
	// DecimalOne 常量 1。
	DecimalOne = decimal.DecimalOne
	// DecimalMax decimal 最大值。
	DecimalMax = decimal.DecimalMax
	// ExcelEpochDays Excel 日期偏移。
	ExcelEpochDays = decimal.ExcelEpochDays
	// DecFromInt 创建整数 Decimal。
	DecFromInt = decimal.DecFromInt
	// DecFromUint 创建无符号整数 Decimal。
	DecFromUint = decimal.DecFromUint
	// DecFromFloat 创建浮点 Decimal。
	DecFromFloat = decimal.DecFromFloat
	// DecFromString 解析 Decimal。
	DecFromString = decimal.DecFromString
	// DecFromUnscaled 由非标度整数创建 Decimal。
	DecFromUnscaled = decimal.DecFromUnscaled
	// TryParseDecimal 尝试解析 Decimal。
	TryParseDecimal = decimal.TryParseDecimal
	// MustDecimal 解析 Decimal（失败返回 0）。
	MustDecimal = decimal.MustDecimal
	// IsNaNOrInf 是否 NaN / Inf。
	IsNaNOrInf = decimal.IsNaNOrInf
	// Plain 等价 BigDecimal.toPlainString。
	Plain = decimal.Plain
	// PlainStripValue 去掉末尾 0 的文本。
	PlainStripValue = decimal.PlainStripValue
	// JsonMapperToObject 解析 JSON 文本。
	JsonMapperToObject = litjson.JsonMapperToObject
)
