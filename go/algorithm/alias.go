package algorithm

import (
	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/decimal"
	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/enums"
	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals"
	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/litjson"
	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/operands"
)

// 公开 API：把各子包的类型与工厂提升到 algorithm 包，
// 使用方只需 import 本包（与 C# / Java 版在根命名空间暴露 AlgorithmEngine / Operand 一致）。

// AlgorithmEngine 表达式引擎。
type AlgorithmEngine = internals.AlgorithmEngine

// Function 表达式节点。
type Function = internals.Function

// BaseFunction 节点公共实现。
type BaseFunction = internals.BaseFunction

// TempParameter 临时参数解析函数。
type TempParameter = internals.TempParameter

// ParameterType 参数类型描述。
type ParameterType = internals.ParameterType

// CalculateTree 计算树节点。
type CalculateTree = internals.CalculateTree

// ConditionTree 条件树节点。
type ConditionTree = internals.ConditionTree

// DiyNameInfo 自定义名称集合。
type DiyNameInfo = internals.DiyNameInfo

// DiyNameKeyInfo 自定义名称位置信息。
type DiyNameKeyInfo = internals.DiyNameKeyInfo

// Operand 操作数接口。
type Operand = operands.Operand

// Decimal 十进制数值。
type Decimal = decimal.Decimal

// MyDate 日期值。
type MyDate = operands.MyDate

// KeyValue 键值对。
type KeyValue = operands.KeyValue

// OperandArray 数组操作数。
type OperandArray = operands.OperandArray

// OperandString 字符串操作数。
type OperandString = operands.OperandString

// OperandBoolean 布尔操作数。
type OperandBoolean = operands.OperandBoolean

// OperandDecimal 十进制操作数。
type OperandDecimal = operands.OperandDecimal

// OperandInt 整数操作数。
type OperandInt = operands.OperandInt

// OperandError 错误操作数。
type OperandError = operands.OperandError

// OperandMyDate 日期操作数。
type OperandMyDate = operands.OperandMyDate

// OperandJson JSON 操作数。
type OperandJson = operands.OperandJson

// OperandNull 空值操作数。
type OperandNull = operands.OperandNull

// OperandNone 未指定值操作数。
type OperandNone = operands.OperandNone

// OperandKeyValueList 键值对列表操作数。
type OperandKeyValueList = operands.OperandKeyValueList

// JsonData JSON 节点。
type JsonData = litjson.JsonData

// JsonType JSON 节点类型。
type JsonType = litjson.JsonType

// OperandType 操作数类型。
type OperandType = enums.OperandType

// DistanceUnitType 长度单位。
type DistanceUnitType = enums.DistanceUnitType

// AreaUnitType 面积单位。
type AreaUnitType = enums.AreaUnitType

// VolumeUnitType 体积单位。
type VolumeUnitType = enums.VolumeUnitType

// MassUnitType 质量单位。
type MassUnitType = enums.MassUnitType

// NumberUnitType 数字单位。
type NumberUnitType = enums.NumberUnitType

// CalculateTreeType 计算树类型。
type CalculateTreeType = enums.CalculateTreeType

// ConditionTreeType 条件树类型。
type ConditionTreeType = enums.ConditionTreeType

// CalculationLogicType 计算逻辑类型。
type CalculationLogicType = enums.CalculationLogicType

// CombineCalculateType 组合计算类型。
type CombineCalculateType = enums.CombineCalculateType

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

	// DistanceUnitTypeMM 毫米。
	DistanceUnitTypeMM = enums.DistanceUnitTypeMM
	// DistanceUnitTypeCM 厘米。
	DistanceUnitTypeCM = enums.DistanceUnitTypeCM
	// DistanceUnitTypeDM 分米。
	DistanceUnitTypeDM = enums.DistanceUnitTypeDM
	// DistanceUnitTypeM 米。
	DistanceUnitTypeM = enums.DistanceUnitTypeM
	// DistanceUnitTypeKM 千米。
	DistanceUnitTypeKM = enums.DistanceUnitTypeKM

	// AreaUnitTypeMM2 平方毫米。
	AreaUnitTypeMM2 = enums.AreaUnitTypeMM2
	// AreaUnitTypeCM2 平方厘米。
	AreaUnitTypeCM2 = enums.AreaUnitTypeCM2
	// AreaUnitTypeDM2 平方分米。
	AreaUnitTypeDM2 = enums.AreaUnitTypeDM2
	// AreaUnitTypeM2 平方米。
	AreaUnitTypeM2 = enums.AreaUnitTypeM2
	// AreaUnitTypeKM2 平方千米。
	AreaUnitTypeKM2 = enums.AreaUnitTypeKM2

	// VolumeUnitTypeMM3 立方毫米。
	VolumeUnitTypeMM3 = enums.VolumeUnitTypeMM3
	// VolumeUnitTypeCM3 立方厘米。
	VolumeUnitTypeCM3 = enums.VolumeUnitTypeCM3
	// VolumeUnitTypeDM3 立方分米。
	VolumeUnitTypeDM3 = enums.VolumeUnitTypeDM3
	// VolumeUnitTypeM3 立方米。
	VolumeUnitTypeM3 = enums.VolumeUnitTypeM3
	// VolumeUnitTypeKM3 立方千米。
	VolumeUnitTypeKM3 = enums.VolumeUnitTypeKM3

	// MassUnitTypeG 克。
	MassUnitTypeG = enums.MassUnitTypeG
	// MassUnitTypeKG 千克。
	MassUnitTypeKG = enums.MassUnitTypeKG
	// MassUnitTypeT 吨。
	MassUnitTypeT = enums.MassUnitTypeT

	// CalculateTreeTypeString 字符串。
	CalculateTreeTypeString = enums.CalculateTreeTypeString
	// CalculateTreeTypeAdd 加法。
	CalculateTreeTypeAdd = enums.CalculateTreeTypeAdd
	// CalculateTreeTypeSub 减法。
	CalculateTreeTypeSub = enums.CalculateTreeTypeSub
	// CalculateTreeTypeMul 乘法。
	CalculateTreeTypeMul = enums.CalculateTreeTypeMul
	// CalculateTreeTypeDiv 除法。
	CalculateTreeTypeDiv = enums.CalculateTreeTypeDiv
	// CalculateTreeTypeMod 取余。
	CalculateTreeTypeMod = enums.CalculateTreeTypeMod
	// CalculateTreeTypeConnect 连接。
	CalculateTreeTypeConnect = enums.CalculateTreeTypeConnect
	// CalculateTreeTypeAnd 逻辑与。
	CalculateTreeTypeAnd = enums.CalculateTreeTypeAnd
	// CalculateTreeTypeOr 逻辑或。
	CalculateTreeTypeOr = enums.CalculateTreeTypeOr
	// CalculateTreeTypeOpGt 大于。
	CalculateTreeTypeOpGt = enums.CalculateTreeTypeOpGt
	// CalculateTreeTypeOpLt 小于。
	CalculateTreeTypeOpLt = enums.CalculateTreeTypeOpLt
	// CalculateTreeTypeOpGe 大于等于。
	CalculateTreeTypeOpGe = enums.CalculateTreeTypeOpGe
	// CalculateTreeTypeOpLe 小于等于。
	CalculateTreeTypeOpLe = enums.CalculateTreeTypeOpLe
	// CalculateTreeTypeOpEq 等于。
	CalculateTreeTypeOpEq = enums.CalculateTreeTypeOpEq
	// CalculateTreeTypeOpNe 不等于。
	CalculateTreeTypeOpNe = enums.CalculateTreeTypeOpNe
	// CalculateTreeTypeError 错误。
	CalculateTreeTypeError = enums.CalculateTreeTypeError

	// ConditionTreeTypeString 字符串。
	ConditionTreeTypeString = enums.ConditionTreeTypeString
	// ConditionTreeTypeAnd 逻辑与。
	ConditionTreeTypeAnd = enums.ConditionTreeTypeAnd
	// ConditionTreeTypeOr 逻辑或。
	ConditionTreeTypeOr = enums.ConditionTreeTypeOr
	// ConditionTreeTypeError 错误。
	ConditionTreeTypeError = enums.ConditionTreeTypeError

	// CombineCalculateTypeAdd 加法。
	CombineCalculateTypeAdd = enums.CombineCalculateTypeAdd
	// CombineCalculateTypeSub 减法。
	CombineCalculateTypeSub = enums.CombineCalculateTypeSub
	// CombineCalculateTypeMul 乘法。
	CombineCalculateTypeMul = enums.CombineCalculateTypeMul
	// CombineCalculateTypeDiv 除法。
	CombineCalculateTypeDiv = enums.CombineCalculateTypeDiv
	// CombineCalculateTypeMod 取余。
	CombineCalculateTypeMod = enums.CombineCalculateTypeMod
	// CombineCalculateTypeConnect 连接。
	CombineCalculateTypeConnect = enums.CombineCalculateTypeConnect
	// CombineCalculateTypeAnd 逻辑与。
	CombineCalculateTypeAnd = enums.CombineCalculateTypeAnd
	// CombineCalculateTypeOr 逻辑或。
	CombineCalculateTypeOr = enums.CombineCalculateTypeOr
	// CombineCalculateTypeOpGt 大于。
	CombineCalculateTypeOpGt = enums.CombineCalculateTypeOpGt
	// CombineCalculateTypeOpLt 小于。
	CombineCalculateTypeOpLt = enums.CombineCalculateTypeOpLt
	// CombineCalculateTypeOpGe 大于等于。
	CombineCalculateTypeOpGe = enums.CombineCalculateTypeOpGe
	// CombineCalculateTypeOpLe 小于等于。
	CombineCalculateTypeOpLe = enums.CombineCalculateTypeOpLe
	// CombineCalculateTypeOpEq 等于。
	CombineCalculateTypeOpEq = enums.CombineCalculateTypeOpEq
	// CombineCalculateTypeOpNe 不等于。
	CombineCalculateTypeOpNe = enums.CombineCalculateTypeOpNe
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
	// Version 版本号操作数。
	Version = operands.Version

	// NewAlgorithmEngine 创建表达式引擎。
	NewAlgorithmEngine = internals.NewAlgorithmEngine
	// NewNoneEngine 创建「所有参数为空」的引擎（用于推导参数类型）。
	NewNoneEngine = internals.NewNoneEngine
	// NewCalculateTree 创建计算树节点。
	NewCalculateTree = internals.NewCalculateTree
	// NewConditionTree 创建条件树节点。
	NewConditionTree = internals.NewConditionTree
	// ToString 输出表达式节点的文本。
	ToString = internals.FormatFunction
	// GetFunction 按注册名创建函数节点。
	GetFunction = internals.GetFunction
	// RegisterFunction 注册函数节点构造器。
	RegisterFunction = internals.RegisterFunction

	// Create 由 Go 原生值创建操作数。
	Create = operands.Create
	// CreateInt 创建整数操作数。
	CreateInt = operands.CreateInt
	// CreateInt64 创建 64 位整数操作数。
	CreateInt64 = operands.CreateInt64
	// CreateUint64 创建无符号整数操作数。
	CreateUint64 = operands.CreateUint64
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
	// NewKeyValue 创建键值对。
	NewKeyValue = operands.NewKeyValue
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

	// DecimalZero 常量 0。
	DecimalZero = decimal.DecimalZero
	// DecimalOne 常量 1。
	DecimalOne = decimal.DecimalOne
	// DecimalMax decimal 最大值。
	DecimalMax = decimal.DecimalMax
	// DecFromInt 创建整数 Decimal。
	DecFromInt = decimal.DecFromInt
	// DecFromFloat 创建浮点 Decimal。
	DecFromFloat = decimal.DecFromFloat
	// DecFromString 解析 Decimal。
	DecFromString = decimal.DecFromString
	// TryParseDecimal 尝试解析 Decimal。
	TryParseDecimal = decimal.TryParseDecimal
	// MustDecimal 解析 Decimal（失败返回 0）。
	MustDecimal = decimal.MustDecimal

	// JsonMapperToObject 解析 JSON 文本。
	JsonMapperToObject = litjson.JsonMapperToObject
)
