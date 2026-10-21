package string

import "unicode"

// FunctionPROPER 对应 C# Function_PROPER：单词首字母大写，其余小写。
type FunctionPROPER struct{ BaseFunction }

func newFunctionPROPER(args []Function) Function {
	RequireArgs(args, "Proper", 1, 1)
	return &FunctionPROPER{BaseFunction: NewBase("Proper", OperandTypeText, args)}
}

func (f *FunctionPROPER) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	text := a1.TextValue()
	if text == "" {
		return CreateString(text)
	}
	runes := []rune(text)

	needModify := false
	isFirst := true
	for _, c := range runes {
		if !unicode.IsLetter(c) {
			isFirst = true
		} else {
			if isFirst {
				if unicode.IsLower(c) {
					needModify = true
				}
			} else {
				if unicode.IsUpper(c) {
					needModify = true
				}
			}
			if needModify {
				break
			}
			isFirst = false
		}
	}
	if !needModify {
		return a1
	}

	isFirst = true
	for i, c := range runes {
		if !unicode.IsLetter(c) {
			isFirst = true
			continue
		}
		if isFirst {
			runes[i] = toUpperRune(c)
		} else {
			runes[i] = toLowerRune(c)
		}
		isFirst = false
	}
	return CreateString(string(runes))
}

// toUpperRune 对应 char.ToUpperInvariant：不会把一个字符展开成多个字符。
func toUpperRune(c rune) rune {
	u := unicode.ToUpper(c)
	if u == c {
		return c
	}
	if len([]rune(string(u))) != 1 {
		return c
	}
	return u
}

// toLowerRune 对应 char.ToLowerInvariant。
func toLowerRune(c rune) rune {
	l := unicode.ToLower(c)
	if l == c {
		return c
	}
	if len([]rune(string(l))) != 1 {
		return c
	}
	return l
}

func init() { RegisterFunction("PROPER", newFunctionPROPER) }
