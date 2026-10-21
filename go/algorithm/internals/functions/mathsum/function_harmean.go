package mathsum

import "github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals"

// FunctionHARMEAN 对应 C# Function_HARMEAN（调和平均数）。
type FunctionHARMEAN struct{ BaseFunction }

func newFunctionHARMEAN(args []Function) Function {
	RequireArgs(args, "HarMean", 1, -1)
	return &FunctionHARMEAN{BaseFunction: NewBase("HarMean", OperandTypeNumber, args)}
}

func (f *FunctionHARMEAN) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
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

	sum := DecimalZero
	for _, v := range list {
		// 调和平均数要求所有数据为正数
		if v.LessOrEqual(DecimalZero) {
			return f.FunctionError()
		}
		sum = sum.Add(DecimalOne.Div(v))
	}
	if sum.IsZero() {
		return f.FunctionError()
	}
	return CreateDecimal(DecFromInt(int64(len(list))).Div(sum))
}

func (f *FunctionHARMEAN) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	argTypes(engine, &f.BaseFunction, result, OperandTypeArray, OperandTypeNumber)
}

func init() { RegisterFunction("HARMEAN", newFunctionHARMEAN) }
