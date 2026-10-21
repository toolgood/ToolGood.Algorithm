package mathsum

import "github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals"

// FunctionQUARTILE 对应 C# Function_QUARTILE（四分位数）。
type FunctionQUARTILE struct{ BaseFunction }

func newFunctionQUARTILE(args []Function) Function {
	RequireArgs(args, "Quartile", 2, 2)
	return &FunctionQUARTILE{BaseFunction: NewBase("Quartile", OperandTypeNumber, args)}
}

func (f *FunctionQUARTILE) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
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

	quant := args2.IntValue()
	if quant < 0 || quant > 4 {
		return f.ParameterError(2)
	}
	switch quant {
	case 0:
		ordered := sortedCopy(list)
		return CreateDecimal(ordered[0])
	case 1:
		return CreateDecimal(quantileCustom(list, MustDecimal("0.25")))
	case 2:
		return CreateDecimal(quantileCustom(list, MustDecimal("0.5")))
	case 3:
		return CreateDecimal(quantileCustom(list, MustDecimal("0.75")))
	}
	ordered := sortedCopy(list)
	return CreateDecimal(ordered[len(ordered)-1])
}

func (f *FunctionQUARTILE) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	f.Arg(0).GetParameterTypes(engine, result, OperandTypeArray)
	f.Arg(1).GetParameterTypes(engine, result, OperandTypeNumber)
}

func init() { RegisterFunction("QUARTILE", newFunctionQUARTILE) }
