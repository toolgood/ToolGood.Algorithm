package csharpweb

import (
	"strconv"
	"strings"
)

// 本文件实现 System.Web.HttpUtility.HtmlEncode / HtmlDecode 的等价逻辑。
//
// HtmlEncode 的行为（InvariantCulture）：'&' '<' '>' '"' 分别转成 &amp; &lt; &gt; &quot;，
// '\'' 转成 &#39;；U+00A0–U+00FF 统一转成数字实体 &#nnn;；其余字符（含中文）原样输出。
//
// HtmlDecode 支持：
//   - 五个基本实体 &amp; &lt; &gt; &quot; &apos;（与 C# 一致，区分大小写）；
//   - 数字实体 &#nnn; 与 &#xhhh;；
//   - Latin-1 区（U+00A0–U+00FF）的常用命名实体（nbsp、copy、reg、times 等）。
//
// 未知实体、缺少结尾分号的实体原样保留，与 .NET 相同。

var htmlLatin1Names = map[string]rune{
	"nbsp": 160, "iexcl": 161, "cent": 162, "pound": 163, "curren": 164, "yen": 165,
	"brvbar": 166, "sect": 167, "uml": 168, "copy": 169, "ordf": 170, "laquo": 171,
	"not": 172, "shy": 173, "reg": 174, "macr": 175, "deg": 176, "plusmn": 177,
	"sup2": 178, "sup3": 179, "acute": 180, "micro": 181, "para": 182, "middot": 183,
	"cedil": 184, "sup1": 185, "ordm": 186, "raquo": 187, "frac14": 188, "frac12": 189,
	"frac34": 190, "iquest": 191, "Agrave": 192, "Aacute": 193, "Acirc": 194,
	"Atilde": 195, "Auml": 196, "Aring": 197, "AElig": 198, "Ccedil": 199,
	"Egrave": 200, "Eacute": 201, "Ecirc": 202, "Euml": 203, "Igrave": 204,
	"Iacute": 205, "Icirc": 206, "Iuml": 207, "ETH": 208, "Ntilde": 209,
	"Ograve": 210, "Oacute": 211, "Ocirc": 212, "Otilde": 213, "Ouml": 214,
	"times": 215, "Oslash": 216, "Ugrave": 217, "Uacute": 218, "Ucirc": 219,
	"Uuml": 220, "Yacute": 221, "THORN": 222, "szlig": 223, "agrave": 224,
	"aacute": 225, "acirc": 226, "atilde": 227, "auml": 228, "aring": 229,
	"aelig": 230, "ccedil": 231, "egrave": 232, "eacute": 233, "ecirc": 234,
	"euml": 235, "igrave": 236, "iacute": 237, "icirc": 238, "iuml": 239,
	"eth": 240, "ntilde": 241, "ograve": 242, "oacute": 243, "ocirc": 244,
	"otilde": 245, "ouml": 246, "divide": 247, "oslash": 248, "ugrave": 249,
	"uacute": 250, "ucirc": 251, "uuml": 252, "yacute": 253, "thorn": 254,
	"yuml": 255,
}

// htmlEncode 对应 HttpUtility.HtmlEncode。
func htmlEncode(s string) string {
	var sb strings.Builder
	sb.Grow(len(s))
	for _, c := range s {
		switch c {
		case '&':
			sb.WriteString("&amp;")
		case '<':
			sb.WriteString("&lt;")
		case '>':
			sb.WriteString("&gt;")
		case '"':
			sb.WriteString("&quot;")
		case '\'':
			sb.WriteString("&#39;")
		default:
			if c >= 160 && c <= 255 {
				sb.WriteString("&#")
				sb.WriteString(strconv.Itoa(int(c)))
				sb.WriteByte(';')
			} else {
				sb.WriteRune(c)
			}
		}
	}
	return sb.String()
}

// htmlDecode 对应 HttpUtility.HtmlDecode。
func htmlDecode(s string) string {
	if !strings.Contains(s, "&") {
		return s
	}
	var sb strings.Builder
	sb.Grow(len(s))
	for i := 0; i < len(s); {
		c := s[i]
		if c != '&' {
			sb.WriteByte(c)
			i++
			continue
		}
		end := strings.IndexByte(s[i:], ';')
		if end < 0 {
			sb.WriteByte(c)
			i++
			continue
		}
		entity := s[i+1 : i+end]
		decoded, ok := decodeEntity(entity)
		if !ok {
			sb.WriteByte(c)
			i++
			continue
		}
		sb.WriteRune(decoded)
		i += end + 1
	}
	return sb.String()
}

// decodeEntity 解析实体名（不含 & 与 ;）。
func decodeEntity(entity string) (rune, bool) {
	if entity == "" {
		return 0, false
	}
	switch entity {
	case "amp":
		return '&', true
	case "lt":
		return '<', true
	case "gt":
		return '>', true
	case "quot":
		return '"', true
	case "apos":
		return '\'', true
	}
	if entity[0] == '#' {
		body := entity[1:]
		base := 10
		if len(body) > 0 && (body[0] == 'x' || body[0] == 'X') {
			base = 16
			body = body[1:]
		}
		if body == "" {
			return 0, false
		}
		for i := 0; i < len(body); i++ {
			ch := body[i]
			valid := ch >= '0' && ch <= '9'
			if base == 16 {
				valid = valid || (ch >= 'a' && ch <= 'f') || (ch >= 'A' && ch <= 'F')
			}
			if !valid {
				return 0, false
			}
		}
		v, err := strconv.ParseInt(body, base, 32)
		if err != nil || v < 0 || v > 0x10FFFF {
			return 0, false
		}
		return rune(v), true
	}
	if r, ok := htmlLatin1Names[entity]; ok {
		return r, true
	}
	return 0, false
}
