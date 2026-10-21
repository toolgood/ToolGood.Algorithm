package operator

// 运算符共用辅助函数。

// exceedsDecimal 判断运算结果是否超出 decimal 范围（对应 C# 的 OverflowException）。
func exceedsDecimal(v Decimal) bool {
	return v.Abs().Greater(DecimalMax)
}
