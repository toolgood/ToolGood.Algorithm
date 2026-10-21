package mathsum2

import "math"

// FunctionFISHERINV 对应 C# Function_FISHERINV。
type FunctionFISHERINV struct{ BaseFunction }

func newFunctionFISHERINV(args []Function) Function {
	RequireArgs(args, "FisherInv", 1, 1)
	return &FunctionFISHERINV{BaseFunction: NewBase("FisherInv", OperandTypeNumber, args)}
}

func (f *FunctionFISHERINV) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args1 := f.GetNumber(1, engine, tp)
	if args1.IsErrorOrNone() {
		return args1
	}
	x := args1.NumberValue().Float64()
	// (e^2x-1)/(e^2x+1)；2x 超出范围时 tanh 趋近 ±1（与 C# 捕获溢出的处理一致）
	if 2*x > 700 {
		return CreateDecimal(DecimalOne)
	}
	if 2*x < -700 {
		return CreateDecimal(DecimalOne.Neg())
	}
	e := math.Exp(2 * x)
	return CreateDecimal(DecFromFloat((e - 1) / (e + 1)))
}

func (f *FunctionFISHERINV) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	f.Arg(0).GetParameterTypes(engine, result, OperandTypeNumber)
}

func init() { RegisterFunction("FISHERINV", newFunctionFISHERINV) }
