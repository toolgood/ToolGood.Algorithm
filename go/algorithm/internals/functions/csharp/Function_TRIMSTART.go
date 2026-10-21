package csharp

import (
	"strings"
	"unicode"
)

// FunctionTRIMSTART 对应 C# Function_TRIMSTART：去掉开头空白或指定字符。
type FunctionTRIMSTART struct{ BaseFunction }

func newFunctionTRIMSTART(args []Function) Function {
	RequireArgs(args, "TrimStart", 1, 2)
	return &FunctionTRIMSTART{BaseFunction: NewBase("TrimStart", OperandTypeText, args)}
}

func (f *FunctionTRIMSTART) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	if f.ArgCount() < 2 {
		// C# string.TrimStart() 去掉所有 Unicode 空白字符
		return CreateString(strings.TrimLeftFunc(a1.TextValue(), unicode.IsSpace))
	}
	a2 := f.GetText(2, engine, tp)
	if a2.IsErrorOrNone() {
		return a2
	}
	return CreateString(strings.TrimLeft(a1.TextValue(), a2.TextValue()))
}

func init() { RegisterFunction("TRIMSTART", newFunctionTRIMSTART) }
