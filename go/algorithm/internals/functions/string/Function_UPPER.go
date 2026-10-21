package string

import (
	"strings"
	"unicode"
)

// FunctionUPPER 对应 C# Function_UPPER：转大写（不变文化）。
type FunctionUPPER struct{ BaseFunction }

func newFunctionUPPER(args []Function) Function {
	RequireArgs(args, "Upper", 1, 1)
	return &FunctionUPPER{BaseFunction: NewBase("Upper", OperandTypeText, args)}
}

func (f *FunctionUPPER) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	text := a1.TextValue()
	for _, c := range text {
		if unicode.IsLower(c) {
			return CreateString(strings.ToUpper(text))
		}
	}
	return a1
}

func init() { RegisterFunction("UPPER", newFunctionUPPER) }
