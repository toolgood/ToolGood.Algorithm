package csharp

import (
	"strings"
	"unicode"
)

// FunctionTRIMEND 对应 C# Function_TRIMEND：去掉结尾空白或指定字符。
type FunctionTRIMEND struct{ BaseFunction }

func newFunctionTRIMEND(args []Function) Function {
	RequireArgs(args, "TrimEnd", 1, 2)
	return &FunctionTRIMEND{BaseFunction: NewBase("TrimEnd", OperandTypeText, args)}
}

func (f *FunctionTRIMEND) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	if f.ArgCount() < 2 {
		// C# string.TrimEnd() 去掉所有 Unicode 空白字符
		return CreateString(strings.TrimRightFunc(a1.TextValue(), unicode.IsSpace))
	}
	a2 := f.GetText(2, engine, tp)
	if a2.IsErrorOrNone() {
		return a2
	}
	return CreateString(strings.TrimRight(a1.TextValue(), a2.TextValue()))
}

func init() { RegisterFunction("TRIMEND", newFunctionTRIMEND) }
