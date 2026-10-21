package value

// 移植自 Internals/Functions/Value/Function_Array.cs。

// FunctionArray 数组字面量节点。
type FunctionArray struct{ BaseFunction }

func newFunctionARRAY(args []Function) Function { return NewFunctionArray(args) }

// NewFunctionArray 构造数组节点（对应 C# 的 Function_ARRAY(FunctionBase[])）。
func NewFunctionArray(args []Function) Function {
	RequireArgs(args, "Array", 1, -1)
	return &FunctionArray{BaseFunction: NewBase("Array", OperandTypeArray, args)}
}

// Evaluate 依次求值参数并组装数组。
func (f *FunctionArray) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args := make([]Operand, 0, f.ArgCount())
	if err := f.EvaluateAll(engine, tp, &args); err != nil {
		return err
	}
	return CreateArray(args)
}

func (f *FunctionArray) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	for i := 0; i < f.ArgCount(); i++ {
		f.Arg(i).GetParameterTypes(engine, result, OperandTypeNone)
	}
}

func init() { RegisterFunction("ARRAY", newFunctionARRAY) }
