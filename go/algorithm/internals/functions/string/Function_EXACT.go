package string

// FunctionEXACT 对应 C# Function_EXACT：序数比较两个文本是否完全相同。
type FunctionEXACT struct{ BaseFunction }

func newFunctionEXACT(args []Function) Function {
	RequireArgs(args, "Exact", 2, 2)
	return &FunctionEXACT{BaseFunction: NewBase("Exact", OperandTypeBoolean, args)}
}

func (f *FunctionEXACT) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	a2 := f.GetText(2, engine, tp)
	if a2.IsErrorOrNone() {
		return a2
	}
	return CreateBool(a1.TextValue() == a2.TextValue())
}

func init() { RegisterFunction("EXACT", newFunctionEXACT) }
