package flow

// 移植自 Internals/Functions/Flow/Function_ISNULL.cs。

// FunctionISNULL 判断是否空值。
type FunctionISNULL struct{ BaseFunction }

func newFunctionISNULL(args []Function) Function {
	RequireArgs(args, "IsNull", 1, 2)
	return &FunctionISNULL{BaseFunction: NewBase("IsNull", OperandTypeBoolean, args)}
}

// Evaluate 单参返回布尔；双参时空值返回第二参数，否则返回原值。
func (f *FunctionISNULL) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.EvalArg(0, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	if f.ArgCount() > 1 {
		if a.IsNull() {
			return f.EvalArg(1, engine, tp)
		}
		return a
	}
	return CreateBool(a.IsNull())
}

// ResultType 双参时取第二参数类型。
func (f *FunctionISNULL) ResultType() OperandType {
	if arg := f.Arg(1); arg != nil {
		return arg.ResultType()
	}
	return OperandTypeBoolean
}

func (f *FunctionISNULL) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	for i := 0; i < f.ArgCount(); i++ {
		f.Arg(i).GetParameterTypes(engine, result, OperandTypeNone)
	}
}

func init() { RegisterFunction("ISNULL", newFunctionISNULL) }
