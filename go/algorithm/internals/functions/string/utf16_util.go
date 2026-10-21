package string

import (
	"math"
	"unicode/utf16"
)

// C# 的 string 以 UTF-16 码元为索引单位（Length / Substring / IndexOf 均按码元计算）。
// 为了让 LEN、LEFT、MID、RIGHT、FIND、CODE、UNICODE 等函数的下标与 C# 完全一致，
// 这些函数统一在 UTF-16 码元序列上计算；只有增补平面字符（surrogate pair）才与
// rune 计数不同。

// toUnits 把字符串转为 UTF-16 码元序列。
func toUnits(s string) []uint16 { return utf16.Encode([]rune(s)) }

// fromUnits 把 UTF-16 码元序列还原为字符串。
func fromUnits(u []uint16) string { return string(utf16.Decode(u)) }

// unitsIndexOf 在 text 中自 start 起查找 pattern，返回码元下标，未找到返回 -1。
func unitsIndexOf(text, pattern []uint16, start int) int {
	if start < 0 {
		start = 0
	}
	if start > len(text) {
		return -1
	}
	if len(pattern) == 0 {
		return start
	}
	for i := start; i+len(pattern) <= len(text); i++ {
		matched := true
		for j := range pattern {
			if text[i+j] != pattern[j] {
				matched = false
				break
			}
		}
		if matched {
			return i
		}
	}
	return -1
}

// tryGetInt 对应 C# 的 TryGetInt：超出 int32 范围时返回 false（C# 抛 OverflowException）。
func tryGetInt(op Operand) (int, bool) {
	d := op.NumberValue().Trunc()
	if d.Less(DecFromInt(math.MinInt32)) || d.Greater(DecFromInt(math.MaxInt32)) {
		return 0, false
	}
	return int(d.Int64()), true
}
