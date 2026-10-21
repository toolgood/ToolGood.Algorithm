package csharp

// FunctionSTARTSWITH 对应 C# Function_STARTSWITH：判断是否以指定文本开头。
type FunctionSTARTSWITH struct{ BaseFunction }

func newFunctionSTARTSWITH(args []Function) Function {
	RequireArgs(args, "StartsWith", 2, 3)
	return &FunctionSTARTSWITH{BaseFunction: NewBase("StartsWith", OperandTypeBoolean, args)}
}

func (f *FunctionSTARTSWITH) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	a2 := f.GetText(2, engine, tp)
	if a2.IsErrorOrNone() {
		return a2
	}
	if f.ArgCount() < 3 {
		return CreateBool(startsWith(a1.TextValue(), a2.TextValue(), false))
	}
	a3 := f.GetBoolean(3, engine, tp)
	if a3.IsErrorOrNone() {
		return a3
	}
	return CreateBool(startsWith(a1.TextValue(), a2.TextValue(), a3.BooleanValue()))
}

func init() { RegisterFunction("STARTSWITH", newFunctionSTARTSWITH) }
