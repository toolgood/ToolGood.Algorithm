package operator

// 移植自 Internals/Functions/Operator/Function_AND_N.cs。

// FunctionAndN 多参数逻辑与节点（AND 函数）。
type FunctionAndN struct{ BaseFunction }

func newFunctionAND(args []Function) Function {
	return &FunctionAndN{BaseFunction: NewBase("And", OperandTypeBoolean, args)}
}

// Evaluate 逐个求值：非严格模式下遇到 false 即停止。
func (f *FunctionAndN) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	result := true
	for i := 0; i < f.ArgCount(); i++ {
		a := f.GetBoolean(i+1, engine, tp)
		if a.IsErrorOrNone() {
			return a
		}
		if !a.BooleanValue() {
			result = false
			if !engine.UseStrictMode {
				break
			}
		}
	}
	return CreateBool(result)
}

func (f *FunctionAndN) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	for i := 0; i < f.ArgCount(); i++ {
		f.Arg(i).GetParameterTypes(engine, result, OperandTypeBoolean)
	}
}

func init() { RegisterFunction("AND", newFunctionAND) }
