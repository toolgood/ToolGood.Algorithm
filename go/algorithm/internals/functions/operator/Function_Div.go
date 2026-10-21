package operator

import "strings"

// 移植自 Internals/Functions/Operator/Function_Div.cs。

// FunctionDiv 除法节点。
type FunctionDiv struct{ BaseFunction }

func newFunctionDIV(args []Function) Function {
	return &FunctionDiv{BaseFunction: NewBase("/", OperandTypeNumber, args)}
}

// Evaluate 求商。
func (f *FunctionDiv) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	b := f.GetNumber(2, engine, tp)
	if b.IsErrorOrNone() {
		return b
	}
	if b.NumberValue().IsZero() {
		return f.Div0Error()
	}
	if b.NumberValue().Equal(DecimalOne) {
		return a
	}
	r := a.NumberValue().Div(b.NumberValue())
	if exceedsDecimal(r) {
		return f.NumError()
	}
	return CreateDecimal(r)
}

// WriteTo 输出 `a / b`。
func (f *FunctionDiv) WriteTo(sb *strings.Builder, addBrackets bool) {
	if addBrackets {
		sb.WriteByte('(')
	}
	writeOperand(sb, f.Arg(0))
	sb.WriteString(" / ")
	writeOperand(sb, f.Arg(1))
	if addBrackets {
		sb.WriteByte(')')
	}
}

func (f *FunctionDiv) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	setNumberArgs(&f.BaseFunction, engine, result)
}

func init() { RegisterFunction("DIV", newFunctionDIV) }
