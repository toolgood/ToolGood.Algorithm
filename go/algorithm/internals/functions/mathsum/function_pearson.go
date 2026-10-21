package mathsum

// FunctionPEARSON 对应 C# Function_PEARSON（Pearson 相关系数，与 CORREL 等价）。
type FunctionPEARSON struct{ BaseFunction }

func newFunctionPEARSON(args []Function) Function {
	RequireArgs(args, "PEARSON", 2, 2)
	return &FunctionPEARSON{BaseFunction: NewBase("PEARSON", OperandTypeNumber, args)}
}

func (f *FunctionPEARSON) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	return correlation(&f.BaseFunction, engine, tp)
}

func (f *FunctionPEARSON) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	f.Arg(0).GetParameterTypes(engine, result, OperandTypeArray)
	f.Arg(1).GetParameterTypes(engine, result, OperandTypeArray)
}

func init() { RegisterFunction("PEARSON", newFunctionPEARSON) }
