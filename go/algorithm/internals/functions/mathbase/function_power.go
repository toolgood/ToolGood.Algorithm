package mathbase

import "math"

// 移植自 Internals/Functions/MathBase/Function_POWER.cs、Function_EXP.cs、Function_LN.cs、
// Function_LOG.cs、Function_LOG10.cs、Function_SQRT.cs、Function_SQRTPI.cs。

// decimalEpsilon 对应 C# MathEx.IsInteger 中使用的误差阈值。
var decimalEpsilon = MustDecimal("0.0000000000000000001")

// isIntegerDecimal 对应 C# MathEx.IsInteger：|x - (long)x| <= 1e-19。
func isIntegerDecimal(x Decimal) bool {
	return x.Sub(x.Trunc()).Abs().LessOrEqual(decimalEpsilon)
}

// tryIntValue 把 Decimal 转成 int，超出 int 范围时返回 false。
func tryIntValue(d Decimal) (int, bool) {
	t := d.Trunc()
	if t.Less(decIntMin) || t.Greater(decIntMax) {
		return 0, false
	}
	return int(t.Int64()), true
}

// powerN 对应 C# MathEx.PowerN，结果超出 decimal 范围时返回 false。
func powerN(value Decimal, power int) (Decimal, bool) {
	if power < 0 {
		value = DecimalOne.Div(value)
		power = -power
	}
	result := DecimalOne
	current := value
	for power > 0 {
		if power&1 == 1 {
			result = result.Mul(current)
			if isOverflow(result) {
				return DecimalZero, false
			}
		}
		power >>= 1
		if power > 0 {
			current = current.Mul(current)
			if isOverflow(current) {
				return DecimalZero, false
			}
		}
	}
	return result, true
}

// powerDecimal 对应 C# MathEx.Pow：整数指数按精确幂计算，
// 其余情况按 e^(pow*ln(value)) 计算；无法用 decimal 表示时返回 false。
func powerDecimal(baseValue, exponent Decimal) (Decimal, bool) {
	if exponent.IsZero() {
		return DecimalOne, true
	}
	if exponent.Equal(DecimalOne) {
		return baseValue, true
	}
	if baseValue.Equal(DecimalOne) {
		return DecimalOne, true
	}
	if baseValue.IsZero() {
		if exponent.Sign() > 0 {
			return DecimalZero, true
		}
		return DecimalZero, false
	}
	if exponent.Equal(DecimalOne.Neg()) {
		return DecimalOne.Div(baseValue), true
	}
	isInteger := isIntegerDecimal(exponent)
	if baseValue.Sign() < 0 && !isInteger {
		return DecimalZero, false
	}
	if isInteger && baseValue.Sign() > 0 {
		n, ok := tryIntValue(exponent)
		if !ok {
			return DecimalZero, false
		}
		return powerN(baseValue, n)
	}
	f := math.Pow(baseValue.Float64(), exponent.Float64())
	if math.IsNaN(f) || math.IsInf(f, 0) {
		return DecimalZero, false
	}
	r := DecFromFloat(f)
	if isOverflow(r) {
		return DecimalZero, false
	}
	return r, true
}

// ---------------------------------------------------------------- POWER

type FunctionPOWER struct{ BaseFunction }

func newFunctionPOWER(args []Function) Function {
	RequireArgs(args, "Power", 2, 2)
	return &FunctionPOWER{BaseFunction: NewBase("Power", OperandTypeNumber, args)}
}

func (f *FunctionPOWER) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	b := f.GetNumber(2, engine, tp)
	if b.IsErrorOrNone() {
		return b
	}
	baseValue := a.NumberValue()
	exponent := b.NumberValue()

	if baseValue.IsZero() && exponent.Sign() < 0 {
		return f.Div0Error()
	}
	if baseValue.Sign() < 0 && !exponent.Mod(DecimalOne).IsZero() {
		return f.ParameterError(1)
	}
	r, ok := powerDecimal(baseValue, exponent)
	if !ok {
		return f.FunctionError()
	}
	return CreateDecimal(r)
}

func (f *FunctionPOWER) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
	if arg := f.Arg(1); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- EXP

type FunctionEXP struct{ BaseFunction }

func newFunctionEXP(args []Function) Function {
	RequireArgs(args, "Exp", 1, 1)
	return &FunctionEXP{BaseFunction: NewBase("Exp", OperandTypeNumber, args)}
}

func (f *FunctionEXP) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	z := a.NumberValue()
	if z.Greater(dec66) {
		return f.ParameterError(1)
	}
	r, ok := decFromFloat(math.Exp(z.Float64()))
	if !ok {
		return f.NumError()
	}
	return CreateDecimal(r)
}

func (f *FunctionEXP) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- LN

type FunctionLN struct{ BaseFunction }

func newFunctionLN(args []Function) Function {
	RequireArgs(args, "Ln", 1, 1)
	return &FunctionLN{BaseFunction: NewBase("Ln", OperandTypeNumber, args)}
}

func (f *FunctionLN) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	z := a.NumberValue()
	if z.Sign() <= 0 {
		return f.ParameterError(1)
	}
	r, ok := decFromFloat(math.Log(z.Float64()))
	if !ok {
		return f.NumError()
	}
	return CreateDecimal(r)
}

func (f *FunctionLN) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- LOG

type FunctionLOG struct{ BaseFunction }

func newFunctionLOG(args []Function) Function {
	RequireArgs(args, "Log", 1, 2)
	return &FunctionLOG{BaseFunction: NewBase("Log", OperandTypeNumber, args)}
}

func (f *FunctionLOG) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	z := a.NumberValue()
	if z.Sign() <= 0 {
		return f.ParameterError(1)
	}
	if f.ArgCount() == 1 {
		r, ok := decFromFloat(math.Log10(z.Float64()))
		if !ok {
			return f.NumError()
		}
		return CreateDecimal(r)
	}
	b := f.GetNumber(2, engine, tp)
	if b.IsErrorOrNone() {
		return b
	}
	baseValue := b.NumberValue()
	if baseValue.Sign() <= 0 || baseValue.Equal(DecimalOne) {
		return f.ParameterError(2)
	}
	r, ok := decFromFloat(math.Log(z.Float64()) / math.Log(baseValue.Float64()))
	if !ok {
		return f.NumError()
	}
	return CreateDecimal(r)
}

func (f *FunctionLOG) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
	if arg := f.Arg(1); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- LOG10

type FunctionLOG10 struct{ BaseFunction }

func newFunctionLOG10(args []Function) Function {
	RequireArgs(args, "Log10", 1, 1)
	return &FunctionLOG10{BaseFunction: NewBase("Log10", OperandTypeNumber, args)}
}

func (f *FunctionLOG10) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	z := a.NumberValue()
	if z.Sign() <= 0 {
		return f.ParameterError(1)
	}
	r, ok := decFromFloat(math.Log10(z.Float64()))
	if !ok {
		return f.NumError()
	}
	return CreateDecimal(r)
}

func (f *FunctionLOG10) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- SQRT

type FunctionSQRT struct{ BaseFunction }

func newFunctionSQRT(args []Function) Function {
	RequireArgs(args, "Sqrt", 1, 1)
	return &FunctionSQRT{BaseFunction: NewBase("Sqrt", OperandTypeNumber, args)}
}

func (f *FunctionSQRT) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	z := a.NumberValue()
	if z.Sign() < 0 {
		return f.ParameterError(1)
	}
	return CreateDecimal(z.Sqrt())
}

func (f *FunctionSQRT) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- SQRTPI

type FunctionSQRTPI struct{ BaseFunction }

func newFunctionSQRTPI(args []Function) Function {
	RequireArgs(args, "SqrtPi", 1, 1)
	return &FunctionSQRTPI{BaseFunction: NewBase("SqrtPi", OperandTypeNumber, args)}
}

func (f *FunctionSQRTPI) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	z := a.NumberValue()
	if z.Sign() < 0 {
		return f.ParameterError(1)
	}
	r := z.Mul(decimalPI)
	if isOverflow(r) {
		return f.NumError()
	}
	return CreateDecimal(r.Sqrt())
}

func (f *FunctionSQRTPI) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

func init() {
	RegisterFunction("POWER", newFunctionPOWER)
	RegisterFunction("EXP", newFunctionEXP)
	RegisterFunction("LN", newFunctionLN)
	RegisterFunction("LOG", newFunctionLOG)
	RegisterFunction("LOG10", newFunctionLOG10)
	RegisterFunction("SQRT", newFunctionSQRT)
	RegisterFunction("SQRTPI", newFunctionSQRTPI)
}
