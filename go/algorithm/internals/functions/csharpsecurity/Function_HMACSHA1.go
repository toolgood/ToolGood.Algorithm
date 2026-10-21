package csharpsecurity

import (
	"crypto/hmac"
	"crypto/sha1"
	"encoding/hex"
	"strings"
)

// FunctionHMACSHA1 对应 C# Function_HMACSHA1：HMAC-SHA1，输出大写十六进制。
type FunctionHMACSHA1 struct{ BaseFunction }

func newFunctionHMACSHA1(args []Function) Function {
	RequireArgs(args, "HmacSHA1", 2, 2)
	return &FunctionHMACSHA1{BaseFunction: NewBase("HmacSHA1", OperandTypeText, args)}
}

func (f *FunctionHMACSHA1) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	a2 := f.GetText(2, engine, tp)
	if a2.IsErrorOrNone() {
		return a2
	}
	mac := hmac.New(sha1.New, []byte(a2.TextValue()))
	mac.Write([]byte(a1.TextValue()))
	return CreateString(strings.ToUpper(hex.EncodeToString(mac.Sum(nil))))
}

func init() { RegisterFunction("HMACSHA1", newFunctionHMACSHA1) }
