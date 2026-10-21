package mathsum

import "github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals"

// FunctionAVEDEV 对应 C# Function_AVEDEV（平均绝对偏差）。
type FunctionAVEDEV struct{ BaseFunction }

func newFunctionAVEDEV(args []Function) Function {
	RequireArgs(args, "AveDev", 1, -1)
	return &FunctionAVEDEV{BaseFunction: NewBase("AveDev", OperandTypeNumber, args)}
}

func (f *FunctionAVEDEV) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	var args []Operand
	if err := f.EvaluateAll(engine, tp, &args); err != nil {
		return err
	}

	var list []Decimal
	if !internals.FlattenToBigDecimalList(args, &list) {
		return f.FunctionError()
	}
	// Excel 对空数据集返回 #NUM!，此处用 NumError() 表示
	if len(list) == 0 {
		return f.NumError()
	}
	sum := DecimalZero
	for _, v := range list {
		sum = sum.Add(v)
	}
	avg := sum.Div(DecFromInt(int64(len(list))))
	sum2 := DecimalZero
	for _, v := range list {
		sum2 = sum2.Add(v.Sub(avg).Abs())
	}
	return CreateDecimal(sum2.Div(DecFromInt(int64(len(list)))))
}

func (f *FunctionAVEDEV) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	argTypes(engine, &f.BaseFunction, result, OperandTypeArray, OperandTypeNumber)
}

func init() { RegisterFunction("AVEDEV", newFunctionAVEDEV) }
