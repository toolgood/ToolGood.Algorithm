package csharp

// FunctionENDSWITH 对应 C# Function_ENDSWITH：判断是否以指定文本结尾。
type FunctionENDSWITH struct{ BaseFunction }

func newFunctionENDSWITH(args []Function) Function {
	RequireArgs(args, "EndsWith", 2, 3)
	return &FunctionENDSWITH{BaseFunction: NewBase("EndsWith", OperandTypeBoolean, args)}
}

func (f *FunctionENDSWITH) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	a2 := f.GetText(2, engine, tp)
	if a2.IsErrorOrNone() {
		return a2
	}
	if f.ArgCount() < 3 {
		return CreateBool(endsWith(a1.TextValue(), a2.TextValue(), false))
	}
	a3 := f.GetBoolean(3, engine, tp)
	if a3.IsErrorOrNone() {
		return a3
	}
	return CreateBool(endsWith(a1.TextValue(), a2.TextValue(), a3.BooleanValue()))
}

func init() { RegisterFunction("ENDSWITH", newFunctionENDSWITH) }
