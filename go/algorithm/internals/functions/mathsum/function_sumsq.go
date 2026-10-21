package mathsum

import "github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals"

// FunctionSUMSQ 对应 C# Function_SUMSQ（平方和）。
type FunctionSUMSQ struct{ BaseFunction }

func newFunctionSUMSQ(args []Function) Function {
	RequireArgs(args, "SumSq", 1, -1)
	return &FunctionSUMSQ{BaseFunction: NewBase("SumSq", OperandTypeNumber, args)}
}

func (f *FunctionSUMSQ) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args := make([]Operand, 0, f.ArgCount())
	for i := 0; i < f.ArgCount(); i++ {
		aa := f.EvalArg(i, engine, tp)
		if aa.IsErrorOrNone() {
			return aa
		}
		args = append(args, aa)
	}

	var list []Decimal
	if !internals.FlattenToBigDecimalList(args, &list) {
		return f.FunctionError()
	}

	d := DecimalZero
	for _, v := range list {
		var ok bool
		square, ok := mulChecked(v, v)
		if !ok {
			// decimal 溢出时统一转为函数错误，避免向调用方抛出异常
			return f.FunctionError()
		}
		d, ok = addChecked(d, square)
		if !ok {
			return f.FunctionError()
		}
	}
	return CreateDecimal(d)
}

func (f *FunctionSUMSQ) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	argTypes(engine, &f.BaseFunction, result, OperandTypeArray, OperandTypeNumber)
}

func init() { RegisterFunction("SUMSQ", newFunctionSUMSQ) }
