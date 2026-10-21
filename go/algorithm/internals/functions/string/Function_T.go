package string

// FunctionT 对应 C# Function_T：文本原样返回，其他类型返回空文本。
type FunctionT struct{ BaseFunction }

func newFunctionT(args []Function) Function {
	RequireArgs(args, "T", 1, 1)
	return &FunctionT{BaseFunction: NewBase("T", OperandTypeText, args)}
}

func (f *FunctionT) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.EvalArg(0, engine, tp)
	if a1.IsText() {
		return a1
	}
	return CreateString("")
}

func init() { RegisterFunction("T", newFunctionT) }
