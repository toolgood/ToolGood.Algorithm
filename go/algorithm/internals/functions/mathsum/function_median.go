package mathsum

import "github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals"

// FunctionMEDIAN 对应 C# Function_MEDIAN（中位数）。
type FunctionMEDIAN struct{ BaseFunction }

func newFunctionMEDIAN(args []Function) Function {
	RequireArgs(args, "Median", 1, -1)
	return &FunctionMEDIAN{BaseFunction: NewBase("Median", OperandTypeNumber, args)}
}

func (f *FunctionMEDIAN) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	var args []Operand
	if err := f.EvaluateAll(engine, tp, &args); err != nil {
		return err
	}

	var list []Decimal
	if !internals.FlattenToBigDecimalList(args, &list) {
		return f.FunctionError()
	}
	if len(list) == 0 {
		return f.FunctionError()
	}

	n := len(list)
	if n == 1 {
		return CreateDecimal(list[0])
	}
	mid := n / 2
	if n%2 == 0 {
		a := internals.QuickSelect(list, mid-1, false)
		b := internals.QuickSelect(list, mid, false)
		return CreateDecimal(a.Add(b).Div(DecFromInt(2)))
	}
	return CreateDecimal(internals.QuickSelect(list, mid, false))
}

func (f *FunctionMEDIAN) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	argTypes(engine, &f.BaseFunction, result, OperandTypeArray, OperandTypeNumber)
}

func init() { RegisterFunction("MEDIAN", newFunctionMEDIAN) }
