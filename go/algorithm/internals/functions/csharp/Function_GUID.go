package csharp

import (
	"crypto/rand"
	"encoding/hex"
	"strings"
)

// FunctionGUID 对应 C# Function_GUID：生成新的 GUID（小写、带连字符）。
type FunctionGUID struct{ BaseFunction }

func newFunctionGUID(args []Function) Function {
	RequireArgs(args, "Guid", 0, 0)
	return &FunctionGUID{BaseFunction: NewBase("Guid", OperandTypeText, args)}
}

func (f *FunctionGUID) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	return CreateString(newGUID())
}

// newGUID 生成与 Guid.NewGuid().ToString() 相同格式的字符串。
func newGUID() string {
	var b [16]byte
	if _, err := rand.Read(b[:]); err != nil {
		return ""
	}
	b[6] = (b[6] & 0x0f) | 0x40 // version 4
	b[8] = (b[8] & 0x3f) | 0x80 // variant 10
	var sb strings.Builder
	sb.WriteString(hex.EncodeToString(b[0:4]))
	sb.WriteByte('-')
	sb.WriteString(hex.EncodeToString(b[4:6]))
	sb.WriteByte('-')
	sb.WriteString(hex.EncodeToString(b[6:8]))
	sb.WriteByte('-')
	sb.WriteString(hex.EncodeToString(b[8:10]))
	sb.WriteByte('-')
	sb.WriteString(hex.EncodeToString(b[10:16]))
	return sb.String()
}

func init() { RegisterFunction("GUID", newFunctionGUID) }
