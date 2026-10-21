package csharp

// FunctionREMOVESTART 对应 C# Function_REMOVESTART：去掉开头的指定文本。
type FunctionREMOVESTART struct{ BaseFunction }

func newFunctionREMOVESTART(args []Function) Function {
	RequireArgs(args, "RemoveStart", 2, 3)
	return &FunctionREMOVESTART{BaseFunction: NewBase("RemoveStart", OperandTypeText, args)}
}

func (f *FunctionREMOVESTART) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	a2 := f.GetText(2, engine, tp)
	if a2.IsErrorOrNone() {
		return a2
	}
	ignoreCase := false
	if f.ArgCount() >= 3 {
		a3 := f.GetBoolean(3, engine, tp)
		if a3.IsErrorOrNone() {
			return a3
		}
		ignoreCase = a3.BooleanValue()
	}
	text := a1.TextValue()
	prefix := a2.TextValue()
	if startsWith(text, prefix, ignoreCase) {
		units := toUnits(text)
		length := len(toUnits(prefix))
		return CreateString(fromUnits(units[length:]))
	}
	return a1
}

func init() { RegisterFunction("REMOVESTART", newFunctionREMOVESTART) }
