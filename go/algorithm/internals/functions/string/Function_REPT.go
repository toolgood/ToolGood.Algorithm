package string

import "strings"

// FunctionREPT 对应 C# Function_REPT：重复文本。
type FunctionREPT struct{ BaseFunction }

func newFunctionREPT(args []Function) Function {
	RequireArgs(args, "Rept", 2, 2)
	return &FunctionREPT{BaseFunction: NewBase("Rept", OperandTypeText, args)}
}

func (f *FunctionREPT) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	a2 := f.GetNumber(2, engine, tp)
	if a2.IsErrorOrNone() {
		return a2
	}
	newtext := a1.TextValue()
	length, ok := tryGetInt(a2)
	if !ok {
		return f.ParameterError(2)
	}
	if length < 0 {
		return f.ParameterError(2)
	}
	if length == 0 {
		return CreateString("")
	}
	textLength := len(toUnits(newtext))
	if textLength == 0 {
		return a1
	}
	if length > 32767/textLength {
		return f.ParameterError(2)
	}
	if length == 1 {
		return a1
	}
	return CreateString(strings.Repeat(newtext, length))
}

func init() { RegisterFunction("REPT", newFunctionREPT) }
