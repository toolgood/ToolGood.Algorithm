package mathsum

// FunctionSUMX2MY2 对应 C# Function_SUMX2MY2（Σ(x²-y²)）。
type FunctionSUMX2MY2 struct{ BaseFunction }

func newFunctionSUMX2MY2(args []Function) Function {
	RequireArgs(args, "SUMX2MY2", 2, 2)
	return &FunctionSUMX2MY2{BaseFunction: NewBase("SUMX2MY2", OperandTypeNumber, args)}
}

func (f *FunctionSUMX2MY2) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	arrayX, arrayY, errOp := pairedNumberArrays(&f.BaseFunction, engine, tp)
	if errOp != nil {
		return errOp
	}

	result := DecimalZero
	for i := range arrayX {
		x2, ok := mulChecked(arrayX[i], arrayX[i])
		if !ok {
			return f.FunctionError()
		}
		y2, ok := mulChecked(arrayY[i], arrayY[i])
		if !ok {
			return f.FunctionError()
		}
		diff, ok := subChecked(x2, y2)
		if !ok {
			return f.FunctionError()
		}
		result, ok = addChecked(result, diff)
		if !ok {
			return f.FunctionError()
		}
	}
	return CreateDecimal(result)
}

func (f *FunctionSUMX2MY2) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	f.Arg(0).GetParameterTypes(engine, result, OperandTypeArray)
	f.Arg(1).GetParameterTypes(engine, result, OperandTypeArray)
}

func init() { RegisterFunction("SUMX2MY2", newFunctionSUMX2MY2) }
