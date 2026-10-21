package string

import (
	"errors"
	"math/big"
	"strings"
)

// 本文件实现 TEXT 函数所需的 .NET 数字格式子集（CultureInfo.InvariantCulture）。
//
// 支持：
//   - 标准格式：C/c、E/e、F/f、G/g、N/n、P/p、R/r（D/d、X/x 在 decimal 上抛
//     FormatException，这里同样返回错误）
//   - 自定义格式：0/# 数字占位符、'.' 小数点、',' 分组与末尾三位缩放、
//     '%' / '‰' 缩放、'E+0' 指数、引号与反斜杠转义，以及最多 3 段的
//     "正;负;零" 分节
//
// 舍入使用 .NET decimal 的"四舍五入（远离零）"。

var errBadFormat = errors.New("format error")

// numVal 表示非负十进制数的数字串：value = digits * 10^-scale。
type numVal struct {
	digits string // 只含 '0'-'9'，允许前导 0，空串表示 0
	scale  int    // 小数位数，可为负（表示需要补 0）
}

func numValFromDecimal(d Decimal) numVal {
	s := d.Abs().String()
	intPart := s
	fracPart := ""
	if idx := strings.IndexByte(s, '.'); idx >= 0 {
		intPart = s[:idx]
		fracPart = s[idx+1:]
	}
	return numVal{digits: intPart + fracPart, scale: len(fracPart)}
}

func (v numVal) isZero() bool {
	for i := 0; i < len(v.digits); i++ {
		if v.digits[i] != '0' {
			return false
		}
	}
	return true
}

// intDigits 返回整数部分数字（无前导 0，可能为空串）。
func (v numVal) intDigits() string {
	if v.scale <= 0 {
		return v.digits + strings.Repeat("0", -v.scale)
	}
	if len(v.digits) > v.scale {
		return v.digits[:len(v.digits)-v.scale]
	}
	return ""
}

// fracDigits 返回小数部分数字（长度等于 scale）。
func (v numVal) fracDigits() string {
	if v.scale <= 0 {
		return ""
	}
	if len(v.digits) >= v.scale {
		return v.digits[len(v.digits)-v.scale:]
	}
	return strings.Repeat("0", v.scale-len(v.digits)) + v.digits
}

// mul10 乘以 10^k（k 可为负）。
func (v numVal) mul10(k int) numVal {
	return numVal{digits: v.digits, scale: v.scale - k}
}

// div10 除以 10^k（截断，不产生无限小数）。
func (v numVal) div10(k int) numVal {
	return numVal{digits: v.digits, scale: v.scale + k}
}

// roundTo 四舍五入（远离零）到 frac 位小数。
func (v numVal) roundTo(frac int) numVal {
	if v.scale <= frac {
		return v
	}
	drop := v.scale - frac
	d := v.digits
	if len(d) < v.scale+1 {
		d = strings.Repeat("0", v.scale+1-len(d)) + d
	}
	keep := d[:len(d)-drop]
	rest := d[len(d)-drop:]
	if rest[0] >= '5' {
		n, _ := new(big.Int).SetString(keep, 10)
		n.Add(n, big.NewInt(1))
		keep = n.String()
	}
	return numVal{digits: keep, scale: frac}
}

// plain 返回去掉小数末尾 0 的普通表示（至少保留 "0"）。
func (v numVal) plain() string {
	intPart := strings.TrimLeft(v.intDigits(), "0")
	fracPart := v.fracDigits()
	if intPart == "" {
		intPart = "0"
	}
	fracPart = strings.TrimRight(fracPart, "0")
	if fracPart == "" {
		return intPart
	}
	return intPart + "." + fracPart
}

// fixed 返回定点表示：保留 frac 位小数（不足补 0）。
func (v numVal) fixed(frac int) string {
	r := v.roundTo(frac)
	intPart := strings.TrimLeft(r.intDigits(), "0")
	fracPart := r.fracDigits()
	if intPart == "" {
		intPart = "0"
	}
	if frac > 0 {
		if len(fracPart) < frac {
			fracPart += strings.Repeat("0", frac-len(fracPart))
		}
		return intPart + "." + fracPart
	}
	return intPart
}

// grouped 在整数部分插入千分位分隔符。
func grouped(s string, groupSize int) string {
	if groupSize <= 0 {
		return s
	}
	neg := strings.HasPrefix(s, "-")
	if neg {
		s = s[1:]
	}
	intPart := s
	rest := ""
	if idx := strings.IndexByte(s, '.'); idx >= 0 {
		intPart = s[:idx]
		rest = s[idx:]
	}
	var sb strings.Builder
	for i := 0; i < len(intPart); i++ {
		if i > 0 && (len(intPart)-i)%groupSize == 0 {
			sb.WriteByte(',')
		}
		sb.WriteByte(intPart[i])
	}
	out := sb.String() + rest
	if neg {
		return "-" + out
	}
	return out
}

// formatNumber 按 .NET 数字格式串格式化 decimal。
func formatNumber(d Decimal, pattern string) (string, error) {
	if pattern == "" {
		return d.String(), nil
	}
	if len(pattern) == 1 {
		spec := pattern[0]
		switch spec {
		case 'C', 'c', 'E', 'e', 'F', 'f', 'G', 'g', 'N', 'n', 'P', 'p', 'R', 'r':
			return formatStandard(d, spec, 0, false)
		case 'D', 'd', 'X', 'x':
			// decimal 不支持整数/十六进制格式，C# 抛 FormatException
			return "", errBadFormat
		}
		// 单个未知字母同样是非法标准格式（C# 抛 FormatException）；
		// 其余单字符按自定义格式处理（如 "0"、"#"）
		if (spec >= 'A' && spec <= 'Z') || (spec >= 'a' && spec <= 'z') {
			return "", errBadFormat
		}
		return formatCustom(d, pattern)
	}
	if isStandardPattern(pattern) {
		prec := 0
		for i := 1; i < len(pattern); i++ {
			prec = prec*10 + int(pattern[i]-'0')
		}
		return formatStandard(d, pattern[0], prec, true)
	}
	return formatCustom(d, pattern)
}

// isStandardPattern 判断是否形如 "C2" / "F3" 的标准格式串。
func isStandardPattern(pattern string) bool {
	switch pattern[0] {
	case 'C', 'c', 'D', 'd', 'E', 'e', 'F', 'f', 'G', 'g', 'N', 'n', 'P', 'p', 'R', 'r', 'X', 'x':
	default:
		return false
	}
	for i := 1; i < len(pattern); i++ {
		if pattern[i] < '0' || pattern[i] > '9' {
			return false
		}
	}
	return true
}

// formatStandard 处理标准数字格式。
func formatStandard(d Decimal, spec byte, prec int, hasPrec bool) (string, error) {
	letter := spec | 0x20
	v := numValFromDecimal(d)
	neg := d.Sign() < 0
	// 舍入到目标小数位后若为 0，.NET 不输出负号
	if letter == 'c' || letter == 'f' || letter == 'n' || letter == 'p' {
		p := prec
		if !hasPrec {
			p = 2
		}
		rounded := v
		if letter == 'p' {
			rounded = v.mul10(2)
		}
		if rounded.roundTo(p).isZero() {
			neg = false
		}
	}
	sign := func(s string) string {
		if neg {
			return "-" + s
		}
		return s
	}
	switch letter {
	case 'd', 'x':
		return "", errBadFormat
	case 'c':
		if !hasPrec {
			prec = 2
		}
		body := "¤" + grouped(v.fixed(prec), 3)
		if neg {
			return "(" + body + ")", nil
		}
		return body, nil
	case 'f':
		if !hasPrec {
			prec = 2
		}
		return sign(v.fixed(prec)), nil
	case 'n':
		if !hasPrec {
			prec = 2
		}
		return sign(grouped(v.fixed(prec), 3)), nil
	case 'p':
		if !hasPrec {
			prec = 2
		}
		return sign(grouped(v.mul10(2).fixed(prec), 3)) + " %", nil
	case 'e':
		if !hasPrec {
			prec = 6
		}
		return formatExponent(d, prec, spec, 3), nil
	case 'g':
		return formatGeneral(d, prec, hasPrec && prec > 0)
	default: // 'r'
		return d.String(), nil
	}
}

// formatGeneral 对应 .NET 的 G 格式。
func formatGeneral(d Decimal, prec int, limited bool) (string, error) {
	if !limited {
		// 不限精度时 decimal 保留原有小数位（与 d.ToString("G") 一致）
		return d.String(), nil
	}
	neg := d.Sign() < 0
	v := numValFromDecimal(d)
	if v.isZero() {
		return "0", nil
	}
	// 先按有效数字位数舍入，再由舍入后的指数决定定点还是科学计数法
	exp := significantExponent(v)
	m := v.mul10(-exp).roundTo(prec - 1)
	if len(strings.TrimLeft(m.intDigits(), "0")) > 1 {
		exp++
		m = v.mul10(-exp).roundTo(prec - 1)
	}
	var text string
	if exp < -4 || exp >= prec {
		text = m.plain() + "E" + formatExponentDigits(exp, 2, '+')
	} else {
		text = m.mul10(exp).plain()
	}
	if neg {
		return "-" + text, nil
	}
	return text, nil
}

// significantExponent 返回首个有效数字对应的十进制指数。
func significantExponent(v numVal) int {
	intPart := strings.TrimLeft(v.intDigits(), "0")
	if len(intPart) > 0 {
		return len(intPart) - 1
	}
	frac := v.fracDigits()
	zeros := 0
	for zeros < len(frac) && frac[zeros] == '0' {
		zeros++
	}
	return -(zeros + 1)
}

// formatExponent 生成 "1.230000E+003" 形式的科学计数法。
func formatExponent(d Decimal, prec int, spec byte, expDigits int) string {
	neg := d.Sign() < 0
	v := numValFromDecimal(d)
	e := 'E'
	if spec >= 'a' {
		e = 'e'
	}
	if v.isZero() {
		mantissa := "0"
		if prec > 0 {
			mantissa += "." + strings.Repeat("0", prec)
		}
		return mantissa + string(e) + formatExponentDigits(0, expDigits, '+')
	}
	exp := significantExponent(v)
	m := v.mul10(-exp).roundTo(prec)
	if len(strings.TrimLeft(m.intDigits(), "0")) > 1 {
		// 进位导致整数部分变成两位数（如 9.99 -> 10.0）
		exp++
		m = v.mul10(-exp).roundTo(prec)
	}
	intPart := strings.TrimLeft(m.intDigits(), "0")
	if intPart == "" {
		intPart = "0"
	}
	fracPart := m.fracDigits()
	if prec > 0 {
		if len(fracPart) < prec {
			fracPart += strings.Repeat("0", prec-len(fracPart))
		}
		fracPart = fracPart[:prec]
	} else {
		fracPart = ""
	}
	text := intPart
	if prec > 0 {
		text += "." + fracPart
	}
	text += string(e) + formatExponentDigits(exp, expDigits, '+')
	if neg {
		return "-" + text
	}
	return text
}

// formatExponentDigits 生成指数字符串（含符号，位数不足补 0）。
func formatExponentDigits(exp int, digits int, signChar byte) string {
	sign := "+"
	if exp < 0 {
		sign = "-"
		exp = -exp
	} else if signChar == '-' {
		sign = ""
	}
	text := itoa(exp)
	for len(text) < digits {
		text = "0" + text
	}
	return sign + text
}

func itoa(v int) string {
	if v == 0 {
		return "0"
	}
	var buf [20]byte
	i := len(buf)
	for v > 0 {
		i--
		buf[i] = byte('0' + v%10)
		v /= 10
	}
	return string(buf[i:])
}
