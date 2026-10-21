package mathsum2

// FunctionGAMMAINV 对应 C# Function_GAMMAINV。
type FunctionGAMMAINV struct{ BaseFunction }

func newFunctionGAMMAINV(args []Function) Function {
	RequireArgs(args, "GammaInv", 3, 3)
	return &FunctionGAMMAINV{BaseFunction: NewBase("GammaInv", OperandTypeNumber, args)}
}

func (f *FunctionGAMMAINV) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args1 := f.GetNumber(1, engine, tp)
	if args1.IsErrorOrNone() {
		return args1
	}
	args2 := f.GetNumber(2, engine, tp)
	if args2.IsErrorOrNone() {
		return args2
	}
	args3 := f.GetNumber(3, engine, tp)
	if args3.IsErrorOrNone() {
		return args3
	}

	probability := args1.NumberValue()
	// 概率必须为开区间 (0,1)：p=0 结果恒为 0，p=1 时分布无上界
	if probability.LessOrEqual(DecimalZero) || probability.GreaterOrEqual(DecimalOne) {
		return f.ParameterError(1)
	}
	alpha := args2.NumberValue()
	if alpha.LessOrEqual(DecimalZero) {
		return f.ParameterError(2)
	}
	beta := args3.NumberValue()
	if beta.LessOrEqual(DecimalZero) {
		return f.ParameterError(3)
	}

	rate := DecimalOne.Div(beta).Float64()
	v := gammaInvCDF(alpha.Float64(), rate, probability.Float64())
	return numericResult(f.FunctionError, v)
}

func (f *FunctionGAMMAINV) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	numberArgTypes(engine, &f.BaseFunction, result, false)
}

func init() { RegisterFunction("GAMMAINV", newFunctionGAMMAINV) }
