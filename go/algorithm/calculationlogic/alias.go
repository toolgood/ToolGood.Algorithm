package calculationlogic

import (
	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/decimal"
	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/enums"
	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals"
	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/operands"
)

// 便于移植代码阅读的类型别名（与 C# / Java 版本的短名保持一致）。

type (
	// Decimal 十进制数值类型。
	Decimal = decimal.Decimal
	// Operand 操作数接口。
	Operand = operands.Operand
	// MyDate 日期类型。
	MyDate = operands.MyDate
	// Function 表达式节点。
	Function = internals.Function
	// AlgorithmEngine 表达式引擎。
	AlgorithmEngine = internals.AlgorithmEngine
	// DiyNameInfo 自定义名称信息。
	DiyNameInfo = internals.DiyNameInfo
	// OperandType 操作数类型枚举。
	OperandType = enums.OperandType
	// CalculationLogicType 计算逻辑类型枚举。
	CalculationLogicType = enums.CalculationLogicType
)

const (
	// OperandTypeText 文本类型。
	OperandTypeText = enums.OperandTypeText
)

var (
	// Create 由 Go 原生值创建操作数。
	Create = operands.Create
	// CreateInt 创建整数操作数。
	CreateInt = operands.CreateInt
	// CreateInt64 创建 64 位整数操作数。
	CreateInt64 = operands.CreateInt64
	// CreateFloat 创建浮点操作数。
	CreateFloat = operands.CreateFloat
	// CreateDecimal 创建十进制操作数。
	CreateDecimal = operands.CreateDecimal
	// CreateString 创建字符串操作数。
	CreateString = operands.CreateString
	// CreateBool 创建布尔操作数。
	CreateBool = operands.CreateBool
	// CreateMyDate 创建日期操作数。
	CreateMyDate = operands.CreateMyDate
	// NewMyDateFromTime 由 time.Time 创建日期。
	NewMyDateFromTime = operands.NewMyDateFromTime
	// NewAlgorithmEngine 创建表达式引擎。
	NewAlgorithmEngine = internals.NewAlgorithmEngine
	// Errorf 创建带格式的错误操作数。
	Errorf = operands.Errorf
)
