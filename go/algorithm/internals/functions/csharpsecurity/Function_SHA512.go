package csharpsecurity

import (
	"crypto/sha512"
	"encoding/hex"
	"strings"
)

// FunctionSHA512 对应 C# Function_SHA512：对 UTF-8 文本求 SHA512，输出大写十六进制。
type FunctionSHA512 struct{ BaseFunction }

func newFunctionSHA512(args []Function) Function {
	RequireArgs(args, "SHA512", 1, 1)
	return &FunctionSHA512{BaseFunction: NewBase("SHA512", OperandTypeText, args)}
}

func (f *FunctionSHA512) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	sum := sha512.Sum512([]byte(a1.TextValue()))
	return CreateString(strings.ToUpper(hex.EncodeToString(sum[:])))
}

func init() { RegisterFunction("SHA512", newFunctionSHA512) }
