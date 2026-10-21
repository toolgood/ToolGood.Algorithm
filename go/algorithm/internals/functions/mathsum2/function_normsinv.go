package mathsum2

// FunctionNORMSINV 对应 C# Function_NORMSINV。
type FunctionNORMSINV struct{ BaseFunction }

func newFunctionNORMSINV(args []Function) Function {
	RequireArgs(args, "NormSInv", 1, 1)
	return &FunctionNORMSINV{BaseFunction: NewBase("NormSInv", OperandTypeNumber, args)}
}

func (f *FunctionNORMSINV) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args1 := f.GetNumber(1, engine, tp)
	if args1.IsErrorOrNone() {
		return args1
	}
	p := args1.NumberValue()
	if p.LessOrEqual(DecimalZero) || p.GreaterOrEqual(DecimalOne) {
		return f.ParameterError(1)
	}
	return numericResult(f.FunctionError, normalInvCDF(0, 1, p.Float64()))
}

func (f *FunctionNORMSINV) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	f.Arg(0).GetParameterTypes(engine, result, OperandTypeNumber)
}

func init() { RegisterFunction("NORMSINV", newFunctionNORMSINV) }
