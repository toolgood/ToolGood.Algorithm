package operator

import "strings"

// 移植自 Internals/Functions/Operator/Function_Mod.cs。

// FunctionMod 取模节点（Excel MOD 语义：结果符号随除数）。
type FunctionMod struct{ BaseFunction }

func newFunctionMOD(args []Function) Function {
	return &FunctionMod{BaseFunction: NewBase("%", OperandTypeNumber, args)}
}

// Evaluate 取模。
func (f *FunctionMod) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	b := f.GetNumber(2, engine, tp)
	if b.IsErrorOrNone() {
		return b
	}
	n1 := a.NumberValue()
	n2 := b.NumberValue()
	if n2.IsZero() {
		return f.Div0Error()
	}
	// Excel MOD 语义：n - d*INT(n/d)；Mod 的符号随被除数，需要修正。
	r := n1.Mod(n2)
	if !r.IsZero() && (r.Sign() < 0) != (n2.Sign() < 0) {
		r = r.Add(n2)
	}
	return CreateDecimal(r)
}

// WriteTo 输出 `a % b`。
func (f *FunctionMod) WriteTo(sb *strings.Builder, addBrackets bool) {
	if addBrackets {
		sb.WriteByte('(')
	}
	writeOperand(sb, f.Arg(0))
	sb.WriteString(" % ")
	writeOperand(sb, f.Arg(1))
	if addBrackets {
		sb.WriteByte(')')
	}
}

func (f *FunctionMod) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	setNumberArgs(&f.BaseFunction, engine, result)
}

func init() { RegisterFunction("MOD", newFunctionMOD) }
