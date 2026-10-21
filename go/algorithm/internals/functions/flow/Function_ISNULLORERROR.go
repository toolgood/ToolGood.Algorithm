package flow

// 移植自 Internals/Functions/Flow/Function_ISNULLORERROR.cs。

// FunctionISNULLORERROR 判断是否空值或错误。
type FunctionISNULLORERROR struct{ BaseFunction }

func newFunctionISNULLORERROR(args []Function) Function {
	RequireArgs(args, "IsNullOrError", 1, 2)
	return &FunctionISNULLORERROR{BaseFunction: NewBase("IsNullOrError", OperandTypeBoolean, args)}
}

// Evaluate 单参返回布尔；双参时为空值或错误返回第二参数，否则返回原值。
func (f *FunctionISNULLORERROR) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.EvalArg(0, engine, tp)
	if f.ArgCount() > 1 {
		if a.IsNull() || a.IsErrorOrNone() {
			return f.EvalArg(1, engine, tp)
		}
		return a
	}
	return CreateBool(a.IsNull() || a.IsErrorOrNone())
}

// ResultType 双参时取第二参数类型。
func (f *FunctionISNULLORERROR) ResultType() OperandType {
	if arg := f.Arg(1); arg != nil {
		return arg.ResultType()
	}
	return OperandTypeBoolean
}

func (f *FunctionISNULLORERROR) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	for i := 0; i < f.ArgCount(); i++ {
		f.Arg(i).GetParameterTypes(engine, result, OperandTypeNone)
	}
}

func init() { RegisterFunction("ISNULLORERROR", newFunctionISNULLORERROR) }
