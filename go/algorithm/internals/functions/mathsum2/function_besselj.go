package mathsum2

// FunctionBESSELJ 对应 C# Function_BESSELJ。
type FunctionBESSELJ struct{ BaseFunction }

func newFunctionBESSELJ(args []Function) Function {
	RequireArgs(args, "BesselJ", 2, 2)
	return &FunctionBESSELJ{BaseFunction: NewBase("BesselJ", OperandTypeNumber, args)}
}

func (f *FunctionBESSELJ) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args1 := f.GetNumber(1, engine, tp)
	if args1.IsErrorOrNone() {
		return args1
	}
	args2 := f.GetNumber(2, engine, tp)
	if args2.IsErrorOrNone() {
		return args2
	}

	x := args1.NumberValue()
	// n 取整数部分，且必须为非负整数（与 Excel BESSELJ 一致）
	n, ok := tryGetInt(args2)
	if !ok || n < 0 {
		return f.ParameterError(2)
	}
	return numericResult(f.FunctionError, besselJ(n, x.Float64()))
}

func (f *FunctionBESSELJ) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	f.Arg(0).GetParameterTypes(engine, result, OperandTypeNumber)
	f.Arg(1).GetParameterTypes(engine, result, OperandTypeNumber)
}

func init() { RegisterFunction("BESSELJ", newFunctionBESSELJ) }
