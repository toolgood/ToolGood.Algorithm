package mathsum2

// FunctionTINV 对应 C# Function_TINV。
type FunctionTINV struct{ BaseFunction }

func newFunctionTINV(args []Function) Function {
	RequireArgs(args, "TInv", 2, 2)
	return &FunctionTINV{BaseFunction: NewBase("TInv", OperandTypeNumber, args)}
}

func (f *FunctionTINV) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args1 := f.GetNumber(1, engine, tp)
	if args1.IsErrorOrNone() {
		return args1
	}
	args2 := f.GetNumber(2, engine, tp)
	if args2.IsErrorOrNone() {
		return args2
	}

	p := args1.NumberValue()
	// Excel 允许 p=1（TInv(1,df)=0），仅当 p<=0 或 p>1 时报错
	if p.LessOrEqual(DecimalZero) || p.Greater(DecimalOne) {
		return f.ParameterError(1)
	}
	degreesFreedom, ok := tryGetInt(args2)
	if !ok || degreesFreedom <= 0 {
		return f.ParameterError(2)
	}
	v := -studentTInvCDF(0, 1, degreesFreedom, p.Float64()/2)
	return numericResult(f.FunctionError, v)
}

func (f *FunctionTINV) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	f.Arg(0).GetParameterTypes(engine, result, OperandTypeNumber)
	f.Arg(1).GetParameterTypes(engine, result, OperandTypeNumber)
}

func init() { RegisterFunction("TINV", newFunctionTINV) }
