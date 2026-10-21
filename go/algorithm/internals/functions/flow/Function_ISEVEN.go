package flow

// 移植自 Internals/Functions/Flow/Function_ISEVEN.cs。

// FunctionISEVEN 判断是否偶数。
type FunctionISEVEN struct{ BaseFunction }

func newFunctionISEVEN(args []Function) Function {
	RequireArgs(args, "IsEven", 1, 1)
	return &FunctionISEVEN{BaseFunction: NewBase("IsEven", OperandTypeBoolean, args)}
}

// Evaluate 判断数字（截断取整后）是否为偶数，非数字返回 false。
func (f *FunctionISEVEN) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.EvalArg(0, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	if a.IsNumber() {
		// 用 Trunc 取整，避免大数强转溢出。
		number := a.NumberValue().Trunc()
		if number.Mod(DecFromInt(2)).IsZero() {
			return True
		}
	}
	return False
}

func (f *FunctionISEVEN) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

func init() { RegisterFunction("ISEVEN", newFunctionISEVEN) }
