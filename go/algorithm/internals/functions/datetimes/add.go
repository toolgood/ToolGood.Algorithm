package datetimes

// ADDDAYS/ADDHOURS/ADDMINUTES/ADDMONTHS/ADDSECONDS/ADDYEARS
// 对应 csharp/ToolGood.Algorithm/Internals/Functions/DateTimes/Function_ADD*.cs

type FunctionADDDAYS struct{ BaseFunction }

func newFunctionADDDAYS(args []Function) Function {
	RequireArgs(args, "ADDDAYS", 2, 2)
	return &FunctionADDDAYS{BaseFunction: NewBase("ADDDAYS", OperandTypeDate, args)}
}

func (f *FunctionADDDAYS) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	d := f.GetDate(1, engine, tp)
	if d.IsErrorOrNone() {
		return d
	}
	n := f.GetNumber(2, engine, tp)
	if n.IsErrorOrNone() {
		return n
	}
	v, ok := intValue(n.NumberValue())
	if !ok {
		return parameterError(f, 2)
	}
	return CreateMyDate(d.DateValue().AddDays(v))
}

type FunctionADDHOURS struct{ BaseFunction }

func newFunctionADDHOURS(args []Function) Function {
	RequireArgs(args, "ADDHOURS", 2, 2)
	return &FunctionADDHOURS{BaseFunction: NewBase("ADDHOURS", OperandTypeDate, args)}
}

func (f *FunctionADDHOURS) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	d := f.GetDate(1, engine, tp)
	if d.IsErrorOrNone() {
		return d
	}
	n := f.GetNumber(2, engine, tp)
	if n.IsErrorOrNone() {
		return n
	}
	v, ok := intValue(n.NumberValue())
	if !ok {
		return parameterError(f, 2)
	}
	return CreateMyDate(d.DateValue().AddHours(v))
}

type FunctionADDMINUTES struct{ BaseFunction }

func newFunctionADDMINUTES(args []Function) Function {
	RequireArgs(args, "ADDMINUTES", 2, 2)
	return &FunctionADDMINUTES{BaseFunction: NewBase("ADDMINUTES", OperandTypeDate, args)}
}

func (f *FunctionADDMINUTES) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	d := f.GetDate(1, engine, tp)
	if d.IsErrorOrNone() {
		return d
	}
	n := f.GetNumber(2, engine, tp)
	if n.IsErrorOrNone() {
		return n
	}
	v, ok := intValue(n.NumberValue())
	if !ok {
		return parameterError(f, 2)
	}
	return CreateMyDate(d.DateValue().AddMinutes(v))
}

type FunctionADDMONTHS struct{ BaseFunction }

func newFunctionADDMONTHS(args []Function) Function {
	RequireArgs(args, "ADDMONTHS", 2, 2)
	return &FunctionADDMONTHS{BaseFunction: NewBase("ADDMONTHS", OperandTypeDate, args)}
}

func (f *FunctionADDMONTHS) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	d := f.GetDate(1, engine, tp)
	if d.IsErrorOrNone() {
		return d
	}
	n := f.GetNumber(2, engine, tp)
	if n.IsErrorOrNone() {
		return n
	}
	v, ok := intValue(n.NumberValue())
	if !ok {
		return parameterError(f, 2)
	}
	return CreateMyDate(d.DateValue().AddMonths(v))
}

type FunctionADDSECONDS struct{ BaseFunction }

func newFunctionADDSECONDS(args []Function) Function {
	RequireArgs(args, "ADDSECONDS", 2, 2)
	return &FunctionADDSECONDS{BaseFunction: NewBase("ADDSECONDS", OperandTypeDate, args)}
}

func (f *FunctionADDSECONDS) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	d := f.GetDate(1, engine, tp)
	if d.IsErrorOrNone() {
		return d
	}
	n := f.GetNumber(2, engine, tp)
	if n.IsErrorOrNone() {
		return n
	}
	v, ok := intValue(n.NumberValue())
	if !ok {
		return parameterError(f, 2)
	}
	return CreateMyDate(d.DateValue().AddSeconds(v))
}

type FunctionADDYEARS struct{ BaseFunction }

func newFunctionADDYEARS(args []Function) Function {
	RequireArgs(args, "ADDYEARS", 2, 2)
	return &FunctionADDYEARS{BaseFunction: NewBase("ADDYEARS", OperandTypeDate, args)}
}

func (f *FunctionADDYEARS) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	d := f.GetDate(1, engine, tp)
	if d.IsErrorOrNone() {
		return d
	}
	n := f.GetNumber(2, engine, tp)
	if n.IsErrorOrNone() {
		return n
	}
	v, ok := intValue(n.NumberValue())
	if !ok {
		return parameterError(f, 2)
	}
	return CreateMyDate(d.DateValue().AddYears(v))
}

func init() {
	RegisterFunction("ADDDAYS", newFunctionADDDAYS)
	RegisterFunction("ADDHOURS", newFunctionADDHOURS)
	RegisterFunction("ADDMINUTES", newFunctionADDMINUTES)
	RegisterFunction("ADDMONTHS", newFunctionADDMONTHS)
	RegisterFunction("ADDSECONDS", newFunctionADDSECONDS)
	RegisterFunction("ADDYEARS", newFunctionADDYEARS)
}
