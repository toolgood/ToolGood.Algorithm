package mathsum

import "github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals"

// FunctionCOUNT 对应 C# Function_COUNT。
type FunctionCOUNT struct{ BaseFunction }

func newFunctionCOUNT(args []Function) Function {
	RequireArgs(args, "Count", 1, -1)
	return &FunctionCOUNT{BaseFunction: NewBase("Count", OperandTypeNumber, args)}
}

func (f *FunctionCOUNT) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	var args []Operand
	if err := f.EvaluateAll(engine, tp, &args); err != nil {
		return err
	}

	var list []Operand
	if !internals.FlattenToList(args, &list) {
		return f.FunctionError()
	}

	// 与 Excel 一致：只统计可转为数值的项，文本、空值等一律忽略
	count := 0
	for _, item := range list {
		if !item.ToNumber("").IsErrorOrNone() {
			count++
		}
	}
	return CreateInt(count)
}

func (f *FunctionCOUNT) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	argTypes(engine, &f.BaseFunction, result, OperandTypeArray, OperandTypeNone)
}

func init() { RegisterFunction("COUNT", newFunctionCOUNT) }
