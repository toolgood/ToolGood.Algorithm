package csharpsecurity

import (
	"crypto/sha1"
	"encoding/hex"
	"strings"
)

// FunctionSHA1 对应 C# Function_SHA1：对 UTF-8 文本求 SHA1，输出大写十六进制。
type FunctionSHA1 struct{ BaseFunction }

func newFunctionSHA1(args []Function) Function {
	RequireArgs(args, "SHA1", 1, 1)
	return &FunctionSHA1{BaseFunction: NewBase("SHA1", OperandTypeText, args)}
}

func (f *FunctionSHA1) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	sum := sha1.Sum([]byte(a1.TextValue()))
	return CreateString(strings.ToUpper(hex.EncodeToString(sum[:])))
}

func init() { RegisterFunction("SHA1", newFunctionSHA1) }
