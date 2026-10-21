package mathsum

import "github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals"

// FunctionLARGE 对应 C# Function_LARGE（第 k 大值）。
type FunctionLARGE struct{ BaseFunction }

func newFunctionLARGE(args []Function) Function {
	RequireArgs(args, "Large", 2, 2)
	return &FunctionLARGE{BaseFunction: NewBase("Large", OperandTypeNumber, args)}
}

func (f *FunctionLARGE) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args1 := f.EvalArg(0, engine, tp)
	args1 = f.ConvertToArray(args1, 1)
	if args1.IsErrorOrNone() {
		return args1
	}
	args2 := f.EvalArg(1, engine, tp)
	args2 = f.ConvertToNumber(args2, 2)
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

	k := args2.IntValue() - engine.ExcelIndex
	if k < 0 || k >= len(list) {
		return f.ParameterError(2)
	}
	return CreateDecimal(internals.QuickSelect(list, k, true))
}

func (f *FunctionLARGE) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	f.Arg(0).GetParameterTypes(engine, result, OperandTypeArray)
	f.Arg(1).GetParameterTypes(engine, result, OperandTypeNumber)
}

func init() { RegisterFunction("LARGE", newFunctionLARGE) }
