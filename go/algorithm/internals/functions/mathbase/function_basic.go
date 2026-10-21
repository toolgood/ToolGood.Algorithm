package mathbase

// 移植自 Internals/Functions/MathBase/Function_ABS.cs 等基础数学函数。
// 本文件包含：ABS、SIGN、EVEN、ODD、DELTA、GESTEP、QUOTIENT、INT、CEILING、FLOOR。

type FunctionABS struct{ BaseFunction }

func newFunctionABS(args []Function) Function {
	RequireArgs(args, "Abs", 1, 1)
	return &FunctionABS{BaseFunction: NewBase("Abs", OperandTypeNumber, args)}
}

func (f *FunctionABS) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	return CreateDecimal(a.NumberValue().Abs())
}

func (f *FunctionABS) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- SIGN

type FunctionSIGN struct{ BaseFunction }

func newFunctionSIGN(args []Function) Function {
	RequireArgs(args, "Sign", 1, 1)
	return &FunctionSIGN{BaseFunction: NewBase("Sign", OperandTypeNumber, args)}
}

func (f *FunctionSIGN) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	return CreateInt(a.NumberValue().Sign())
}

func (f *FunctionSIGN) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- EVEN

type FunctionEVEN struct{ BaseFunction }

func newFunctionEVEN(args []Function) Function {
	RequireArgs(args, "Even", 1, 1)
	return &FunctionEVEN{BaseFunction: NewBase("Even", OperandTypeNumber, args)}
}

func (f *FunctionEVEN) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	z := a.NumberValue()
	if z.IsZero() {
		return Zero
	}
	two := DecFromInt(2)
	if z.Sign() > 0 {
		if z.Mod(two).IsZero() {
			return a
		}
		z = z.Ceil()
		if z.Mod(two).IsZero() {
			return CreateDecimal(z)
		}
		return CreateDecimal(z.Add(DecimalOne))
	}
	if z.Mod(two).IsZero() {
		return a
	}
	z = z.Floor()
	if z.Mod(two).IsZero() {
		return CreateDecimal(z)
	}
	return CreateDecimal(z.Sub(DecimalOne))
}

func (f *FunctionEVEN) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- ODD

type FunctionODD struct{ BaseFunction }

func newFunctionODD(args []Function) Function {
	RequireArgs(args, "Odd", 1, 1)
	return &FunctionODD{BaseFunction: NewBase("Odd", OperandTypeNumber, args)}
}

func (f *FunctionODD) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	z := a.NumberValue()
	if z.IsZero() {
		return CreateInt(1)
	}
	two := DecFromInt(2)
	one := DecimalOne
	if z.Sign() > 0 {
		r := z.Mod(two)
		if r.Equal(one) || r.Equal(one.Neg()) {
			return a
		}
		z = z.Ceil()
		if !z.Mod(two).IsZero() {
			return CreateDecimal(z)
		}
		return CreateDecimal(z.Add(one))
	}
	r := z.Mod(two)
	if r.Equal(one) || r.Equal(one.Neg()) {
		return a
	}
	z = z.Floor()
	if !z.Mod(two).IsZero() {
		return CreateDecimal(z)
	}
	return CreateDecimal(z.Sub(one))
}

func (f *FunctionODD) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- DELTA

type FunctionDELTA struct{ BaseFunction }

func newFunctionDELTA(args []Function) Function {
	RequireArgs(args, "Delta", 1, 2)
	return &FunctionDELTA{BaseFunction: NewBase("Delta", OperandTypeNumber, args)}
}

func (f *FunctionDELTA) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	num1 := a.NumberValue()

	num2 := DecimalZero
	if f.ArgCount() > 1 {
		b := f.GetNumber(2, engine, tp)
		if b.IsErrorOrNone() {
			return b
		}
		num2 = b.NumberValue()
	}
	if num1.Equal(num2) {
		return CreateInt(1)
	}
	return CreateInt(0)
}

func (f *FunctionDELTA) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
	if arg := f.Arg(1); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- GESTEP

type FunctionGESTEP struct{ BaseFunction }

func newFunctionGESTEP(args []Function) Function {
	RequireArgs(args, "GeStep", 1, 2)
	return &FunctionGESTEP{BaseFunction: NewBase("GeStep", OperandTypeNumber, args)}
}

func (f *FunctionGESTEP) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	number := a.NumberValue()

	step := DecimalZero
	if f.ArgCount() > 1 {
		b := f.GetNumber(2, engine, tp)
		if b.IsErrorOrNone() {
			return b
		}
		step = b.NumberValue()
	}
	if number.GreaterOrEqual(step) {
		return CreateInt(1)
	}
	return CreateInt(0)
}

func (f *FunctionGESTEP) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
	if arg := f.Arg(1); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- QUOTIENT

type FunctionQUOTIENT struct{ BaseFunction }

func newFunctionQUOTIENT(args []Function) Function {
	RequireArgs(args, "Quotient", 2, 2)
	return &FunctionQUOTIENT{BaseFunction: NewBase("Quotient", OperandTypeNumber, args)}
}

func (f *FunctionQUOTIENT) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	b := f.GetNumber(2, engine, tp)
	if b.IsErrorOrNone() {
		return b
	}
	divisor := b.NumberValue()
	if divisor.IsZero() {
		return f.Div0Error()
	}
	result := a.NumberValue().Div(divisor)
	if result.Less(decIntMin) || result.Greater(decIntMax) {
		return f.FunctionError()
	}
	return CreateInt(int(result.Int64()))
}

func (f *FunctionQUOTIENT) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
	if arg := f.Arg(1); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- INT

type FunctionINT struct{ BaseFunction }

func newFunctionINT(args []Function) Function {
	RequireArgs(args, "INT", 1, 1)
	return &FunctionINT{BaseFunction: NewBase("INT", OperandTypeNumber, args)}
}

func (f *FunctionINT) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	// Excel INT 语义：向下取整(Floor)。
	return CreateDecimal(a.NumberValue().Floor())
}

func (f *FunctionINT) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- CEILING

type FunctionCEILING struct{ BaseFunction }

func newFunctionCEILING(args []Function) Function {
	RequireArgs(args, "Ceiling", 1, 2)
	return &FunctionCEILING{BaseFunction: NewBase("Ceiling", OperandTypeNumber, args)}
}

func (f *FunctionCEILING) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	if f.ArgCount() == 1 {
		return CreateDecimal(a.NumberValue().Ceil())
	}
	b := f.GetNumber(2, engine, tp)
	if b.IsErrorOrNone() {
		return b
	}
	multiple := b.NumberValue()
	if multiple.IsZero() {
		return Zero
	}
	value := a.NumberValue()
	if multiple.Sign() > 0 {
		r := value.Div(multiple).Ceil().Mul(multiple)
		if isOverflow(r) {
			return f.NumError()
		}
		return CreateDecimal(r)
	}
	if value.Sign() > 0 {
		return f.ParameterError(1)
	}
	r := value.Div(multiple).Ceil().Mul(multiple)
	if isOverflow(r) {
		return f.NumError()
	}
	return CreateDecimal(r)
}

func (f *FunctionCEILING) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
	if arg := f.Arg(1); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- FLOOR

type FunctionFLOOR struct{ BaseFunction }

func newFunctionFLOOR(args []Function) Function {
	RequireArgs(args, "Floor", 1, 2)
	return &FunctionFLOOR{BaseFunction: NewBase("Floor", OperandTypeNumber, args)}
}

func (f *FunctionFLOOR) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	if f.ArgCount() == 1 {
		return CreateDecimal(a.NumberValue().Floor())
	}
	b := f.GetNumber(2, engine, tp)
	if b.IsErrorOrNone() {
		return b
	}
	multiple := b.NumberValue()
	if multiple.IsZero() {
		return Zero
	}
	value := a.NumberValue()
	if multiple.Sign() > 0 {
		r := value.Div(multiple).Floor().Mul(multiple)
		if isOverflow(r) {
			return f.NumError()
		}
		return CreateDecimal(r)
	}
	if value.Sign() > 0 {
		return f.ParameterError(1)
	}
	r := value.Div(multiple).Floor().Mul(multiple)
	if isOverflow(r) {
		return f.NumError()
	}
	return CreateDecimal(r)
}

func (f *FunctionFLOOR) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
	if arg := f.Arg(1); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

func init() {
	RegisterFunction("ABS", newFunctionABS)
	RegisterFunction("SIGN", newFunctionSIGN)
	RegisterFunction("EVEN", newFunctionEVEN)
	RegisterFunction("ODD", newFunctionODD)
	RegisterFunction("DELTA", newFunctionDELTA)
	RegisterFunction("GESTEP", newFunctionGESTEP)
	RegisterFunction("QUOTIENT", newFunctionQUOTIENT)
	RegisterFunction("INT", newFunctionINT)
	RegisterFunction("CEILING", newFunctionCEILING)
	RegisterFunction("FLOOR", newFunctionFLOOR)
}
