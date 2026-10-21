package mathsum

// FunctionSTDEV 对应 C# Function_STDEV（样本标准差）。
type FunctionSTDEV struct{ BaseFunction }

func newFunctionSTDEV(args []Function) Function {
	RequireArgs(args, "StDev", 1, -1)
	return &FunctionSTDEV{BaseFunction: NewBase("StDev", OperandTypeNumber, args)}
}

func (f *FunctionSTDEV) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	m2, count, errOp := welfordM2(&f.BaseFunction, engine, tp)
	if errOp != nil {
		return errOp
	}
	if count <= 1 {
		return f.FunctionError()
	}
	return CreateDecimal(m2.Div(DecFromInt(int64(count - 1))).Sqrt())
}

func (f *FunctionSTDEV) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	argTypes(engine, &f.BaseFunction, result, OperandTypeArray, OperandTypeNumber)
}

func init() { RegisterFunction("STDEV", newFunctionSTDEV) }
