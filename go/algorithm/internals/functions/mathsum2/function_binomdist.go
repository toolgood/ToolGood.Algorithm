package mathsum2

// FunctionBINOMDIST 对应 C# Function_BINOMDIST。
type FunctionBINOMDIST struct{ BaseFunction }

func newFunctionBINOMDIST(args []Function) Function {
	RequireArgs(args, "BinomDist", 4, 4)
	return &FunctionBINOMDIST{BaseFunction: NewBase("BinomDist", OperandTypeNumber, args)}
}

func (f *FunctionBINOMDIST) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
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

	n2, ok := tryGetInt(args2)
	if !ok || n2 < 0 {
		return f.ParameterError(2)
	}
	k, ok := tryGetInt(args1)
	if !ok || k < 0 || k > n2 {
		return f.ParameterError(1)
	}
	n3 := args3.NumberValue()
	if n3.Less(DecimalZero) || n3.Greater(DecimalOne) {
		return f.ParameterError(3)
	}

	p := n3.Float64()
	var v float64
	if args4.BooleanValue() {
		v = binomCDF(p, n2, float64(k))
	} else {
		v = binomPMF(p, n2, k)
	}
	return numericResult(f.FunctionError, v)
}

func (f *FunctionBINOMDIST) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	numberArgTypes(engine, &f.BaseFunction, result, true)
}

func init() { RegisterFunction("BINOMDIST", newFunctionBINOMDIST) }
