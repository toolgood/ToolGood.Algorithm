package compare

import "strings"

// 移植自 Internals/Functions/Compare/Function_GT.cs。

// FunctionGT 大于比较节点。
type FunctionGT struct{ BaseFunction }

func newFunctionGT(args []Function) Function {
	return &FunctionGT{BaseFunction: NewBase(">", OperandTypeBoolean, args)}
}

// Evaluate 判断左侧是否大于右侧。
func (f *FunctionGT) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	return evaluateCompare(&f.BaseFunction, compareGT, engine, tp)
}

// WriteTo 输出 `a > b`。
func (f *FunctionGT) WriteTo(sb *strings.Builder, addBrackets bool) {
	writeCompare(sb, &f.BaseFunction, addBrackets)
}

func (f *FunctionGT) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	compareGetParameterTypes(&f.BaseFunction, engine, result)
}

func init() { RegisterFunction("GT", newFunctionGT) }
