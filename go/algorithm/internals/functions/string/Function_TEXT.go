package string

// FunctionTEXT 对应 C# Function_TEXT：按格式串把值转为文本。
type FunctionTEXT struct{ BaseFunction }

func newFunctionTEXT(args []Function) Function {
	RequireArgs(args, "Text", 2, 2)
	return &FunctionTEXT{BaseFunction: NewBase("Text", OperandTypeText, args)}
}

func (f *FunctionTEXT) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.EvalArg(0, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	a2 := f.GetText(2, engine, tp)
	if a2.IsErrorOrNone() {
		return a2
	}
	switch {
	case a1.IsText():
		return a1
	case a1.IsBoolean():
		if a1.BooleanValue() {
			return CreateString("TRUE")
		}
		return CreateString("FALSE")
	case a1.IsNumber():
		// 非法格式串在 C# 中抛 FormatException，此处返回参数错误
		text, err := formatNumber(a1.NumberValue(), a2.TextValue())
		if err != nil {
			return f.ParameterError(2)
		}
		return CreateString(text)
	case a1.IsDate():
		// 说明：C# 的 MyDate.ToString(format) 在格式非法时抛 FormatException，
		// Go 的 FormatDateTime 不会报错，未知字符按字面量输出。
		return CreateString(a1.DateValue().ToString(a2.TextValue()))
	}
	converted := f.ConvertToText(a1, 1)
	if converted.IsErrorOrNone() {
		return converted
	}
	return CreateString(converted.TextValue())
}

func init() { RegisterFunction("TEXT", newFunctionTEXT) }
