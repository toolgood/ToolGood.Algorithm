package csharpweb

import (
	"encoding/base64"
	"strings"
)

// FunctionBASE64TOTEXT 对应 C# Function_BASE64TOTEXT：标准 Base64 按 UTF-8 解码为文本。
type FunctionBASE64TOTEXT struct{ BaseFunction }

func newFunctionBASE64TOTEXT(args []Function) Function {
	RequireArgs(args, "Base64ToText", 1, 1)
	return &FunctionBASE64TOTEXT{BaseFunction: NewBase("Base64ToText", OperandTypeText, args)}
}

func (f *FunctionBASE64TOTEXT) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	// Convert.FromBase64String 会忽略空白字符，这里同样先剔除
	text := stripBase64Whitespace(a1.TextValue())
	bytes, err := base64.StdEncoding.DecodeString(text)
	if err != nil {
		return f.ParameterError(1)
	}
	return CreateString(string(bytes))
}

// stripBase64Whitespace 去掉 Base64 字符串中的空白字符（.NET 的行为）。
func stripBase64Whitespace(s string) string {
	if !strings.ContainsAny(s, " \t\r\n\v\f") {
		return s
	}
	var sb strings.Builder
	sb.Grow(len(s))
	for i := 0; i < len(s); i++ {
		switch s[i] {
		case ' ', '\t', '\r', '\n', '\v', '\f':
		default:
			sb.WriteByte(s[i])
		}
	}
	return sb.String()
}

func init() { RegisterFunction("BASE64TOTEXT", newFunctionBASE64TOTEXT) }
