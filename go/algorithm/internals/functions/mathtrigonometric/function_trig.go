package mathtrigonometric

import "math"

// 移植自 Internals/Functions/MathTrigonometric/Function_SIN.cs、Function_COS.cs、
// Function_TAN.cs、Function_COT.cs、Function_SEC.cs、Function_CSC.cs。

type FunctionSIN struct{ BaseFunction }

func newFunctionSIN(args []Function) Function {
	RequireArgs(args, "Sin", 1, 1)
	return &FunctionSIN{BaseFunction: NewBase("Sin", OperandTypeNumber, args)}
}

func (f *FunctionSIN) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	r, ok := fromFloat(math.Sin(a.NumberValue().Float64()))
	if !ok {
		return f.NumError()
	}
	return CreateDecimal(r)
}

func (f *FunctionSIN) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- COS

type FunctionCOS struct{ BaseFunction }

func newFunctionCOS(args []Function) Function {
	RequireArgs(args, "Cos", 1, 1)
	return &FunctionCOS{BaseFunction: NewBase("Cos", OperandTypeNumber, args)}
}

func (f *FunctionCOS) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	r, ok := fromFloat(math.Cos(a.NumberValue().Float64()))
	if !ok {
		return f.NumError()
	}
	return CreateDecimal(r)
}

func (f *FunctionCOS) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- TAN

type FunctionTAN struct{ BaseFunction }

func newFunctionTAN(args []Function) Function {
	RequireArgs(args, "Tan", 1, 1)
	return &FunctionTAN{BaseFunction: NewBase("Tan", OperandTypeNumber, args)}
}

func (f *FunctionTAN) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	v := a.NumberValue().Float64()
	cos := math.Cos(v)
	if cos == 0 {
		return f.Div0Error()
	}
	r, ok := fromFloat(math.Sin(v) / cos)
	if !ok {
		return f.NumError()
	}
	return CreateDecimal(r)
}

func (f *FunctionTAN) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- COT

type FunctionCOT struct{ BaseFunction }

func newFunctionCOT(args []Function) Function {
	RequireArgs(args, "Cot", 1, 1)
	return &FunctionCOT{BaseFunction: NewBase("Cot", OperandTypeNumber, args)}
}

func (f *FunctionCOT) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	d := math.Tan(a.NumberValue().Float64())
	if d == 0 {
		return f.Div0Error()
	}
	r, ok := fromFloat(1.0 / d)
	if !ok {
		return f.NumError()
	}
	return CreateDecimal(r)
}

func (f *FunctionCOT) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- SEC

type FunctionSEC struct{ BaseFunction }

func newFunctionSEC(args []Function) Function {
	RequireArgs(args, "Sec", 1, 1)
	return &FunctionSEC{BaseFunction: NewBase("Sec", OperandTypeNumber, args)}
}

func (f *FunctionSEC) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	d := math.Cos(a.NumberValue().Float64())
	if d == 0 {
		return f.Div0Error()
	}
	r, ok := fromFloat(1.0 / d)
	if !ok {
		return f.NumError()
	}
	return CreateDecimal(r)
}

func (f *FunctionSEC) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

// ---------------------------------------------------------------- CSC

type FunctionCSC struct{ BaseFunction }

func newFunctionCSC(args []Function) Function {
	RequireArgs(args, "Csc", 1, 1)
	return &FunctionCSC{BaseFunction: NewBase("Csc", OperandTypeNumber, args)}
}

func (f *FunctionCSC) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	d := math.Sin(a.NumberValue().Float64())
	if d == 0 {
		return f.Div0Error()
	}
	r, ok := fromFloat(1.0 / d)
	if !ok {
		return f.NumError()
	}
	return CreateDecimal(r)
}

func (f *FunctionCSC) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

func init() {
	RegisterFunction("SIN", newFunctionSIN)
	RegisterFunction("COS", newFunctionCOS)
	RegisterFunction("TAN", newFunctionTAN)
	RegisterFunction("COT", newFunctionCOT)
	RegisterFunction("SEC", newFunctionSEC)
	RegisterFunction("CSC", newFunctionCSC)
}
