package mathsum

import "github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals"

// FunctionCOVAR 对应 C# Function_COVAR（协方差，除以 n）。
type FunctionCOVAR struct{ BaseFunction }

func newFunctionCOVAR(args []Function) Function {
	RequireArgs(args, "Covar", 2, 2)
	return &FunctionCOVAR{BaseFunction: NewBase("Covar", OperandTypeNumber, args)}
}

func (f *FunctionCOVAR) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	return covarEvaluate(&f.BaseFunction, engine, tp)
}

func (f *FunctionCOVAR) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	f.Arg(0).GetParameterTypes(engine, result, OperandTypeArray)
	f.Arg(1).GetParameterTypes(engine, result, OperandTypeArray)
}

// covarEvaluate 是 COVAR / COVARIANCES 的公共逻辑。
func covarEvaluate(f *BaseFunction, engine *AlgorithmEngine, tp TempParameter) Operand {
	args1 := f.EvalArg(0, engine, tp)
	if args1.IsErrorOrNone() {
		return args1
	}
	args2 := f.EvalArg(1, engine, tp)
	if args2.IsErrorOrNone() {
		return args2
	}
	var list1, list2 []Decimal
	o1 := internals.FlattenToNumberList(args1, &list1)
	o2 := internals.FlattenToNumberList(args2, &list2)
	if !o1 {
		return f.ParameterError(1)
	}
	if !o2 {
		return f.ParameterError(2)
	}
	if len(list1) != len(list2) {
		return f.FunctionError()
	}
	if len(list1) == 0 {
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
	return CreateDecimal(c.Div(DecFromInt(int64(len(list1)))))
}

func init() { RegisterFunction("COVAR", newFunctionCOVAR) }
