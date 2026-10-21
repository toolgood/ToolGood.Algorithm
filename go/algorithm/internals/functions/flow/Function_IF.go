package flow

// 移植自 Internals/Functions/Flow/Function_IF.cs。

// FunctionIF 三元条件节点。
type FunctionIF struct{ BaseFunction }

// NewFunctionIF 构造三元 `?:` 节点（对应 C# 的 Function_IF(FunctionBase[])）。
func NewFunctionIF(args []Function) Function {
	RequireArgs(args, "If", 2, 3)
	return &FunctionIF{BaseFunction: NewBase("If", OperandTypeNone, args)}
}

func newFunctionIF(args []Function) Function { return NewFunctionIF(args) }

// Evaluate 惰性求值：只求值命中的分支。
func (f *FunctionIF) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	cond := f.GetBoolean(1, engine, tp)
	if cond.IsErrorOrNone() {
		return cond
	}
	if cond.BooleanValue() {
		return f.EvalArg(1, engine, tp)
	}
	if f.ArgCount() < 3 {
		return False
	}
	return f.EvalArg(2, engine, tp)
}

// ResultType 取命中的结果分支类型。
func (f *FunctionIF) ResultType() OperandType {
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

func (f *FunctionIF) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeBoolean)
	}
	if arg := f.Arg(1); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNone)
	}
	if arg := f.Arg(2); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNone)
	}
}

func init() { RegisterFunction("IF", newFunctionIF) }
