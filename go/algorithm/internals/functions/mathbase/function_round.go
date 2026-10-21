package mathbase

// 移植自 Internals/Functions/MathBase/Function_ROUND.cs、Function_ROUNDDOWN.cs、
// Function_ROUNDUP.cs、Function_TRUNC.cs、Function_FIXED.cs、Function_MROUND.cs。
// 舍入统一使用 Decimal.RoundHalfUp（等价 C# MidpointRounding.AwayFromZero）。

type FunctionROUND struct{ BaseFunction }

func newFunctionROUND(args []Function) Function {
	RequireArgs(args, "Round", 1, 2)
	return &FunctionROUND{BaseFunction: NewBase("Round", OperandTypeNumber, args)}
}

func (f *FunctionROUND) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	if f.ArgCount() == 1 {
		return CreateDecimal(a.NumberValue().RoundHalfUp(0))
	}
	b := f.GetNumber(2, engine, tp)
	if b.IsErrorOrNone() {
		return b
	}
	digits, ok := tryGetInt(b)
	if !ok || digits < -15 || digits > 15 {
		return f.ParameterError(2)
	}
	num := a.NumberValue()
	if digits >= 0 {
		return CreateDecimal(num.RoundHalfUp(int32(digits)))
	}
	// Math.Round(decimal, int) 只支持非负位数，负数位数(向左取整)改用先除后乘
	factor := pow10(-digits)
	return CreateDecimal(num.Div(factor).RoundHalfUp(0).Mul(factor))
}

func (f *FunctionROUND) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
	if arg := f.Arg(1); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- ROUNDDOWN

type FunctionROUNDDOWN struct{ BaseFunction }

func newFunctionROUNDDOWN(args []Function) Function {
	RequireArgs(args, "RoundDown", 2, 2)
	return &FunctionROUNDDOWN{BaseFunction: NewBase("RoundDown", OperandTypeNumber, args)}
}

func (f *FunctionROUNDDOWN) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	b := f.GetNumber(2, engine, tp)
	if b.IsErrorOrNone() {
		return b
	}
	digits, ok := tryGetInt(b)
	if !ok || digits < -15 || digits > 15 {
		return f.ParameterError(2)
	}
	num := a.NumberValue()
	if num.IsZero() {
		return a
	}
	factor := pow10(digits)
	scaled := num.Mul(factor)
	if isOverflow(scaled) {
		return f.NumError()
	}
	if num.Sign() > 0 {
		return CreateDecimal(scaled.Floor().Div(factor))
	}
	return CreateDecimal(scaled.Ceil().Div(factor))
}

func (f *FunctionROUNDDOWN) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
	if arg := f.Arg(1); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- ROUNDUP

type FunctionROUNDUP struct{ BaseFunction }

func newFunctionROUNDUP(args []Function) Function {
	RequireArgs(args, "RoundUp", 2, 2)
	return &FunctionROUNDUP{BaseFunction: NewBase("RoundUp", OperandTypeNumber, args)}
}

func (f *FunctionROUNDUP) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	b := f.GetNumber(2, engine, tp)
	if b.IsErrorOrNone() {
		return b
	}
	digits, ok := tryGetInt(b)
	if !ok || digits < -15 || digits > 15 {
		return f.ParameterError(2)
	}
	num := a.NumberValue()
	if num.IsZero() {
		return a
	}
	factor := pow10(digits)
	scaled := num.Abs().Mul(factor)
	if isOverflow(scaled) {
		return f.NumError()
	}
	t := scaled.Ceil().Div(factor)
	if num.Sign() > 0 {
		return CreateDecimal(t)
	}
	return CreateDecimal(t.Neg())
}

func (f *FunctionROUNDUP) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
	if arg := f.Arg(1); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- TRUNC

type FunctionTRUNC struct{ BaseFunction }

func newFunctionTRUNC(args []Function) Function {
	RequireArgs(args, "Trunc", 1, 2)
	return &FunctionTRUNC{BaseFunction: NewBase("Trunc", OperandTypeNumber, args)}
}

func (f *FunctionTRUNC) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	digits := 0
	if f.ArgCount() > 1 {
		b := f.GetNumber(2, engine, tp)
		if b.IsErrorOrNone() {
			return b
		}
		d, ok := tryGetInt(b)
		if !ok || d < -15 || d > 15 {
			return f.ParameterError(2)
		}
		digits = d
	}
	num := a.NumberValue()
	if num.IsZero() {
		return a
	}
	factor := pow10(digits)
	scaled := num.Mul(factor)
	if isOverflow(scaled) {
		return f.NumError()
	}
	if num.Sign() > 0 {
		return CreateDecimal(scaled.Floor().Div(factor))
	}
	return CreateDecimal(scaled.Ceil().Div(factor))
}

func (f *FunctionTRUNC) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
	if arg := f.Arg(1); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- FIXED

type FunctionFIXED struct{ BaseFunction }

func newFunctionFIXED(args []Function) Function {
	RequireArgs(args, "Fixed", 1, 3)
	return &FunctionFIXED{BaseFunction: NewBase("Fixed", OperandTypeText, args)}
}

func (f *FunctionFIXED) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	// 参数按从左到右的顺序求值,保证副作用与错误序号一致
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	num := 2
	if f.ArgCount() > 1 {
		b := f.GetNumber(2, engine, tp)
		if b.IsErrorOrNone() {
			return b
		}
		d, ok := tryGetInt(b)
		if !ok || d < -15 || d > 15 {
			return f.ParameterError(2)
		}
		num = d
	}
	s := a.NumberValue()
	if num >= 0 {
		s = s.RoundHalfUp(int32(num))
	} else {
		factor := pow10(-num)
		s = s.Div(factor).RoundHalfUp(0).Mul(factor)
	}
	no := false
	if f.ArgCount() > 2 {
		c := f.GetBoolean(3, engine, tp)
		if c.IsErrorOrNone() {
			return c
		}
		no = c.BooleanValue()
	}
	if !no {
		if num < 0 {
			// 负数位数取整后无小数位,用 N0 保持千分位
			return CreateString(formatGrouped(s, 0))
		}
		return CreateString(formatGrouped(s, num))
	}
	return CreateString(s.String())
}

// ResultType Fixed 固定输出文本。
func (f *FunctionFIXED) ResultType() OperandType { return OperandTypeText }

func (f *FunctionFIXED) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
	if arg := f.Arg(1); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
	if arg := f.Arg(2); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeBoolean)
	}
}

// ---------------------------------------------------------------- MROUND

type FunctionMROUND struct{ BaseFunction }

func newFunctionMROUND(args []Function) Function {
	RequireArgs(args, "Mround", 2, 2)
	return &FunctionMROUND{BaseFunction: NewBase("Mround", OperandTypeNumber, args)}
}

func (f *FunctionMROUND) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	b := f.GetNumber(2, engine, tp)
	if b.IsErrorOrNone() {
		return b
	}
	multiple := b.NumberValue()
	if multiple.IsZero() {
		return f.ParameterError(2)
	}
	number := a.NumberValue()
	if (number.Sign() > 0 && multiple.Sign() < 0) || (number.Sign() < 0 && multiple.Sign() > 0) {
		return f.ParameterError(2)
	}
	r := number.Div(multiple).RoundHalfUp(0).Mul(multiple)
	if isOverflow(r) {
		return f.NumError()
	}
	return CreateDecimal(r)
}

func (f *FunctionMROUND) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
	if arg := f.Arg(1); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

func init() {
	RegisterFunction("ROUND", newFunctionROUND)
	RegisterFunction("ROUNDDOWN", newFunctionROUNDDOWN)
	RegisterFunction("ROUNDUP", newFunctionROUNDUP)
	RegisterFunction("TRUNC", newFunctionTRUNC)
	RegisterFunction("FIXED", newFunctionFIXED)
	RegisterFunction("MROUND", newFunctionMROUND)
}
