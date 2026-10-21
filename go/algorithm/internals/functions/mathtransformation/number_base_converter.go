package mathtransformation

import (
	"math"
	"strconv"
	"strings"
)

// 移植自 Internals/Functions/MathTransformation/NumberBaseConverter.cs。
// 进制转换函数（DEC2BIN、BIN2DEC、HEX2OCT 等）的公共常量与解析/校验逻辑：
// 统一 10 位补码语义、places 参数校验与文本合法性判断。

const (
	// maxPlaces places 参数允许的最大位数（与 10 位补码表示一致）
	maxPlaces = 10

	// binHalf 二进制 10 位补码的符号位权值 (2^9)
	binHalf = 512
	// binModulus 二进制 10 位补码的模 (2^10)
	binModulus = 1024

	// octHalf 八进制 10 位补码的符号位权值 (2^29)
	octHalf = 536870912
	// octModulus 八进制 10 位补码的模 (2^30)
	octModulus = 1073741824
	// octMask 八进制 10 位补码的掩码 (2^30-1)
	octMask = 0x3FFFFFFF

	// hexHalf 十六进制 10 位补码的符号位权值 (2^39)
	hexHalf = int64(0x8000000000)
	// hexModulus 十六进制 10 位补码的模 (2^40)
	hexModulus = int64(0x10000000000)
	// hexMask 十六进制 10 位补码的掩码 (2^40-1)
	hexMask = int64(0xFFFFFFFFFF)
)

var (
	decTen    = DecFromInt(10)
	decIntMin = DecFromInt(math.MinInt32)
	decIntMax = DecFromInt(math.MaxInt32)
)

// tryGetPlaces 读取并校验 places 参数：必须为 0~10 的整数。
// 数值超出 int 范围或不在 0~10 之间时返回 false（对应 Excel #NUM!）。
func tryGetPlaces(arg Operand) (int, bool) {
	t := arg.NumberValue().Trunc()
	if t.Less(DecimalZero) || t.Greater(decTen) {
		return 0, false
	}
	return int(t.Int64()), true
}

// tryGetInt 对应 C# 的 TryGetInt：向零截断后判断是否落在 int 范围内。
func tryGetInt(op Operand) (int, bool) {
	t := op.NumberValue().Trunc()
	if t.Less(decIntMin) || t.Greater(decIntMax) {
		return 0, false
	}
	return int(t.Int64()), true
}

// parseComplement 解析进制文本为有符号整数：先裁剪首尾空白，再校验字符与长度
// （不超过 10 位），最后按 10 位补码规则还原符号。
func parseComplement(value string, fromBase int) (int64, bool) {
	if strings.TrimSpace(value) == "" {
		return 0, false
	}
	text := strings.TrimSpace(value)
	if len(text) > maxPlaces {
		return 0, false
	}
	if !isValidText(text, fromBase) {
		return 0, false
	}
	num, err := strconv.ParseInt(text, fromBase, 64)
	if err != nil {
		return 0, false
	}
	half := getHalf(fromBase)
	if num >= half {
		num -= half * 2
	}
	return num, true
}

// tryFormat 按 places 补零格式化进制文本。
// 负数结果固定为 10 位补码，按 Excel 语义忽略 places 的位数限制。
func tryFormat(value string, negative bool, places int, hasPlaces bool) (string, bool) {
	if !hasPlaces {
		return value, true
	}
	if !negative && len(value) > places {
		return "", false
	}
	return padLeft(value, places, '0'), true
}

// tryFormatDecimal 按 places 对齐十进制文本（负号占一位）。
func tryFormatDecimal(value int64, places int) (string, bool) {
	text := strconv.FormatInt(value, 10)
	if len(text) > places {
		return "", false
	}
	if value < 0 {
		return "-" + padLeft(text[1:], places-1, '0'), true
	}
	return padLeft(text, places, '0'), true
}

func padLeft(text string, length int, pad byte) string {
	if len(text) >= length {
		return text
	}
	return strings.Repeat(string(pad), length-len(text)) + text
}

// toUpperHex 等价于 .NET 的 ToString("X")：大写十六进制文本。
func toUpperHex(v int64) string {
	return strings.ToUpper(strconv.FormatInt(v, 16))
}

func isValidText(text string, fromBase int) bool {
	if fromBase == 2 {
		return isBin(text)
	}
	if fromBase == 8 {
		return isOct(text)
	}
	return isHex(text)
}

func getHalf(fromBase int) int64 {
	if fromBase == 2 {
		return binHalf
	}
	if fromBase == 8 {
		return octHalf
	}
	return hexHalf
}

// isHex 对应 RegexHelper.IsHex：仅允许 0-9 / A-F / a-f。
func isHex(text string) bool {
	if text == "" {
		return false
	}
	for i := 0; i < len(text); i++ {
		c := text[i]
		if (c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f') {
			continue
		}
		return false
	}
	return true
}

// isOct 对应 RegexHelper.IsOct：仅允许 0-7。
func isOct(text string) bool {
	if text == "" {
		return false
	}
	for i := 0; i < len(text); i++ {
		if text[i] < '0' || text[i] > '7' {
			return false
		}
	}
	return true
}

// isBin 对应 RegexHelper.IsBin：仅允许 0/1。
func isBin(text string) bool {
	if text == "" {
		return false
	}
	for i := 0; i < len(text); i++ {
		if text[i] != '0' && text[i] != '1' {
			return false
		}
	}
	return true
}
