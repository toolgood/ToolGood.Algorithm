package csharp

import (
	"strconv"
	"strings"
	"unicode/utf16"
)

// C# 的 string 以 UTF-16 码元为索引单位，INDEXOF / LASTINDEXOF / SUBSTRING 等
// 返回下标或按位置截取的函数统一在码元序列上计算，保证与 C# 结果一致。

// toUnits 把字符串转为 UTF-16 码元序列。
func toUnits(s string) []uint16 { return utf16.Encode([]rune(s)) }

// fromUnits 把 UTF-16 码元序列还原为字符串。
func fromUnits(u []uint16) string { return string(utf16.Decode(u)) }

// unitsIndexOf 在 text 中自 start 起查找 pattern（Ordinal），返回码元下标，未找到返回 -1。
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

// unitsIndexOfCount 对应 string.IndexOf(value, startIndex, count, Ordinal)。
func unitsIndexOfCount(text, pattern []uint16, startIndex, count int) int {
	if startIndex < 0 || count < 0 || startIndex+count > len(text) {
		return -1
	}
	return unitsIndexOf(text[:startIndex+count], pattern, startIndex)
}

// unitsLastIndexOf 对应 string.LastIndexOf(value, startIndex, Ordinal)。
func unitsLastIndexOf(text, pattern []uint16, startIndex int) int {
	if startIndex < 0 || len(text) == 0 {
		return -1
	}
	if startIndex >= len(text) {
		startIndex = len(text) - 1
	}
	if len(pattern) == 0 {
		return startIndex
	}
	// .NET 的 LastIndexOf(value, startIndex) 要求匹配的**结束位置**不超过 startIndex。
	for i := startIndex - len(pattern) + 1; i >= 0; i-- {
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

// unitsLastIndexOfCount 对应 string.LastIndexOf(value, startIndex, count, Ordinal)。
func unitsLastIndexOfCount(text, pattern []uint16, startIndex, count int) int {
	if count <= 0 || startIndex < 0 || len(text) == 0 {
		return -1
	}
	if startIndex >= len(text) {
		startIndex = len(text) - 1
	}
	low := startIndex - count + 1
	if low < 0 {
		low = 0
	}
	for i := startIndex - len(pattern) + 1; i >= low; i-- {
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

// ordinalIgnoreCaseEquals 近似 StringComparison.OrdinalIgnoreCase：
// C# 逐字符 ToUpperInvariant，Go 用 strings.EqualFold（Unicode 简单大小写折叠）。
func ordinalIgnoreCaseEquals(a, b string) bool {
	return strings.EqualFold(a, b)
}

// startsWith 对应 string.StartsWith(value, comparison)。
func startsWith(text, value string, ignoreCase bool) bool {
	if !ignoreCase {
		return strings.HasPrefix(text, value)
	}
	if len(value) > len(text) {
		return false
	}
	return ordinalIgnoreCaseEquals(text[:len(value)], value)
}

// endsWith 对应 string.EndsWith(value, comparison)。
func endsWith(text, value string, ignoreCase bool) bool {
	if !ignoreCase {
		return strings.HasSuffix(text, value)
	}
	if len(value) > len(text) {
		return false
	}
	return ordinalIgnoreCaseEquals(text[len(text)-len(value):], value)
}

// flattenToStrings 对应 FunctionUtil.FlattenToList(Operand, List<string>)。
func flattenToStrings(arg Operand, list *[]string) bool {
	if arg.IsError() {
		return false
	}
	switch arg.Type() {
	case OperandTypeArray:
		for _, item := range arg.ArrayValue() {
			if item.IsArray() || item.IsJson() {
				if !flattenToStrings(item, list) {
					return false
				}
				continue
			}
			converted := item.ToText("")
			if converted.IsError() {
				return false
			}
			*list = append(*list, converted.TextValue())
		}
		return true
	case OperandTypeJson:
		converted := arg.ToArray("")
		if converted.IsError() {
			return false
		}
		return flattenToStrings(converted, list)
	default:
		converted := arg.ToText("")
		if converted.IsError() {
			return false
		}
		*list = append(*list, converted.TextValue())
		return true
	}
}

// flattenToDecimals 对应 FunctionUtil.FlattenToList(Operand, List<decimal>)。
func flattenToDecimals(arg Operand, list *[]Decimal) bool {
	if arg.IsError() {
		return false
	}
	switch arg.Type() {
	case OperandTypeArray:
		for _, item := range arg.ArrayValue() {
			if item.IsArray() || item.IsJson() {
				if !flattenToDecimals(item, list) {
					return false
				}
				continue
			}
			converted := item.ToNumber("")
			if converted.IsError() {
				return false
			}
			*list = append(*list, converted.NumberValue())
		}
		return true
	case OperandTypeJson:
		converted := arg.ToArray("")
		if converted.IsError() {
			return false
		}
		return flattenToDecimals(converted, list)
	default:
		converted := arg.ToNumber("")
		if converted.IsError() {
			return false
		}
		*list = append(*list, converted.NumberValue())
		return true
	}
}

// jsonValueEquals 对应 FunctionUtil.JsonValueEquals。
func jsonValueEquals(v *JsonData, text string) bool {
	if v == nil {
		return false
	}
	if v.IsString() {
		return v.StringValue() == text
	}
	if v.IsDouble() {
		return Plain(v.NumberValue()) == text
	}
	if v.IsBoolean() {
		return strings.EqualFold(strconv.FormatBool(v.BooleanValue()), text)
	}
	return false
}
