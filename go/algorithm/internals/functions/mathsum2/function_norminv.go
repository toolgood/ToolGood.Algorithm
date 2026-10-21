package mathsum2

// FunctionNORMINV 对应 C# Function_NORMINV。
type FunctionNORMINV struct{ BaseFunction }

func newFunctionNORMINV(args []Function) Function {
	RequireArgs(args, "NormInv", 3, 3)
	return &FunctionNORMINV{BaseFunction: NewBase("NormInv", OperandTypeNumber, args)}
}

func (f *FunctionNORMINV) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
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

	p := args1.NumberValue()
	if p.LessOrEqual(DecimalZero) || p.GreaterOrEqual(DecimalOne) {
		return f.ParameterError(1)
	}
	avg := args2.NumberValue()
	stdev := args3.NumberValue()
	if stdev.LessOrEqual(DecimalZero) {
		return f.ParameterError(3)
	}
	return numericResult(f.FunctionError, normalInvCDF(avg.Float64(), stdev.Float64(), p.Float64()))
}

func (f *FunctionNORMINV) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	numberArgTypes(engine, &f.BaseFunction, result, false)
}

func init() { RegisterFunction("NORMINV", newFunctionNORMINV) }
