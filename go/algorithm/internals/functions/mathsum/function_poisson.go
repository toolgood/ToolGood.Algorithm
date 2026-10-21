package mathsum

import "math"

// FunctionPOISSON 对应 C# Function_POISSON（泊松分布）。
type FunctionPOISSON struct{ BaseFunction }

func newFunctionPOISSON(args []Function) Function {
	RequireArgs(args, "Poisson", 3, 3)
	return &FunctionPOISSON{BaseFunction: NewBase("Poisson", OperandTypeNumber, args)}
}

func (f *FunctionPOISSON) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args1 := f.GetNumber(1, engine, tp)
	if args1.IsErrorOrNone() {
		return args1
	}
	args2 := f.GetNumber(2, engine, tp)
	if args2.IsErrorOrNone() {
		return args2
	}
	args3 := f.GetBoolean(3, engine, tp)
	if args3.IsErrorOrNone() {
		return args3
	}

	k := args1.IntValue()
	if k < 0 {
		return f.ParameterError(1)
	}
	lambda := args2.NumberValue()
	if !lambda.Greater(DecimalZero) {
		return f.ParameterError(2)
	}
	state := args3.BooleanValue()

	// 与 C# ExcelFunctions.Poisson 一致：PMF 用对数公式，CDF 用 1-P(k+1, λ)
	lam := lambda.Float64()
	var v float64
	if state {
		v = 1 - gammaLowerRegularized(float64(k)+1, lam)
	} else {
		v = math.Exp(-lam + float64(k)*math.Log(lam) - lgamma(float64(k)+1))
	}
	return numericResult(f.FunctionError, v)
}

func (f *FunctionPOISSON) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	f.Arg(0).GetParameterTypes(engine, result, OperandTypeNumber)
	f.Arg(1).GetParameterTypes(engine, result, OperandTypeNumber)
	f.Arg(2).GetParameterTypes(engine, result, OperandTypeBoolean)
}

func init() { RegisterFunction("POISSON", newFunctionPOISSON) }
