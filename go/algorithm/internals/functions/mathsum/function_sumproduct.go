package mathsum

// FunctionSUMPRODUCT 对应 C# Function_SUMPRODUCT（数组对应元素乘积之和）。
type FunctionSUMPRODUCT struct{ BaseFunction }

func newFunctionSUMPRODUCT(args []Function) Function {
	RequireArgs(args, "SUMPRODUCT", 1, -1)
	return &FunctionSUMPRODUCT{BaseFunction: NewBase("SUMPRODUCT", OperandTypeNumber, args)}
}

func (f *FunctionSUMPRODUCT) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	arrays := make([][]Decimal, 0, f.ArgCount())
	for i := 0; i < f.ArgCount(); i++ {
		arg := f.GetArray(i+1, engine, tp)
		if arg.IsErrorOrNone() {
			return arg
		}
		list := make([]Decimal, 0, len(arg.ArrayValue()))
		for _, item := range arg.ArrayValue() {
			// 与 Excel 一致：非数值项按 0 参与运算，保留占位以保证各数组元素位置对齐
			if item.IsNumber() {
				list = append(list, item.NumberValue())
			} else {
				list = append(list, DecimalZero)
			}
		}
		arrays = append(arrays, list)
	}

	// 与 Excel 一致：各数组长度必须相同，否则返回错误
	length := len(arrays[0])
	for i := 1; i < len(arrays); i++ {
		if len(arrays[i]) != length {
			return f.FunctionError()
		}
	}

	result := DecimalZero
	for i := 0; i < length; i++ {
		product := DecimalOne
		for j := range arrays {
			product = product.Mul(arrays[j][i])
		}
		result = result.Add(product)
	}
	return CreateDecimal(result)
}

func (f *FunctionSUMPRODUCT) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	for i := 0; i < f.ArgCount(); i++ {
		f.Arg(i).GetParameterTypes(engine, result, OperandTypeArray)
	}
}

func init() { RegisterFunction("SUMPRODUCT", newFunctionSUMPRODUCT) }
