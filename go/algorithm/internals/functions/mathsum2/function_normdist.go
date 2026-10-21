package mathsum2

// FunctionNORMDIST 对应 C# Function_NORMDIST。
type FunctionNORMDIST struct{ BaseFunction }

func newFunctionNORMDIST(args []Function) Function {
	RequireArgs(args, "NormDist", 4, 4)
	return &FunctionNORMDIST{BaseFunction: NewBase("NormDist", OperandTypeNumber, args)}
}

func (f *FunctionNORMDIST) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
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

	num := args1.NumberValue()
	avg := args2.NumberValue()
	stdev := args3.NumberValue()
	if stdev.LessOrEqual(DecimalZero) {
		return f.ParameterError(3)
	}

	var v float64
	if args4.BooleanValue() {
		v = normalCDF(avg.Float64(), stdev.Float64(), num.Float64())
	} else {
		v = normalPDF(avg.Float64(), stdev.Float64(), num.Float64())
	}
	return numericResult(f.FunctionError, v)
}

func (f *FunctionNORMDIST) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	numberArgTypes(engine, &f.BaseFunction, result, true)
}

func init() { RegisterFunction("NORMDIST", newFunctionNORMDIST) }
