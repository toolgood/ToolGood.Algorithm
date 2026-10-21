package compare

import "strings"

// 移植自 Internals/Functions/Compare/Function_GE.cs。

// FunctionGE 大于等于比较节点。
type FunctionGE struct{ BaseFunction }

func newFunctionGE(args []Function) Function {
	return &FunctionGE{BaseFunction: NewBase(">=", OperandTypeBoolean, args)}
}

// Evaluate 判断左侧是否大于等于右侧。
func (f *FunctionGE) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	return evaluateCompare(&f.BaseFunction, compareGE, engine, tp)
}

// WriteTo 输出 `a >= b`。
func (f *FunctionGE) WriteTo(sb *strings.Builder, addBrackets bool) {
	writeCompare(sb, &f.BaseFunction, addBrackets)
}

func (f *FunctionGE) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	compareGetParameterTypes(&f.BaseFunction, engine, result)
}

func init() { RegisterFunction("GE", newFunctionGE) }
