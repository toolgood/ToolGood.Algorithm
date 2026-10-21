package string

import (
	"strings"
	"unicode"
)

// FunctionLOWER 对应 C# Function_LOWER：转小写（不变文化）。
type FunctionLOWER struct{ BaseFunction }

func newFunctionLOWER(args []Function) Function {
	RequireArgs(args, "Lower", 1, 1)
	return &FunctionLOWER{BaseFunction: NewBase("Lower", OperandTypeText, args)}
}

func (f *FunctionLOWER) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	text := a1.TextValue()
	for _, c := range text {
		if unicode.IsUpper(c) {
			return CreateString(strings.ToLower(text))
		}
	}
	return a1
}

func init() { RegisterFunction("LOWER", newFunctionLOWER) }
