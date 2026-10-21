package csharpweb

// FunctionURLENCODE 对应 C# Function_URLENCODE：URL 百分号编码（空格编码为 '+'）。
type FunctionURLENCODE struct{ BaseFunction }

func newFunctionURLENCODE(args []Function) Function {
	RequireArgs(args, "UrlEncode", 1, 1)
	return &FunctionURLENCODE{BaseFunction: NewBase("UrlEncode", OperandTypeText, args)}
}

func (f *FunctionURLENCODE) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	return CreateString(urlEncode(a1.TextValue()))
}

func init() { RegisterFunction("URLENCODE", newFunctionURLENCODE) }
