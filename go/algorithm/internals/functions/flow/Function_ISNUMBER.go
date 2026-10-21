package flow

// 移植自 Internals/Functions/Flow/Function_ISNUMBER.cs。

// FunctionISNUMBER 判断是否数字。
type FunctionISNUMBER struct{ BaseFunction }

func newFunctionISNUMBER(args []Function) Function {
	RequireArgs(args, "IsNumber", 1, 1)
	return &FunctionISNUMBER{BaseFunction: NewBase("IsNumber", OperandTypeBoolean, args)}
}

// Evaluate 判断是否数字。
func (f *FunctionISNUMBER) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.EvalArg(0, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	return CreateBool(a.IsNumber())
}

func (f *FunctionISNUMBER) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNone)
	}
}

func init() { RegisterFunction("ISNUMBER", newFunctionISNUMBER) }
