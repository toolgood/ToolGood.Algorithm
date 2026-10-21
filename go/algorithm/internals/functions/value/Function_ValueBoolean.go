package value

import "strings"

// 移植自 Internals/Functions/Value/Function_ValueBoolean.cs。

// FunctionValueBoolean 布尔字面量节点。
type FunctionValueBoolean struct {
	BaseFunction
	value bool
}

// NewFunctionValueBoolean 构造布尔字面量节点（对应 C# 的 Function_ValueBoolean(bool)）。
func NewFunctionValueBoolean(v bool) Function {
	name := "False"
	if v {
		name = "True"
	}
	return &FunctionValueBoolean{BaseFunction: NewBase(name, OperandTypeBoolean, nil), value: v}
}

// Name 返回 True / False。
func (f *FunctionValueBoolean) Name() string {
	if f.value {
		return "True"
	}
	return "False"
}

// Evaluate 返回布尔值。
func (f *FunctionValueBoolean) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	return CreateBool(f.value)
}

// WriteTo 输出 True / False。
func (f *FunctionValueBoolean) WriteTo(sb *strings.Builder, addBrackets bool) {
	sb.WriteString(f.Name())
}
