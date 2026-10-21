package mathsum

import "github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals"

// FunctionPERCENTRANK 对应 C# Function_PERCENTRANK（百分位排名）。
type FunctionPERCENTRANK struct{ BaseFunction }

func newFunctionPERCENTRANK(args []Function) Function {
	RequireArgs(args, "PercentRank", 2, 3)
	return &FunctionPERCENTRANK{BaseFunction: NewBase("PercentRank", OperandTypeNumber, args)}
}

func (f *FunctionPERCENTRANK) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
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

	v := quantileRank(list, args2.NumberValue())
	d := 3
	if f.Arg(2) != nil {
		args3 := f.GetNumber(3, engine, tp)
		if args3.IsErrorOrNone() {
			return args3
		}
		d = args3.IntValue()
		// Math.Round(decimal,int) 最多支持 28 位小数，超过会抛异常，直接报参数错误
		if d < 0 || d > 28 {
			return f.ParameterError(3)
		}
	}
	return CreateDecimal(v.RoundHalfUp(int32(d)))
}

func (f *FunctionPERCENTRANK) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	f.Arg(0).GetParameterTypes(engine, result, OperandTypeArray)
	f.Arg(1).GetParameterTypes(engine, result, OperandTypeNumber)
	if f.Arg(2) != nil {
		f.Arg(2).GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

func init() { RegisterFunction("PERCENTRANK", newFunctionPERCENTRANK) }
