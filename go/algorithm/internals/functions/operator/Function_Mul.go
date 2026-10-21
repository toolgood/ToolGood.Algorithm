package operator

import "strings"

// 移植自 Internals/Functions/Operator/Function_Mul.cs。

// FunctionMul 乘法节点。
type FunctionMul struct{ BaseFunction }

func newFunctionMUL(args []Function) Function {
	return &FunctionMul{BaseFunction: NewBase("*", OperandTypeNumber, args)}
}

// Evaluate 求积。
func (f *FunctionMul) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	b := f.GetNumber(2, engine, tp)
	if b.IsErrorOrNone() {
		return b
	}
	if a.NumberValue().Equal(DecimalOne) {
		return b
	}
	if b.NumberValue().Equal(DecimalOne) {
		return a
	}
	r := a.NumberValue().Mul(b.NumberValue())
	if exceedsDecimal(r) {
		return f.NumError()
	}
	return CreateDecimal(r)
}

// WriteTo 输出 `a * b`。
func (f *FunctionMul) WriteTo(sb *strings.Builder, addBrackets bool) {
	if addBrackets {
		sb.WriteByte('(')
	}
	writeOperand(sb, f.Arg(0))
	sb.WriteString(" * ")
	writeOperand(sb, f.Arg(1))
	if addBrackets {
		sb.WriteByte(')')
	}
}

func (f *FunctionMul) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	setNumberArgs(&f.BaseFunction, engine, result)
}

func init() { RegisterFunction("MUL", newFunctionMUL) }
