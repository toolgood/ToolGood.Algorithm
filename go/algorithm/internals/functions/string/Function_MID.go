package string

// FunctionMID 对应 C# Function_MID：从指定位置截取指定长度。
type FunctionMID struct{ BaseFunction }

func newFunctionMID(args []Function) Function {
	RequireArgs(args, "Mid", 3, 3)
	return &FunctionMID{BaseFunction: NewBase("Mid", OperandTypeText, args)}
}

func (f *FunctionMID) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	a2 := f.GetNumber(2, engine, tp)
	if a2.IsErrorOrNone() {
		return a2
	}
	a3 := f.GetNumber(3, engine, tp)
	if a3.IsErrorOrNone() {
		return a3
	}
	text := toUnits(a1.TextValue())
	startValue, ok := tryGetInt(a2)
	if !ok {
		return f.ParameterError(2)
	}
	length, ok := tryGetInt(a3)
	if !ok {
		return f.ParameterError(3)
	}
	// 用 int64 计算，避免 startValue 为 int32 最小值时相减溢出
	startIndex := int64(startValue) - int64(engine.ExcelIndex)
	if startIndex < 0 {
		return f.ParameterError(2)
	}
	if length < 0 {
		return f.ParameterError(3)
	}
	if startIndex == 0 && int64(length) >= int64(len(text)) {
		return a1
	}
	if startIndex >= int64(len(text)) {
		return CreateString("")
	}
	// 用减法比较，避免 startIndex+length 溢出
	if int64(length) > int64(len(text))-startIndex {
		length = len(text) - int(startIndex)
	}
	return CreateString(fromUnits(text[int(startIndex) : int(startIndex)+length]))
}

func init() { RegisterFunction("MID", newFunctionMID) }
