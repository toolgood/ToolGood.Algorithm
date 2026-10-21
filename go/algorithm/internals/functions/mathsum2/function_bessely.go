package mathsum2

// FunctionBESSELY 对应 C# Function_BESSELY。
type FunctionBESSELY struct{ BaseFunction }

func newFunctionBESSELY(args []Function) Function {
	RequireArgs(args, "BesselY", 2, 2)
	return &FunctionBESSELY{BaseFunction: NewBase("BesselY", OperandTypeNumber, args)}
}

func (f *FunctionBESSELY) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args1 := f.GetNumber(1, engine, tp)
	if args1.IsErrorOrNone() {
		return args1
	}
	args2 := f.GetNumber(2, engine, tp)
	if args2.IsErrorOrNone() {
		return args2
	}

	x := args1.NumberValue()
	n, ok := tryGetInt(args2)
	if !ok {
		return f.ParameterError(2)
	}

	if x.LessOrEqual(DecimalZero) {
		return f.ParameterError(1)
	}
	if n < 0 {
		return f.ParameterError(2)
	}
	return numericResult(f.FunctionError, besselY(n, x.Float64()))
}

func (f *FunctionBESSELY) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	f.Arg(0).GetParameterTypes(engine, result, OperandTypeNumber)
	f.Arg(1).GetParameterTypes(engine, result, OperandTypeNumber)
}

func init() { RegisterFunction("BESSELY", newFunctionBESSELY) }
