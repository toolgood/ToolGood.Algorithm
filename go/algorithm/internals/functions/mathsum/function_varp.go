package mathsum

import "github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals"

// FunctionVARP 对应 C# Function_VARP（总体方差）。
type FunctionVARP struct{ BaseFunction }

func newFunctionVARP(args []Function) Function {
	RequireArgs(args, "VarP", 1, -1)
	return &FunctionVARP{BaseFunction: NewBase("VarP", OperandTypeNumber, args)}
}

func (f *FunctionVARP) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
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
	if len(list) == 1 {
		return Zero
	}

	mean, m2 := DecimalZero, DecimalZero
	for i, v := range list {
		delta := v.Sub(mean)
		mean = mean.Add(delta.Div(DecFromInt(int64(i + 1))))
		m2 = m2.Add(delta.Mul(v.Sub(mean)))
	}
	return CreateDecimal(m2.Div(DecFromInt(int64(len(list)))))
}

func (f *FunctionVARP) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	argTypes(engine, &f.BaseFunction, result, OperandTypeArray, OperandTypeNumber)
}

func init() { RegisterFunction("VARP", newFunctionVARP) }
