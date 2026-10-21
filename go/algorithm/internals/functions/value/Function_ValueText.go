package value

import "strings"

// 移植自 Internals/Functions/Value/Function_ValueText.cs。

// FunctionValueText 字符串字面量节点。
type FunctionValueText struct {
	BaseFunction
	value Operand
}

// NewFunctionValueText 构造字符串字面量节点（对应 C# 的 Function_ValueText(Operand)）。
func NewFunctionValueText(v Operand) Function {
	return &FunctionValueText{BaseFunction: NewBase("Value", OperandTypeText, nil), value: v}
}

// Evaluate 返回字面量值。
func (f *FunctionValueText) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	return f.value
}

// WriteTo 输出带引号并转义的字符串。
func (f *FunctionValueText) WriteTo(sb *strings.Builder, addBrackets bool) {
	sb.WriteByte('"')
	for _, c := range f.value.TextValue() {
		switch c {
		case '\\':
			sb.WriteString("\\\\")
		case '"':
			sb.WriteString("\\\"")
		case '\r':
			sb.WriteString("\\r")
		case '\n':
			sb.WriteString("\\n")
		case '\t':
			sb.WriteString("\\t")
		case '\x00':
			sb.WriteString("\\0")
		case '\v':
			sb.WriteString("\\v")
		case '\a':
			sb.WriteString("\\a")
		case '\b':
			sb.WriteString("\\b")
		case '\f':
			sb.WriteString("\\f")
		default:
			sb.WriteRune(c)
		}
	}
	sb.WriteByte('"')
}
