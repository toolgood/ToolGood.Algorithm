package mathsum

import "github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals"

// FunctionPERCENTILE 对应 C# Function_PERCENTILE（百分位数）。
type FunctionPERCENTILE struct{ BaseFunction }

func newFunctionPERCENTILE(args []Function) Function {
	RequireArgs(args, "Percentile", 2, 2)
	return &FunctionPERCENTILE{BaseFunction: NewBase("Percentile", OperandTypeNumber, args)}
}

func (f *FunctionPERCENTILE) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args1 := f.GetArray(1, engine, tp)
	if args1.IsErrorOrNone() {
		return args1
	}
	args2 := f.GetNumber(2, engine, tp)
	if args2.IsErrorOrNone() {
		return args2
	}

	var list []Decimal
	if !internals.FlattenToNumberList(args1, &list) {
		return f.ParameterError(1)
	}
	if len(list) == 0 {
		return f.ParameterError(1)
	}
	k := args2.NumberValue()
	if k.Less(DecimalZero) || k.Greater(DecimalOne) {
		return f.ParameterError(2)
	}
	return CreateDecimal(quantileCustom(list, k))
}

func (f *FunctionPERCENTILE) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	f.Arg(0).GetParameterTypes(engine, result, OperandTypeArray)
	f.Arg(1).GetParameterTypes(engine, result, OperandTypeNumber)
}

func init() { RegisterFunction("PERCENTILE", newFunctionPERCENTILE) }
