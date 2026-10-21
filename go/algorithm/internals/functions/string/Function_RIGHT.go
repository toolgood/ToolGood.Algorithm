package string

// FunctionRIGHT 对应 C# Function_RIGHT：取右侧若干字符。
type FunctionRIGHT struct{ BaseFunction }

func newFunctionRIGHT(args []Function) Function {
	RequireArgs(args, "Right", 1, 2)
	return &FunctionRIGHT{BaseFunction: NewBase("Right", OperandTypeText, args)}
}

func (f *FunctionRIGHT) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	text := toUnits(a1.TextValue())
	if len(text) == 0 {
		return CreateString("")
	}
	if f.ArgCount() < 2 {
		return CreateString(fromUnits(text[len(text)-1:]))
	}
	a2 := f.GetNumber(2, engine, tp)
	if a2.IsErrorOrNone() {
		return a2
	}
	length, ok := tryGetInt(a2)
	if !ok {
		return f.ParameterError(2)
	}
	if length < 0 {
		return f.ParameterError(2)
	}
	if length >= len(text) {
		return a1
	}
	start := len(text) - length
	return CreateString(fromUnits(text[start : start+length]))
}

func init() { RegisterFunction("RIGHT", newFunctionRIGHT) }
