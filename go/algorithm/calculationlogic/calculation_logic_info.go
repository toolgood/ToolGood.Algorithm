package calculationlogic

import "strings"

// C# CalculationLogicType 的成员（顺序与 C# 枚举一致）。
//
// enums.CalculationLogicType 目前只有 None/Add/Sub/... 一组与 CombineCalculateType 同值的成员，
// 缺少 C# CalculationLogicType 的语义成员，此处按 C# 定义补齐；类型仍复用 enums.CalculationLogicType。
const (
	// LogicTypeScene 场景。
	LogicTypeScene CalculationLogicType = iota
	// LogicTypeInitValue 初始值。
	LogicTypeInitValue
	// LogicTypeCondition 条件。
	LogicTypeCondition
	// LogicTypeSetFormula 用公式赋值。
	LogicTypeSetFormula
	// LogicTypeSetValue 赋值。
	LogicTypeSetValue
	// LogicTypeError 错误。
	LogicTypeError
	// LogicTypeBlankLine 空行。
	LogicTypeBlankLine
)

// CalculationLogicInfo 计算逻辑信息（对应 C# CalculationLogicInfo）。
type CalculationLogicInfo struct {
	// LogicType 类型。
	LogicType CalculationLogicType
	// Layer 层级。
	Layer int
	// Name 逻辑名称。
	Name string
	// Exp 公式。
	Exp string
	// Exp2 公式（参数已替换为值）。
	Exp2 string
	// Remark 备注。
	Remark string
	// Value 值。
	Value Operand
}

// ToInfoString 转换为信息字符串。
func (i *CalculationLogicInfo) ToInfoString() string {
	remark := ""
	if i.Remark != "" {
		remark = " // " + i.Remark
	}
	layerStr := strings.Repeat(" ", layerWidth(i.Layer)*3)
	switch i.LogicType {
	case LogicTypeBlankLine:
		return ""
	case LogicTypeScene:
		return "===== " + i.Name + " ====="
	case LogicTypeInitValue:
		return i.Name + "=" + i.Exp + ";"
	case LogicTypeCondition:
		if i.Value.IsError() {
			return "[条件][错误] " + layerStr + "if " + i.Exp + ": // " + i.Exp2 + " = " + i.Value.ErrorMsg() + remark
		}
		if i.Value.BooleanValue() {
			return "[成功] " + layerStr + "if " + i.Exp + ": // " + i.Exp2 + remark
		}
		return "[失败] " + layerStr + "if " + i.Exp + ": // " + i.Exp2 + remark
	case LogicTypeSetFormula:
		if i.Value.IsError() {
			return "[赋值][错误] " + layerStr + i.Name + " = " + i.Exp + " = " + i.Exp2 + " // " + i.Value.ErrorMsg() + remark
		}
		return "[赋值] " + layerStr + i.Name + " = " + i.Exp + " = " + i.Exp2 + " = " + i.Value.ToText("").TextValue() + remark
	case LogicTypeSetValue:
		return "[赋值] " + layerStr + i.Name + " = " + i.Exp + remark
	case LogicTypeError:
		return "[错误] " + layerStr + i.Exp
	}
	return "[错误]: " + layerStr + i.Name + i.Exp + remark
}

// layerWidth 对应 C# Math.Min(Math.Max(0, Layer), 1000)。
func layerWidth(layer int) int {
	if layer < 0 {
		return 0
	}
	if layer > 1000 {
		return 1000
	}
	return layer
}
