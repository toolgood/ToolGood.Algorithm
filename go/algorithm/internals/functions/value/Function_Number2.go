package value

import (
	"strings"

	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/enums"
)

// 移植自 Internals/Functions/Value/Function_Number2.cs。

// FunctionNumber2 带单位的数字字面量节点（如 1KM2）。
type FunctionNumber2 struct {
	BaseFunction
	d    Decimal
	unit string
}

// NewFunctionNumber2 构造带单位的数字节点（对应 C# 的 Function_Number2(decimal, string)）。
func NewFunctionNumber2(v Decimal, unit string) Function {
	return &FunctionNumber2{BaseFunction: NewBase("Num", OperandTypeNumber, nil), d: v, unit: unit}
}

// Evaluate 按引擎当前配置的单位做换算。
func (f *FunctionNumber2) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	unitType, ok := unitTypeDict[strings.ToUpper(f.unit)]
	if !ok {
		return Errorf("Number unit '{0}' is invalid!", f.unit)
	}
	d2, ok := transformationUnit(f.d, unitType, engine.DistanceUnit, engine.AreaUnit, engine.VolumeUnit, engine.MassUnit)
	if !ok {
		return f.NumError()
	}
	return CreateDecimal(d2)
}

// WriteTo 输出 `数字+单位`。
func (f *FunctionNumber2) WriteTo(sb *strings.Builder, addBrackets bool) {
	sb.WriteString(f.d.String())
	sb.WriteString(f.unit)
}

// unitTypeDict 单位名称（忽略大小写）到单位类型的映射。
var unitTypeDict = map[string]enums.NumberUnitType{
	"KM":  enums.NumberUnitTypeKM,
	"M":   enums.NumberUnitTypeM,
	"DM":  enums.NumberUnitTypeDM,
	"CM":  enums.NumberUnitTypeCM,
	"MM":  enums.NumberUnitTypeMM,
	"KM2": enums.NumberUnitTypeKM2,
	"M2":  enums.NumberUnitTypeM2,
	"DM2": enums.NumberUnitTypeDM2,
	"CM2": enums.NumberUnitTypeCM2,
	"MM2": enums.NumberUnitTypeMM2,
	"M3":  enums.NumberUnitTypeM3,
	"DM3": enums.NumberUnitTypeDM3,
	"CM3": enums.NumberUnitTypeCM3,
	"MM3": enums.NumberUnitTypeMM3,
	"KM3": enums.NumberUnitTypeKM3,
	"L":   enums.NumberUnitTypeDM3,
	"ML":  enums.NumberUnitTypeCM3,
	"T":   enums.NumberUnitTypeT,
	"KG":  enums.NumberUnitTypeKG,
	"G":   enums.NumberUnitTypeG,
}

// unitFactors 各单位相对基准单位（M / M2 / M3 / G）的系数。
var unitFactors = map[enums.NumberUnitType]Decimal{
	enums.NumberUnitTypeMM: MustDecimal("0.001"),
	enums.NumberUnitTypeCM: MustDecimal("0.01"),
	enums.NumberUnitTypeDM: MustDecimal("0.1"),
	enums.NumberUnitTypeM:  DecimalOne,
	enums.NumberUnitTypeKM: DecFromInt(1000),

	enums.NumberUnitTypeMM2: MustDecimal("0.000001"),
	enums.NumberUnitTypeCM2: MustDecimal("0.0001"),
	enums.NumberUnitTypeDM2: MustDecimal("0.01"),
	enums.NumberUnitTypeM2:  DecimalOne,
	enums.NumberUnitTypeKM2: DecFromInt(1000000),

	enums.NumberUnitTypeMM3: MustDecimal("0.000000001"),
	enums.NumberUnitTypeCM3: MustDecimal("0.000001"),
	enums.NumberUnitTypeDM3: MustDecimal("0.001"),
	enums.NumberUnitTypeM3:  DecimalOne,
	enums.NumberUnitTypeKM3: DecFromInt(1000000000),

	enums.NumberUnitTypeG:  DecimalOne,
	enums.NumberUnitTypeKG: DecFromInt(1000),
	enums.NumberUnitTypeT:  DecFromInt(1000000),
}

// transformationUnit 把以 type1 表示的 src 换算成引擎配置的单位。
// 返回 false 表示换算结果超出 decimal 范围（对应 C# 的 OverflowException）。
func transformationUnit(src Decimal, type1 enums.NumberUnitType, distance enums.DistanceUnitType, area enums.AreaUnitType, volume enums.VolumeUnitType, mass enums.MassUnitType) (Decimal, bool) {
	factor1, ok := unitFactors[type1]
	if !ok {
		panic("Number unit is error!")
	}
	var target enums.NumberUnitType
	switch {
	case type1 >= enums.NumberUnitTypeMM && type1 <= enums.NumberUnitTypeKM:
		target = enums.NumberUnitType(distance)
	case type1 >= enums.NumberUnitTypeMM2 && type1 <= enums.NumberUnitTypeKM2:
		target = enums.NumberUnitType(area)
	case type1 >= enums.NumberUnitTypeMM3 && type1 <= enums.NumberUnitTypeKM3:
		target = enums.NumberUnitType(volume)
	case type1 >= enums.NumberUnitTypeG && type1 <= enums.NumberUnitTypeT:
		target = enums.NumberUnitType(mass)
	default:
		panic("Number unit is error!")
	}
	factor2, ok := unitFactors[target]
	if !ok {
		panic("Number unit is error!")
	}
	result := src.Mul(factor1).Div(factor2)
	if result.Abs().Greater(DecimalMax) {
		return Decimal{}, false
	}
	return result, true
}
