package csharp

// FunctionLASTINDEXOF 对应 C# Function_LASTINDEXOF：序数反向查找子串位置。
type FunctionLASTINDEXOF struct{ BaseFunction }

func newFunctionLASTINDEXOF(args []Function) Function {
	RequireArgs(args, "LastIndexOf", 2, 4)
	return &FunctionLASTINDEXOF{BaseFunction: NewBase("LastIndexOf", OperandTypeNumber, args)}
}

func (f *FunctionLASTINDEXOF) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
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
		if len(pattern) == 0 {
			return CreateInt(len(text) + engine.ExcelIndex)
		}
		index := unitsLastIndexOf(text, pattern, len(text)-1)
		return CreateInt(index + engine.ExcelIndex)
	}
	a3 := f.GetNumber(3, engine, tp)
	if a3.IsErrorOrNone() {
		return a3
	}
	startIndex := a3.IntValue() - engine.ExcelIndex
	if startIndex < 0 || startIndex > len(text) {
		return f.ParameterError(3)
	}
	if startIndex >= len(text) {
		// C# string.LastIndexOf(value, startIndex) 允许 startIndex == Length，等价于从末尾开始搜索。
		startIndex = len(text) - 1
	}
	if f.ArgCount() < 4 {
		return CreateInt(unitsLastIndexOf(text, pattern, startIndex) + engine.ExcelIndex)
	}
	a4 := f.GetNumber(4, engine, tp)
	if a4.IsErrorOrNone() {
		return a4
	}
	count := a4.IntValue()
	if count < 0 || count > startIndex+1 {
		return f.ParameterError(4)
	}
	return CreateInt(unitsLastIndexOfCount(text, pattern, startIndex, count) + engine.ExcelIndex)
}

func init() { RegisterFunction("LASTINDEXOF", newFunctionLASTINDEXOF) }
