package mathtransformation

import (
	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/decimal"
	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/enums"
	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals"
	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/litjson"
	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/operands"
)

// 本文件把 internals / operands / decimal / enums 中的公共名字引入本包，
// 使函数实现代码与 C# / Java / Python 版本保持相近的书写方式。

type Operand = operands.Operand
type Decimal = decimal.Decimal
type MyDate = operands.MyDate
type JsonData = litjson.JsonData
type Function = internals.Function
type BaseFunction = internals.BaseFunction
type TempParameter = internals.TempParameter
type AlgorithmEngine = internals.AlgorithmEngine
type OperandType = enums.OperandType
type ParameterType = internals.ParameterType
type KeyValue = operands.KeyValue
type OperandKeyValue = operands.OperandKeyValue
type OperandKeyValueList = operands.OperandKeyValueList

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
)

var (
	True    = operands.True
	False   = operands.False
	Null    = operands.Null
	None    = operands.None
	Zero    = operands.Zero
	One     = operands.One
	Version = operands.Version

	Create         = operands.Create
	CreateInt      = operands.CreateInt
	CreateInt64    = operands.CreateInt64
	CreateFloat    = operands.CreateFloat
	CreateDecimal  = operands.CreateDecimal
	CreateString   = operands.CreateString
	CreateBool     = operands.CreateBool
	CreateMyDate   = operands.CreateMyDate
	CreateDate     = operands.CreateDate
	CreateArray    = operands.CreateArray
	CreateJsonData = operands.CreateJsonData
	CreateJson     = operands.CreateJson
	Error          = operands.Error
	Errorf         = operands.Errorf

	ParseMyDate         = operands.ParseMyDate
	NewMyDate           = operands.NewMyDate
	NewMyDateFromTime   = operands.NewMyDateFromTime
	NewMyDateFromDate   = operands.NewMyDateFromDate
	NewMyDateFromNumber = operands.NewMyDateFromNumber
	MyDateNow           = operands.MyDateNow

	DecimalZero     = decimal.DecimalZero
	DecimalOne      = decimal.DecimalOne
	DecimalMax      = decimal.DecimalMax
	DecFromInt      = decimal.DecFromInt
	DecFromUint     = decimal.DecFromUint
	DecFromFloat    = decimal.DecFromFloat
	DecFromString   = decimal.DecFromString
	TryParseDecimal = decimal.TryParseDecimal
	MustDecimal     = decimal.MustDecimal
	Divide          = decimal.Divide
	TruncDecimal    = decimal.TruncDecimal
	RoundHalfAway   = decimal.RoundHalfAway
	IsNaNOrInf      = decimal.IsNaNOrInf
	Plain           = decimal.Plain
	PlainStripValue = decimal.PlainStripValue
	ExcelEpochDays  = decimal.ExcelEpochDays

	JsonMapperToObject = litjson.JsonMapperToObject

	NewBase          = internals.NewBase
	RegisterFunction = internals.RegisterFunction
	RequireArgs      = internals.RequireArgs
	MustArgs         = internals.MustArgs
)
