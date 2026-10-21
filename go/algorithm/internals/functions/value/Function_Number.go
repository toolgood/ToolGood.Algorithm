package value

import "strings"

// 移植自 Internals/Functions/Value/Function_Number.cs。

// FunctionNumber 数字字面量节点。
type FunctionNumber struct {
	BaseFunction
	value Operand
}

// NewFunctionNumber 构造数字字面量节点（对应 C# 的 Function_Number(Operand)）。
func NewFunctionNumber(v Operand) Function {
	return &FunctionNumber{BaseFunction: NewBase("Num", OperandTypeNumber, nil), value: v}
}

// Evaluate 返回字面量值。
func (f *FunctionNumber) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	return f.value
}

// WriteTo 输出数字文本（等价 InvariantCulture 下的 decimal.ToString）。
func (f *FunctionNumber) WriteTo(sb *strings.Builder, addBrackets bool) {
	sb.WriteString(f.value.NumberValue().String())
}
