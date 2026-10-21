package csharp

import "strings"

// FunctionJOIN 对应 C# Function_JOIN：把数组（或后续参数）用分隔符拼接为文本。
type FunctionJOIN struct{ BaseFunction }

func newFunctionJOIN(args []Function) Function {
	RequireArgs(args, "Join", 2, -1)
	return &FunctionJOIN{BaseFunction: NewBase("Join", OperandTypeText, args)}
}

func (f *FunctionJOIN) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args := make([]Operand, 0, f.ArgCount())
	if err := f.EvaluateAll(engine, tp, &args); err != nil {
		return err
	}
	a1 := args[0]
	if a1.IsJson() {
		if converted := a1.ToArray(""); !converted.IsErrorOrNone() {
			a1 = converted
		}
	}
	if a1.IsArray() {
		var list []string
		if !flattenToStrings(a1, &list) {
			return f.ParameterError(1)
		}
		a2 := f.ConvertToText(args[1], 2)
		if a2.IsErrorOrNone() {
			return a2
		}
		return CreateString(strings.Join(list, a2.TextValue()))
	}
	a1 = f.ConvertToText(a1, 1)
	if a1.IsErrorOrNone() {
		return a1
	}
	var list []string
	for i := 1; i < len(args); i++ {
		if !flattenToStrings(args[i], &list) {
			return f.ParameterError(i + 1)
		}
	}
	return CreateString(strings.Join(list, a1.TextValue()))
}

func init() { RegisterFunction("JOIN", newFunctionJOIN) }
