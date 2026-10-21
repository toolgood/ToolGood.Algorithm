package operator

import "strings"

// 移植自 Internals/Functions/Operator/Function_Add.cs。

// FunctionAdd 加法节点。
type FunctionAdd struct{ BaseFunction }

func newFunctionADD(args []Function) Function {
	return &FunctionAdd{BaseFunction: NewBase("+", OperandTypeNumber, args)}
}

// Evaluate 求和。
func (f *FunctionAdd) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	b := f.GetNumber(2, engine, tp)
	if b.IsErrorOrNone() {
		return b
	}
	r := a.NumberValue().Add(b.NumberValue())
	if exceedsDecimal(r) {
		return f.NumError()
	}
	return CreateDecimal(r)
}

// WriteTo 输出 `a + b`。
func (f *FunctionAdd) WriteTo(sb *strings.Builder, addBrackets bool) {
	if addBrackets {
		sb.WriteByte('(')
	}
	if fn := f.Arg(0); fn != nil {
		fn.WriteTo(sb, true)
	}
	sb.WriteString(" + ")
	if fn := f.Arg(1); fn != nil {
		fn.WriteTo(sb, true)
	}
	if addBrackets {
		sb.WriteByte(')')
	}
}

func (f *FunctionAdd) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	setNumberArgs(&f.BaseFunction, engine, result)
}

// writeOperand 以「需要括号」的方式输出子节点，对应 C# 的 `func.ToString(sb, true)`。
func writeOperand(sb *strings.Builder, fn Function) {
	if fn != nil {
		fn.WriteTo(sb, true)
	}
}

// setNumberArgs 两个参数都按数字传播。
func setNumberArgs(f *BaseFunction, engine *AlgorithmEngine, result *[]ParameterType) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
	if arg := f.Arg(1); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// setBooleanArgs 两个参数都按布尔传播。
func setBooleanArgs(f *BaseFunction, engine *AlgorithmEngine, result *[]ParameterType) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeBoolean)
	}
	if arg := f.Arg(1); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeBoolean)
	}
}

func init() { RegisterFunction("ADD", newFunctionADD) }
