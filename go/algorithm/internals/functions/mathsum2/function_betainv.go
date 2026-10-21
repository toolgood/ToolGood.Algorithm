package mathsum2

// FunctionBETAINV 对应 C# Function_BETAINV。
type FunctionBETAINV struct{ BaseFunction }

func newFunctionBETAINV(args []Function) Function {
	RequireArgs(args, "BetaInv", 3, 3)
	return &FunctionBETAINV{BaseFunction: NewBase("BetaInv", OperandTypeNumber, args)}
}

func (f *FunctionBETAINV) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
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
	// 概率必须为开区间 (0,1)，与 Excel BETA.INV 的 #NUM! 判定保持一致
	if p.LessOrEqual(DecimalZero) || p.GreaterOrEqual(DecimalOne) {
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
	return numericResult(f.FunctionError, betaInvCDF(alpha.Float64(), beta.Float64(), p.Float64()))
}

func (f *FunctionBETAINV) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	numberArgTypes(engine, &f.BaseFunction, result, false)
}

func init() { RegisterFunction("BETAINV", newFunctionBETAINV) }
