package value

import (
	"strings"
	"time"
)

// 移植自 Internals/Functions/DateTimes/Function_NOW.cs（解析器把它当作 value 组的常量节点使用）。

// FunctionNOW 当前时间节点。
type FunctionNOW struct{ BaseFunction }

// NewFunctionNOW 构造 NOW 节点。
func NewFunctionNOW() Function {
	return &FunctionNOW{BaseFunction: NewBase("Now", OperandTypeDate, nil)}
}

// Evaluate 返回当前时间（按引擎配置使用本地时间或 UTC）。
func (f *FunctionNOW) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	now := time.Now()
	if !engine.UseLocalTime {
		now = now.UTC()
	}
	return CreateMyDate(NewMyDateFromTime(now))
}

// WriteTo 输出 Now()。
func (f *FunctionNOW) WriteTo(sb *strings.Builder, addBrackets bool) {
	sb.WriteString("Now()")
}
