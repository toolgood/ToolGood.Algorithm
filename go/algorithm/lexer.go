package algorithm

import (
	"fmt"
	"strings"

	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals"
)

// ParseError 解析错误。
type ParseError struct{ Message string }

// Error 实现 error 接口。
func (e *ParseError) Error() string { return e.Message }

// Token 词法单元。
type Token struct {
	Kind  string
	Text  string
	Start int // rune 下标
	End   int // rune 下标（含）
}

// TokEOF 结束标记。
const TokEOF = "EOF"

// keywordSpellings 别名 -> 规范名。
var keywordSpellings = map[string]string{
	"ALGORITHMVERSION": "ALGORITHMVERSION", "ENGINEVERSION": "ALGORITHMVERSION",
	"FALSE": "FALSE", "NO": "FALSE",
	"TRUE": "TRUE", "YES": "TRUE",
	"NULL": "NULL",
	"E":    "E", "GUID": "GUID", "NOW": "NOW", "PI": "PI", "RAND": "RAND", "TODAY": "TODAY",

	"BETADIST": "BETADIST", "BETA.DIST": "BETADIST",
	"BETAINV": "BETAINV", "BETA.INV": "BETAINV",
	"BINOMDIST": "BINOMDIST", "BINOM.DIST": "BINOMDIST",
	"CEIL": "CEILING", "CEILING": "CEILING",
	"CONCAT": "CONCATENATE", "CONCATENATE": "CONCATENATE",
	"COVAR": "COVAR", "COVARIANCE.P": "COVAR",
	"COVARIANCE.S": "COVARIANCES",
	"EXPONDIST":    "EXPONDIST", "EXPON.DIST": "EXPONDIST",
	"FDIST": "FDIST", "F.DIST": "FDIST",
	"FINV": "FINV", "F.INV": "FINV",
	"GAMMADIST": "GAMMADIST", "GAMMA.DIST": "GAMMADIST",
	"GAMMAINV": "GAMMAINV", "GAMMA.INV": "GAMMAINV",
	"GAMMALN": "GAMMALN", "GAMMALN.PRECISE": "GAMMALN",
	"HAS": "HAS", "HASKEY": "HAS", "CONTAINS": "HAS", "CONTAINSKEY": "HAS",
	"HASVALUE": "HASVALUE", "CONTAINSVALUE": "HASVALUE",
	"HYPGEOMDIST": "HYPGEOMDIST", "HYPGEOM.DIST": "HYPGEOMDIST",
	"ISREGEX": "ISREGEX", "ISMATCH": "ISREGEX",
	"JIS": "JIS", "WIDECHAR": "JIS",
	"LOGINV": "LOGINV", "LOGNORM.INV": "LOGINV",
	"LOGNORMDIST": "LOGNORMDIST", "LOGNORM.DIST": "LOGNORMDIST",
	"LOOKCEIL": "LOOKCEILING", "LOOKCEILING": "LOOKCEILING",
	"LOWER": "LOWER", "TOLOWER": "LOWER",
	"NEGBINOMDIST": "NEGBINOMDIST", "NEGBINOM.DIST": "NEGBINOMDIST",
	"NORMDIST": "NORMDIST", "NORM.DIST": "NORMDIST",
	"NORMINV": "NORMINV", "NORM.INV": "NORMINV",
	"NORMSDIST": "NORMSDIST", "NORM.S.DIST": "NORMSDIST",
	"NORMSINV": "NORMSINV", "NORM.S.INV": "NORMSINV",
	"PARAM": "PARAM", "PARAMETER": "PARAM", "GETPARAMETER": "PARAM",
	"PERCENTILE": "PERCENTILE", "PERCENTILE.INC": "PERCENTILE",
	"PERCENTRANK": "PERCENTRANK", "PERCENTRANK.INC": "PERCENTRANK",
	"POISSON": "POISSON", "POISSON.DIST": "POISSON",
	"STDEV": "STDEV", "STDEV.S": "STDEV",
	"STDEVP": "STDEVP", "STDEV.P": "STDEVP",
	"TDIST": "TDIST", "T.DIST": "TDIST",
	"TINV": "TINV", "T.INV": "TINV",
	"TRIMEND": "TRIMEND", "RTRIM": "TRIMEND",
	"TRIMSTART": "TRIMSTART", "LTRIM": "TRIMSTART",
	"UPPER": "UPPER", "TOUPPER": "UPPER",
	"VAR": "VAR", "VAR.S": "VAR",
	"VARP": "VARP", "VAR.P": "VARP",
}

// Keywords 大写拼写 -> 规范 token 名。
var Keywords = map[string]string{}

var maxKeywordLen int

func init() {
	for spelling, canonical := range keywordSpellings {
		Keywords[spelling] = canonical
	}
	plain := "ABS ACOS ACOSH ACOT ACOTH ADDDAYS ADDHOURS ADDMINUTES ADDMONTHS ADDSECONDS ADDYEARS " +
		"AND ARABIC ARRAY ASC ASIN ASINH ATAN ATAN2 ATANH AVEDEV AVERAGE AVERAGEIF COVARIANCES " +
		"BASE64TOTEXT BASE64URLTOTEXT BESSELI BESSELJ BESSELK BESSELY " +
		"BIN2DEC BIN2HEX BIN2OCT CHAR CLEAN CODE COMBIN CORREL COS COSH COT COTH " +
		"COUNT COUNTIF CSC CSCH DATE DATEDIF DATEVALUE DAY DAYS DAYS360 DB DDB DEC2BIN DEC2HEX " +
		"DEC2OCT DEGREES DELTA DEVSQ EDATE ENDSWITH EOMONTH ERF ERFC ERROR EVEN EXACT EXP FACT " +
		"FACTDOUBLE FIND FISHER FISHERINV FIXED FLOOR FORECAST FV GCD GEOMEAN GESTEP HARMEAN " +
		"HEX2BIN HEX2DEC HEX2OCT HMACMD5 HMACSHA1 HMACSHA256 HMACSHA512 HOUR HTMLDECODE HTMLENCODE " +
		"IF IFERROR IFS INDEXOF INT INTERCEPT IPMT IRR ISERROR ISEVEN ISLOGICAL ISNONTEXT ISNULL " +
		"ISNULLOREMPTY ISNULLORERROR ISNULLORWHITESPACE ISNUMBER ISODD ISTEXT JOIN JSON LARGE " +
		"LASTINDEXOF LCM LEFT LEN LN LOG LOG10 LOOKFLOOR MAX MD5 MEDIAN MID MIN MINUTE MIRR MOD " +
		"MODE MONTH MROUND MULTINOMIAL NETWORKDAYS NOT NPER NPV OCT2BIN OCT2DEC OCT2HEX ODD OR " +
		"PEARSON PERMUT PMT POWER PPMT PRODUCT PROPER PV QUARTILE QUOTIENT RADIANS RANDBETWEEN " +
		"RANK RATE REGEX REGEXREPLACE REMOVEEND REMOVESTART REPLACE REPT RIGHT RMB ROMAN ROUND " +
		"ROUNDDOWN ROUNDUP SEARCH SEC SECH SECOND SERIESSUM SHA1 SHA256 SHA512 SIGN SIN SINH SLN " +
		"SLOPE SMALL SPLIT SQRT SQRTPI STARTSWITH SUBSTITUTE SUBSTRING SUM SUMIF SUMPRODUCT SUMSQ " +
		"SUMX2MY2 SUMX2PY2 SUMXMY2 SWITCH SYD T TAN TANH TEXT TEXTTOBASE64 TEXTTOBASE64URL TIME " +
		"TIMESTAMP TIMEVALUE TRIM TRUNC UNICHAR UNICODE URLDECODE URLENCODE VALUE WEEKDAY WEEKNUM " +
		"WEIBULL WORKDAY XIRR XNPV XOR YEAR YEARFRAC"
	for _, word := range strings.Fields(plain) {
		if _, ok := Keywords[word]; !ok {
			Keywords[word] = word
		}
	}
	for k := range Keywords {
		if len(k) > maxKeywordLen {
			maxKeywordLen = len(k)
		}
	}
}

var punctuations = map[rune]string{
	'(': "LPAREN",
	')': "RPAREN",
	'[': "LBRACKET",
	']': "RBRACKET",
	'{': "LBRACE",
	'}': "RBRACE",
	',': "COMMA",
	':': "COLON",
	'?': "QUESTION",
	'.': "DOT",
	'+': "OPADD",
	'-': "OPSUB",
	'*': "OPMUL",
	'/': "OPDIV",
	'%': "OPMOD",
	'&': "OPCAT",
}

// Tokenizer 词法分析器。
type Tokenizer struct {
	text  []rune
	std   []rune
	pos   int
	token []Token
}

// NewTokenizer 创建词法分析器。
func NewTokenizer(text string) *Tokenizer {
	runes := []rune(text)
	return &Tokenizer{text: runes, std: internals.StandardRunes(runes)}
}

func isDigit(c rune) bool      { return c >= '0' && c <= '9' }
func isIdentStart(c rune) bool { return (c >= 'A' && c <= 'Z') || c == '_' }
func isIdentPart(c rune) bool {
	return (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') || c == '_'
}

func matchNumSuffix(runes []rune, pos int) string {
	best := ""
	consider := func(s string) {
		if len(s) > len(best) {
			best = s
		}
	}
	// E[+-]?digits
	if pos < len(runes) && runes[pos] == 'E' {
		i := pos + 1
		if i < len(runes) && (runes[i] == '+' || runes[i] == '-') {
			i++
		}
		start := i
		for i < len(runes) && isDigit(runes[i]) {
			i++
		}
		if i > start {
			consider(string(runes[pos:i]))
		}
	}
	// [KDCM]?M[23]?
	if pos < len(runes) {
		i := pos
		switch {
		case strings.ContainsRune("KDCM", runes[i]) && i+1 < len(runes) && runes[i+1] == 'M':
			i += 2
		case runes[i] == 'M':
			i++
		default:
			i = pos
		}
		if i > pos {
			if i < len(runes) && (runes[i] == '2' || runes[i] == '3') {
				i++
			}
			consider(string(runes[pos:i]))
		}
	}
	// M?L
	if pos < len(runes) {
		i := pos
		if runes[i] == 'M' && i+1 < len(runes) && runes[i+1] == 'L' {
			i += 2
			consider(string(runes[pos:i]))
		} else if runes[i] == 'L' {
			i++
			consider(string(runes[pos:i]))
		}
	}
	// K?G
	if pos < len(runes) {
		i := pos
		if runes[i] == 'K' && i+1 < len(runes) && runes[i+1] == 'G' {
			i += 2
			consider(string(runes[pos:i]))
		} else if runes[i] == 'G' {
			i++
			consider(string(runes[pos:i]))
		}
	}
	// T
	if pos < len(runes) && runes[pos] == 'T' {
		consider("T")
	}
	return best
}

// Tokenize 执行词法分析。
func (t *Tokenizer) Tokenize() []Token {
	n := len(t.text)
	for t.pos < n {
		i := t.pos
		c := t.std[i]
		switch {
		case c == ' ' || c == '\t' || c == '\r' || c == '\n' || c == '\f':
			t.pos++
		case c == '/' && i+1 < n && t.std[i+1] == '*':
			end := indexRunes(t.std, i+2, []rune("*/"))
			if end < 0 {
				panic(&ParseError{Message: fmt.Sprintf("token recognition error at: '%s'", string(t.text[i:]))})
			}
			t.pos = end + 2
		case c == '/' && i+1 < n && t.std[i+1] == '/':
			end := indexRune(t.std, i+2, '\n')
			if end < 0 {
				t.pos = n
			} else {
				t.pos = end + 1
			}
		case isDigit(c):
			t.readNumber()
		case c == '\'' || c == '"' || c == '`':
			t.readString(c)
		case isIdentStart(c):
			t.readWord()
		default:
			t.readOperator()
		}
	}
	t.token = append(t.token, Token{Kind: TokEOF, Start: n, End: n})
	return t.token
}

func indexRune(runes []rune, from int, target rune) int {
	for i := from; i < len(runes); i++ {
		if runes[i] == target {
			return i
		}
	}
	return -1
}

func indexRunes(runes []rune, from int, target []rune) int {
	for i := from; i+len(target) <= len(runes); i++ {
		match := true
		for j, c := range target {
			if runes[i+j] != c {
				match = false
				break
			}
		}
		if match {
			return i
		}
	}
	return -1
}

func (t *Tokenizer) readNumber() {
	i := t.pos
	n := len(t.std)
	for i < n && isDigit(t.std[i]) {
		i++
	}
	if i < n && t.std[i] == '.' && i+1 < n && isDigit(t.std[i+1]) {
		i++
		for i < n && isDigit(t.std[i]) {
			i++
		}
	}
	suffix := matchNumSuffix(t.std, i)
	i += len([]rune(suffix))
	t.token = append(t.token, Token{Kind: "NUM", Text: string(t.text[t.pos:i]), Start: t.pos, End: i - 1})
	t.pos = i
}

func (t *Tokenizer) readString(quote rune) {
	i := t.pos + 1
	n := len(t.std)
	for i < n {
		if t.std[i] == '\\' {
			i += 2
			continue
		}
		if t.std[i] == quote {
			i++
			t.token = append(t.token, Token{Kind: "STRING", Text: string(t.text[t.pos:i]), Start: t.pos, End: i - 1})
			t.pos = i
			return
		}
		i++
	}
	panic(&ParseError{Message: fmt.Sprintf("token recognition error at: '%s'", string(t.text[t.pos:]))})
}

func (t *Tokenizer) readWord() {
	i := t.pos
	n := len(t.std)
	for i < n && isIdentPart(t.std[i]) {
		i++
	}
	identLen := i - t.pos

	bestKind := ""
	bestLen := 0
	upper := t.std[t.pos:minInt(t.pos+maxKeywordLen, n)]
	for size := len(upper); size > 0; size-- {
		candidate := string(upper[:size])
		if kind, ok := Keywords[candidate]; ok && size > bestLen {
			bestKind = kind
			bestLen = size
			break
		}
	}
	if bestKind != "" && bestLen >= identLen {
		end := t.pos + bestLen
		t.token = append(t.token, Token{Kind: bestKind, Text: string(t.text[t.pos:end]), Start: t.pos, End: end - 1})
		t.pos = end
		return
	}
	t.token = append(t.token, Token{Kind: "PARAMETER", Text: string(t.text[t.pos:i]), Start: t.pos, End: i - 1})
	t.pos = i
}

func minInt(a, b int) int {
	if a < b {
		return a
	}
	return b
}

func (t *Tokenizer) readOperator() {
	i := t.pos
	n := len(t.std)
	two := ""
	if i+1 < n {
		two = string(t.std[i : i+2])
	}
	add := func(kind string, length int) {
		t.token = append(t.token, Token{Kind: kind, Text: string(t.text[i : i+length]), Start: i, End: i + length - 1})
		t.pos = i + length
	}
	switch two {
	case "&&":
		add("OPAND", 2)
		return
	case "||":
		add("OPOR", 2)
		return
	case ">=":
		add("OPGE", 2)
		return
	case "<=":
		add("OPLE", 2)
		return
	case "!=":
		end := i + 2
		if end < n && t.std[end] == '=' {
			add("OPNE", 3)
		} else {
			add("OPNE", 2)
		}
		return
	case "<>":
		add("OPNE", 2)
		return
	case "==":
		j := i + 2
		if j < n && t.std[j] == '=' {
			add("OPEQ", 3)
		} else {
			add("OPEQ", 2)
		}
		return
	}
	c := t.std[i]
	switch c {
	case '=':
		add("OPEQ", 1)
	case '!':
		add("NOT", 1)
	case '>':
		add("OPGT", 1)
	case '<':
		add("OPLT", 1)
	default:
		if kind, ok := punctuations[c]; ok {
			add(kind, 1)
			return
		}
		panic(&ParseError{Message: fmt.Sprintf("token recognition error at: '%c'", c)})
	}
}

// Tokenize 便捷函数。
func Tokenize(text string) []Token { return NewTokenizer(text).Tokenize() }
