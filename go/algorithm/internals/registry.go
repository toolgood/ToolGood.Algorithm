package internals

import "strings"

// functionRegistry 关键字（大写）-> 构造函数。
var functionRegistry = map[string]func(args []Function) Function{}

// RegisterFunction 注册函数实现（供各函数文件在 init 中调用）。
func RegisterFunction(name string, ctor func(args []Function) Function) {
	functionRegistry[strings.ToUpper(name)] = ctor
}

// UnregisterFunction 移除注册（主要供测试使用）。
func UnregisterFunction(name string) {
	delete(functionRegistry, strings.ToUpper(name))
}

// GetFunction 按名字创建节点，不存在时返回 nil。
func GetFunction(name string, args []Function) Function {
	if ctor, ok := functionRegistry[strings.ToUpper(name)]; ok {
		return ctor(args)
	}
	return nil
}

// IsRegisteredFunction 是否已注册该函数名。
func IsRegisteredFunction(name string) bool {
	_, ok := functionRegistry[strings.ToUpper(name)]
	return ok
}

// RegisteredFunctionNames 返回所有已注册函数名。
func RegisteredFunctionNames() []string {
	names := make([]string, 0, len(functionRegistry))
	for name := range functionRegistry {
		names = append(names, name)
	}
	return names
}
