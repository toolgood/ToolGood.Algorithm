package flow

// 移植自 Internals/Functions/Flow/Function_ISNONTEXT.cs。

// FunctionISNONTEXT 判断是否非文本。
type FunctionISNONTEXT struct{ BaseFunction }

func newFunctionISNONTEXT(args []Function) Function {
	RequireArgs(args, "IsNonText", 1, 1)
	return &FunctionISNONTEXT{BaseFunction: NewBase("IsNonText", OperandTypeBoolean, args)}
}

// Evaluate 判断是否非文本。
func (f *FunctionISNONTEXT) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.EvalArg(0, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	return CreateBool(!a.IsText())
}

func (f *FunctionISNONTEXT) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNone)
	}
}

func init() { RegisterFunction("ISNONTEXT", newFunctionISNONTEXT) }
