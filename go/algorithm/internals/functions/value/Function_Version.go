package value

import "strings"

// 移植自 Internals/Functions/Value/Function_Version.cs。

// FunctionVersion 算法版本节点。
type FunctionVersion struct{ BaseFunction }

// NewFunctionVersion 构造版本节点。
func NewFunctionVersion() Function {
	return &FunctionVersion{BaseFunction: NewBase("ALGORITHMVERSION", OperandTypeText, nil)}
}

// Evaluate 返回版本号。
func (f *FunctionVersion) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	return Version
}

// WriteTo 输出 ALGORITHMVERSION。
func (f *FunctionVersion) WriteTo(sb *strings.Builder, addBrackets bool) {
	sb.WriteString(f.Name())
}

func newFunctionVersion(args []Function) Function {
	RequireArgs(args, "ALGORITHMVERSION", 0, 0)
	return NewFunctionVersion()
}

func init() { RegisterFunction("ALGORITHMVERSION", newFunctionVersion) }
