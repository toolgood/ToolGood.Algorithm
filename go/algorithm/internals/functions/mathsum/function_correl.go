package mathsum

// FunctionCORREL 对应 C# Function_CORREL（相关系数）。
type FunctionCORREL struct{ BaseFunction }

func newFunctionCORREL(args []Function) Function {
	RequireArgs(args, "CORREL", 2, 2)
	return &FunctionCORREL{BaseFunction: NewBase("CORREL", OperandTypeNumber, args)}
}

func (f *FunctionCORREL) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	return correlation(&f.BaseFunction, engine, tp)
}

func (f *FunctionCORREL) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	f.Arg(0).GetParameterTypes(engine, result, OperandTypeArray)
	f.Arg(1).GetParameterTypes(engine, result, OperandTypeArray)
}

func init() { RegisterFunction("CORREL", newFunctionCORREL) }
