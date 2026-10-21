package string

// FunctionJIS 对应 C# Function_JIS：半角转全角。
type FunctionJIS struct{ BaseFunction }

func newFunctionJIS(args []Function) Function {
	RequireArgs(args, "Jis", 1, 1)
	return &FunctionJIS{BaseFunction: NewBase("Jis", OperandTypeText, args)}
}

func (f *FunctionJIS) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetText(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	return CreateString(fBaseToSBC(a.TextValue()))
}

// fBaseToSBC 半角字符转全角：空格转 0x3000，0x21-0x7E 转 0xFF01-0xFF5E。
func fBaseToSBC(input string) string {
	runes := []rune(input)
	needModify := false
	for _, c := range runes {
		if c == ' ' || (c > 32 && c < 127) {
			needModify = true
			break
		}
	}
	if !needModify {
		return input
	}
	for i, c := range runes {
		if c == ' ' {
			runes[i] = 12288
		} else if c > 32 && c < 127 {
			runes[i] = c + 65248
		}
	}
	return string(runes)
}

func init() { RegisterFunction("JIS", newFunctionJIS) }
