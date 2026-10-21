package mathsum2

// FunctionBETADIST 对应 C# Function_BETADIST。
type FunctionBETADIST struct{ BaseFunction }

func newFunctionBETADIST(args []Function) Function {
	RequireArgs(args, "BetaDist", 3, 3)
	return &FunctionBETADIST{BaseFunction: NewBase("BetaDist", OperandTypeNumber, args)}
}

func (f *FunctionBETADIST) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
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
	if x.Less(DecimalZero) || x.Greater(DecimalOne) {
		return f.ParameterError(1)
	}
	alpha := args2.NumberValue()
	if alpha.LessOrEqual(DecimalZero) {
		return f.ParameterError(2)
	}
	beta := args3.NumberValue()
	if beta.LessOrEqual(DecimalZero) {
		return f.ParameterError(3)
	}
	return numericResult(f.FunctionError, betaCDF(alpha.Float64(), beta.Float64(), x.Float64()))
}

func (f *FunctionBETADIST) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	numberArgTypes(engine, &f.BaseFunction, result, false)
}

func init() { RegisterFunction("BETADIST", newFunctionBETADIST) }
