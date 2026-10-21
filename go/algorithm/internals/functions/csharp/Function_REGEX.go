package csharp

// FunctionREGEX 对应 C# Function_REGEX：返回第一个匹配到的文本。
type FunctionREGEX struct{ BaseFunction }

func newFunctionREGEX(args []Function) Function {
	RequireArgs(args, "Regex", 2, 2)
	return &FunctionREGEX{BaseFunction: NewBase("Regex", OperandTypeText, args)}
}

func (f *FunctionREGEX) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	a2 := f.GetText(2, engine, tp)
	if a2.IsErrorOrNone() {
		return a2
	}
	re, err := getRegex(a2.TextValue())
	if err != nil {
		return f.ParameterError(2)
	}
	text := a1.TextValue()
	loc := re.FindStringIndex(text)
	if loc == nil {
		return f.FunctionError()
	}
	return CreateString(text[loc[0]:loc[1]])
}

func init() { RegisterFunction("REGEX", newFunctionREGEX) }
