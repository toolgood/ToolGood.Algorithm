package compare

import "strings"

// 移植自 Internals/Functions/Compare/Function_NE.cs。

// FunctionNE 不等比较节点。
type FunctionNE struct{ BaseFunction }

func newFunctionNE(args []Function) Function {
	return &FunctionNE{BaseFunction: NewBase("!=", OperandTypeBoolean, args)}
}

// Evaluate 比较两个操作数是否不相等。
func (f *FunctionNE) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	return evaluateCompare(&f.BaseFunction, compareNE, engine, tp)
}

// WriteTo 输出 `a != b`。
func (f *FunctionNE) WriteTo(sb *strings.Builder, addBrackets bool) {
	writeCompare(sb, &f.BaseFunction, addBrackets)
}

func (f *FunctionNE) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	compareGetParameterTypes(&f.BaseFunction, engine, result)
}

func init() { RegisterFunction("NE", newFunctionNE) }
