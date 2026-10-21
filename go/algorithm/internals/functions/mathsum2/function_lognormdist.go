package mathsum2

// FunctionLOGNORMDIST 对应 C# Function_LOGNORMDIST。
type FunctionLOGNORMDIST struct{ BaseFunction }

func newFunctionLOGNORMDIST(args []Function) Function {
	RequireArgs(args, "LogNormDist", 3, 3)
	return &FunctionLOGNORMDIST{BaseFunction: NewBase("LogNormDist", OperandTypeNumber, args)}
}

func (f *FunctionLOGNORMDIST) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args1 := f.GetNumber(1, engine, tp)
	if args1.IsErrorOrNone() {
		return args1
	}
	x := args1.NumberValue()
	if x.LessOrEqual(DecimalZero) {
		return f.ParameterError(1)
	}

	args2 := f.GetNumber(2, engine, tp)
	if args2.IsErrorOrNone() {
		return args2
	}
	args3 := f.GetNumber(3, engine, tp)
	if args3.IsErrorOrNone() {
		return args3
	}

	n3 := args3.NumberValue()
	if n3.LessOrEqual(DecimalZero) {
		return f.ParameterError(3)
	}
	v := logNormalCDF(args2.NumberValue().Float64(), n3.Float64(), x.Float64())
	return numericResult(f.FunctionError, v)
}

func (f *FunctionLOGNORMDIST) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	numberArgTypes(engine, &f.BaseFunction, result, false)
}

func init() { RegisterFunction("LOGNORMDIST", newFunctionLOGNORMDIST) }
