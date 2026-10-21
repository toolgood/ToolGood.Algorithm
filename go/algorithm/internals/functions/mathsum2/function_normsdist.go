package mathsum2

// FunctionNORMSDIST 对应 C# Function_NORMSDIST。
type FunctionNORMSDIST struct{ BaseFunction }

func newFunctionNORMSDIST(args []Function) Function {
	RequireArgs(args, "NormSDist", 1, 1)
	return &FunctionNORMSDIST{BaseFunction: NewBase("NormSDist", OperandTypeNumber, args)}
}

func (f *FunctionNORMSDIST) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args1 := f.GetNumber(1, engine, tp)
	if args1.IsErrorOrNone() {
		return args1
	}
	num := args1.NumberValue().Float64()
	return CreateDecimal(DecFromFloat(normalCDF(0, 1, num)))
}

func (f *FunctionNORMSDIST) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	f.Arg(0).GetParameterTypes(engine, result, OperandTypeNumber)
}

func init() { RegisterFunction("NORMSDIST", newFunctionNORMSDIST) }
