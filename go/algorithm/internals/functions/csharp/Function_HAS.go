package csharp

// FunctionHAS 对应 C# Function_HAS：判断键值集合 / JSON / 数组是否包含指定 key。
type FunctionHAS struct{ BaseFunction }

func newFunctionHAS(args []Function) Function {
	RequireArgs(args, "Has", 2, 2)
	return &FunctionHAS{BaseFunction: NewBase("Has", OperandTypeBoolean, args)}
}

func (f *FunctionHAS) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.EvalArg(0, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	a2 := f.GetText(2, engine, tp)
	if a2.IsErrorOrNone() {
		return a2
	}
	text := a2.TextValue()
	switch v := a1.(type) {
	case *OperandKeyValueList:
		return CreateBool(v.ContainsKey(a2))
	}
	// 说明：Go 的 operands.OperandKeyValue 未实现 Operand 接口（缺少 String 方法），
	// 因此 C# 中 "args1 is OperandKeyValue" 的分支在 Go 中不可达。
	if a1.IsJson() {
		json := a1.JsonValue()
		if json.IsArray() {
			for i := 0; i < json.Count(); i++ {
				if jsonValueEquals(json.GetChild(i), text) {
					return True
				}
			}
		} else if json.GetChild(text) != nil {
			return True
		}
		return False
	}
	if a1.IsArray() {
		for _, item := range a1.ArrayValue() {
			t := item.ToText("")
			if t.IsErrorOrNone() {
				continue
			}
			if t.TextValue() == text {
				return True
			}
		}
		return False
	}
	return f.ParameterError(1)
}

func init() { RegisterFunction("HAS", newFunctionHAS) }
