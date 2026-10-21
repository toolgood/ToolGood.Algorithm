package csharpsecurity

import (
	"crypto/sha256"
	"encoding/hex"
	"strings"
)

// FunctionSHA256 对应 C# Function_SHA256：对 UTF-8 文本求 SHA256，输出大写十六进制。
type FunctionSHA256 struct{ BaseFunction }

func newFunctionSHA256(args []Function) Function {
	RequireArgs(args, "SHA256", 1, 1)
	return &FunctionSHA256{BaseFunction: NewBase("SHA256", OperandTypeText, args)}
}

func (f *FunctionSHA256) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	sum := sha256.Sum256([]byte(a1.TextValue()))
	return CreateString(strings.ToUpper(hex.EncodeToString(sum[:])))
}

func init() { RegisterFunction("SHA256", newFunctionSHA256) }
