package csharpweb

import "encoding/base64"

// FunctionTEXTTOBASE64URL 对应 C# Function_TEXTTOBASE64URL：
// 文本按 UTF-8 编码为 URL 安全型 Base64（'-'/'_' 替代 '+'/'/'，去掉结尾 '='）。
type FunctionTEXTTOBASE64URL struct{ BaseFunction }

func newFunctionTEXTTOBASE64URL(args []Function) Function {
	RequireArgs(args, "TextToBase64Url", 1, 1)
	return &FunctionTEXTTOBASE64URL{BaseFunction: NewBase("TextToBase64Url", OperandTypeText, args)}
}

func (f *FunctionTEXTTOBASE64URL) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	return CreateString(base64.RawURLEncoding.EncodeToString([]byte(a1.TextValue())))
}

func init() { RegisterFunction("TEXTTOBASE64URL", newFunctionTEXTTOBASE64URL) }
