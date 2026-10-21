package flow

// 移植自 Internals/Functions/Flow/Function_NOT.cs。

// FunctionNOT 逻辑非节点。
type FunctionNOT struct{ BaseFunction }

// NewFunctionNOT 构造一元 `!` 节点（对应 C# 的 Function_NOT(FunctionBase)）。
func NewFunctionNOT(arg Function) Function {
	return &FunctionNOT{BaseFunction: NewBase("Not", OperandTypeBoolean, []Function{arg})}
}

func newFunctionNOT(args []Function) Function {
	RequireArgs(args, "Not", 1, 1)
	return &FunctionNOT{BaseFunction: NewBase("Not", OperandTypeBoolean, args)}
}

// Evaluate 取反。
func (f *FunctionNOT) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetBoolean(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	return CreateBool(!a.BooleanValue())
}

func (f *FunctionNOT) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeBoolean)
	}
}

func init() { RegisterFunction("NOT", newFunctionNOT) }
