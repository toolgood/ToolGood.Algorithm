package string

// FunctionUNICODE 对应 C# Function_UNICODE：返回首字符的 Unicode 码点。
type FunctionUNICODE struct{ BaseFunction }

func newFunctionUNICODE(args []Function) Function {
	RequireArgs(args, "Unicode", 1, 1)
	return &FunctionUNICODE{BaseFunction: NewBase("Unicode", OperandTypeNumber, args)}
}

func (f *FunctionUNICODE) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	units := toUnits(a1.TextValue())
	if len(units) == 0 {
		return f.ParameterError(1)
	}
	first := units[0]
	// 首字符为孤立代理项时按参数错误处理
	if first >= 0xD800 && first <= 0xDBFF {
		if len(units) == 1 || units[1] < 0xDC00 || units[1] > 0xDFFF {
			return f.ParameterError(1)
		}
		code := (int(first)-0xD800)*0x400 + (int(units[1]) - 0xDC00) + 0x10000
		return CreateInt(code)
	}
	if first >= 0xDC00 && first <= 0xDFFF {
		return f.ParameterError(1)
	}
	return CreateInt(int(first))
}

func init() { RegisterFunction("UNICODE", newFunctionUNICODE) }
