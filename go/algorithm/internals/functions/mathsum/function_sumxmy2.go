package mathsum

// FunctionSUMXMY2 对应 C# Function_SUMXMY2（Σ(x-y)²）。
type FunctionSUMXMY2 struct{ BaseFunction }

func newFunctionSUMXMY2(args []Function) Function {
	RequireArgs(args, "SUMXMY2", 2, 2)
	return &FunctionSUMXMY2{BaseFunction: NewBase("SUMXMY2", OperandTypeNumber, args)}
}

func (f *FunctionSUMXMY2) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	arrayX, arrayY, errOp := pairedNumberArrays(&f.BaseFunction, engine, tp)
	if errOp != nil {
		return errOp
	}

	result := DecimalZero
	for i := range arrayX {
		diff, ok := subChecked(arrayX[i], arrayY[i])
		if !ok {
			return f.FunctionError()
		}
		square, ok := mulChecked(diff, diff)
		if !ok {
			return f.FunctionError()
		}
		result, ok = addChecked(result, square)
		if !ok {
			return f.FunctionError()
		}
	}
	return CreateDecimal(result)
}

func (f *FunctionSUMXMY2) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	f.Arg(0).GetParameterTypes(engine, result, OperandTypeArray)
	f.Arg(1).GetParameterTypes(engine, result, OperandTypeArray)
}

func init() { RegisterFunction("SUMXMY2", newFunctionSUMXMY2) }
