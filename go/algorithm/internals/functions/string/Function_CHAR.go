package string

// FunctionCHAR 对应 C# Function_CHAR：按 UTF-16 码元值返回字符。
type FunctionCHAR struct{ BaseFunction }

func newFunctionCHAR(args []Function) Function {
	RequireArgs(args, "Char", 1, 1)
	return &FunctionCHAR{BaseFunction: NewBase("Char", OperandTypeText, args)}
}

func (f *FunctionCHAR) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	code, ok := tryGetInt(a)
	if !ok {
		return f.ParameterError(1)
	}
	if code < 0 || code > 65535 {
		return f.ParameterError(1)
	}
	// 说明：C# 允许 0xD800-0xDFFF 生成孤立代理项；Go 的 string(rune) 会把它替换为
	// U+FFFD，此处保持 Go 的语义（仅该区段与 C# 有偏差）。
	return CreateString(string(rune(code)))
}

func init() { RegisterFunction("CHAR", newFunctionCHAR) }
