package mathsum2

// FunctionEXPONDIST 对应 C# Function_EXPONDIST（指数分布）。
type FunctionEXPONDIST struct{ BaseFunction }

func newFunctionEXPONDIST(args []Function) Function {
	RequireArgs(args, "ExpOnDist", 3, 3)
	return &FunctionEXPONDIST{BaseFunction: NewBase("ExpOnDist", OperandTypeNumber, args)}
}

func (f *FunctionEXPONDIST) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args1 := f.GetNumber(1, engine, tp)
	if args1.IsErrorOrNone() {
		return args1
	}
	args2 := f.GetNumber(2, engine, tp)
	if args2.IsErrorOrNone() {
		return args2
	}
	args3 := f.GetBoolean(3, engine, tp)
	if args3.IsErrorOrNone() {
		return args3
	}

	n1 := args1.NumberValue()
	if n1.Less(DecimalZero) {
		return f.ParameterError(1)
	}
	rate := args2.NumberValue()
	if rate.LessOrEqual(DecimalZero) {
		return f.ParameterError(2)
	}

	x := n1.Float64()
	r := rate.Float64()
	if args3.BooleanValue() {
		return CreateDecimal(DecFromFloat(exponentialCDF(r, x)))
	}
	return CreateDecimal(DecFromFloat(exponentialPDF(r, x)))
}

func (f *FunctionEXPONDIST) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	numberArgTypes(engine, &f.BaseFunction, result, true)
}

func init() { RegisterFunction("EXPONDIST", newFunctionEXPONDIST) }
