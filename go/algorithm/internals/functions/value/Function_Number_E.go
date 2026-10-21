package value

import "strings"

// 移植自 Internals/Functions/Value/Function_Number_E.cs。

// constE C# 的 MathEx.E（decimal 常量），按 decimal 的 28 位小数精度舍入。
var constE = MustDecimal("2.7182818284590452353602874713526624977572470936999595749").RoundHalfUp(28)

// FunctionNumberE 自然常数 E 节点。
type FunctionNumberE struct{ BaseFunction }

// NewFunctionNumberE 构造 E 节点。
func NewFunctionNumberE() Function {
	return &FunctionNumberE{BaseFunction: NewBase("E", OperandTypeNumber, nil)}
}

// Evaluate 返回 E。
func (f *FunctionNumberE) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	return CreateDecimal(constE)
}

// WriteTo 输出 E()。
func (f *FunctionNumberE) WriteTo(sb *strings.Builder, addBrackets bool) {
	sb.WriteString("E()")
}

func newFunctionE(args []Function) Function {
	RequireArgs(args, "E", 0, 0)
	return NewFunctionNumberE()
}

func init() { RegisterFunction("E", newFunctionE) }
