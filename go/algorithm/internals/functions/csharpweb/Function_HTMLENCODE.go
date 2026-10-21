package csharpweb

// FunctionHTMLENCODE 对应 C# Function_HTMLENCODE：HTML 转义。
type FunctionHTMLENCODE struct{ BaseFunction }

func newFunctionHTMLENCODE(args []Function) Function {
	RequireArgs(args, "HtmlEncode", 1, 1)
	return &FunctionHTMLENCODE{BaseFunction: NewBase("HtmlEncode", OperandTypeText, args)}
}

func (f *FunctionHTMLENCODE) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	return CreateString(htmlEncode(a1.TextValue()))
}

func init() { RegisterFunction("HTMLENCODE", newFunctionHTMLENCODE) }
