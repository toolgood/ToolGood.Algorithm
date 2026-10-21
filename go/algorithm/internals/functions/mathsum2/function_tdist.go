package mathsum2

// FunctionTDIST 对应 C# Function_TDIST。
type FunctionTDIST struct{ BaseFunction }

func newFunctionTDIST(args []Function) Function {
	RequireArgs(args, "TDist", 3, 3)
	return &FunctionTDIST{BaseFunction: NewBase("TDist", OperandTypeNumber, args)}
}

func (f *FunctionTDIST) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
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

	x := args1.NumberValue()
	// Excel 允许 x=0（TDIST(0,df,1)=0.5, TDIST(0,df,2)=1），仅当 x<0 时报错
	if x.Less(DecimalZero) {
		return f.ParameterError(1)
	}
	degreesFreedom, ok := tryGetInt(args2)
	if !ok || degreesFreedom <= 0 {
		return f.ParameterError(2)
	}
	tails, ok := tryGetInt(args3)
	if !ok || tails < 1 || tails > 2 {
		return f.ParameterError(3)
	}

	xv := x.Float64()
	var v float64
	switch tails {
	case 1:
		v = 1 - studentTCDF(0, 1, degreesFreedom, xv)
	case 2:
		v = 1 - studentTCDF(0, 1, degreesFreedom, xv) + studentTCDF(0, 1, degreesFreedom, -xv)
	}
	return numericResult(f.FunctionError, v)
}

func (f *FunctionTDIST) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	numberArgTypes(engine, &f.BaseFunction, result, false)
}

func init() { RegisterFunction("TDIST", newFunctionTDIST) }
