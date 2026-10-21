package value

import (
	"strings"
	"time"
)

// 移植自 Internals/Functions/DateTimes/Function_TODAY.cs（解析器把它当作 value 组的常量节点使用）。

// FunctionTODAY 当前日期节点。
type FunctionTODAY struct{ BaseFunction }

// NewFunctionTODAY 构造 TODAY 节点。
func NewFunctionTODAY() Function {
	return &FunctionTODAY{BaseFunction: NewBase("Today", OperandTypeDate, nil)}
}

// Evaluate 返回当前日期（时分秒为 0）。
func (f *FunctionTODAY) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	now := time.Now()
	if !engine.UseLocalTime {
		now = now.UTC()
	}
	return CreateMyDate(NewMyDate(now.Year(), int(now.Month()), now.Day(), 0, 0, 0))
}

// WriteTo 输出 Today()。
func (f *FunctionTODAY) WriteTo(sb *strings.Builder, addBrackets bool) {
	sb.WriteString("Today()")
}
