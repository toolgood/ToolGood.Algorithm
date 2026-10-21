package mathsum

import "github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals"

// FunctionRANK 对应 C# Function_RANK（排名）。
type FunctionRANK struct{ BaseFunction }

func newFunctionRANK(args []Function) Function {
	RequireArgs(args, "RANK", 2, 3)
	return &FunctionRANK{BaseFunction: NewBase("RANK", OperandTypeNumber, args)}
}

func (f *FunctionRANK) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	if f.ArgCount() < 2 {
		return f.ParameterError(1)
	}

	numArg := f.GetNumber(1, engine, tp)
	if numArg.IsErrorOrNone() {
		return numArg
	}
	num := numArg.NumberValue()

	arrayArg := f.GetArray(2, engine, tp)
	if arrayArg.IsErrorOrNone() {
		return arrayArg
	}

	order := 0
	if f.ArgCount() > 2 {
		orderArg := f.GetNumber(3, engine, tp)
		if orderArg.IsErrorOrNone() {
			return orderArg
		}
		order = orderArg.IntValue()
	}

	values := make([]Decimal, 0, len(arrayArg.ArrayValue()))
	for _, item := range arrayArg.ArrayValue() {
		if item.IsNumber() {
			values = append(values, item.NumberValue())
		}
	}
	if len(values) == 0 {
		return f.ParameterError(2)
	}

	descending := order == 0
	rank := internals.GetRank(values, num, descending)
	if rank == 0 {
		// 数值未出现在数组中，Excel 返回 #N/A，此处复用通用函数错误表示
		return f.FunctionError()
	}
	return CreateInt(rank)
}

func (f *FunctionRANK) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	f.Arg(0).GetParameterTypes(engine, result, OperandTypeNumber)
	f.Arg(1).GetParameterTypes(engine, result, OperandTypeArray)
	if f.ArgCount() > 2 {
		f.Arg(2).GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

func init() { RegisterFunction("RANK", newFunctionRANK) }
