package internals

import "strings"

// FunctionUtil 函数实现共用的工具方法（对应 C# 的 FunctionUtil）。

// EstimateCount 估算参数展开后的元素个数。
func EstimateCount(args []Operand) int {
	count := 0
	for _, item := range args {
		switch item.Type() {
		case OperandTypeArray:
			count += len(item.ArrayValue())
		case OperandTypeJson:
			count += 8
		default:
			count += 1
		}
	}
	return count
}

// FlattenToList 把参数（含数组 / JSON）展开为元素列表。
func FlattenToList(args []Operand, result *[]Operand) bool {
	stack := make([]Operand, len(args))
	copy(stack, args)
	for len(stack) > 0 {
		item := stack[len(stack)-1]
		stack = stack[:len(stack)-1]
		switch item.Type() {
		case OperandTypeArray:
			list := item.ArrayValue()
			for i := len(list) - 1; i >= 0; i-- {
				stack = append(stack, list[i])
			}
		case OperandTypeJson:
			converted := item.ToArray("")
			if converted.IsError() {
				return false
			}
			list := converted.ArrayValue()
			for i := len(list) - 1; i >= 0; i-- {
				stack = append(stack, list[i])
			}
		default:
			*result = append(*result, item)
		}
	}
	return true
}

// FlattenToBigDecimalList 展开并转换为数值列表。
func FlattenToBigDecimalList(args []Operand, result *[]Decimal) bool {
	stack := make([]Operand, len(args))
	copy(stack, args)
	for len(stack) > 0 {
		item := stack[len(stack)-1]
		stack = stack[:len(stack)-1]
		switch item.Type() {
		case OperandTypeArray:
			list := item.ArrayValue()
			for i := len(list) - 1; i >= 0; i-- {
				stack = append(stack, list[i])
			}
		case OperandTypeJson:
			converted := item.ToArray("")
			if converted.IsError() {
				return false
			}
			list := converted.ArrayValue()
			for i := len(list) - 1; i >= 0; i-- {
				stack = append(stack, list[i])
			}
		default:
			if item.Type() == OperandTypeNumber {
				*result = append(*result, item.NumberValue())
			} else {
				converted := item.ToNumber("")
				if converted.IsError() {
					return false
				}
				*result = append(*result, converted.NumberValue())
			}
		}
	}
	return true
}

// FlattenToNumberList 展开单个参数为数值列表。
func FlattenToNumberList(arg Operand, result *[]Decimal) bool {
	if arg.IsError() {
		return false
	}
	switch arg.Type() {
	case OperandTypeArray:
		return FlattenToBigDecimalList(arg.ArrayValue(), result)
	case OperandTypeJson:
		converted := arg.ToArray("")
		if converted.IsError() {
			return false
		}
		return FlattenToBigDecimalList(converted.ArrayValue(), result)
	case OperandTypeNumber:
		*result = append(*result, arg.NumberValue())
	default:
		converted := arg.ToNumber("")
		if converted.IsError() {
			return false
		}
		*result = append(*result, converted.NumberValue())
	}
	return true
}

// FlattenToStringList 展开单个参数为字符串列表。
func FlattenToStringList(arg Operand, result *[]string) bool {
	if arg.IsError() {
		return false
	}
	switch arg.Type() {
	case OperandTypeArray:
		for _, item := range arg.ArrayValue() {
			if item.Type() == OperandTypeArray || item.Type() == OperandTypeJson {
				if !FlattenToStringList(item, result) {
					return false
				}
				continue
			}
			converted := item.ToText("")
			if converted.IsError() {
				return false
			}
			*result = append(*result, converted.TextValue())
		}
		return true
	case OperandTypeJson:
		converted := arg.ToArray("")
		if converted.IsError() {
			return false
		}
		return FlattenToStringList(converted, result)
	}
	converted := arg.ToText("")
	if converted.IsError() {
		return false
	}
	*result = append(*result, converted.TextValue())
	return true
}

// GetCountIf 统计等于指定值的元素个数。
func GetCountIf(values []Decimal, value Decimal) int {
	count := 0
	for _, item := range values {
		if item.Equal(value) {
			count++
		}
	}
	return count
}

// GetCountIfOp 按比较运算符统计元素个数。
func GetCountIfOp(values []Decimal, op string, value Decimal) int {
	count := 0
	for _, item := range values {
		if CompareValues(item, value, op) {
			count++
		}
	}
	return count
}

// GetSumIf 对等于指定值的元素求和。
func GetSumIf(values []Decimal, value Decimal, sums []Decimal) Decimal {
	total := DecimalZero
	n := len(values)
	if len(sums) < n {
		n = len(sums)
	}
	for i := 0; i < n; i++ {
		if values[i].Equal(value) {
			total = total.Add(sums[i])
		}
	}
	return total
}

// GetSumIfOp 按比较运算符对元素求和。
func GetSumIfOp(values []Decimal, op string, value Decimal, sums []Decimal) Decimal {
	total := DecimalZero
	n := len(values)
	if len(sums) < n {
		n = len(sums)
	}
	for i := 0; i < n; i++ {
		if CompareValues(values[i], value, op) {
			total = total.Add(sums[i])
		}
	}
	return total
}

// CompareValues 按运算符比较两个数值。
func CompareValues(a, b Decimal, op string) bool {
	switch {
	case StrEquals(op, "<"):
		return a.Less(b)
	case StrEquals(op, "<="):
		return a.LessOrEqual(b)
	case StrEquals(op, ">"):
		return a.Greater(b)
	case StrEquals(op, ">="):
		return a.GreaterOrEqual(b)
	case StrEquals(op, "=", "==", "==="):
		return a.Equal(b)
	}
	return !a.Equal(b)
}

// GetGCDTwo 求两个数的最大公约数。
func GetGCDTwo(a, b Decimal) Decimal {
	for !b.IsZero() {
		a, b = b, a.FloorMod(b)
	}
	return a
}

// GetGCD 求最大公约数。
func GetGCD(values []Decimal) Decimal {
	if len(values) == 0 {
		return DecimalOne
	}
	g := values[0].Trunc()
	for _, value := range values[1:] {
		g = GetGCDTwo(g, value.Trunc())
		if g.Equal(DecimalOne) {
			break
		}
	}
	return g
}

// GetLCM 求最小公倍数。
func GetLCM(values []Decimal) Decimal {
	if len(values) == 0 {
		return DecimalOne
	}
	var result Decimal
	has := false
	for _, value := range values {
		v := value.Trunc()
		if v.LessOrEqual(DecimalOne) {
			continue
		}
		if !has {
			result = v
			has = true
			continue
		}
		var g Decimal
		if v.Greater(result) {
			g = GetGCDTwo(v, result)
		} else {
			g = GetGCDTwo(result, v)
		}
		result = result.Div(g).Trunc().Mul(v)
	}
	if !has {
		return DecimalOne
	}
	return result
}

// GetFactorial 阶乘。
func GetFactorial(value Decimal) Decimal {
	if value.LessOrEqual(DecimalZero) {
		return DecimalOne
	}
	n := int(value.Trunc().Int64())
	result := DecimalOne
	for i := n; i > 0; i-- {
		result = result.Mul(DecFromInt(int64(i)))
	}
	return result
}

func tryParseDecimalText(text string) (Decimal, bool) {
	return TryParseDecimal(text)
}

// ParseSumIfMatch 解析 SUMIF / COUNTIF 的条件文本（如 ">=10"）。
func ParseSumIfMatch(text string) (string, Decimal, bool) {
	if text == "" {
		return "", DecimalZero, false
	}
	runes := []rune(text)
	first := runes[0]
	rest := string(runes[1:])
	restTrim := strings.TrimSpace(rest)
	switch first {
	case '>', '＞':
		if len(runes) > 1 && (runes[1] == '=' || runes[1] == '＝') {
			if d, ok := tryParseDecimalText(strings.TrimSpace(string(runes[2:]))); ok {
				return ">=", d, true
			}
			return "", DecimalZero, false
		}
		if d, ok := tryParseDecimalText(restTrim); ok {
			return ">", d, true
		}
		return "", DecimalZero, false
	case '<', '＜':
		if len(runes) > 1 && (runes[1] == '=' || runes[1] == '＝') {
			if d, ok := tryParseDecimalText(strings.TrimSpace(string(runes[2:]))); ok {
				return "<=", d, true
			}
			return "", DecimalZero, false
		}
		if len(runes) > 1 && (runes[1] == '>' || runes[1] == '＞') {
			if d, ok := tryParseDecimalText(strings.TrimSpace(string(runes[2:]))); ok {
				return "!=", d, true
			}
			return "", DecimalZero, false
		}
		if d, ok := tryParseDecimalText(restTrim); ok {
			return "<", d, true
		}
		return "", DecimalZero, false
	case '=', '＝':
		index := 1
		if len(runes) > 1 && (runes[1] == '=' || runes[1] == '＝') {
			index = 2
			if len(runes) > 2 && (runes[2] == '=' || runes[2] == '＝') {
				index = 3
			}
		}
		if index > len(runes) {
			return "", DecimalZero, false
		}
		if d, ok := tryParseDecimalText(strings.TrimSpace(string(runes[index:]))); ok {
			return "=", d, true
		}
		return "", DecimalZero, false
	case '!', '！':
		index := 1
		if len(runes) > 1 && (runes[1] == '=' || runes[1] == '＝') {
			index = 2
			if len(runes) > 2 && (runes[2] == '=' || runes[2] == '＝') {
				index = 3
			}
		}
		if index > len(runes) {
			return "", DecimalZero, false
		}
		if d, ok := tryParseDecimalText(strings.TrimSpace(string(runes[index:]))); ok {
			return "!=", d, true
		}
		return "", DecimalZero, false
	}
	return "", DecimalZero, false
}

// TryParseBoolean 解析布尔文本，无法识别时返回 (false,false)。
func TryParseBoolean(text string) (bool, bool) {
	switch len(text) {
	case 1:
		switch text {
		case "1", "是", "有":
			return true, true
		case "0", "否", "无":
			return false, true
		}
	case 2:
		if strings.EqualFold(text, "no") || text == "不是" || text == "没有" {
			return false, true
		}
	case 3:
		if strings.EqualFold(text, "yes") {
			return true, true
		}
	case 4:
		if strings.EqualFold(text, "true") {
			return true, true
		}
	case 5:
		if strings.EqualFold(text, "false") {
			return false, true
		}
	}
	return false, false
}

func selectPivot(values []Decimal, left, right int) int {
	mid := left + (right-left)/2
	a, b, c := values[left], values[mid], values[right]
	if a.Less(b) {
		if b.Less(c) {
			return mid
		}
		if a.Less(c) {
			return right
		}
		return left
	}
	if a.Less(c) {
		return left
	}
	if b.Less(c) {
		return right
	}
	return mid
}

func partition(values []Decimal, left, right, pivotIndex int) int {
	pivot := values[pivotIndex]
	values[pivotIndex], values[right] = values[right], pivot
	store := left
	for i := left; i < right; i++ {
		if values[i].Less(pivot) {
			values[store], values[i] = values[i], values[store]
			store++
		}
	}
	values[right], values[store] = values[store], pivot
	return store
}

// QuickSelect 快速选择第 k 小（或第 k 大）元素。
func QuickSelect(values []Decimal, k int, largest bool) Decimal {
	if len(values) == 1 {
		return values[0]
	}
	target := k
	if largest {
		target = len(values) - 1 - k
	}
	left, right := 0, len(values)-1
	for left < right {
		pivotIndex := partition(values, left, right, selectPivot(values, left, right))
		if target == pivotIndex {
			return values[target]
		}
		if target < pivotIndex {
			right = pivotIndex - 1
		} else {
			left = pivotIndex + 1
		}
	}
	return values[left]
}

// GetRank 计算数值在列表中的排名（0 表示不存在）。
func GetRank(values []Decimal, num Decimal, descending bool) int {
	rank := 1
	count := 0
	for _, item := range values {
		if item.Equal(num) {
			count++
		} else if (descending && item.Greater(num)) || (!descending && item.Less(num)) {
			rank++
		}
	}
	if count > 0 {
		return rank
	}
	return 0
}
