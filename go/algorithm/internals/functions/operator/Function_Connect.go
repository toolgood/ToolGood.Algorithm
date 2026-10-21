package operator

import "strings"

// 移植自 Internals/Functions/Operator/Function_Connect.cs。

// FunctionConnect 字符串连接节点。
type FunctionConnect struct{ BaseFunction }

func newFunctionCONNECT(args []Function) Function {
	return &FunctionConnect{BaseFunction: NewBase("&", OperandTypeText, args)}
}

// Evaluate 连接两个文本。
func (f *FunctionConnect) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetText(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	b := f.GetText(2, engine, tp)
	if b.IsErrorOrNone() {
		return b
	}
	return CreateString(a.TextValue() + b.TextValue())
}

// WriteTo 输出 `a & b`。
func (f *FunctionConnect) WriteTo(sb *strings.Builder, addBrackets bool) {
	if addBrackets {
		sb.WriteByte('(')
	}
	if fn := f.Arg(0); fn != nil {
		fn.WriteTo(sb, true)
	}
	sb.WriteString(" & ")
	if fn := f.Arg(1); fn != nil {
		fn.WriteTo(sb, true)
	}
	if addBrackets {
		sb.WriteByte(')')
	}
}

func (f *FunctionConnect) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeText)
	}
	if arg := f.Arg(1); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeText)
	}
}

func init() { RegisterFunction("CONNECT", newFunctionCONNECT) }
