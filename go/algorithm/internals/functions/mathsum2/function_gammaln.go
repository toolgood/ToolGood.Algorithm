package mathsum2

// FunctionGAMMALN 对应 C# Function_GAMMALN。
type FunctionGAMMALN struct{ BaseFunction }

func newFunctionGAMMALN(args []Function) Function {
	RequireArgs(args, "GammaLn", 1, 1)
	return &FunctionGAMMALN{BaseFunction: NewBase("GammaLn", OperandTypeNumber, args)}
}

func (f *FunctionGAMMALN) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args1 := f.GetNumber(1, engine, tp)
	if args1.IsErrorOrNone() {
		return args1
	}
	if args1.NumberValue().LessOrEqual(DecimalZero) {
		return f.ParameterError(1)
	}
	return numericResult(f.FunctionError, gammaLn(args1.NumberValue().Float64()))
}

func (f *FunctionGAMMALN) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	f.Arg(0).GetParameterTypes(engine, result, OperandTypeNumber)
}

func init() { RegisterFunction("GAMMALN", newFunctionGAMMALN) }
