package csharpsecurity

import (
	"crypto/md5"
	"encoding/hex"
	"strings"
)

// FunctionMD5 对应 C# Function_MD5：对 UTF-8 文本求 MD5，输出大写十六进制。
type FunctionMD5 struct{ BaseFunction }

func newFunctionMD5(args []Function) Function {
	RequireArgs(args, "MD5", 1, 1)
	return &FunctionMD5{BaseFunction: NewBase("MD5", OperandTypeText, args)}
}

func (f *FunctionMD5) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	sum := md5.Sum([]byte(a1.TextValue()))
	return CreateString(strings.ToUpper(hex.EncodeToString(sum[:])))
}

func init() { RegisterFunction("MD5", newFunctionMD5) }
