package mathbase

import "strings"

// 移植自 Internals/Functions/MathBase/Function_Percentage.cs。
// 后缀 `%` 运算符的实现：把第一个参数除以 100。

type FunctionPercentage struct{ BaseFunction }

// NewFunctionPercentage 构造百分比节点（对应 C# 的 Function_Percentage(FunctionBase)），
// 供解析器把后缀 `%` 运算符转换为函数节点。
func NewFunctionPercentage(args []Function) Function {
	return &FunctionPercentage{BaseFunction: NewBase("Percentage", OperandTypeNumber, args)}
}

func newFunctionPercentage(args []Function) Function {
	RequireArgs(args, "Percentage", 1, 1)
	return NewFunctionPercentage(args)
}

func (f *FunctionPercentage) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.GetNumber(1, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	return CreateDecimal(a.NumberValue().Div(decHundred))
}

// WriteTo 输出 `参数%`（不添加括号），对应 C# ToString 实现。
func (f *FunctionPercentage) WriteTo(sb *strings.Builder, addBrackets bool) {
	if arg := f.Arg(0); arg != nil {
		arg.WriteTo(sb, false)
	}
	sb.WriteByte('%')
}

func (f *FunctionPercentage) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

func init() {
	RegisterFunction("PERCENTAGE", newFunctionPercentage)
}
