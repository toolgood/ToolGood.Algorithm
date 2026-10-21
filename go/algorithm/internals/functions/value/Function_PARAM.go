package value

// 移植自 Internals/Functions/Value/Function_PARAM.cs。

// FunctionPARAM 取参数值，可带默认值。
type FunctionPARAM struct{ BaseFunction }

func newFunctionPARAM(args []Function) Function {
	RequireArgs(args, "Param", 1, 2)
	return &FunctionPARAM{BaseFunction: NewBase("Param", OperandTypeNone, args)}
}

// Evaluate 先走临时参数解析函数，再走引擎参数；缺失时返回默认值。
func (f *FunctionPARAM) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	name := f.GetText(1, engine, tp)
	if name.IsErrorOrNone() {
		return name
	}
	if tp != nil {
		if r := tp(name.TextValue()); r != nil {
			return r
		}
	}
	result := engine.GetParameter(name.TextValue())
	if result.IsErrorOrNone() && f.ArgCount() > 1 {
		return f.EvalArg(1, engine, tp)
	}
	return result
}

func (f *FunctionPARAM) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeText)
	}
	if arg := f.Arg(1); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNone)
	}
}

func init() { RegisterFunction("PARAM", newFunctionPARAM) }
