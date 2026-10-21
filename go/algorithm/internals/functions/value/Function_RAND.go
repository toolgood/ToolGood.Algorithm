package value

import (
	"math/rand"
	"strings"
)

// 移植自 Internals/Functions/MathBase/Function_RAND.cs（解析器把它当作 value 组的常量节点使用）。

// FunctionRAND 随机数节点。
type FunctionRAND struct{ BaseFunction }

// NewFunctionRAND 构造 RAND 节点。
func NewFunctionRAND() Function {
	return &FunctionRAND{BaseFunction: NewBase("Rand", OperandTypeNumber, nil)}
}

// Evaluate 返回 [0,1) 之间的随机数。
func (f *FunctionRAND) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	return CreateFloat(rand.Float64())
}

// WriteTo 输出 Rand()。
func (f *FunctionRAND) WriteTo(sb *strings.Builder, addBrackets bool) {
	sb.WriteString("Rand()")
}
