package csharp

// FunctionINDEXOF 对应 C# Function_INDEXOF：序数查找子串位置。
type FunctionINDEXOF struct{ BaseFunction }

func newFunctionINDEXOF(args []Function) Function {
	RequireArgs(args, "IndexOf", 2, 4)
	return &FunctionINDEXOF{BaseFunction: NewBase("IndexOf", OperandTypeNumber, args)}
}

func (f *FunctionINDEXOF) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	a2 := f.GetText(2, engine, tp)
	if a2.IsErrorOrNone() {
		return a2
	}
	text := toUnits(a1.TextValue())
	pattern := toUnits(a2.TextValue())
	if f.ArgCount() < 3 {
		return CreateInt(unitsIndexOf(text, pattern, 0) + engine.ExcelIndex)
	}
	a3 := f.GetNumber(3, engine, tp)
	if a3.IsErrorOrNone() {
		return a3
	}
	startIndex := a3.IntValue() - engine.ExcelIndex
	if startIndex < 0 || startIndex > len(text) {
		return f.ParameterError(3)
	}
	if f.ArgCount() < 4 {
		return CreateInt(unitsIndexOf(text, pattern, startIndex) + engine.ExcelIndex)
	}
	a4 := f.GetNumber(4, engine, tp)
	if a4.IsErrorOrNone() {
		return a4
	}
	count := a4.IntValue()
	if count < 0 || startIndex+count > len(text) {
		return f.ParameterError(4)
	}
	return CreateInt(unitsIndexOfCount(text, pattern, startIndex, count) + engine.ExcelIndex)
}

func init() { RegisterFunction("INDEXOF", newFunctionINDEXOF) }
