package csharpweb

import "encoding/base64"

// FunctionTEXTTOBASE64 对应 C# Function_TEXTTOBASE64：文本按 UTF-8 编码为标准 Base64。
type FunctionTEXTTOBASE64 struct{ BaseFunction }

func newFunctionTEXTTOBASE64(args []Function) Function {
	RequireArgs(args, "TextToBase64", 1, 1)
	return &FunctionTEXTTOBASE64{BaseFunction: NewBase("TextToBase64", OperandTypeText, args)}
}

func (f *FunctionTEXTTOBASE64) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	return CreateString(base64.StdEncoding.EncodeToString([]byte(a1.TextValue())))
}

func init() { RegisterFunction("TEXTTOBASE64", newFunctionTEXTTOBASE64) }
