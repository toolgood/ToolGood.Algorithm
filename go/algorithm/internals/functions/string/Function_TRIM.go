package string

import "strings"

// FunctionTRIM 对应 C# Function_TRIM：去掉首尾半角空格，并把连续空格压成一个。
type FunctionTRIM struct{ BaseFunction }

func newFunctionTRIM(args []Function) Function {
	RequireArgs(args, "Trim", 1, 1)
	return &FunctionTRIM{BaseFunction: NewBase("Trim", OperandTypeText, args)}
}

func (f *FunctionTRIM) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	// Excel 的 TRIM 只处理半角空格(0x20)，不处理 Tab/换行/不换行空格等其他空白字符
	text := strings.Trim(a1.TextValue(), " ")
	text = collapseSpaces(text)
	if text == a1.TextValue() {
		return a1
	}
	return CreateString(text)
}

// collapseSpaces 把连续的多个半角空格压成一个（等价于正则 " +" 替换）。
func collapseSpaces(text string) string {
	var sb strings.Builder
	sb.Grow(len(text))
	prevSpace := false
	for i := 0; i < len(text); i++ {
		c := text[i]
		if c == ' ' {
			if prevSpace {
				continue
			}
			prevSpace = true
		} else {
			prevSpace = false
		}
		sb.WriteByte(c)
	}
	return sb.String()
}

func init() { RegisterFunction("TRIM", newFunctionTRIM) }
