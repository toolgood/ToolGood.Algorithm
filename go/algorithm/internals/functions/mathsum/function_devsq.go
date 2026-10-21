package mathsum

import "github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals"

// FunctionDEVSQ 对应 C# Function_DEVSQ（偏差平方和）。
type FunctionDEVSQ struct{ BaseFunction }

func newFunctionDEVSQ(args []Function) Function {
	RequireArgs(args, "DevSq", 1, -1)
	return &FunctionDEVSQ{BaseFunction: NewBase("DevSq", OperandTypeNumber, args)}
}

func (f *FunctionDEVSQ) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
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
	return CreateDecimal(m2)
}

func (f *FunctionDEVSQ) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	argTypes(engine, &f.BaseFunction, result, OperandTypeArray, OperandTypeNumber)
}

func init() { RegisterFunction("DEVSQ", newFunctionDEVSQ) }
