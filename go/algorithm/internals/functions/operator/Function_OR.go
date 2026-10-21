package operator

import "strings"

// 移植自 Internals/Functions/Operator/Function_OR.cs（二元 `||`，由解析器使用，不注册）。

// FunctionOr 二元逻辑或节点。
type FunctionOr struct{ BaseFunction }

// NewFunctionOr2 构造二元 `||` 节点（对应 C# 的 Function_OR(FunctionBase, FunctionBase)）。
func NewFunctionOr2(args []Function) Function {
	return &FunctionOr{BaseFunction: NewBase("Or", OperandTypeBoolean, args)}
}

// Evaluate 逻辑或：非严格模式下左侧为 true 时短路。
func (f *FunctionOr) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetBoolean(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	if a.BooleanValue() {
		if engine.UseStrictMode {
			if b := f.GetBoolean(2, engine, tp); b.IsErrorOrNone() {
				return b
			}
		}
		return True
	}
	return f.GetBoolean(2, engine, tp)
}

// WriteTo 输出 `a || b`。
func (f *FunctionOr) WriteTo(sb *strings.Builder, addBrackets bool) {
	if addBrackets {
		sb.WriteByte('(')
	}
	writeOperand(sb, f.Arg(0))
	sb.WriteString(" || ")
	writeOperand(sb, f.Arg(1))
	if addBrackets {
		sb.WriteByte(')')
	}
}

func (f *FunctionOr) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	setBooleanArgs(&f.BaseFunction, engine, result)
}
