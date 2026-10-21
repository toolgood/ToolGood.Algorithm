package enums

// OperandType 操作数类型。
//
// 与 C# / Java 版 toolgood.algorithm.enums.OperandType 一一对应。
type OperandType int

const (
	OperandTypeNone OperandType = iota
	OperandTypeNull
	OperandTypeError
	OperandTypeDate
	OperandTypeArray
	OperandTypeNumber
	OperandTypeBoolean
	OperandTypeText
	OperandTypeJson
	OperandTypeArrayJson
)

// String 返回与 C# 枚举一致的名称。
func (t OperandType) String() string {
	switch t {
	case OperandTypeNone:
		return "NONE"
	case OperandTypeNull:
		return "NULL"
	case OperandTypeError:
		return "ERROR"
	case OperandTypeDate:
		return "DATE"
	case OperandTypeArray:
		return "ARRAY"
	case OperandTypeNumber:
		return "NUMBER"
	case OperandTypeBoolean:
		return "BOOLEAN"
	case OperandTypeText:
		return "TEXT"
	case OperandTypeJson:
		return "JSON"
	case OperandTypeArrayJson:
		return "ARRAYJSON"
	}
	return "NONE"
}

// CalculateTreeType 计算树节点类型。
type CalculateTreeType int

const (
	CalculateTreeTypeString CalculateTreeType = iota
	CalculateTreeTypeAdd
	CalculateTreeTypeSub
	CalculateTreeTypeMul
	CalculateTreeTypeDiv
	CalculateTreeTypeMod
	CalculateTreeTypeConnect
	CalculateTreeTypeAnd
	CalculateTreeTypeOr
	CalculateTreeTypeOpGt
	CalculateTreeTypeOpLt
	CalculateTreeTypeOpGe
	CalculateTreeTypeOpLe
	CalculateTreeTypeOpEq
	CalculateTreeTypeOpNe
	CalculateTreeTypeError
)

// ConditionTreeType 条件树节点类型。
type ConditionTreeType int

const (
	ConditionTreeTypeString ConditionTreeType = iota
	ConditionTreeTypeAnd
	ConditionTreeTypeOr
	ConditionTreeTypeError
)

// CombineCalculateType 组合计算类型。
type CombineCalculateType int

const (
	CombineCalculateTypeAdd CombineCalculateType = iota + 1
	CombineCalculateTypeSub
	CombineCalculateTypeMul
	CombineCalculateTypeDiv
	CombineCalculateTypeMod
	CombineCalculateTypeConnect
	CombineCalculateTypeAnd
	CombineCalculateTypeOr
	CombineCalculateTypeOpGt
	CombineCalculateTypeOpLt
	CombineCalculateTypeOpGe
	CombineCalculateTypeOpLe
	CombineCalculateTypeOpEq
	CombineCalculateTypeOpNe
)

// NumberUnitType 数字单位（长度 / 面积 / 体积 / 质量）。
type NumberUnitType int

const (
	NumberUnitTypeMM NumberUnitType = 1
	NumberUnitTypeCM NumberUnitType = 2
	NumberUnitTypeDM NumberUnitType = 3
	NumberUnitTypeM  NumberUnitType = 4
	NumberUnitTypeKM NumberUnitType = 5

	NumberUnitTypeMM2 NumberUnitType = 11
	NumberUnitTypeCM2 NumberUnitType = 12
	NumberUnitTypeDM2 NumberUnitType = 13
	NumberUnitTypeM2  NumberUnitType = 14
	NumberUnitTypeKM2 NumberUnitType = 15

	NumberUnitTypeMM3 NumberUnitType = 21
	NumberUnitTypeCM3 NumberUnitType = 22
	NumberUnitTypeDM3 NumberUnitType = 23
	NumberUnitTypeM3  NumberUnitType = 24
	NumberUnitTypeKM3 NumberUnitType = 25

	NumberUnitTypeG  NumberUnitType = 31
	NumberUnitTypeKG NumberUnitType = 32
	NumberUnitTypeT  NumberUnitType = 33
)

// DistanceUnitType 长度单位。
type DistanceUnitType int

const (
	DistanceUnitTypeMM DistanceUnitType = 1
	DistanceUnitTypeCM DistanceUnitType = 2
	DistanceUnitTypeDM DistanceUnitType = 3
	DistanceUnitTypeM  DistanceUnitType = 4
	DistanceUnitTypeKM DistanceUnitType = 5
)

// AreaUnitType 面积单位。
type AreaUnitType int

const (
	AreaUnitTypeMM2 AreaUnitType = 11
	AreaUnitTypeCM2 AreaUnitType = 12
	AreaUnitTypeDM2 AreaUnitType = 13
	AreaUnitTypeM2  AreaUnitType = 14
	AreaUnitTypeKM2 AreaUnitType = 15
)

// VolumeUnitType 体积单位。
type VolumeUnitType int

const (
	VolumeUnitTypeMM3 VolumeUnitType = 21
	VolumeUnitTypeCM3 VolumeUnitType = 22
	VolumeUnitTypeDM3 VolumeUnitType = 23
	VolumeUnitTypeM3  VolumeUnitType = 24
	VolumeUnitTypeKM3 VolumeUnitType = 25
)

// MassUnitType 质量单位。
type MassUnitType int

const (
	MassUnitTypeG  MassUnitType = 31
	MassUnitTypeKG MassUnitType = 32
	MassUnitTypeT  MassUnitType = 33
)

// CalculationLogicType 计算逻辑类型。
type CalculationLogicType int

const (
	CalculationLogicTypeNone CalculationLogicType = iota
	CalculationLogicTypeAdd
	CalculationLogicTypeSub
	CalculationLogicTypeMul
	CalculationLogicTypeDiv
	CalculationLogicTypeMod
	CalculationLogicTypeConnect
	CalculationLogicTypeAnd
	CalculationLogicTypeOr
	CalculationLogicTypeOpGt
	CalculationLogicTypeOpLt
	CalculationLogicTypeOpGe
	CalculationLogicTypeOpLe
	CalculationLogicTypeOpEq
	CalculationLogicTypeOpNe
	CalculationLogicTypeIf
)

func intToCalculateTreeType(value int) (CalculateTreeType, bool) {
	if value < 0 || value > int(CalculateTreeTypeError) {
		return CalculateTreeTypeString, false
	}
	return CalculateTreeType(value), true
}

func intToConditionTreeType(value int) (ConditionTreeType, bool) {
	if value < 0 || value > int(ConditionTreeTypeError) {
		return ConditionTreeTypeString, false
	}
	return ConditionTreeType(value), true
}

func intToCombineCalculateType(value int) (CombineCalculateType, bool) {
	if value < 1 || value > int(CombineCalculateTypeOpNe) {
		return CombineCalculateTypeAdd, false
	}
	return CombineCalculateType(value), true
}
