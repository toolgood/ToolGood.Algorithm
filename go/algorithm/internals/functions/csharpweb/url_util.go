package csharpweb

import "strings"

// 本文件实现 System.Web.HttpUtility.UrlEncode / UrlDecode 的等价逻辑。
//
// UrlEncode 的规则（与 .NET 一致）：
//   - 空格编码为 '+'；
//   - 不编码：A-Z a-z 0-9 及 '-' '_' '.' '!' '*' '(' ')'；
//   - 其余字符按 UTF-8 字节编码为 %xx，十六进制字母为**小写**（注意 Go 的
//     url.QueryEscape 会保留 '~' 并编码 '!' '*' '(' ')'，两者不同）。

// urlEncode 对应 HttpUtility.UrlEncode。
func urlEncode(s string) string {
	var sb strings.Builder
	sb.Grow(len(s) * 3)
	const hexDigits = "0123456789abcdef"
	for i := 0; i < len(s); i++ {
		c := s[i]
		switch {
		case c == ' ':
			sb.WriteByte('+')
		case c >= 'a' && c <= 'z', c >= 'A' && c <= 'Z', c >= '0' && c <= '9',
			c == '-', c == '_', c == '.', c == '!', c == '*', c == '(', c == ')':
			sb.WriteByte(c)
		default:
			sb.WriteByte('%')
			sb.WriteByte(hexDigits[c>>4])
			sb.WriteByte(hexDigits[c&0x0f])
		}
	}
	return sb.String()
}

// urlDecode 对应 HttpUtility.UrlDecode：'+' 还原为空格，%XX 按字节还原。
// 非法的百分号序列原样保留（与 .NET 一致）。
func urlDecode(s string) string {
	if !strings.ContainsAny(s, "+%") {
		return s
	}
	out := make([]byte, 0, len(s))
	for i := 0; i < len(s); i++ {
		c := s[i]
		switch c {
		case '+':
			out = append(out, ' ')
		case '%':
			if i+2 < len(s) {
				hi, ok1 := fromHexDigit(s[i+1])
				lo, ok2 := fromHexDigit(s[i+2])
				if ok1 && ok2 {
					out = append(out, hi<<4|lo)
					i += 2
					continue
				}
			}
			out = append(out, c)
		default:
			out = append(out, c)
		}
	}
	return string(out)
}

func fromHexDigit(c byte) (byte, bool) {
	switch {
	case c >= '0' && c <= '9':
		return c - '0', true
	case c >= 'a' && c <= 'f':
		return c - 'a' + 10, true
	case c >= 'A' && c <= 'F':
		return c - 'A' + 10, true
	}
	return 0, false
}
