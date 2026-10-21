package mathbase

import "math/rand"

// 移植自 Internals/Functions/MathBase/Function_RAND.cs、Function_RANDBETWEEN.cs。

type FunctionRAND struct{ BaseFunction }

func newFunctionRAND(args []Function) Function {
	return &FunctionRAND{BaseFunction: NewBase("Rand", OperandTypeNumber, args)}
}

func (f *FunctionRAND) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	return CreateFloat(rand.Float64())
}

// GetParameterTypes RAND 无参数，不传播参数类型。
func (f *FunctionRAND) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
}

// ---------------------------------------------------------------- RANDBETWEEN

type FunctionRANDBETWEEN struct{ BaseFunction }

func newFunctionRANDBETWEEN(args []Function) Function {
	RequireArgs(args, "RandBetween", 2, 2)
	return &FunctionRANDBETWEEN{BaseFunction: NewBase("RandBetween", OperandTypeNumber, args)}
}

func (f *FunctionRANDBETWEEN) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	b := f.GetNumber(2, engine, tp)
	if b.IsErrorOrNone() {
		return b
	}
	bottom := a.NumberValue()
	top := b.NumberValue()
	if bottom.Greater(top) {
		return f.ParameterError(1)
	}
	r := DecFromFloat(rand.Float64()).Mul(top.Sub(bottom).Add(DecimalOne)).Add(bottom).Floor()
	return CreateDecimal(r)
}

func (f *FunctionRANDBETWEEN) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
	if arg := f.Arg(1); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

func init() {
	RegisterFunction("RAND", newFunctionRAND)
	RegisterFunction("RANDBETWEEN", newFunctionRANDBETWEEN)
}
