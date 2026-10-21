package mathsum2

// FunctionWEIBULL 对应 C# Function_WEIBULL（韦伯分布）。
type FunctionWEIBULL struct{ BaseFunction }

func newFunctionWEIBULL(args []Function) Function {
	RequireArgs(args, "Weibull", 4, 4)
	return &FunctionWEIBULL{BaseFunction: NewBase("Weibull", OperandTypeNumber, args)}
}

func (f *FunctionWEIBULL) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
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
	args4 := f.GetBoolean(4, engine, tp)
	if args4.IsErrorOrNone() {
		return args4
	}

	x := args1.NumberValue()
	shape := args2.NumberValue()
	if shape.LessOrEqual(DecimalZero) {
		return f.ParameterError(2)
	}
	scale := args3.NumberValue()
	if scale.LessOrEqual(DecimalZero) {
		return f.ParameterError(3)
	}

	var v float64
	if args4.BooleanValue() {
		v = weibullCDF(shape.Float64(), scale.Float64(), x.Float64())
	} else {
		v = weibullPDF(shape.Float64(), scale.Float64(), x.Float64())
	}
	return numericResult(f.FunctionError, v)
}

func (f *FunctionWEIBULL) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	numberArgTypes(engine, &f.BaseFunction, result, true)
}

func init() { RegisterFunction("WEIBULL", newFunctionWEIBULL) }
