package internals

import "strings"

var fullWidthRanges = [][2]rune{
	{0x00C0, 0x00D6},
	{0x00D8, 0x00F6},
	{0x00F8, 0x00FF},
	{0x0100, 0x1FFF},
	{0x2C00, 0x2FFF},
	{0x3040, 0x318F},
	{0x3300, 0x337F},
	{0x3400, 0x3FFF},
	{0x4E00, 0x9FFF},
	{0xA000, 0xD7FF},
	{0xF900, 0xFAFF},
	{0xFF00, 0xFFF0},
}

var specialChars = map[rune]string{
	215:    "*",
	247:    "/",
	8216:   "'",
	8217:   "'",
	8220:   `"`,
	8221:   `"`,
	12288:  " ",
	12304:  "[",
	12305:  "]",
	12308:  "(",
	12309:  ")",
}

// StandardChar 标准化单个字符：小写转大写、全角转半角、CJK 统一转为下划线。
func StandardChar(c rune) rune {
	if c <= 0 {
		return c
	}
	if c == '\r' {
		return '\n'
	}
	if c == '\t' || c == '\f' {
		return ' '
	}
	if c < 'a' {
		return c
	}
	if c <= 'z' {
		return c - 32
	}
	if c > 65280 && c < 65375 {
		c = c - 65248
		return toUpperRune(c)
	}
	for _, r := range fullWidthRanges {
		if c >= r[0] && c <= r[1] {
			return '_'
		}
	}
	if c < 127 {
		return c
	}
	if mapped, ok := specialChars[c]; ok {
		return []rune(mapped)[0]
	}
	return toUpperRune(c)
}

func toUpperRune(c rune) rune {
	if c >= 'a' && c <= 'z' {
		return c - 32
	}
	return toUpperRuneSlow(c)
}

func toUpperRuneSlow(c rune) rune {
	s := strings.ToUpper(string(c))
	r := []rune(s)
	if len(r) == 1 {
		return r[0]
	}
	return c
}

// StandardString 标准化整个字符串。
func StandardString(s string) string {
	runes := []rune(s)
	out := make([]rune, len(runes))
	for i, c := range runes {
		out[i] = StandardChar(c)
	}
	return string(out)
}

func StandardRunes(runes []rune) []rune {
	out := make([]rune, len(runes))
	for i, c := range runes {
		out[i] = StandardChar(c)
	}
	return out
}

func equalsOnce(left, right string) bool {
	l := []rune(left)
	r := []rune(right)
	if len(l) != len(r) {
		return false
	}
	for i := range l {
		if l[i] != r[i] && StandardChar(l[i]) != StandardChar(r[i]) {
			return false
		}
	}
	return true
}

// StrEquals 使用标准化字符比较 left 与任意一个 args。
func StrEquals(left string, args ...string) bool {
	if left == "" && len(args) == 0 {
		return false
	}
	for _, arg := range args {
		if equalsOnce(left, arg) {
			return true
		}
	}
	return false
}

// SplitFormula 按顶层分隔符切分公式。
func SplitFormula(formula string, splitChars string) []string {
	result := []string{}
	inSquare, inBrace, inText := false, false, false
	var textChar rune
	brackets := 0
	var buf []rune
	runes := []rune(formula)
	n := len(runes)
	for i := 0; i < n; i++ {
		c := runes[i]
		switch {
		case inSquare:
			buf = append(buf, c)
			if c == ']' {
				inSquare = false
			}
		case inBrace:
			buf = append(buf, c)
			if c == '}' {
				inBrace = false
			}
		case inText:
			buf = append(buf, c)
			if c == '\\' {
				i++
				if i < n {
					buf = append(buf, runes[i])
				}
			} else if c == textChar {
				inText = false
			}
		case brackets == 0 && strings.ContainsRune(splitChars, c):
			result = append(result, string(buf), string(c))
			buf = nil
		default:
			buf = append(buf, c)
			if c == '\'' || c == '"' || c == '`' {
				textChar = c
				inText = true
			} else if c == '[' {
				inSquare = true
			} else if c == '{' {
				inBrace = true
			} else if c == '(' {
				brackets++
			} else if c == ')' {
				brackets--
			}
		}
	}
	if len(buf) > 0 {
		result = append(result, string(buf))
	}
	return result
}

// SplitFormulaForAnd 按顶层 && / || 切分公式。
func SplitFormulaForAnd(formula string) []string {
	result := []string{}
	inSquare, inBrace, inText := false, false, false
	var textChar rune
	brackets := 0
	var buf []rune
	runes := []rune(formula)
	n := len(runes)
	for i := 0; i < n; i++ {
		c := runes[i]
		switch {
		case inSquare:
			buf = append(buf, c)
			if c == ']' {
				inSquare = false
			}
		case inBrace:
			buf = append(buf, c)
			if c == '}' {
				inBrace = false
			}
		case inText:
			buf = append(buf, c)
			if c == '\\' {
				i++
				if i < n {
					buf = append(buf, runes[i])
				}
			} else if c == textChar {
				inText = false
			}
		case c == '&' && brackets == 0:
			if i+1 < n && runes[i+1] == '&' {
				i++
				result = append(result, string(buf))
				buf = nil
			} else {
				result = append(result, string(buf))
			}
		case c == '|' && brackets == 0:
			if i+1 < n && runes[i+1] == '|' {
				i++
				result = append(result, string(buf))
				buf = nil
				joined := strings.Join(result, "&&")
				buf = []rune(joined + "||")
				result = nil
			} else {
				result = append(result, string(buf))
			}
		default:
			buf = append(buf, c)
			if c == '\'' || c == '"' || c == '`' {
				textChar = c
				inText = true
			} else if c == '[' {
				inSquare = true
			} else if c == '{' {
				inBrace = true
			} else if c == '(' {
				brackets++
			} else if c == ')' {
				brackets--
			}
		}
	}
	if len(buf) > 0 {
		result = append(result, string(buf))
	}
	return result
}
