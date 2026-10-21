package mathsum2

import "math"

// FunctionFISHER 对应 C# Function_FISHER。
type FunctionFISHER struct{ BaseFunction }

func newFunctionFISHER(args []Function) Function {
	RequireArgs(args, "Fisher", 1, 1)
	return &FunctionFISHER{BaseFunction: NewBase("Fisher", OperandTypeNumber, args)}
}

func (f *FunctionFISHER) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args1 := f.GetNumber(1, engine, tp)
	if args1.IsErrorOrNone() {
		return args1
	}
	x := args1.NumberValue()
	if x.GreaterOrEqual(DecimalOne) || x.LessOrEqual(DecimalOne.Neg()) {
		return f.ParameterError(1)
	}
	v := 0.5 * math.Log((1+x.Float64())/(1-x.Float64()))
	return numericResult(f.FunctionError, v)
}

func (f *FunctionFISHER) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	f.Arg(0).GetParameterTypes(engine, result, OperandTypeNumber)
}

func init() { RegisterFunction("FISHER", newFunctionFISHER) }
