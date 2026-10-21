package csharp

// FunctionISREGEX 对应 C# Function_ISREGEX：判断文本是否匹配正则。
type FunctionISREGEX struct{ BaseFunction }

func newFunctionISREGEX(args []Function) Function {
	RequireArgs(args, "IsRegex", 2, 2)
	return &FunctionISREGEX{BaseFunction: NewBase("IsRegex", OperandTypeBoolean, args)}
}

func (f *FunctionISREGEX) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	a2 := f.GetText(2, engine, tp)
	if a2.IsErrorOrNone() {
		return a2
	}
	re, err := getRegex(a2.TextValue())
	if err != nil {
		// 正则非法（含 .NET 专有语法）：与 C# 捕获 ArgumentException 的行为一致
		return f.ParameterError(2)
	}
	return CreateBool(re.MatchString(a1.TextValue()))
}

func init() { RegisterFunction("ISREGEX", newFunctionISREGEX) }
