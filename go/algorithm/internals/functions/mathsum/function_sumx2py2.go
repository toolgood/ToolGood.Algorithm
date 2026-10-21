package mathsum

// FunctionSUMX2PY2 对应 C# Function_SUMX2PY2（Σ(x²+y²)）。
type FunctionSUMX2PY2 struct{ BaseFunction }

func newFunctionSUMX2PY2(args []Function) Function {
	RequireArgs(args, "SUMX2PY2", 2, 2)
	return &FunctionSUMX2PY2{BaseFunction: NewBase("SUMX2PY2", OperandTypeNumber, args)}
}

func (f *FunctionSUMX2PY2) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
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
		sum, ok := addChecked(x2, y2)
		if !ok {
			return f.FunctionError()
		}
		result, ok = addChecked(result, sum)
		if !ok {
			return f.FunctionError()
		}
	}
	return CreateDecimal(result)
}

func (f *FunctionSUMX2PY2) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	f.Arg(0).GetParameterTypes(engine, result, OperandTypeArray)
	f.Arg(1).GetParameterTypes(engine, result, OperandTypeArray)
}

func init() { RegisterFunction("SUMX2PY2", newFunctionSUMX2PY2) }
