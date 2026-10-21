package mathsum2

// FunctionNEGBINOMDIST 对应 C# Function_NEGBINOMDIST（负二项分布）。
type FunctionNEGBINOMDIST struct{ BaseFunction }

func newFunctionNEGBINOMDIST(args []Function) Function {
	RequireArgs(args, "NegBinomDist", 3, 3)
	return &FunctionNEGBINOMDIST{BaseFunction: NewBase("NegBinomDist", OperandTypeNumber, args)}
}

func (f *FunctionNEGBINOMDIST) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
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

	k, ok := tryGetInt(args1)
	if !ok || k < 0 {
		return f.ParameterError(1)
	}
	r := args2.NumberValue()
	if r.LessOrEqual(DecimalZero) {
		return f.ParameterError(2)
	}
	p := args3.NumberValue()
	// p 必须为开区间 (0,1)：p=0 或 p=1 时底层对数无意义
	if p.LessOrEqual(DecimalZero) || p.GreaterOrEqual(DecimalOne) {
		return f.ParameterError(3)
	}
	return numericResult(f.FunctionError, negativeBinomPMF(r.Float64(), p.Float64(), k))
}

func (f *FunctionNEGBINOMDIST) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	numberArgTypes(engine, &f.BaseFunction, result, false)
}

func init() { RegisterFunction("NEGBINOMDIST", newFunctionNEGBINOMDIST) }
