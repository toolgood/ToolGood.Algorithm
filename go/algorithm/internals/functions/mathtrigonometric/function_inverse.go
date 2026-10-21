package mathtrigonometric

import "math"

// 移植自 Internals/Functions/MathTrigonometric/Function_ASIN.cs、Function_ACOS.cs、
// Function_ATAN.cs、Function_ATAN2.cs、Function_ACOT.cs、
// Function_ASINH.cs、Function_ACOSH.cs、Function_ATANH.cs、Function_ACOTH.cs。

type FunctionASIN struct{ BaseFunction }

func newFunctionASIN(args []Function) Function {
	RequireArgs(args, "Asin", 1, 1)
	return &FunctionASIN{BaseFunction: NewBase("Asin", OperandTypeNumber, args)}
}

func (f *FunctionASIN) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	x := a.NumberValue()
	if x.Less(DecimalOne.Neg()) || x.Greater(DecimalOne) {
		return f.NumError()
	}
	r, ok := fromFloat(math.Asin(x.Float64()))
	if !ok {
		return f.NumError()
	}
	return CreateDecimal(r)
}

func (f *FunctionASIN) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- ACOS

type FunctionACOS struct{ BaseFunction }

func newFunctionACOS(args []Function) Function {
	RequireArgs(args, "Acos", 1, 1)
	return &FunctionACOS{BaseFunction: NewBase("Acos", OperandTypeNumber, args)}
}

func (f *FunctionACOS) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	x := a.NumberValue()
	if x.Less(DecimalOne.Neg()) || x.Greater(DecimalOne) {
		return f.NumError()
	}
	r, ok := fromFloat(math.Acos(x.Float64()))
	if !ok {
		return f.NumError()
	}
	return CreateDecimal(r)
}

func (f *FunctionACOS) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- ATAN

type FunctionATAN struct{ BaseFunction }

func newFunctionATAN(args []Function) Function {
	RequireArgs(args, "Atan", 1, 1)
	return &FunctionATAN{BaseFunction: NewBase("Atan", OperandTypeNumber, args)}
}

func (f *FunctionATAN) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	r, ok := fromFloat(math.Atan(a.NumberValue().Float64()))
	if !ok {
		return f.NumError()
	}
	return CreateDecimal(r)
}

func (f *FunctionATAN) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- ATAN2

type FunctionATAN2 struct{ BaseFunction }

func newFunctionATAN2(args []Function) Function {
	RequireArgs(args, "Atan2", 2, 2)
	return &FunctionATAN2{BaseFunction: NewBase("Atan2", OperandTypeNumber, args)}
}

func (f *FunctionATAN2) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	b := f.GetNumber(2, engine, tp)
	if b.IsErrorOrNone() {
		return b
	}
	x := a.NumberValue()
	y := b.NumberValue()
	if x.IsZero() && y.IsZero() {
		return f.Div0Error()
	}
	// C# MathEx.Atan2(y, x)
	r, ok := fromFloat(math.Atan2(y.Float64(), x.Float64()))
	if !ok {
		return f.NumError()
	}
	return CreateDecimal(r)
}

func (f *FunctionATAN2) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
	if arg := f.Arg(1); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- ACOT

type FunctionACOT struct{ BaseFunction }

func newFunctionACOT(args []Function) Function {
	RequireArgs(args, "Acot", 1, 1)
	return &FunctionACOT{BaseFunction: NewBase("Acot", OperandTypeNumber, args)}
}

func (f *FunctionACOT) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	// acot(x) = π/2 - atan(x)，使用 decimal 的 π/2 常量
	r := decimalHalfPi.Sub(DecFromFloat(math.Atan(a.NumberValue().Float64())))
	if isOverflow(r) {
		return f.NumError()
	}
	return CreateDecimal(r)
}

func (f *FunctionACOT) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- ASINH

type FunctionASINH struct{ BaseFunction }

func newFunctionASINH(args []Function) Function {
	RequireArgs(args, "Asinh", 1, 1)
	return &FunctionASINH{BaseFunction: NewBase("Asinh", OperandTypeNumber, args)}
}

func (f *FunctionASINH) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	r, ok := fromFloat(math.Asinh(a.NumberValue().Float64()))
	if !ok || isOverflow(r) {
		return f.NumError()
	}
	return CreateDecimal(r)
}

func (f *FunctionASINH) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- ACOSH

type FunctionACOSH struct{ BaseFunction }

func newFunctionACOSH(args []Function) Function {
	RequireArgs(args, "Acosh", 1, 1)
	return &FunctionACOSH{BaseFunction: NewBase("Acosh", OperandTypeNumber, args)}
}

func (f *FunctionACOSH) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	z := a.NumberValue()
	if z.Less(DecimalOne) {
		return f.NumError()
	}
	r, ok := fromFloat(math.Acosh(z.Float64()))
	if !ok || isOverflow(r) {
		return f.NumError()
	}
	return CreateDecimal(r)
}

func (f *FunctionACOSH) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- ATANH

type FunctionATANH struct{ BaseFunction }

func newFunctionATANH(args []Function) Function {
	RequireArgs(args, "Atanh", 1, 1)
	return &FunctionATANH{BaseFunction: NewBase("Atanh", OperandTypeNumber, args)}
}

func (f *FunctionATANH) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	x := a.NumberValue()
	if x.GreaterOrEqual(DecimalOne) || x.LessOrEqual(DecimalOne.Neg()) {
		return f.NumError()
	}
	r, ok := fromFloat(math.Atanh(x.Float64()))
	if !ok || isOverflow(r) {
		return f.NumError()
	}
	return CreateDecimal(r)
}

func (f *FunctionATANH) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- ACOTH

type FunctionACOTH struct{ BaseFunction }

func newFunctionACOTH(args []Function) Function {
	RequireArgs(args, "Acoth", 1, 1)
	return &FunctionACOTH{BaseFunction: NewBase("Acoth", OperandTypeNumber, args)}
}

func (f *FunctionACOTH) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	d := a.NumberValue()
	// 不能写成 |d| <= 1：decimal.MinValue 取绝对值会溢出
	if d.GreaterOrEqual(DecimalOne.Neg()) && d.LessOrEqual(DecimalOne) {
		return f.NumError()
	}
	// acoth(d) = atanh(1/d)，用倒数形式避免大 d 时结果恒为 0
	r, ok := fromFloat(math.Atanh(DecimalOne.Div(d).Float64()))
	if !ok || isOverflow(r) {
		return f.NumError()
	}
	return CreateDecimal(r)
}

func (f *FunctionACOTH) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

func init() {
	RegisterFunction("ASIN", newFunctionASIN)
	RegisterFunction("ACOS", newFunctionACOS)
	RegisterFunction("ATAN", newFunctionATAN)
	RegisterFunction("ATAN2", newFunctionATAN2)
	RegisterFunction("ACOT", newFunctionACOT)
	RegisterFunction("ASINH", newFunctionASINH)
	RegisterFunction("ACOSH", newFunctionACOSH)
	RegisterFunction("ATANH", newFunctionATANH)
	RegisterFunction("ACOTH", newFunctionACOTH)
}
