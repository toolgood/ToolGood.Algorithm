package mathsum2

import "math"

// FunctionERFC 对应 C# Function_ERFC。
type FunctionERFC struct{ BaseFunction }

func newFunctionERFC(args []Function) Function {
	RequireArgs(args, "Erfc", 1, 1)
	return &FunctionERFC{BaseFunction: NewBase("Erfc", OperandTypeNumber, args)}
}

func (f *FunctionERFC) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args1 := f.GetNumber(1, engine, tp)
	if args1.IsErrorOrNone() {
		return args1
	}
	return CreateDecimal(DecFromFloat(math.Erfc(args1.NumberValue().Float64())))
}

func (f *FunctionERFC) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	f.Arg(0).GetParameterTypes(engine, result, OperandTypeNumber)
}

func init() { RegisterFunction("ERFC", newFunctionERFC) }
