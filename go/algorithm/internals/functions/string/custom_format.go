package string

import "strings"

// 本文件实现 .NET 自定义数字格式（custom numeric format string）的子集。

type fmtKind int

const (
	fmtLiteral fmtKind = iota
	fmtDigitZero
	fmtDigitHash
	fmtPoint
	fmtComma
	fmtExponent
)

type fmtToken struct {
	kind      fmtKind
	text      string
	expDigits int
	expSign   byte
}

type fmtMeta struct {
	intPlaceholders  int
	intZeros         int
	fracPlaceholders int
	fracZeros        int
	hasPoint         bool
	grouping         bool
	groupSize        int
	scaling          int
	percent          bool
	permille         bool
	hasExponent      bool
}

// splitFormatSections 按顶层 ';' 切分格式串（引号与反斜杠内的分号不切分）。
func splitFormatSections(pattern string) []string {
	var sections []string
	var sb strings.Builder
	runes := []rune(pattern)
	for i := 0; i < len(runes); i++ {
		c := runes[i]
		switch {
		case c == '\'' || c == '"':
			quote := c
			sb.WriteRune(c)
			i++
			for i < len(runes) {
				sb.WriteRune(runes[i])
				if runes[i] == quote {
					if i+1 < len(runes) && runes[i+1] == quote {
						sb.WriteRune(runes[i+1])
						i += 2
						continue
					}
					break
				}
				i++
			}
		case c == '\\' && i+1 < len(runes):
			sb.WriteRune(c)
			sb.WriteRune(runes[i+1])
			i++
		case c == ';':
			sections = append(sections, sb.String())
			sb.Reset()
		default:
			sb.WriteRune(c)
		}
	}
	sections = append(sections, sb.String())
	return sections
}

// tokenizeFormat 解析单个自定义格式段。
func tokenizeFormat(section string) ([]fmtToken, fmtMeta, error) {
	var tokens []fmtToken
	var meta fmtMeta

	intDigits := 0     // 整数区数字占位符个数（用于判断指数是否有效）
	lastIntDigit := -1 // 整数区最后一个数字占位符的 token 下标
	var commaIdx []int // 整数区逗号位置
	inFrac := false

	runes := []rune(section)
	i := 0
	for i < len(runes) {
		c := runes[i]
		switch {
		case c == '\'' || c == '"':
			quote := c
			var sb strings.Builder
			i++
			closed := false
			for i < len(runes) {
				if runes[i] == quote {
					if i+1 < len(runes) && runes[i+1] == quote {
						sb.WriteRune(quote)
						i += 2
						continue
					}
					i++
					closed = true
					break
				}
				sb.WriteRune(runes[i])
				i++
			}
			if !closed {
				return nil, meta, errBadFormat
			}
			tokens = append(tokens, fmtToken{kind: fmtLiteral, text: sb.String()})
		case c == '\\' && i+1 < len(runes):
			tokens = append(tokens, fmtToken{kind: fmtLiteral, text: string(runes[i+1])})
			i += 2
		case c == '0' || c == '#':
			if c == '0' {
				tokens = append(tokens, fmtToken{kind: fmtDigitZero})
			} else {
				tokens = append(tokens, fmtToken{kind: fmtDigitHash})
			}
			if inFrac {
				meta.fracPlaceholders++
				if c == '0' {
					meta.fracZeros++
				}
			} else {
				meta.intPlaceholders++
				intDigits++
				if c == '0' {
					meta.intZeros++
				}
				lastIntDigit = len(tokens) - 1
			}
			i++
		case c == '.':
			if !meta.hasPoint && !inFrac {
				meta.hasPoint = true
				inFrac = true
				tokens = append(tokens, fmtToken{kind: fmtPoint})
			} else {
				tokens = append(tokens, fmtToken{kind: fmtLiteral, text: "."})
			}
			i++
		case c == ',':
			if !inFrac {
				commaIdx = append(commaIdx, len(tokens))
			}
			tokens = append(tokens, fmtToken{kind: fmtComma})
			i++
		case c == '%':
			meta.percent = true
			tokens = append(tokens, fmtToken{kind: fmtLiteral, text: "%"})
			i++
		case c == '‰':
			meta.permille = true
			tokens = append(tokens, fmtToken{kind: fmtLiteral, text: "‰"})
			i++
		case c == 'E' || c == 'e':
			digits, sign, next, ok := parseExponentPart(runes, i)
			if ok && intDigits > 0 {
				tokens = append(tokens, fmtToken{kind: fmtExponent, expDigits: digits, expSign: sign})
				meta.hasExponent = true
				i = next
				continue
			}
			if ok && intDigits == 0 {
				// 没有数字占位符时 "E+0" 整体按字面量输出
				tokens = append(tokens, fmtToken{kind: fmtLiteral, text: string(runes[i:next])})
				i = next
				continue
			}
			tokens = append(tokens, fmtToken{kind: fmtLiteral, text: string(c)})
			i++
		default:
			tokens = append(tokens, fmtToken{kind: fmtLiteral, text: string(c)})
			i++
		}
	}

	// 分组与末尾缩放：整数区中最后一个数字占位符之后的逗号为缩放标记
	last := lastIntDigit
	for _, idx := range commaIdx {
		if last < 0 || idx > last {
			if last >= 0 {
				meta.scaling++
			}
			continue
		}
		// 统计该逗号之后的数字占位符个数作为分组大小
		count := 0
		for k := idx + 1; k <= last; k++ {
			if tokens[k].kind == fmtDigitZero || tokens[k].kind == fmtDigitHash {
				count++
			}
		}
		if count <= 0 {
			return nil, meta, errBadFormat
		}
		meta.grouping = true
		meta.groupSize = count
	}
	if meta.percent || meta.permille {
		if meta.intPlaceholders+meta.fracPlaceholders == 0 {
			meta.percent = false
			meta.permille = false
		}
	}
	return tokens, meta, nil
}

// parseExponentPart 尝试解析指数部分（E[+|-]0...），返回位数、符号与结束下标。
func parseExponentPart(runes []rune, start int) (digits int, sign byte, next int, ok bool) {
	i := start + 1
	sign = '+'
	if i < len(runes) && (runes[i] == '+' || runes[i] == '-') {
		sign = byte(runes[i])
		i++
	}
	count := 0
	for i < len(runes) && (runes[i] == '0' || runes[i] == '#') {
		count++
		i++
	}
	if count == 0 {
		return 0, '+', start, false
	}
	return count, sign, i, true
}

// formatCustom 按自定义格式串格式化 decimal。
func formatCustom(d Decimal, pattern string) (string, error) {
	sections := splitFormatSections(pattern)
	if len(sections) > 3 {
		sections = sections[:3]
	}
	neg := d.Sign() < 0

	type section struct {
		tokens []fmtToken
		meta   fmtMeta
	}
	parsed := make([]section, len(sections))
	maxFrac := 0
	for i, text := range sections {
		tokens, meta, err := tokenizeFormat(text)
		if err != nil {
			return "", err
		}
		parsed[i] = section{tokens: tokens, meta: meta}
		if meta.fracPlaceholders > maxFrac {
			maxFrac = meta.fracPlaceholders
		}
	}

	// .NET 先用格式串的小数位数舍入，再按舍入后的正负/是否为零选择分节
	pre := numValFromDecimal(d)
	if parsed[0].meta.percent {
		pre = pre.mul10(2)
	}
	if parsed[0].meta.permille {
		pre = pre.mul10(3)
	}
	if parsed[0].meta.scaling > 0 {
		pre = pre.div10(3 * parsed[0].meta.scaling)
	}
	roundedToZero := pre.roundTo(maxFrac).isZero()

	sectionIndex := 0
	switch {
	case roundedToZero && len(parsed) >= 3:
		sectionIndex = 2
	case roundedToZero:
		sectionIndex = 0
	case neg && len(parsed) >= 2:
		sectionIndex = 1
	}

	// 分节为空时回退到第一段（负分节回退时补负号），与 .NET 行为一致
	forceMinus := false
	if sectionIndex == 1 && sections[1] == "" {
		sectionIndex = 0
		forceMinus = true
	}
	if sectionIndex == 2 && sections[2] == "" {
		sectionIndex = 0
	}
	withMinus := len(sections) < 2 || forceMinus

	tokens := parsed[sectionIndex].tokens
	meta := parsed[sectionIndex].meta
	hasPlaceholders := meta.intPlaceholders+meta.fracPlaceholders > 0

	v := numValFromDecimal(d)
	if meta.percent {
		v = v.mul10(2)
	}
	if meta.permille {
		v = v.mul10(3)
	}
	if meta.scaling > 0 {
		v = v.div10(3 * meta.scaling)
	}

	if !hasPlaceholders {
		var sb strings.Builder
		if neg && withMinus && !v.isZero() {
			sb.WriteByte('-')
		}
		for _, tok := range tokens {
			if tok.kind == fmtLiteral {
				sb.WriteString(tok.text)
			}
		}
		return sb.String(), nil
	}

	if meta.hasExponent {
		showMinus := neg && withMinus && !v.isZero()
		return renderCustomExponent(v, tokens, meta, showMinus), nil
	}

	v = v.roundTo(meta.fracPlaceholders)
	// 舍入为 0 时 .NET 不输出负号
	showMinus := neg && withMinus && !v.isZero()
	intDigits := strings.TrimLeft(v.intDigits(), "0")
	fracDigits := v.fracDigits()
	if len(fracDigits) < meta.fracPlaceholders {
		fracDigits += strings.Repeat("0", meta.fracPlaceholders-len(fracDigits))
	}
	if len(fracDigits) > meta.fracPlaceholders {
		fracDigits = fracDigits[:meta.fracPlaceholders]
	}
	if len(intDigits) < meta.intZeros {
		intDigits = strings.Repeat("0", meta.intZeros-len(intDigits)) + intDigits
	}
	for len(fracDigits) > meta.fracZeros && len(fracDigits) > 0 && fracDigits[len(fracDigits)-1] == '0' {
		fracDigits = fracDigits[:len(fracDigits)-1]
	}
	return renderCustomTokens(tokens, meta, intDigits, fracDigits, 0, showMinus), nil
}

// renderCustomExponent 渲染带指数的自定义格式。
func renderCustomExponent(v numVal, tokens []fmtToken, meta fmtMeta, showMinus bool) string {
	exp := 0
	if !v.isZero() {
		exp = significantExponent(v)
	}
	m := v.mul10(-exp).roundTo(meta.fracPlaceholders)
	if len(strings.TrimLeft(m.intDigits(), "0")) > 1 {
		exp++
		m = v.mul10(-exp).roundTo(meta.fracPlaceholders)
	}
	intDigits := strings.TrimLeft(m.intDigits(), "0")
	fracDigits := m.fracDigits()
	if len(fracDigits) < meta.fracPlaceholders {
		fracDigits += strings.Repeat("0", meta.fracPlaceholders-len(fracDigits))
	}
	if len(fracDigits) > meta.fracPlaceholders {
		fracDigits = fracDigits[:meta.fracPlaceholders]
	}
	if len(intDigits) < meta.intZeros {
		intDigits = strings.Repeat("0", meta.intZeros-len(intDigits)) + intDigits
	}
	for len(fracDigits) > meta.fracZeros && len(fracDigits) > 0 && fracDigits[len(fracDigits)-1] == '0' {
		fracDigits = fracDigits[:len(fracDigits)-1]
	}
	return renderCustomTokens(tokens, meta, intDigits, fracDigits, exp, showMinus)
}

// renderCustomTokens 按 token 顺序输出结果。
func renderCustomTokens(tokens []fmtToken, meta fmtMeta, intDigits, fracDigits string, exp int, showMinus bool) string {
	// 把整数部分数字分配到各个占位符（右对齐，位数过多时首个占位符吞下多余数字）
	slots := make([]string, meta.intPlaceholders)
	if meta.intPlaceholders > 0 {
		k := len(intDigits) - meta.intPlaceholders
		if k >= 0 {
			for i := 0; i < meta.intPlaceholders; i++ {
				slots[i] = string(intDigits[i+k])
			}
			if k > 0 {
				slots[0] = intDigits[:k+1]
			}
		} else {
			start := -k
			for i := start; i < meta.intPlaceholders; i++ {
				slots[i] = string(intDigits[i-start])
			}
		}
	}

	var sb strings.Builder
	if showMinus {
		sb.WriteByte('-')
	}
	if meta.intPlaceholders == 0 && intDigits != "" {
		sb.WriteString(intDigits)
	}
	// 有分组时整数部分作为整体输出（占位符只决定位置，不再逐个分配）
	groupedMode := meta.grouping && meta.intPlaceholders > 0
	intAll := ""
	if groupedMode {
		intAll = groupDigits(intDigits, meta.groupSize)
	}
	intIdx := 0
	fracIdx := 0
	pastPoint := false
	for _, tok := range tokens {
		switch tok.kind {
		case fmtLiteral:
			sb.WriteString(tok.text)
		case fmtDigitZero, fmtDigitHash:
			if pastPoint {
				if fracIdx < len(fracDigits) {
					sb.WriteByte(fracDigits[fracIdx])
				} else if tok.kind == fmtDigitZero {
					sb.WriteByte('0')
				}
				fracIdx++
				continue
			}
			if groupedMode {
				if intIdx == 0 {
					sb.WriteString(intAll)
				}
				intIdx++
				continue
			}
			if intIdx < len(slots) {
				sb.WriteString(slots[intIdx])
				intIdx++
			}
		case fmtPoint:
			pastPoint = true
			// .NET 只有在存在小数位时才输出小数点
			if fracDigits != "" {
				sb.WriteByte('.')
			}
		case fmtComma:
			// 分组/缩放标记本身不输出
		case fmtExponent:
			sb.WriteByte('E')
			if tok.expSign == '-' {
				sb.WriteString(formatExponentDigits(exp, tok.expDigits, '-'))
			} else {
				sb.WriteString(formatExponentDigits(exp, tok.expDigits, '+'))
			}
		}
	}
	return sb.String()
}

// groupDigits 在整数部分插入千分位分隔符（不含符号）。
func groupDigits(intDigits string, groupSize int) string {
	if groupSize <= 0 || len(intDigits) <= groupSize {
		return intDigits
	}
	var sb strings.Builder
	for i := 0; i < len(intDigits); i++ {
		if i > 0 && (len(intDigits)-i)%groupSize == 0 {
			sb.WriteByte(',')
		}
		sb.WriteByte(intDigits[i])
	}
	return sb.String()
}
