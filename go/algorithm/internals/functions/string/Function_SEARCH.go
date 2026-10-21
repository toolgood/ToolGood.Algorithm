package string

import "unicode"

// FunctionSEARCH 对应 C# Function_SEARCH：大小写不敏感的通配符查找，
// 支持 Excel 的 ? 与 * 及 ~ 转义。
type FunctionSEARCH struct{ BaseFunction }

func newFunctionSEARCH(args []Function) Function {
	// 与 Java 版一致：要求 2~3 个参数(find_text, within_text, [start_num])
	RequireArgs(args, "Search", 2, 3)
	return &FunctionSEARCH{BaseFunction: NewBase("Search", OperandTypeNumber, args)}
}

func (f *FunctionSEARCH) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a1 := f.GetText(1, engine, tp)
	if a1.IsErrorOrNone() {
		return a1
	}
	a2 := f.GetText(2, engine, tp)
	if a2.IsErrorOrNone() {
		return a2
	}
	text := []rune(a2.TextValue())
	pattern := a1.TextValue()

	if f.ArgCount() < 3 {
		index := wildcardIndexOf(text, pattern, 0)
		if index < 0 {
			// 未找到：Excel 模式（下标从 1 开始）返回错误，C# 模式（下标从 0 开始）返回 -1
			if engine.ExcelIndex == 1 {
				return f.FunctionError()
			}
			return CreateInt(-1)
		}
		return CreateInt(index + engine.ExcelIndex)
	}

	a3 := f.GetNumber(3, engine, tp)
	if a3.IsErrorOrNone() {
		return a3
	}
	startValue, ok := tryGetInt(a3)
	if !ok {
		return f.ParameterError(3)
	}
	startIndex := startValue - engine.ExcelIndex
	if startIndex < 0 || startIndex >= len(text) {
		return f.ParameterError(3)
	}
	p2 := wildcardIndexOf(text, pattern, startIndex)
	if p2 < 0 {
		if engine.ExcelIndex == 1 {
			return f.FunctionError()
		}
		return CreateInt(-1)
	}
	return CreateInt(p2 + engine.ExcelIndex)
}

type wildcardTokenKind int

const (
	wildcardLiteral wildcardTokenKind = iota
	wildcardAnyOne
	wildcardAnySeq
)

type wildcardToken struct {
	kind  wildcardTokenKind
	value rune
}

// wildcardIndexOf 在指定起始位置起做大小写不敏感的通配符查找。
// 使用手写匹配器，避免每次调用都编译正则的开销。
func wildcardIndexOf(text []rune, pattern string, startIndex int) int {
	if startIndex < 0 {
		startIndex = 0
	}
	if startIndex > len(text) {
		return -1
	}
	tokens := parseWildcardPattern(pattern)
	if len(tokens) == 0 {
		return startIndex
	}
	for start := startIndex; start <= len(text); start++ {
		if matchWildcard(text, tokens, start) {
			return start
		}
	}
	return -1
}

// parseWildcardPattern 将通配符 pattern 解析为 token 序列，字面字符统一转为大写。
func parseWildcardPattern(pattern string) []wildcardToken {
	runes := []rune(pattern)
	list := make([]wildcardToken, 0, len(runes))
	for i := 0; i < len(runes); i++ {
		c := runes[i]
		switch {
		case c == '~':
			if i+1 < len(runes) && (runes[i+1] == '?' || runes[i+1] == '*' || runes[i+1] == '~') {
				list = append(list, wildcardToken{kind: wildcardLiteral, value: unicode.ToUpper(runes[i+1])})
				i++
			} else {
				list = append(list, wildcardToken{kind: wildcardLiteral, value: '~'})
			}
		case c == '*':
			list = append(list, wildcardToken{kind: wildcardAnySeq})
		case c == '?':
			list = append(list, wildcardToken{kind: wildcardAnyOne})
		default:
			list = append(list, wildcardToken{kind: wildcardLiteral, value: unicode.ToUpper(c)})
		}
	}
	return list
}

// matchWildcard 判断 pattern 是否从 text 的 start 位置开始完整匹配（贪心 + 回溯）。
func matchWildcard(text []rune, tokens []wildcardToken, start int) bool {
	ti := 0
	si := start
	starToken := -1
	starMatch := 0

	for ti < len(tokens) {
		if si >= len(text) {
			break
		}
		tok := tokens[ti]
		switch tok.kind {
		case wildcardLiteral:
			if unicode.ToUpper(text[si]) == tok.value {
				ti++
				si++
			} else if starToken != -1 {
				ti = starToken + 1
				starMatch++
				si = starMatch
			} else {
				return false
			}
		case wildcardAnyOne:
			ti++
			si++
		default:
			starToken = ti
			starMatch = si
			ti++
		}
	}
	for ti < len(tokens) && tokens[ti].kind == wildcardAnySeq {
		ti++
	}
	return ti == len(tokens)
}

func init() { RegisterFunction("SEARCH", newFunctionSEARCH) }
