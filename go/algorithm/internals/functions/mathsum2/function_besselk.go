package mathsum2

// FunctionBESSELK 对应 C# Function_BESSELK。
type FunctionBESSELK struct{ BaseFunction }

func newFunctionBESSELK(args []Function) Function {
	RequireArgs(args, "BesselK", 2, 2)
	return &FunctionBESSELK{BaseFunction: NewBase("BesselK", OperandTypeNumber, args)}
}

func (f *FunctionBESSELK) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
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
	return numericResult(f.FunctionError, besselK(n, x.Float64()))
}

func (f *FunctionBESSELK) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	f.Arg(0).GetParameterTypes(engine, result, OperandTypeNumber)
	f.Arg(1).GetParameterTypes(engine, result, OperandTypeNumber)
}

func init() { RegisterFunction("BESSELK", newFunctionBESSELK) }
