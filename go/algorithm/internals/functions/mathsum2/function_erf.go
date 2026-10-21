package mathsum2

import "math"

// FunctionERF 对应 C# Function_ERF。
type FunctionERF struct{ BaseFunction }

func newFunctionERF(args []Function) Function {
	RequireArgs(args, "Erf", 1, 1)
	return &FunctionERF{BaseFunction: NewBase("Erf", OperandTypeNumber, args)}
}

func (f *FunctionERF) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args1 := f.GetNumber(1, engine, tp)
	if args1.IsErrorOrNone() {
		return args1
	}
	return CreateDecimal(DecFromFloat(math.Erf(args1.NumberValue().Float64())))
}

func (f *FunctionERF) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	f.Arg(0).GetParameterTypes(engine, result, OperandTypeNumber)
}

func init() { RegisterFunction("ERF", newFunctionERF) }
