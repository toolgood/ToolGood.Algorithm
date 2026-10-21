package value

import (
	"strings"

	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/operands"
)

// 移植自 Internals/Functions/Value/Function_ArrayJson.cs。

// FunctionArrayJson 键值集合字面量节点（`{ k: v, ... }`）。
type FunctionArrayJson struct{ BaseFunction }

func newFunctionARRAYJSON(args []Function) Function {
	return &FunctionArrayJson{BaseFunction: NewBase("ArrayJson", OperandTypeArrayJson, args)}
}

// NewFunctionArrayJson 构造键值集合节点（对应 C# 的 Function_ArrayJson(FunctionBase[])）。
func NewFunctionArrayJson(args []Function) Function { return newFunctionARRAYJSON(args) }

// Evaluate 组装键值集合；子节点必须产生键值对。
func (f *FunctionArrayJson) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	result := operands.NewOperandKeyValueList()
	for i := 0; i < f.ArgCount(); i++ {
		o := f.Arg(i).Evaluate(engine, tp)
		if o.IsErrorOrNone() {
			return o
		}
		kv, ok := o.(*keyValueOperand)
		if !ok {
			return f.ParameterError(1)
		}
		result.AddValue(kv.Value())
	}
	return result
}

// WriteTo 输出 `{ k: v, ... }`。
func (f *FunctionArrayJson) WriteTo(sb *strings.Builder, addBrackets bool) {
	sb.WriteByte('{')
	for i := 0; i < f.ArgCount(); i++ {
		if i > 0 {
			sb.WriteString(", ")
		}
		f.Arg(i).WriteTo(sb, false)
	}
	sb.WriteByte('}')
}

func (f *FunctionArrayJson) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	for i := 0; i < f.ArgCount(); i++ {
		f.Arg(i).GetParameterTypes(engine, result, OperandTypeNone)
	}
}

func init() { RegisterFunction("ARRAYJSON", newFunctionARRAYJSON) }
