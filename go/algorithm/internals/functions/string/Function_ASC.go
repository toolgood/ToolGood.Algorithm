package string

// FunctionASC 对应 C# Function_ASC：全角转半角。
type FunctionASC struct{ BaseFunction }

func newFunctionASC(args []Function) Function {
	return &FunctionASC{BaseFunction: NewBase("Asc", OperandTypeText, args)}
}

func (f *FunctionASC) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetText(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	return CreateString(fBaseToDBC(a.TextValue()))
}

// fBaseToDBC 全角字符转半角：只处理 0x3000（全角空格）与 0xFF01-0xFF5E。
func fBaseToDBC(input string) string {
	runes := []rune(input)
	needModify := false
	for _, c := range runes {
		if c == 12288 || (c > 65280 && c < 65375) {
			needModify = true
			break
		}
	}
	if !needModify {
		return input
	}
	for i, c := range runes {
		if c == 12288 {
			runes[i] = 32
		} else if c > 65280 && c < 65375 {
			runes[i] = c - 65248
		}
	}
	return string(runes)
}

func init() { RegisterFunction("ASC", newFunctionASC) }
