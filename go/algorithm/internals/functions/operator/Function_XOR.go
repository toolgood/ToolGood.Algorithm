package operator

// 移植自 Internals/Functions/Operator/Function_XOR.cs。

// FunctionXor 多参数异或节点（XOR 函数）。
type FunctionXor struct{ BaseFunction }

func newFunctionXOR(args []Function) Function {
	return &FunctionXor{BaseFunction: NewBase("Xor", OperandTypeBoolean, args)}
}

// Evaluate 真值个数为奇数时返回 true。
func (f *FunctionXor) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	trueCount := 0
	for i := 0; i < f.ArgCount(); i++ {
		a := f.GetBoolean(i+1, engine, tp)
		if a.IsErrorOrNone() {
			return a
		}
		if a.BooleanValue() {
			trueCount++
		}
	}
	return CreateBool(trueCount%2 == 1)
}

func (f *FunctionXor) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	for i := 0; i < f.ArgCount(); i++ {
		f.Arg(i).GetParameterTypes(engine, result, OperandTypeBoolean)
	}
}

func init() { RegisterFunction("XOR", newFunctionXOR) }
