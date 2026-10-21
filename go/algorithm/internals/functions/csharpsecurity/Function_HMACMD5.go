package csharpsecurity

import (
	"crypto/hmac"
	"crypto/md5"
	"encoding/hex"
	"strings"
)

// FunctionHMACMD5 对应 C# Function_HMACMD5：HMAC-MD5，输出大写十六进制。
type FunctionHMACMD5 struct{ BaseFunction }

func newFunctionHMACMD5(args []Function) Function {
	RequireArgs(args, "HmacMD5", 2, 2)
	return &FunctionHMACMD5{BaseFunction: NewBase("HmacMD5", OperandTypeText, args)}
}

func (f *FunctionHMACMD5) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	a2 := f.GetText(2, engine, tp)
	if a2.IsErrorOrNone() {
		return a2
	}
	mac := hmac.New(md5.New, []byte(a2.TextValue()))
	mac.Write([]byte(a1.TextValue()))
	return CreateString(strings.ToUpper(hex.EncodeToString(mac.Sum(nil))))
}

func init() { RegisterFunction("HMACMD5", newFunctionHMACMD5) }
