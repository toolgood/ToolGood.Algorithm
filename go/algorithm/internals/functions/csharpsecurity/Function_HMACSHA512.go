package csharpsecurity

import (
	"crypto/hmac"
	"crypto/sha512"
	"encoding/hex"
	"strings"
)

// FunctionHMACSHA512 对应 C# Function_HMACSHA512：HMAC-SHA512，输出大写十六进制。
type FunctionHMACSHA512 struct{ BaseFunction }

func newFunctionHMACSHA512(args []Function) Function {
	RequireArgs(args, "HmacSHA512", 2, 2)
	return &FunctionHMACSHA512{BaseFunction: NewBase("HmacSHA512", OperandTypeText, args)}
}

func (f *FunctionHMACSHA512) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	a2 := f.GetText(2, engine, tp)
	if a2.IsErrorOrNone() {
		return a2
	}
	mac := hmac.New(sha512.New, []byte(a2.TextValue()))
	mac.Write([]byte(a1.TextValue()))
	return CreateString(strings.ToUpper(hex.EncodeToString(mac.Sum(nil))))
}

func init() { RegisterFunction("HMACSHA512", newFunctionHMACSHA512) }
