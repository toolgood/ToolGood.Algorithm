package mathsum

import "github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals"

// FunctionSTDEVP 对应 C# Function_STDEVP（总体标准差）。
type FunctionSTDEVP struct{ BaseFunction }

func newFunctionSTDEVP(args []Function) Function {
	RequireArgs(args, "StDevP", 1, -1)
	return &FunctionSTDEVP{BaseFunction: NewBase("StDevP", OperandTypeNumber, args)}
}

func (f *FunctionSTDEVP) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
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
	mean, m2 := DecimalZero, DecimalZero
	for i, v := range list {
		delta := v.Sub(mean)
		mean = mean.Add(delta.Div(DecFromInt(int64(i + 1))))
		m2 = m2.Add(delta.Mul(v.Sub(mean)))
	}
	return CreateDecimal(m2.Div(DecFromInt(int64(len(list)))).Sqrt())
}

func (f *FunctionSTDEVP) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	argTypes(engine, &f.BaseFunction, result, OperandTypeArray, OperandTypeNumber)
}

func init() { RegisterFunction("STDEVP", newFunctionSTDEVP) }
