package mathsum

// FunctionFORECAST 对应 C# Function_FORECAST（线性回归预测）。
type FunctionFORECAST struct{ BaseFunction }

func newFunctionFORECAST(args []Function) Function {
	RequireArgs(args, "FORECAST", 3, 3)
	return &FunctionFORECAST{BaseFunction: NewBase("FORECAST", OperandTypeNumber, args)}
}

func (f *FunctionFORECAST) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	xArg := f.GetNumber(1, engine, tp)
	if xArg.IsErrorOrNone() {
		return xArg
	}
	x := xArg.NumberValue()

	slope, intercept, err := regression(&f.BaseFunction, engine, tp, 2, 3)
	if err != nil {
		return err
	}
	return CreateDecimal(intercept.Add(slope.Mul(x)))
}

func (f *FunctionFORECAST) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	f.Arg(0).GetParameterTypes(engine, result, OperandTypeNumber)
	f.Arg(1).GetParameterTypes(engine, result, OperandTypeArray)
	f.Arg(2).GetParameterTypes(engine, result, OperandTypeArray)
}

func init() { RegisterFunction("FORECAST", newFunctionFORECAST) }
