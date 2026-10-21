package mathsum

// FunctionSLOPE 对应 C# Function_SLOPE（线性回归斜率）。
type FunctionSLOPE struct{ BaseFunction }

func newFunctionSLOPE(args []Function) Function {
	RequireArgs(args, "SLOPE", 2, 2)
	return &FunctionSLOPE{BaseFunction: NewBase("SLOPE", OperandTypeNumber, args)}
}

func (f *FunctionSLOPE) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	slope, _, err := regression(&f.BaseFunction, engine, tp, 1, 2)
	if err != nil {
		return err
	}
	return CreateDecimal(slope)
}

func (f *FunctionSLOPE) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	f.Arg(0).GetParameterTypes(engine, result, OperandTypeArray)
	f.Arg(1).GetParameterTypes(engine, result, OperandTypeArray)
}

func init() { RegisterFunction("SLOPE", newFunctionSLOPE) }
