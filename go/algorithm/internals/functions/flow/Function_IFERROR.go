package flow

// 移植自 Internals/Functions/Flow/Function_IFERROR.cs。

// FunctionIFERROR 错误兜底节点。
type FunctionIFERROR struct{ BaseFunction }

func newFunctionIFERROR(args []Function) Function {
	RequireArgs(args, "IfError", 2, 3)
	return &FunctionIFERROR{BaseFunction: NewBase("IfError", OperandTypeNone, args)}
}

// Evaluate 第一个参数出错时返回第二个参数，否则（有三参时）返回第三个参数。
func (f *FunctionIFERROR) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.EvalArg(0, engine, tp)
	if a.IsNone() {
		return a
	}
	if a.IsError() {
		return f.EvalArg(1, engine, tp)
	}
	if f.ArgCount() < 3 {
		return a
	}
	return f.EvalArg(2, engine, tp)
}

// ResultType 取结果分支类型。
func (f *FunctionIFERROR) ResultType() OperandType {
	if arg := f.Arg(1); arg != nil {
		if t := arg.ResultType(); t != OperandTypeNone {
			return t
		}
	}
	if arg := f.Arg(2); arg != nil {
		return arg.ResultType()
	}
	return OperandTypeNone
}

func (f *FunctionIFERROR) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	for i := 0; i < f.ArgCount(); i++ {
		f.Arg(i).GetParameterTypes(engine, result, OperandTypeNone)
	}
}

func init() { RegisterFunction("IFERROR", newFunctionIFERROR) }
