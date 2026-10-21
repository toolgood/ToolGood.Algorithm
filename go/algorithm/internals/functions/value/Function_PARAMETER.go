package value

import "strings"

// 移植自 Internals/Functions/Value/Function_PARAMETER.cs。

// FunctionParameter 参数名节点。
type FunctionParameter struct {
	BaseFunction
	name string
}

// NewFunctionParameter 构造参数节点（对应 C# 的 Function_Parameter(string)）。
func NewFunctionParameter(name string) Function {
	return &FunctionParameter{BaseFunction: NewBase("Parameter", OperandTypeNone, nil), name: name}
}

// Evaluate 取参数值。
func (f *FunctionParameter) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	if tp != nil {
		if r := tp(f.name); r != nil {
			return r
		}
	}
	return engine.GetParameter(f.name)
}

// WriteTo 输出参数名。
func (f *FunctionParameter) WriteTo(sb *strings.Builder, addBrackets bool) {
	sb.WriteString(f.name)
}

func (f *FunctionParameter) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	op, val := parameterOpValue(opVal)
	*result = append(*result, ParameterType{Name: f.name, Type: operandType, Operator: op, Value: val})
}

// parameterOpValue 取出可选的运算符与比较值（对应 C# 的 op / val 参数）。
func parameterOpValue(opVal []string) (string, string) {
	var op, val string
	if len(opVal) > 0 {
		op = opVal[0]
	}
	if len(opVal) > 1 {
		val = opVal[1]
	}
	return op, val
}
