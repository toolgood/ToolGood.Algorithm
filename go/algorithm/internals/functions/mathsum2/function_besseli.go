package mathsum2

// FunctionBESSELI 对应 C# Function_BESSELI。
type FunctionBESSELI struct{ BaseFunction }

func newFunctionBESSELI(args []Function) Function {
	RequireArgs(args, "BesselI", 2, 2)
	return &FunctionBESSELI{BaseFunction: NewBase("BesselI", OperandTypeNumber, args)}
}

func (f *FunctionBESSELI) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args1 := f.GetNumber(1, engine, tp)
	if args1.IsErrorOrNone() {
		return args1
	}
	args2 := f.GetNumber(2, engine, tp)
	if args2.IsErrorOrNone() {
		return args2
	}

	x := args1.NumberValue()
	// n 取整数部分，且必须为非负整数（与 Excel BESSELI 一致）
	n, ok := tryGetInt(args2)
	if !ok || n < 0 {
		return f.ParameterError(2)
	}
	return numericResult(f.FunctionError, besselI(n, x.Float64()))
}

func (f *FunctionBESSELI) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	f.Arg(0).GetParameterTypes(engine, result, OperandTypeNumber)
	f.Arg(1).GetParameterTypes(engine, result, OperandTypeNumber)
}

func init() { RegisterFunction("BESSELI", newFunctionBESSELI) }
