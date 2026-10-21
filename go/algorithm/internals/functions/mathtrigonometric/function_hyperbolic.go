package mathtrigonometric

import "math"

// 移植自 Internals/Functions/MathTrigonometric/Function_SINH.cs、Function_COSH.cs、
// Function_TANH.cs、Function_COTH.cs、Function_SECH.cs、Function_CSCH.cs、
// Function_DEGREES.cs、Function_RADIANS.cs。

type FunctionSINH struct{ BaseFunction }

func newFunctionSINH(args []Function) Function {
	RequireArgs(args, "Sinh", 1, 1)
	return &FunctionSINH{BaseFunction: NewBase("Sinh", OperandTypeNumber, args)}
}

func (f *FunctionSINH) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	r, ok := fromFloat(math.Sinh(a.NumberValue().Float64()))
	// |x| 超过约 67.235 时结果超出 decimal 可表示范围
	if !ok || isOverflow(r) {
		return f.NumError()
	}
	return CreateDecimal(r)
}

func (f *FunctionSINH) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- COSH

type FunctionCOSH struct{ BaseFunction }

func newFunctionCOSH(args []Function) Function {
	RequireArgs(args, "Cosh", 1, 1)
	return &FunctionCOSH{BaseFunction: NewBase("Cosh", OperandTypeNumber, args)}
}

func (f *FunctionCOSH) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	r, ok := fromFloat(math.Cosh(a.NumberValue().Float64()))
	if !ok || isOverflow(r) {
		return f.NumError()
	}
	return CreateDecimal(r)
}

func (f *FunctionCOSH) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- TANH

type FunctionTANH struct{ BaseFunction }

func newFunctionTANH(args []Function) Function {
	RequireArgs(args, "Tanh", 1, 1)
	return &FunctionTANH{BaseFunction: NewBase("Tanh", OperandTypeNumber, args)}
}

func (f *FunctionTANH) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	x := a.NumberValue()
	// tanh 在 |x| 较大时趋近 ±1，提前返回避免 e^|x| 溢出
	if x.GreaterOrEqual(dec66) {
		return CreateDecimal(DecimalOne)
	}
	if x.LessOrEqual(dec66.Neg()) {
		return CreateDecimal(DecimalOne.Neg())
	}
	r, ok := fromFloat(math.Tanh(x.Float64()))
	if !ok {
		return f.NumError()
	}
	return CreateDecimal(r)
}

func (f *FunctionTANH) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- COTH

type FunctionCOTH struct{ BaseFunction }

func newFunctionCOTH(args []Function) Function {
	RequireArgs(args, "Coth", 1, 1)
	return &FunctionCOTH{BaseFunction: NewBase("Coth", OperandTypeNumber, args)}
}

func (f *FunctionCOTH) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	x := a.NumberValue()
	// coth 在 |x| 较大时趋近 ±1，提前返回避免 e^|x| 溢出
	if x.GreaterOrEqual(dec66) {
		return CreateDecimal(DecimalOne)
	}
	if x.LessOrEqual(dec66.Neg()) {
		return CreateDecimal(DecimalOne.Neg())
	}
	v := x.Float64()
	d := math.Sinh(v)
	if d == 0 {
		return f.Div0Error()
	}
	r, ok := fromFloat(math.Cosh(v) / d)
	if !ok {
		return f.NumError()
	}
	return CreateDecimal(r)
}

func (f *FunctionCOTH) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- SECH

type FunctionSECH struct{ BaseFunction }

func newFunctionSECH(args []Function) Function {
	RequireArgs(args, "Sech", 1, 1)
	return &FunctionSECH{BaseFunction: NewBase("Sech", OperandTypeNumber, args)}
}

func (f *FunctionSECH) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	x := a.NumberValue()
	// sech 在 |x| 较大时趋近 0，提前返回避免 e^|x| 溢出
	if x.GreaterOrEqual(dec66) || x.LessOrEqual(dec66.Neg()) {
		return CreateDecimal(DecimalZero)
	}
	r, ok := fromFloat(1.0 / math.Cosh(x.Float64()))
	if !ok {
		return f.NumError()
	}
	return CreateDecimal(r)
}

func (f *FunctionSECH) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- CSCH

type FunctionCSCH struct{ BaseFunction }

func newFunctionCSCH(args []Function) Function {
	RequireArgs(args, "Csch", 1, 1)
	return &FunctionCSCH{BaseFunction: NewBase("Csch", OperandTypeNumber, args)}
}

func (f *FunctionCSCH) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	x := a.NumberValue()
	// csch 在 |x| 较大时趋近 0，提前返回避免 e^|x| 溢出
	if x.GreaterOrEqual(dec66) || x.LessOrEqual(dec66.Neg()) {
		return CreateDecimal(DecimalZero)
	}
	d := math.Sinh(x.Float64())
	if d == 0 {
		return f.Div0Error()
	}
	r, ok := fromFloat(1.0 / d)
	if !ok {
		return f.NumError()
	}
	return CreateDecimal(r)
}

func (f *FunctionCSCH) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- DEGREES

type FunctionDEGREES struct{ BaseFunction }

func newFunctionDEGREES(args []Function) Function {
	RequireArgs(args, "Degrees", 1, 1)
	return &FunctionDEGREES{BaseFunction: NewBase("Degrees", OperandTypeNumber, args)}
}

func (f *FunctionDEGREES) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	// 弧度值过大时换算结果超出 decimal 可表示范围
	r := a.NumberValue().Div(decimalPI).Mul(dec180)
	if isOverflow(r) {
		return f.NumError()
	}
	return CreateDecimal(r)
}

func (f *FunctionDEGREES) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- RADIANS

type FunctionRADIANS struct{ BaseFunction }

func newFunctionRADIANS(args []Function) Function {
	RequireArgs(args, "Radians", 1, 1)
	return &FunctionRADIANS{BaseFunction: NewBase("Radians", OperandTypeNumber, args)}
}

func (f *FunctionRADIANS) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	r := a.NumberValue().Div(dec180).Mul(decimalPI)
	return CreateDecimal(r)
}

func (f *FunctionRADIANS) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

func init() {
	RegisterFunction("SINH", newFunctionSINH)
	RegisterFunction("COSH", newFunctionCOSH)
	RegisterFunction("TANH", newFunctionTANH)
	RegisterFunction("COTH", newFunctionCOTH)
	RegisterFunction("SECH", newFunctionSECH)
	RegisterFunction("CSCH", newFunctionCSCH)
	RegisterFunction("DEGREES", newFunctionDEGREES)
	RegisterFunction("RADIANS", newFunctionRADIANS)
}
