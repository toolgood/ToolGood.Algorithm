package internals

import (
	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/decimal"
	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/enums"
	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/operands"
)

// 便于移植代码阅读的类型别名。

// Operand 操作数接口。
type Operand = operands.Operand

// Decimal 十进制数值类型。
type Decimal = decimal.Decimal

// MyDate 日期类型。
type MyDate = operands.MyDate

// KeyValue 键值对类型。
type KeyValue = operands.KeyValue

// OperandType 操作数类型枚举。
type OperandType = enums.OperandType

// CalculateTreeType 计算树类型枚举。
type CalculateTreeType = enums.CalculateTreeType

// ConditionTreeType 条件树类型枚举。
type ConditionTreeType = enums.ConditionTreeType

// CombineCalculateType 组合计算类型枚举。
type CombineCalculateType = enums.CombineCalculateType

// DistanceUnitType 长度单位枚举。
type DistanceUnitType = enums.DistanceUnitType

// AreaUnitType 面积单位枚举。
type AreaUnitType = enums.AreaUnitType

// VolumeUnitType 体积单位枚举。
type VolumeUnitType = enums.VolumeUnitType

// MassUnitType 质量单位枚举。
type MassUnitType = enums.MassUnitType

// NumberUnitType 数字单位枚举。
type NumberUnitType = enums.NumberUnitType

// CalculationLogicType 计算逻辑类型枚举。
type CalculationLogicType = enums.CalculationLogicType

const (
	OperandTypeNone      = enums.OperandTypeNone
	OperandTypeNull      = enums.OperandTypeNull
	OperandTypeError     = enums.OperandTypeError
	OperandTypeDate      = enums.OperandTypeDate
	OperandTypeArray     = enums.OperandTypeArray
	OperandTypeNumber    = enums.OperandTypeNumber
	OperandTypeBoolean   = enums.OperandTypeBoolean
	OperandTypeText      = enums.OperandTypeText
	OperandTypeJson      = enums.OperandTypeJson
	OperandTypeArrayJson = enums.OperandTypeArrayJson

	CalculateTreeTypeString  = enums.CalculateTreeTypeString
	CalculateTreeTypeAdd     = enums.CalculateTreeTypeAdd
	CalculateTreeTypeSub     = enums.CalculateTreeTypeSub
	CalculateTreeTypeMul     = enums.CalculateTreeTypeMul
	CalculateTreeTypeDiv     = enums.CalculateTreeTypeDiv
	CalculateTreeTypeMod     = enums.CalculateTreeTypeMod
	CalculateTreeTypeConnect = enums.CalculateTreeTypeConnect
	CalculateTreeTypeAnd     = enums.CalculateTreeTypeAnd
	CalculateTreeTypeOr      = enums.CalculateTreeTypeOr
	CalculateTreeTypeOpGt    = enums.CalculateTreeTypeOpGt
	CalculateTreeTypeOpLt    = enums.CalculateTreeTypeOpLt
	CalculateTreeTypeOpGe    = enums.CalculateTreeTypeOpGe
	CalculateTreeTypeOpLe    = enums.CalculateTreeTypeOpLe
	CalculateTreeTypeOpEq    = enums.CalculateTreeTypeOpEq
	CalculateTreeTypeOpNe    = enums.CalculateTreeTypeOpNe
	CalculateTreeTypeError   = enums.CalculateTreeTypeError

	ConditionTreeTypeString = enums.ConditionTreeTypeString
	ConditionTreeTypeAnd    = enums.ConditionTreeTypeAnd
	ConditionTreeTypeOr     = enums.ConditionTreeTypeOr
	ConditionTreeTypeError  = enums.ConditionTreeTypeError

	DistanceUnitTypeMM = enums.DistanceUnitTypeMM
	DistanceUnitTypeCM = enums.DistanceUnitTypeCM
	DistanceUnitTypeDM = enums.DistanceUnitTypeDM
	DistanceUnitTypeM  = enums.DistanceUnitTypeM
	DistanceUnitTypeKM = enums.DistanceUnitTypeKM

	AreaUnitTypeMM2 = enums.AreaUnitTypeMM2
	AreaUnitTypeCM2 = enums.AreaUnitTypeCM2
	AreaUnitTypeDM2 = enums.AreaUnitTypeDM2
	AreaUnitTypeM2  = enums.AreaUnitTypeM2
	AreaUnitTypeKM2 = enums.AreaUnitTypeKM2

	VolumeUnitTypeMM3 = enums.VolumeUnitTypeMM3
	VolumeUnitTypeCM3 = enums.VolumeUnitTypeCM3
	VolumeUnitTypeDM3 = enums.VolumeUnitTypeDM3
	VolumeUnitTypeM3  = enums.VolumeUnitTypeM3
	VolumeUnitTypeKM3 = enums.VolumeUnitTypeKM3

	MassUnitTypeG  = enums.MassUnitTypeG
	MassUnitTypeKG = enums.MassUnitTypeKG
	MassUnitTypeT  = enums.MassUnitTypeT

	NumberUnitTypeMM  = enums.NumberUnitTypeMM
	NumberUnitTypeCM  = enums.NumberUnitTypeCM
	NumberUnitTypeDM  = enums.NumberUnitTypeDM
	NumberUnitTypeM   = enums.NumberUnitTypeM
	NumberUnitTypeKM  = enums.NumberUnitTypeKM
	NumberUnitTypeMM2 = enums.NumberUnitTypeMM2
	NumberUnitTypeCM2 = enums.NumberUnitTypeCM2
	NumberUnitTypeDM2 = enums.NumberUnitTypeDM2
	NumberUnitTypeM2  = enums.NumberUnitTypeM2
	NumberUnitTypeKM2 = enums.NumberUnitTypeKM2
	NumberUnitTypeMM3 = enums.NumberUnitTypeMM3
	NumberUnitTypeCM3 = enums.NumberUnitTypeCM3
	NumberUnitTypeDM3 = enums.NumberUnitTypeDM3
	NumberUnitTypeM3  = enums.NumberUnitTypeM3
	NumberUnitTypeKM3 = enums.NumberUnitTypeKM3
	NumberUnitTypeG   = enums.NumberUnitTypeG
	NumberUnitTypeKG  = enums.NumberUnitTypeKG
	NumberUnitTypeT   = enums.NumberUnitTypeT
)

var (
	// True 布尔真。
	True = operands.True
	// False 布尔假。
	False = operands.False
	// Null 空值。
	Null = operands.Null
	// None 未指定值。
	None = operands.None
	// Zero 数字 0。
	Zero = operands.Zero
	// One 数字 1。
	One = operands.One
	// Version 版本号。
	Version = operands.Version

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
	// CreateDate 由时间分量创建日期操作数。
	CreateDate = operands.CreateDate
	// CreateArray 创建数组操作数。
	CreateArray = operands.CreateArray
	// CreateJsonData 创建 JSON 操作数。
	CreateJsonData = operands.CreateJsonData
	// CreateJson 解析 JSON 文本。
	CreateJson = operands.CreateJson
	// Error 创建错误操作数。
	Error = operands.Error
	// Errorf 创建带格式的错误操作数。
	Errorf = operands.Errorf

	// ParseMyDate 解析日期文本。
	ParseMyDate = operands.ParseMyDate
	// NewMyDate 创建日期。
	NewMyDate = operands.NewMyDate
	// NewMyDateFromTime 由 time.Time 创建日期。
	NewMyDateFromTime = operands.NewMyDateFromTime
	// NewMyDateFromNumber 由序列值创建日期。
	NewMyDateFromNumber = operands.NewMyDateFromNumber
	// MyDateNow 当前时间。
	MyDateNow = operands.MyDateNow

	// DecimalZero 常量 0。
	DecimalZero = decimal.DecimalZero
	// DecimalOne 常量 1。
	DecimalOne = decimal.DecimalOne
	// DecimalMax decimal 最大值。
	DecimalMax = decimal.DecimalMax

	// DecFromInt 创建整数 Decimal。
	DecFromInt = decimal.DecFromInt
	// DecFromUint 创建无符号 Decimal。
	DecFromUint = decimal.DecFromUint
	// DecFromFloat 创建浮点 Decimal。
	DecFromFloat = decimal.DecFromFloat
	// DecFromString 解析 Decimal。
	DecFromString = decimal.DecFromString
	// DecFromUnscaled 由非标度整数创建 Decimal。
	DecFromUnscaled = decimal.DecFromUnscaled
	// TryParseDecimal 尝试解析 Decimal。
	TryParseDecimal = decimal.TryParseDecimal
	// MustDecimal 解析 Decimal。
	MustDecimal = decimal.MustDecimal
	// Divide Decimal 除法。
	Divide = decimal.Divide
	// TruncDecimal 向零取整。
	TruncDecimal = decimal.TruncDecimal
	// RoundHalfAway 四舍五入。
	RoundHalfAway = decimal.RoundHalfAway
	// IsNaNOrInf 是否 NaN / Inf。
	IsNaNOrInf = decimal.IsNaNOrInf
	// Plain 等价 toPlainString。
	Plain = decimal.Plain
	// PlainStripValue 去尾零文本。
	PlainStripValue = decimal.PlainStripValue
	// ExcelEpochDays Excel 日期偏移。
	ExcelEpochDays = decimal.ExcelEpochDays
)
