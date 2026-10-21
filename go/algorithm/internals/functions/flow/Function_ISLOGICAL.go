package flow

// 移植自 Internals/Functions/Flow/Function_ISLOGICAL.cs。

// FunctionISLOGICAL 判断是否布尔值。
type FunctionISLOGICAL struct{ BaseFunction }

func newFunctionISLOGICAL(args []Function) Function {
	RequireArgs(args, "IsLogical", 1, 1)
	return &FunctionISLOGICAL{BaseFunction: NewBase("IsLogical", OperandTypeBoolean, args)}
}

// Evaluate 判断是否布尔值。
func (f *FunctionISLOGICAL) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.EvalArg(0, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	return CreateBool(a.IsBoolean())
}

func (f *FunctionISLOGICAL) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNone)
	}
}

func init() { RegisterFunction("ISLOGICAL", newFunctionISLOGICAL) }
