package string

// FunctionUNICHAR 对应 C# Function_UNICHAR：按 Unicode 码点返回字符。
type FunctionUNICHAR struct{ BaseFunction }

func newFunctionUNICHAR(args []Function) Function {
	RequireArgs(args, "UniChar", 1, 1)
	return &FunctionUNICHAR{BaseFunction: NewBase("UniChar", OperandTypeText, args)}
}

func (f *FunctionUNICHAR) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetNumber(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	code, ok := tryGetInt(a1)
	if !ok {
		return f.ParameterError(1)
	}
	if code < 0 || code > 0x10FFFF || (code >= 0xD800 && code <= 0xDFFF) {
		return f.ParameterError(1)
	}
	return CreateString(string(rune(code)))
}

func init() { RegisterFunction("UNICHAR", newFunctionUNICHAR) }
