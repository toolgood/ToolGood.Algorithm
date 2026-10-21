package csharp

// FunctionREMOVEEND 对应 C# Function_REMOVEEND：去掉结尾的指定文本。
type FunctionREMOVEEND struct{ BaseFunction }

func newFunctionREMOVEEND(args []Function) Function {
	RequireArgs(args, "RemoveEnd", 2, 3)
	return &FunctionREMOVEEND{BaseFunction: NewBase("RemoveEnd", OperandTypeText, args)}
}

func (f *FunctionREMOVEEND) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
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
	suffix := a2.TextValue()
	if endsWith(text, suffix, ignoreCase) {
		units := toUnits(text)
		length := len(toUnits(suffix))
		return CreateString(fromUnits(units[:len(units)-length]))
	}
	return a1
}

func init() { RegisterFunction("REMOVEEND", newFunctionREMOVEEND) }
