package flow

// 移植自 Internals/Functions/Flow/Function_ISODD.cs。

// FunctionISODD 判断是否奇数。
type FunctionISODD struct{ BaseFunction }

func newFunctionISODD(args []Function) Function {
	RequireArgs(args, "IsOdd", 1, 1)
	return &FunctionISODD{BaseFunction: NewBase("IsOdd", OperandTypeBoolean, args)}
}

// Evaluate 判断数字（截断取整后）是否为奇数，非数字返回 false。
func (f *FunctionISODD) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.EvalArg(0, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	if a.IsNumber() {
		// 用 Trunc 取整，避免大数强转溢出；取模符号跟随被除数，需先取绝对值。
		number := a.NumberValue().Trunc()
		if number.Abs().Mod(DecFromInt(2)).Equal(DecimalOne) {
			return True
		}
	}
	return False
}

func (f *FunctionISODD) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

func init() { RegisterFunction("ISODD", newFunctionISODD) }
