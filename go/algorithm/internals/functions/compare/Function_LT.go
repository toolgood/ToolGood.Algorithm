package compare

import "strings"

// 移植自 Internals/Functions/Compare/Function_LT.cs。

// FunctionLT 小于比较节点。
type FunctionLT struct{ BaseFunction }

func newFunctionLT(args []Function) Function {
	return &FunctionLT{BaseFunction: NewBase("<", OperandTypeBoolean, args)}
}

// Evaluate 判断左侧是否小于右侧。
func (f *FunctionLT) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	return evaluateCompare(&f.BaseFunction, compareLT, engine, tp)
}

// WriteTo 输出 `a < b`。
func (f *FunctionLT) WriteTo(sb *strings.Builder, addBrackets bool) {
	writeCompare(sb, &f.BaseFunction, addBrackets)
}

func (f *FunctionLT) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	compareGetParameterTypes(&f.BaseFunction, engine, result)
}

func init() { RegisterFunction("LT", newFunctionLT) }
