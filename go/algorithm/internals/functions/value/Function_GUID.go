package value

import (
	"crypto/rand"
	"encoding/hex"
	"strings"
)

// 移植自 Internals/Functions/Csharp/Function_GUID.cs（解析器把它当作 value 组的常量节点使用）。

// FunctionGUID GUID 生成节点。
type FunctionGUID struct{ BaseFunction }

// NewFunctionGUID 构造 GUID 节点。
func NewFunctionGUID() Function {
	return &FunctionGUID{BaseFunction: NewBase("Guid", OperandTypeText, nil)}
}

// Evaluate 生成新的 GUID（小写、带连字符）。
func (f *FunctionGUID) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	return CreateString(newGUID())
}

// WriteTo 输出 Guid()。
func (f *FunctionGUID) WriteTo(sb *strings.Builder, addBrackets bool) {
	sb.WriteString("Guid()")
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
