package mathsum

// FunctionSERIESSUM 对应 C# Function_SERIESSUM（幂级数求和）。
type FunctionSERIESSUM struct{ BaseFunction }

func newFunctionSERIESSUM(args []Function) Function {
	RequireArgs(args, "SERIESSUM", 4, 4)
	return &FunctionSERIESSUM{BaseFunction: NewBase("SERIESSUM", OperandTypeNumber, args)}
}

func (f *FunctionSERIESSUM) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	xArg := f.GetNumber(1, engine, tp)
	if xArg.IsErrorOrNone() {
		return xArg
	}
	x := xArg.NumberValue()

	nArg := f.GetNumber(2, engine, tp)
	if nArg.IsErrorOrNone() {
		return nArg
	}
	n := nArg.NumberValue()

	mArg := f.GetNumber(3, engine, tp)
	if mArg.IsErrorOrNone() {
		return mArg
	}
	m := mArg.NumberValue()

	coefficientsArg := f.GetArray(4, engine, tp)
	if coefficientsArg.IsErrorOrNone() {
		return coefficientsArg
	}

	result := DecimalZero
	for i, coef := range coefficientsArg.ArrayValue() {
		if !coef.IsNumber() {
			continue
		}
		power := n.Add(DecFromInt(int64(i)).Mul(m))
		p, ok := powDecimal(x, power)
		if !ok {
			// 负底数非整数幂等非法运算，与 Excel 返回 #NUM! 保持一致
			return f.FunctionError()
		}
		term, ok := mulChecked(coef.NumberValue(), p)
		if !ok {
			return f.FunctionError()
		}
		result, ok = addChecked(result, term)
		if !ok {
			return f.FunctionError()
		}
	}
	return CreateDecimal(result)
}

func (f *FunctionSERIESSUM) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	f.Arg(0).GetParameterTypes(engine, result, OperandTypeNumber)
	f.Arg(1).GetParameterTypes(engine, result, OperandTypeNumber)
	f.Arg(2).GetParameterTypes(engine, result, OperandTypeNumber)
	f.Arg(3).GetParameterTypes(engine, result, OperandTypeArray)
}

func init() { RegisterFunction("SERIESSUM", newFunctionSERIESSUM) }
