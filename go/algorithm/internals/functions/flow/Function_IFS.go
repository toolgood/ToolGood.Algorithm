package flow

// 移植自 Internals/Functions/Flow/Function_IFS.cs。

// FunctionIFS 多条件节点。
type FunctionIFS struct{ BaseFunction }

func newFunctionIFS(args []Function) Function {
	if len(args) < 4 {
		panic("Function 'Ifs' requires at least 4 parameters.")
	}
	if len(args)%2 != 0 {
		panic("Function 'Ifs' requires an even number of parameters.")
	}
	return &FunctionIFS{BaseFunction: NewBase("Ifs", OperandTypeNone, args)}
}

// Evaluate 依次判断条件，命中时惰性求值对应结果。
func (f *FunctionIFS) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	for i := 0; i < f.ArgCount()-1; i += 2 {
		condition := f.GetBoolean(i+1, engine, tp)
		if condition.IsErrorOrNone() {
			return condition
		}
		if condition.BooleanValue() {
			return f.Arg(i+1).Evaluate(engine, tp)
		}
	}
	return f.FunctionError()
}

// ResultType 取第一个非 NONE 的结果分支类型。
func (f *FunctionIFS) ResultType() OperandType {
	for i := 0; i < f.ArgCount()-1; i += 2 {
		if t := f.Arg(i + 1).ResultType(); t != OperandTypeNone {
			return t
		}
	}
	return OperandTypeNone
}

func (f *FunctionIFS) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	for i := 0; i < f.ArgCount()-1; i += 2 {
		f.Arg(i).GetParameterTypes(engine, result, OperandTypeBoolean)
		f.Arg(i+1).GetParameterTypes(engine, result, OperandTypeNone)
	}
}

func init() { RegisterFunction("IFS", newFunctionIFS) }
