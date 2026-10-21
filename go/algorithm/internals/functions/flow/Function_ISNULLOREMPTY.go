package flow

// 移植自 Internals/Functions/Flow/Function_ISNULLOREMPTY.cs。

// FunctionISNULLOREMPTY 判断是否空值或空字符串。
type FunctionISNULLOREMPTY struct{ BaseFunction }

func newFunctionISNULLOREMPTY(args []Function) Function {
	RequireArgs(args, "IsNullOrEmpty", 1, 1)
	return &FunctionISNULLOREMPTY{BaseFunction: NewBase("IsNullOrEmpty", OperandTypeBoolean, args)}
}

// Evaluate 判断是否空值或空字符串。
func (f *FunctionISNULLOREMPTY) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.EvalArg(0, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	if a.IsNull() {
		return True
	}
	textArg := f.ConvertToText(a, 1)
	if textArg.IsErrorOrNone() {
		return textArg
	}
	return CreateBool(textArg.TextValue() == "")
}

func (f *FunctionISNULLOREMPTY) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNone)
	}
}

func init() { RegisterFunction("ISNULLOREMPTY", newFunctionISNULLOREMPTY) }
