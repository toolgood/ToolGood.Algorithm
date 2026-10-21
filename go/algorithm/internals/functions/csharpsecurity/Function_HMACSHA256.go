package csharpsecurity

import (
	"crypto/hmac"
	"crypto/sha256"
	"encoding/hex"
	"strings"
)

// FunctionHMACSHA256 对应 C# Function_HMACSHA256：HMAC-SHA256，输出大写十六进制。
type FunctionHMACSHA256 struct{ BaseFunction }

func newFunctionHMACSHA256(args []Function) Function {
	RequireArgs(args, "HmacSHA256", 2, 2)
	return &FunctionHMACSHA256{BaseFunction: NewBase("HmacSHA256", OperandTypeText, args)}
}

func (f *FunctionHMACSHA256) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	a2 := f.GetText(2, engine, tp)
	if a2.IsErrorOrNone() {
		return a2
	}
	mac := hmac.New(sha256.New, []byte(a2.TextValue()))
	mac.Write([]byte(a1.TextValue()))
	return CreateString(strings.ToUpper(hex.EncodeToString(mac.Sum(nil))))
}

func init() { RegisterFunction("HMACSHA256", newFunctionHMACSHA256) }
