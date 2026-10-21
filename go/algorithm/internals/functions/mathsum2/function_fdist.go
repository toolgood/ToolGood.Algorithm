package mathsum2

// FunctionFDIST 对应 C# Function_FDIST。
type FunctionFDIST struct{ BaseFunction }

func newFunctionFDIST(args []Function) Function {
	RequireArgs(args, "FDist", 3, 3)
	return &FunctionFDIST{BaseFunction: NewBase("FDist", OperandTypeNumber, args)}
}

func (f *FunctionFDIST) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
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
	// Excel 允许 x=0（FDIST(0,d1,d2)=1），仅当 x<0 时报错
	if x.Less(DecimalZero) {
		return f.ParameterError(1)
	}
	degreesFreedom, ok := tryGetInt(args2)
	if !ok || degreesFreedom <= 0 {
		return f.ParameterError(2)
	}
	degreesFreedom2, ok := tryGetInt(args3)
	if !ok || degreesFreedom2 <= 0 {
		return f.ParameterError(3)
	}

	v := 1 - fisherSnedecorCDF(degreesFreedom, degreesFreedom2, x.Float64())
	return numericResult(f.FunctionError, v)
}

func (f *FunctionFDIST) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	numberArgTypes(engine, &f.BaseFunction, result, false)
}

func init() { RegisterFunction("FDIST", newFunctionFDIST) }
