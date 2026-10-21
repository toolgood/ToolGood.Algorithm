package string

// FunctionLEFT 对应 C# Function_LEFT：取左侧若干字符。
type FunctionLEFT struct{ BaseFunction }

func newFunctionLEFT(args []Function) Function {
	RequireArgs(args, "Left", 1, 2)
	return &FunctionLEFT{BaseFunction: NewBase("Left", OperandTypeText, args)}
}

func (f *FunctionLEFT) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	text := toUnits(a1.TextValue())
	if len(text) == 0 {
		return CreateString("")
	}
	if f.ArgCount() < 2 {
		return CreateString(fromUnits(text[:1]))
	}
	a2 := f.GetNumber(2, engine, tp)
	if a2.IsErrorOrNone() {
		return a2
	}
	count, ok := tryGetInt(a2)
	if !ok {
		return f.ParameterError(2)
	}
	if count < 0 {
		return f.ParameterError(2)
	}
	if count >= len(text) {
		return a1
	}
	return CreateString(fromUnits(text[:count]))
}

func init() { RegisterFunction("LEFT", newFunctionLEFT) }
