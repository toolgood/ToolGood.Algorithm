package compare

import "strings"

// 移植自 Internals/Functions/Compare/Function_EQ.cs。

// FunctionEQ 相等比较节点。
type FunctionEQ struct{ BaseFunction }

func newFunctionEQ(args []Function) Function {
	return &FunctionEQ{BaseFunction: NewBase("==", OperandTypeBoolean, args)}
}

// Evaluate 比较两个操作数是否相等。
func (f *FunctionEQ) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	return evaluateCompare(&f.BaseFunction, compareEQ, engine, tp)
}

// WriteTo 输出 `a == b`。
func (f *FunctionEQ) WriteTo(sb *strings.Builder, addBrackets bool) {
	writeCompare(sb, &f.BaseFunction, addBrackets)
}

func (f *FunctionEQ) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	compareGetParameterTypes(&f.BaseFunction, engine, result)
}

func init() { RegisterFunction("EQ", newFunctionEQ) }
