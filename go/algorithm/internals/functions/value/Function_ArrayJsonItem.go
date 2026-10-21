package value

import (
	"strings"

	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/operands"
)

// 移植自 Internals/Functions/Value/Function_ArrayJsonItem.cs。

// FunctionArrayJsonItem 键值集合中的单个成员节点（`k: v`）。
type FunctionArrayJsonItem struct {
	BaseFunction
	key string
}

// NewFunctionArrayJsonItem 构造成员节点（对应 C# 的 Function_ArrayJsonItem(string, FunctionBase)）。
func NewFunctionArrayJsonItem(key string, value Function) Function {
	return &FunctionArrayJsonItem{BaseFunction: NewBase("ArrayJsonItem", OperandTypeArrayJson, []Function{value}), key: key}
}

// Evaluate 求值成员值并包装为键值对。
func (f *FunctionArrayJsonItem) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	v := f.EvalArg(0, engine, tp)
	// 必须在此向上传播错误：若把错误操作数包裹进 KeyValue，外层 Function_ArrayJson 的检查会失效。
	if v.IsErrorOrNone() {
		return v
	}
	return newKeyValueOperand(operands.NewKeyValue(f.key, v))
}

// WriteTo 输出 `"key": 值`。
func (f *FunctionArrayJsonItem) WriteTo(sb *strings.Builder, addBrackets bool) {
	sb.WriteString(CreateString(f.key).String())
	sb.WriteByte(':')
	if value := f.Arg(0); value != nil {
		value.WriteTo(sb, false)
	}
}

func (f *FunctionArrayJsonItem) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if value := f.Arg(0); value != nil {
		value.GetParameterTypes(engine, result, OperandTypeNone)
	}
}

// keyValueOperand 单个键值对操作数（对应 C# 的 OperandKeyValue）。
//
// operands.OperandKeyValue 的值字段不可导出、也没有构造函数，且未实现 Operand 接口
// 要求的 String()，无法在包外构造；这里用等价的本地实现，仅在本包内使用。
type keyValueOperand struct {
	Operand
	keyValue *KeyValue
}

func newKeyValueOperand(kv *KeyValue) Operand {
	return &keyValueOperand{Operand: Null, keyValue: kv}
}

// Type 返回 ARRAYJSON。
func (o *keyValueOperand) Type() OperandType { return OperandTypeArrayJson }

// IsArrayJson 是否键值集合成员。
func (o *keyValueOperand) IsArrayJson() bool { return true }

// Value 返回键值对。
func (o *keyValueOperand) Value() *KeyValue { return o.keyValue }

// String 输出 JSON 风格的 `"key":value`。
func (o *keyValueOperand) String() string {
	var sb strings.Builder
	sb.WriteString(CreateString(o.keyValue.Key).String())
	sb.WriteByte(':')
	sb.WriteString(o.keyValue.Value.String())
	return sb.String()
}

// ToText 输出 JSON 风格的 `"key":value`。
func (o *keyValueOperand) ToText(msg string) Operand { return CreateString(o.String()) }
