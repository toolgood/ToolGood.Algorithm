package csharp

// FunctionREGEXREPLACE 对应 C# Function_REGEXREPLACE：正则替换。
type FunctionREGEXREPLACE struct{ BaseFunction }

func newFunctionREGEXREPLACE(args []Function) Function {
	RequireArgs(args, "RegexReplace", 3, 3)
	return &FunctionREGEXREPLACE{BaseFunction: NewBase("RegexReplace", OperandTypeText, args)}
}

func (f *FunctionREGEXREPLACE) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	a2 := f.GetText(2, engine, tp)
	if a2.IsErrorOrNone() {
		return a2
	}
	a3 := f.GetText(3, engine, tp)
	if a3.IsErrorOrNone() {
		return a3
	}
	re, err := getRegex(a2.TextValue())
	if err != nil {
		return f.ParameterError(2)
	}
	return CreateString(re.ReplaceAllString(a1.TextValue(), translateReplacement(a3.TextValue())))
}

func init() { RegisterFunction("REGEXREPLACE", newFunctionREGEXREPLACE) }
