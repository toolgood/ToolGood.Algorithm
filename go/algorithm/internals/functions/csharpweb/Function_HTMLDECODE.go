package csharpweb

// FunctionHTMLDECODE 对应 C# Function_HTMLDECODE：HTML 实体还原。
type FunctionHTMLDECODE struct{ BaseFunction }

func newFunctionHTMLDECODE(args []Function) Function {
	RequireArgs(args, "HtmlDecode", 1, 1)
	return &FunctionHTMLDECODE{BaseFunction: NewBase("HtmlDecode", OperandTypeText, args)}
}

func (f *FunctionHTMLDECODE) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	return CreateString(htmlDecode(a1.TextValue()))
}

func init() { RegisterFunction("HTMLDECODE", newFunctionHTMLDECODE) }
