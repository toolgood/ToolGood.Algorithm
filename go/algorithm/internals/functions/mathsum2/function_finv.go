package mathsum2

// FunctionFINV 对应 C# Function_FINV。
type FunctionFINV struct{ BaseFunction }

func newFunctionFINV(args []Function) Function {
	RequireArgs(args, "FInv", 3, 3)
	return &FunctionFINV{BaseFunction: NewBase("FInv", OperandTypeNumber, args)}
}

func (f *FunctionFINV) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
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
	degreesFreedom, ok := tryGetInt(args2)
	if !ok || degreesFreedom <= 0 {
		return f.ParameterError(2)
	}
	degreesFreedom2, ok := tryGetInt(args3)
	if !ok || degreesFreedom2 <= 0 {
		return f.ParameterError(3)
	}

	v := fisherSnedecorInvCDF(degreesFreedom, degreesFreedom2, 1-p.Float64())
	return numericResult(f.FunctionError, v)
}

func (f *FunctionFINV) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	numberArgTypes(engine, &f.BaseFunction, result, false)
}

func init() { RegisterFunction("FINV", newFunctionFINV) }
