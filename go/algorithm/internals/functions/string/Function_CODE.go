package string

// FunctionCODE 对应 C# Function_CODE：返回首字符的 UTF-16 码元值。
type FunctionCODE struct{ BaseFunction }

func newFunctionCODE(args []Function) Function {
	RequireArgs(args, "Code", 1, 1)
	return &FunctionCODE{BaseFunction: NewBase("Code", OperandTypeNumber, args)}
}

func (f *FunctionCODE) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetText(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	units := toUnits(a.TextValue())
	if len(units) == 0 {
		return f.ParameterError(1)
	}
	return CreateInt(int(units[0]))
}

func init() { RegisterFunction("CODE", newFunctionCODE) }
