package compare

import "strings"

// 移植自 Internals/Functions/Compare/Function_LE.cs。

// FunctionLE 小于等于比较节点。
type FunctionLE struct{ BaseFunction }

func newFunctionLE(args []Function) Function {
	return &FunctionLE{BaseFunction: NewBase("<=", OperandTypeBoolean, args)}
}

// Evaluate 判断左侧是否小于等于右侧。
func (f *FunctionLE) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	return evaluateCompare(&f.BaseFunction, compareLE, engine, tp)
}

// WriteTo 输出 `a <= b`。
func (f *FunctionLE) WriteTo(sb *strings.Builder, addBrackets bool) {
	writeCompare(sb, &f.BaseFunction, addBrackets)
}

func (f *FunctionLE) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	compareGetParameterTypes(&f.BaseFunction, engine, result)
}

func init() { RegisterFunction("LE", newFunctionLE) }
