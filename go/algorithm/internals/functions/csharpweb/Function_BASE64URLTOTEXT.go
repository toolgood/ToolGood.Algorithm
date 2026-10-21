package csharpweb

import (
	"encoding/base64"
	"strings"
)

// FunctionBASE64URLTOTEXT 对应 C# Function_BASE64URLTOTEXT：
// URL 安全型 Base64（'-'/'_' 等价于 '+'/'/'，结尾 '=' 可省略）按 UTF-8 解码。
type FunctionBASE64URLTOTEXT struct{ BaseFunction }

func newFunctionBASE64URLTOTEXT(args []Function) Function {
	RequireArgs(args, "Base64UrlToText", 1, 1)
	return &FunctionBASE64URLTOTEXT{BaseFunction: NewBase("Base64UrlToText", OperandTypeText, args)}
}

func (f *FunctionBASE64URLTOTEXT) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	text := strings.ReplaceAll(a1.TextValue(), "-", "+")
	text = strings.ReplaceAll(text, "_", "/")
	if padding := 4 - len(text)%4; padding < 4 {
		text += strings.Repeat("=", padding)
	}
	bytes, err := base64.StdEncoding.DecodeString(text)
	if err != nil {
		return f.ParameterError(1)
	}
	return CreateString(string(bytes))
}

func init() { RegisterFunction("BASE64URLTOTEXT", newFunctionBASE64URLTOTEXT) }
