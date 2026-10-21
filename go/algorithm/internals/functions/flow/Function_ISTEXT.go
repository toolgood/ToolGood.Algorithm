package flow

// 移植自 Internals/Functions/Flow/Function_ISTEXT.cs。

// FunctionISTEXT 判断是否文本。
type FunctionISTEXT struct{ BaseFunction }

func newFunctionISTEXT(args []Function) Function {
	RequireArgs(args, "IsText", 1, 1)
	return &FunctionISTEXT{BaseFunction: NewBase("IsText", OperandTypeBoolean, args)}
}

// Evaluate 判断是否文本。
func (f *FunctionISTEXT) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.EvalArg(0, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	return CreateBool(a.IsText())
}

func (f *FunctionISTEXT) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNone)
	}
}

func init() { RegisterFunction("ISTEXT", newFunctionISTEXT) }
