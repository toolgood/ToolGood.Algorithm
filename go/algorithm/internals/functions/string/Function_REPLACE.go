package string

import "strings"

// FunctionREPLACE 对应 C# Function_REPLACE：
// 3 个参数为全量替换，4 个参数为按位置替换。
type FunctionREPLACE struct{ BaseFunction }

func newFunctionREPLACE(args []Function) Function {
	RequireArgs(args, "Replace", 3, 4)
	return &FunctionREPLACE{BaseFunction: NewBase("Replace", OperandTypeText, args)}
}

func (f *FunctionREPLACE) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	oldtext := a1.TextValue()
	if f.ArgCount() < 4 {
		a22 := f.GetText(2, engine, tp)
		if a22.IsErrorOrNone() {
			return a22
		}
		a32 := f.GetText(3, engine, tp)
		if a32.IsErrorOrNone() {
			return a32
		}
		old := a22.TextValue()
		newstr := a32.TextValue()
		if len(old) == 0 || !strings.Contains(oldtext, old) {
			return a1
		}
		return CreateString(strings.ReplaceAll(oldtext, old, newstr))
	}

	a2 := f.GetNumber(2, engine, tp)
	if a2.IsErrorOrNone() {
		return a2
	}
	a3 := f.GetNumber(3, engine, tp)
	if a3.IsErrorOrNone() {
		return a3
	}
	a4 := f.GetText(4, engine, tp)
	if a4.IsErrorOrNone() {
		return a4
	}
	startValue, ok := tryGetInt(a2)
	if !ok {
		return f.ParameterError(2)
	}
	length, ok := tryGetInt(a3)
	if !ok {
		return f.ParameterError(3)
	}
	newtext := a4.TextValue()

	units := toUnits(oldtext)
	startIndex := int64(startValue) - int64(engine.ExcelIndex)
	if startIndex < 0 {
		return f.ParameterError(2)
	}
	if length < 0 {
		return f.ParameterError(3)
	}
	if startIndex >= int64(len(units)) {
		return CreateString(oldtext + newtext)
	}
	if int64(length) > int64(len(units))-startIndex {
		length = len(units) - int(startIndex)
	}
	start := int(startIndex)

	var sb strings.Builder
	sb.WriteString(fromUnits(units[:start]))
	sb.WriteString(newtext)
	endIndex := start + length
	if endIndex < len(units) {
		sb.WriteString(fromUnits(units[endIndex:]))
	}
	return CreateString(sb.String())
}

func init() { RegisterFunction("REPLACE", newFunctionREPLACE) }
