package mathsum2

// FunctionLOGINV 对应 C# Function_LOGINV。
type FunctionLOGINV struct{ BaseFunction }

func newFunctionLOGINV(args []Function) Function {
	RequireArgs(args, "LogInv", 3, 3)
	return &FunctionLOGINV{BaseFunction: NewBase("LogInv", OperandTypeNumber, args)}
}

func (f *FunctionLOGINV) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args1 := f.GetNumber(1, engine, tp)
	if args1.IsErrorOrNone() {
		return args1
	}
	probability := args1.NumberValue()
	if probability.LessOrEqual(DecimalZero) || probability.GreaterOrEqual(DecimalOne) {
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
	v := logNormalInvCDF(args2.NumberValue().Float64(), n3.Float64(), probability.Float64())
	return numericResult(f.FunctionError, v)
}

func (f *FunctionLOGINV) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	numberArgTypes(engine, &f.BaseFunction, result, false)
}

func init() { RegisterFunction("LOGINV", newFunctionLOGINV) }
