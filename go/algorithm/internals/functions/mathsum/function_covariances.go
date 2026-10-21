package mathsum

import "github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals"

// FunctionCOVARIANCES 对应 C# Function_COVARIANCES（样本协方差，除以 n-1）。
type FunctionCOVARIANCES struct{ BaseFunction }

func newFunctionCOVARIANCES(args []Function) Function {
	RequireArgs(args, "Covariances", 2, 2)
	return &FunctionCOVARIANCES{BaseFunction: NewBase("Covariances", OperandTypeNumber, args)}
}

func (f *FunctionCOVARIANCES) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args1 := f.EvalArg(0, engine, tp)
	if args1.IsErrorOrNone() {
		return args1
	}
	args2 := f.EvalArg(1, engine, tp)
	if args2.IsErrorOrNone() {
		return args2
	}
	var list1, list2 []Decimal
	if !internals.FlattenToNumberList(args1, &list1) {
		return f.ParameterError(1)
	}
	if !internals.FlattenToNumberList(args2, &list2) {
		return f.ParameterError(2)
	}
	if len(list1) != len(list2) {
		return f.FunctionError()
	}
	if len(list1) < 2 {
		return f.FunctionError()
	}

	mean1, mean2, c := DecimalZero, DecimalZero, DecimalZero
	for i := range list1 {
		delta1 := list1[i].Sub(mean1)
		delta2 := list2[i].Sub(mean2)
		mean1 = mean1.Add(delta1.Div(DecFromInt(int64(i + 1))))
		mean2 = mean2.Add(delta2.Div(DecFromInt(int64(i + 1))))
		c = c.Add(delta1.Mul(list2[i].Sub(mean2)))
	}
	return CreateDecimal(c.Div(DecFromInt(int64(len(list1) - 1))))
}

func (f *FunctionCOVARIANCES) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	f.Arg(0).GetParameterTypes(engine, result, OperandTypeArray)
	f.Arg(1).GetParameterTypes(engine, result, OperandTypeArray)
}

func init() { RegisterFunction("COVARIANCES", newFunctionCOVARIANCES) }
