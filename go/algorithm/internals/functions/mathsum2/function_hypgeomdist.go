package mathsum2

// FunctionHYPGEOMDIST 对应 C# Function_HYPGEOMDIST（超几何分布）。
type FunctionHYPGEOMDIST struct{ BaseFunction }

func newFunctionHYPGEOMDIST(args []Function) Function {
	RequireArgs(args, "HypgeomDist", 4, 4)
	return &FunctionHYPGEOMDIST{BaseFunction: NewBase("HypgeomDist", OperandTypeNumber, args)}
}

func (f *FunctionHYPGEOMDIST) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
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
	args4 := f.GetNumber(4, engine, tp)
	if args4.IsErrorOrNone() {
		return args4
	}

	k, ok := tryGetInt(args1)
	if !ok || k < 0 {
		return f.ParameterError(1)
	}
	draws, ok := tryGetInt(args2)
	if !ok || draws < 0 {
		return f.ParameterError(2)
	}
	success, ok := tryGetInt(args3)
	if !ok || success < 0 {
		return f.ParameterError(3)
	}
	population, ok := tryGetInt(args4)
	if !ok || population < 0 {
		return f.ParameterError(4)
	}
	// 交叉约束：k 必须落在 [max(0, draws+success-population), min(draws, success)] 区间内
	if k > draws {
		return f.ParameterError(1)
	}
	if k > success {
		return f.ParameterError(1)
	}
	if k < draws+success-population {
		return f.ParameterError(1)
	}
	if success > population {
		return f.ParameterError(3)
	}
	if draws > population {
		return f.ParameterError(2)
	}
	return numericResult(f.FunctionError, hypergeomPMF(population, success, draws, k))
}

func (f *FunctionHYPGEOMDIST) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	numberArgTypes(engine, &f.BaseFunction, result, false)
}

func init() { RegisterFunction("HYPGEOMDIST", newFunctionHYPGEOMDIST) }
