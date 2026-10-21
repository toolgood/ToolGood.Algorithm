package mathsum2

// FunctionGAMMADIST 对应 C# Function_GAMMADIST。
type FunctionGAMMADIST struct{ BaseFunction }

func newFunctionGAMMADIST(args []Function) Function {
	RequireArgs(args, "GammaDist", 4, 4)
	return &FunctionGAMMADIST{BaseFunction: NewBase("GammaDist", OperandTypeNumber, args)}
}

func (f *FunctionGAMMADIST) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
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
	args4 := f.GetBoolean(4, engine, tp)
	if args4.IsErrorOrNone() {
		return args4
	}

	x := args1.NumberValue()
	if x.Less(DecimalZero) {
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

	// C# 以 rate = 1/beta 调用 Gamma 分布
	rate := DecimalOne.Div(beta).Float64()
	var v float64
	if args4.BooleanValue() {
		v = gammaCDF(alpha.Float64(), rate, x.Float64())
	} else {
		v = gammaPDF(alpha.Float64(), rate, x.Float64())
	}
	return numericResult(f.FunctionError, v)
}

func (f *FunctionGAMMADIST) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	numberArgTypes(engine, &f.BaseFunction, result, true)
}

func init() { RegisterFunction("GAMMADIST", newFunctionGAMMADIST) }
