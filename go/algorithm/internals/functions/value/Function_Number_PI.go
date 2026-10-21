package value

import "strings"

// 移植自 Internals/Functions/Value/Function_Number_PI.cs。

// constPI C# 的 MathEx.PI（decimal 常量），按 decimal 的 28 位小数精度舍入。
var constPI = MustDecimal("3.14159265358979323846264338327950288419716939937510").RoundHalfUp(28)

// FunctionNumberPI 圆周率 PI 节点。
type FunctionNumberPI struct{ BaseFunction }

// NewFunctionNumberPI 构造 PI 节点。
func NewFunctionNumberPI() Function {
	return &FunctionNumberPI{BaseFunction: NewBase("PI", OperandTypeNumber, nil)}
}

// Evaluate 返回 PI。
func (f *FunctionNumberPI) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	return CreateDecimal(constPI)
}

// WriteTo 输出 PI()。
func (f *FunctionNumberPI) WriteTo(sb *strings.Builder, addBrackets bool) {
	sb.WriteString("PI()")
}

func newFunctionPI(args []Function) Function {
	RequireArgs(args, "PI", 0, 0)
	return NewFunctionNumberPI()
}

func init() { RegisterFunction("PI", newFunctionPI) }
