package string

// FunctionLEN 对应 C# Function_LEN：返回 UTF-16 码元长度（与 C# string.Length 一致）。
type FunctionLEN struct{ BaseFunction }

func newFunctionLEN(args []Function) Function {
	RequireArgs(args, "Len", 1, 1)
	return &FunctionLEN{BaseFunction: NewBase("Len", OperandTypeNumber, args)}
}

func (f *FunctionLEN) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	return CreateInt(len(toUnits(a1.TextValue())))
}

func init() { RegisterFunction("LEN", newFunctionLEN) }
