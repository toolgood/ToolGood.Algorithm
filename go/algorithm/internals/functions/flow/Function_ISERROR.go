package flow

// 移植自 Internals/Functions/Flow/Function_ISERROR.cs。

// FunctionISERROR 判断是否错误。
type FunctionISERROR struct{ BaseFunction }

func newFunctionISERROR(args []Function) Function {
	RequireArgs(args, "IsError", 1, 2)
	return &FunctionISERROR{BaseFunction: NewBase("IsError", OperandTypeBoolean, args)}
}

// Evaluate 单参返回布尔；双参时出错返回第二参数，否则返回原值。
func (f *FunctionISERROR) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.EvalArg(0, engine, tp)
	if a.IsNone() {
		return a
	}
	if f.ArgCount() > 1 {
		if a.IsError() {
			return f.EvalArg(1, engine, tp)
		}
		return a
	}
	return CreateBool(a.IsError())
}

// ResultType 双参时取第二参数类型。
func (f *FunctionISERROR) ResultType() OperandType {
	if arg := f.Arg(1); arg != nil {
		return arg.ResultType()
	}
	return OperandTypeBoolean
}

func (f *FunctionISERROR) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	for i := 0; i < f.ArgCount(); i++ {
		f.Arg(i).GetParameterTypes(engine, result, OperandTypeNone)
	}
}

func init() { RegisterFunction("ISERROR", newFunctionISERROR) }
