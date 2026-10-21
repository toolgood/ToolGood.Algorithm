package mathsum

// FunctionVAR 对应 C# Function_VAR（样本方差）。
type FunctionVAR struct{ BaseFunction }

func newFunctionVAR(args []Function) Function {
	RequireArgs(args, "Var", 1, -1)
	return &FunctionVAR{BaseFunction: NewBase("Var", OperandTypeNumber, args)}
}

func (f *FunctionVAR) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	m2, count, errOp := welfordM2(&f.BaseFunction, engine, tp)
	if errOp != nil {
		return errOp
	}
	if count <= 1 {
		return f.FunctionError()
	}
	return CreateDecimal(m2.Div(DecFromInt(int64(count - 1))))
}

func (f *FunctionVAR) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	argTypes(engine, &f.BaseFunction, result, OperandTypeArray, OperandTypeNumber)
}

func init() { RegisterFunction("VAR", newFunctionVAR) }
