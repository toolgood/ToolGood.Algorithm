package csharp

import "strings"

// FunctionSPLIT 对应 C# Function_SPLIT：按分隔符切分为数组。
type FunctionSPLIT struct{ BaseFunction }

func newFunctionSPLIT(args []Function) Function {
	RequireArgs(args, "Split", 2, 2)
	return &FunctionSPLIT{BaseFunction: NewBase("Split", OperandTypeArray, args)}
}

func (f *FunctionSPLIT) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	a2 := f.GetText(2, engine, tp)
	if a2.IsErrorOrNone() {
		return a2
	}
	separator := a2.TextValue()
	if separator == "" {
		return f.ParameterError(2)
	}
	parts := strings.Split(a1.TextValue(), separator)
	result := make([]Operand, len(parts))
	for i, part := range parts {
		result[i] = CreateString(part)
	}
	return CreateArray(result)
}

func init() { RegisterFunction("SPLIT", newFunctionSPLIT) }
