package operator

import "strings"

// 移植自 Internals/Functions/Operator/Function_AND.cs（二元 `&&`，由解析器使用，不注册）。

// FunctionAnd 二元逻辑与节点。
type FunctionAnd struct{ BaseFunction }

// NewFunctionAnd2 构造二元 `&&` 节点（对应 C# 的 Function_AND(FunctionBase, FunctionBase)）。
func NewFunctionAnd2(args []Function) Function {
	return &FunctionAnd{BaseFunction: NewBase("And", OperandTypeBoolean, args)}
}

// Evaluate 逻辑与：非严格模式下左侧为 false 时短路。
func (f *FunctionAnd) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetBoolean(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	if !a.BooleanValue() {
		if engine.UseStrictMode {
			if b := f.GetBoolean(2, engine, tp); b.IsErrorOrNone() {
				return b
			}
		}
		return False
	}
	return f.GetBoolean(2, engine, tp)
}

// WriteTo 输出 `a && b`。
func (f *FunctionAnd) WriteTo(sb *strings.Builder, addBrackets bool) {
	if addBrackets {
		sb.WriteByte('(')
	}
	if fn := f.Arg(0); fn != nil {
		fn.WriteTo(sb, true)
	}
	sb.WriteString(" && ")
	if fn := f.Arg(1); fn != nil {
		fn.WriteTo(sb, true)
	}
	if addBrackets {
		sb.WriteByte(')')
	}
}

func (f *FunctionAnd) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	setBooleanArgs(&f.BaseFunction, engine, result)
}
