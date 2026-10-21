package string

// FunctionFIND 对应 C# Function_FIND：区分大小写的子串查找（不支持通配符）。
type FunctionFIND struct{ BaseFunction }

func newFunctionFIND(args []Function) Function {
	RequireArgs(args, "Find", 2, 3)
	return &FunctionFIND{BaseFunction: NewBase("Find", OperandTypeNumber, args)}
}

func (f *FunctionFIND) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	a2 := f.GetText(2, engine, tp)
	if a2.IsErrorOrNone() {
		return a2
	}
	text := toUnits(a2.TextValue())
	pattern := toUnits(a1.TextValue())
	if f.ArgCount() < 3 {
		index := unitsIndexOf(text, pattern, 0)
		if index < 0 {
			// 未找到：Excel 模式（下标从 1 开始）返回错误，C# 模式（下标从 0 开始）返回 -1
			if engine.ExcelIndex == 1 {
				return f.FunctionError()
			}
			return CreateInt(-1)
		}
		return CreateInt(index + engine.ExcelIndex)
	}
	count := f.GetNumber(3, engine, tp)
	if count.IsErrorOrNone() {
		return count
	}
	startValue, ok := tryGetInt(count)
	if !ok {
		return f.ParameterError(3)
	}
	startIndex := startValue - engine.ExcelIndex
	if startIndex < 0 || startIndex >= len(text) {
		return f.ParameterError(3)
	}
	p2 := unitsIndexOf(text, pattern, startIndex)
	if p2 < 0 {
		if engine.ExcelIndex == 1 {
			return f.FunctionError()
		}
		return CreateInt(-1)
	}
	return CreateInt(p2 + engine.ExcelIndex)
}

func init() { RegisterFunction("FIND", newFunctionFIND) }
