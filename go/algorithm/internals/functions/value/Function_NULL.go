package value

import "strings"

// 移植自 Internals/Functions/Value/Function_NULL.cs。

// FunctionNull NULL 字面量节点。
type FunctionNull struct{ BaseFunction }

// NewFunctionNull 构造 NULL 节点。
func NewFunctionNull() Function {
	return &FunctionNull{BaseFunction: NewBase("NULL", OperandTypeNull, nil)}
}

// Evaluate 返回 Null。
func (f *FunctionNull) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	return Null
}

// WriteTo 输出 NULL。
func (f *FunctionNull) WriteTo(sb *strings.Builder, addBrackets bool) {
	sb.WriteString("NULL")
}

func newFunctionNULL(args []Function) Function {
	RequireArgs(args, "NULL", 0, 0)
	return NewFunctionNull()
}

func init() { RegisterFunction("NULL", newFunctionNULL) }
