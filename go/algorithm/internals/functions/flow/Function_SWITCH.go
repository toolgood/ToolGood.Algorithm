package flow

// 移植自 Internals/Functions/Flow/Function_SWITCH.cs。

// FunctionSWITCH 多分支节点。
type FunctionSWITCH struct{ BaseFunction }

func newFunctionSWITCH(args []Function) Function {
	RequireArgs(args, "Switch", 3, -1)
	return &FunctionSWITCH{BaseFunction: NewBase("Switch", OperandTypeNone, args)}
}

// Evaluate 依次比较候选值，命中时惰性求值对应结果；偶数个参数时最后一个是默认值。
func (f *FunctionSWITCH) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	exprValue := f.EvalArg(0, engine, tp)
	if exprValue.IsErrorOrNone() {
		return exprValue
	}

	i := 1
	for i < f.ArgCount()-1 {
		compareValue := f.Arg(i).Evaluate(engine, tp)
		if compareValue.IsErrorOrNone() {
			return compareValue
		}
		if equalsOperand(exprValue, compareValue) {
			return f.Arg(i+1).Evaluate(engine, tp)
		}
		i += 2
	}

	if f.ArgCount()%2 == 0 {
		return f.Arg(f.ArgCount()-1).Evaluate(engine, tp)
	}
	return f.FunctionError()
}

// ResultType 取第一个非 NONE 的结果分支类型（含默认值）。
func (f *FunctionSWITCH) ResultType() OperandType {
	i := 1
	for i < f.ArgCount()-1 {
		if t := f.Arg(i + 1).ResultType(); t != OperandTypeNone {
			return t
		}
		i += 2
	}
	if f.ArgCount()%2 == 0 {
		if t := f.Arg(f.ArgCount() - 1).ResultType(); t != OperandTypeNone {
			return t
		}
	}
	return OperandTypeNone
}

func (f *FunctionSWITCH) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	f.Arg(0).GetParameterTypes(engine, result, OperandTypeNone)
	i := 1
	for i < f.ArgCount()-1 {
		f.Arg(i).GetParameterTypes(engine, result, OperandTypeNone)
		f.Arg(i+1).GetParameterTypes(engine, result, OperandTypeNone)
		i += 2
	}
	if f.ArgCount()%2 == 0 {
		f.Arg(f.ArgCount()-1).GetParameterTypes(engine, result, OperandTypeNone)
	}
}

// equalsOperand 判断两个操作数是否匹配（对应 C# Function_SWITCH.EqualsOperand）。
func equalsOperand(a, b Operand) bool {
	if a.IsNumber() && b.IsNumber() {
		return a.NumberValue().Equal(b.NumberValue())
	}
	if a.IsText() && b.IsText() {
		return a.TextValue() == b.TextValue()
	}
	if a.IsBoolean() && b.IsBoolean() {
		return a.BooleanValue() == b.BooleanValue()
	}
	if a.IsDate() && b.IsDate() {
		return a.DateValue().ToLong() == b.DateValue().ToLong()
	}
	if a.IsNull() && b.IsNull() {
		return true
	}
	if a.IsNull() || b.IsNull() {
		return false
	}
	// 跨类型标量比较：与 == 运算符保持一致，统一转数字后再比较。
	// 日期/数组/Json 不参与跨类型比较；无法转成数字的组合同样视为不匹配。
	if a.IsDate() || b.IsDate() || a.IsArray() || b.IsArray() ||
		a.IsJson() || b.IsJson() || a.IsArrayJson() || b.IsArrayJson() {
		return false
	}
	na := a.ToNumber("")
	if na.IsErrorOrNone() {
		return false
	}
	nb := b.ToNumber("")
	if nb.IsErrorOrNone() {
		return false
	}
	return na.NumberValue().Equal(nb.NumberValue())
}

func init() { RegisterFunction("SWITCH", newFunctionSWITCH) }
