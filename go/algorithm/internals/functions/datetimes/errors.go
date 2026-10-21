package datetimes

// 错误文案集中定义，与 C# 的 BaseFunction 固定错误一致。
// 使用 Errorf 的 {n} 占位符，保证渲染后与 C# 文案完全相同。

type named interface {
	Name() string
}

// parameterError Function 'X' parameter N is error!
func parameterError(f named, index int) Operand {
	return Errorf("Function '{0}' parameter {1} is error!", f.Name(), index)
}
