package mathsum

// FunctionINTERCEPT 对应 C# Function_INTERCEPT（线性回归截距）。
type FunctionINTERCEPT struct{ BaseFunction }

func newFunctionINTERCEPT(args []Function) Function {
	RequireArgs(args, "INTERCEPT", 2, 2)
	return &FunctionINTERCEPT{BaseFunction: NewBase("INTERCEPT", OperandTypeNumber, args)}
}

func (f *FunctionINTERCEPT) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	_, intercept, err := regression(&f.BaseFunction, engine, tp, 1, 2)
	if err != nil {
		return err
	}
	return CreateDecimal(intercept)
}

func (f *FunctionINTERCEPT) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	f.Arg(0).GetParameterTypes(engine, result, OperandTypeArray)
	f.Arg(1).GetParameterTypes(engine, result, OperandTypeArray)
}

func init() { RegisterFunction("INTERCEPT", newFunctionINTERCEPT) }
