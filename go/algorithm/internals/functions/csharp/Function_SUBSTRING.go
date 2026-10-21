package csharp

// FunctionSUBSTRING 对应 C# Function_SUBSTRING：按位置截取子串。
type FunctionSUBSTRING struct{ BaseFunction }

func newFunctionSUBSTRING(args []Function) Function {
	RequireArgs(args, "Substring", 2, 3)
	return &FunctionSUBSTRING{BaseFunction: NewBase("Substring", OperandTypeText, args)}
}

func (f *FunctionSUBSTRING) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	a2 := f.GetNumber(2, engine, tp)
	if a2.IsErrorOrNone() {
		return a2
	}
	units := toUnits(a1.TextValue())
	startIndex := a2.IntValue() - engine.ExcelIndex
	if startIndex < 0 {
		return f.ParameterError(2)
	}
	if startIndex >= len(units) {
		return CreateString("")
	}
	if f.ArgCount() < 3 {
		return CreateString(fromUnits(units[startIndex:]))
	}
	a3 := f.GetNumber(3, engine, tp)
	if a3.IsErrorOrNone() {
		return a3
	}
	length := a3.IntValue()
	if length < 0 {
		return f.ParameterError(3)
	}
	if startIndex+length > len(units) {
		length = len(units) - startIndex
	}
	return CreateString(fromUnits(units[startIndex : startIndex+length]))
}

func init() { RegisterFunction("SUBSTRING", newFunctionSUBSTRING) }
