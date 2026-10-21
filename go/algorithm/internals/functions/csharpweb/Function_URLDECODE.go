package csharpweb

// FunctionURLDECODE 对应 C# Function_URLDECODE：URL 百分号解码（'+' 还原为空格）。
type FunctionURLDECODE struct{ BaseFunction }

func newFunctionURLDECODE(args []Function) Function {
	RequireArgs(args, "UrlDecode", 1, 1)
	return &FunctionURLDECODE{BaseFunction: NewBase("UrlDecode", OperandTypeText, args)}
}

func (f *FunctionURLDECODE) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	return CreateString(urlDecode(a1.TextValue()))
}

func init() { RegisterFunction("URLDECODE", newFunctionURLDECODE) }
