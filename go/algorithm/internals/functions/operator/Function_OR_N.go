package operator

// 移植自 Internals/Functions/Operator/Function_OR_N.cs。

// FunctionOrN 多参数逻辑或节点（OR 函数）。
type FunctionOrN struct{ BaseFunction }

func newFunctionOR(args []Function) Function {
	return &FunctionOrN{BaseFunction: NewBase("Or", OperandTypeBoolean, args)}
}

// Evaluate 逐个求值：非严格模式下遇到 true 即停止。
func (f *FunctionOrN) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	result := false
	for i := 0; i < f.ArgCount(); i++ {
		a := f.GetBoolean(i+1, engine, tp)
		if a.IsErrorOrNone() {
			return a
		}
		if a.BooleanValue() {
			result = true
			if !engine.UseStrictMode {
				break
			}
		}
	}
	return CreateBool(result)
}

func (f *FunctionOrN) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	for i := 0; i < f.ArgCount(); i++ {
		f.Arg(i).GetParameterTypes(engine, result, OperandTypeBoolean)
	}
}

func init() { RegisterFunction("OR", newFunctionOR) }
