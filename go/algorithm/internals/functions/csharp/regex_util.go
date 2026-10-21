package csharp

import (
	"regexp"
	"strings"
	"sync"
)

// Go 的 regexp 使用 RE2 语法，与 .NET 正则有如下差异：
//   - 不支持反向引用、环视(?=)(?<=)、条件表达式与平衡组；
//   - 命名分组写作 (?P<name>...)，不支持 (?<name>...)；
//   - 替换串中 "$&" 表示整个匹配（Go 用 "${0}"），"$$" 表示字面 $。
//
// 因此遇到 .NET 专有语法时编译失败，函数按"正则非法"返回参数错误，
// 这与 C# 捕获 ArgumentException 的行为一致。C# 的 1 秒匹配超时（ReDoS 防护）
// 在 Go 中无法等价实现，Go 的 RE2 本身保证线性时间，不存在回溯爆炸。

var (
	regexCacheMu sync.RWMutex
	regexCache   = map[string]*regexp.Regexp{}
)

// getRegex 获取（并缓存）指定模式的正则。
func getRegex(pattern string) (*regexp.Regexp, error) {
	regexCacheMu.RLock()
	re, ok := regexCache[pattern]
	regexCacheMu.RUnlock()
	if ok {
		return re, nil
	}
	re, err := regexp.Compile(pattern)
	if err != nil {
		return nil, err
	}
	regexCacheMu.Lock()
	regexCache[pattern] = re
	regexCacheMu.Unlock()
	return re, nil
}

// translateReplacement 把 .NET 替换串里的 "$&" 转成 Go 的 "${0}"。
func translateReplacement(replacement string) string {
	if !strings.Contains(replacement, "$&") {
		return replacement
	}
	var sb strings.Builder
	for i := 0; i < len(replacement); i++ {
		if replacement[i] == '$' && i+1 < len(replacement) && replacement[i+1] == '&' {
			sb.WriteString("${0}")
			i++
			continue
		}
		sb.WriteByte(replacement[i])
	}
	return sb.String()
}
