package value

// 移植自 Internals/Functions/Value/Function_ERROR.cs。

// FunctionERROR 生成错误操作数。
type FunctionERROR struct{ BaseFunction }

func newFunctionERROR(args []Function) Function {
	RequireArgs(args, "Error", 1, 1)
	return &FunctionERROR{BaseFunction: NewBase("Error", OperandTypeError, args)}
}

// Evaluate 把第一个参数的文本作为错误信息。
func (f *FunctionERROR) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetText(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	return Error(a.TextValue())
}

func (f *FunctionERROR) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeText)
	}
}

func init() { RegisterFunction("ERROR", newFunctionERROR) }
