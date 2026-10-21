package string

// FunctionCLEAN 对应 C# Function_CLEAN：删除所有 ASCII 控制字符（< 32）。
type FunctionCLEAN struct{ BaseFunction }

func newFunctionCLEAN(args []Function) Function {
	RequireArgs(args, "Clean", 1, 1)
	return &FunctionCLEAN{BaseFunction: NewBase("Clean", OperandTypeText, args)}
}

func (f *FunctionCLEAN) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetText(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	text := a.TextValue()
	needClean := false
	for _, c := range text {
		if c < 32 {
			needClean = true
			break
		}
	}
	if !needClean {
		return a
	}
	buf := make([]rune, 0, len(text))
	for _, c := range text {
		if c >= 32 {
			buf = append(buf, c)
		}
	}
	return CreateString(string(buf))
}

func init() { RegisterFunction("CLEAN", newFunctionCLEAN) }
