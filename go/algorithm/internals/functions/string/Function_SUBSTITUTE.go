package string

import "strings"

// FunctionSUBSTITUTE 对应 C# Function_SUBSTITUTE：替换文本；
// 第 4 个参数指定替换第几次出现。
type FunctionSUBSTITUTE struct{ BaseFunction }

func newFunctionSUBSTITUTE(args []Function) Function {
	RequireArgs(args, "Substitute", 3, 4)
	return &FunctionSUBSTITUTE{BaseFunction: NewBase("Substitute", OperandTypeText, args)}
}

func (f *FunctionSUBSTITUTE) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	a2 := f.GetText(2, engine, tp)
	if a2.IsErrorOrNone() {
		return a2
	}
	a3 := f.GetText(3, engine, tp)
	if a3.IsErrorOrNone() {
		return a3
	}
	if f.ArgCount() < 4 {
		if len(a2.TextValue()) == 0 {
			return CreateString(a1.TextValue())
		}
		return CreateString(strings.ReplaceAll(a1.TextValue(), a2.TextValue(), a3.TextValue()))
	}

	a4 := f.GetNumber(4, engine, tp)
	if a4.IsErrorOrNone() {
		return a4
	}
	text := toUnits(a1.TextValue())
	oldtext := toUnits(a2.TextValue())
	newtext := a3.TextValue()
	replaceIndex, ok := tryGetInt(a4)
	if !ok {
		return f.ParameterError(4)
	}
	if len(oldtext) == 0 {
		return CreateString(a1.TextValue())
	}
	if replaceIndex < 1 {
		return f.ParameterError(4)
	}

	var sb strings.Builder
	currentIndex := 0
	foundCount := 0
	searchPos := 0
	for searchPos <= len(text)-len(oldtext) {
		foundPos := unitsIndexOf(text, oldtext, searchPos)
		if foundPos < 0 {
			break
		}
		foundCount++
		if foundCount == replaceIndex {
			sb.WriteString(fromUnits(text[currentIndex:foundPos]))
			sb.WriteString(newtext)
			currentIndex = foundPos + len(oldtext)
			break
		}
		searchPos = foundPos + len(oldtext)
	}
	if currentIndex == 0 {
		return CreateString(a1.TextValue())
	}
	sb.WriteString(fromUnits(text[currentIndex:]))
	return CreateString(sb.String())
}

func init() { RegisterFunction("SUBSTITUTE", newFunctionSUBSTITUTE) }
