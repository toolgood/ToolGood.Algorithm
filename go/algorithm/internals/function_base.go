package internals

import (
	"fmt"
	"strings"
)

// TempParameter 临时参数解析函数（对应原实现的 TempParameter 委托）。
type TempParameter func(name string) Operand

// Function 表达式节点。
//
// 对应 C# 的 FunctionBase / Function_0..Function_N。
type Function interface {
	// Name 节点名称。
	Name() string
	// Evaluate 求值。
	Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand
	// ResultType 结果类型。
	ResultType() OperandType
	// GetParameterTypes 收集表达式中的参数信息。
	GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string)
	// WriteTo 输出表达式文本。
	WriteTo(sb *strings.Builder, addBrackets bool)
}

// BaseFunction 所有函数节点的公共实现。
type BaseFunction struct {
	name       string
	resultType OperandType
	funcs      []Function
}

// Name 返回节点名称。
func (f *BaseFunction) Name() string { return f.name }

// ResultType 返回结果类型。
func (f *BaseFunction) ResultType() OperandType { return f.resultType }

// WriteTo 输出 name(arg1, arg2, ...)。
func (f *BaseFunction) WriteTo(sb *strings.Builder, addBrackets bool) {
	sb.WriteString(f.name)
	sb.WriteByte('(')
	for i, fn := range f.funcs {
		if i > 0 {
			sb.WriteString(", ")
		}
		fn.WriteTo(sb, false)
	}
	sb.WriteByte(')')
}

// FormatFunction 返回节点的表达式文本。
//
// 注意：不能用 BaseFunction.String() 做动态分派（Go 的嵌入方法不会回溯到外层类型），
// 因此文本输出统一走该函数，它会调用具体节点自己的 WriteTo。
func FormatFunction(fn Function) string {
	var sb strings.Builder
	fn.WriteTo(&sb, false)
	return sb.String()
}

// String 返回节点名称（仅用于调试；表达式文本请用 FormatFunction）。
func (f *BaseFunction) String() string {
	return f.name
}

// functionParameterTypes 函数名 -> 各参数期望类型（对应 C# 各 Function_XXX 里的 GetParameterTypes 重写）。
var functionParameterTypes = map[string][]OperandType{}

// RegisterParameterTypes 声明某个函数各参数的期望类型；参数多于声明时沿用最后一项。
func RegisterParameterTypes(name string, types ...OperandType) {
	functionParameterTypes[name] = types
}

// GetParameterTypes 按声明表传播参数类型；未声明时把调用方的类型原样下传。
func (f *BaseFunction) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if types, ok := functionParameterTypes[f.name]; ok && len(types) > 0 {
		for i, fn := range f.funcs {
			t := types[len(types)-1]
			if i < len(types) {
				t = types[i]
			}
			fn.GetParameterTypes(engine, result, t)
		}
		return
	}
	for _, fn := range f.funcs {
		fn.GetParameterTypes(engine, result, operandType)
	}
}

// arg 返回第 i 个参数（0 基）。
func (f *BaseFunction) Arg(i int) Function {
	if i < 0 || i >= len(f.funcs) {
		return nil
	}
	return f.funcs[i]
}

// ArgCount 参数个数。
func (f *BaseFunction) ArgCount() int { return len(f.funcs) }

// EvalArg 求值第 i 个参数（0 基）。
func (f *BaseFunction) EvalArg(i int, engine *AlgorithmEngine, tp TempParameter) Operand {
	fn := f.Arg(i)
	if fn == nil {
		return Errorf("Function '{0}' parameter {1} is error!", f.name, i+1)
	}
	return fn.Evaluate(engine, tp)
}

// ------------------------------------------------------------------ 类型转换

// ConvertToText 转文本，失败时给出带参数位置的错误信息。
func (f *BaseFunction) ConvertToText(arg Operand, index int) Operand {
	return arg.ToText(fmt.Sprintf("Function '%s' parameter %d is error!", f.name, index))
}

// ConvertToBoolean 转布尔。
func (f *BaseFunction) ConvertToBoolean(arg Operand, index int) Operand {
	return arg.ToBoolean(fmt.Sprintf("Function '%s' parameter %d is error!", f.name, index))
}

// ConvertToNumber 转数值。
func (f *BaseFunction) ConvertToNumber(arg Operand, index int) Operand {
	return arg.ToNumber(fmt.Sprintf("Function '%s' parameter %d is error!", f.name, index))
}

// ConvertToArray 转数组。
func (f *BaseFunction) ConvertToArray(arg Operand, index int) Operand {
	return arg.ToArray(fmt.Sprintf("Function '%s' parameter %d is error!", f.name, index))
}

// ConvertToDate 转日期。
func (f *BaseFunction) ConvertToDate(arg Operand, index int) Operand {
	return arg.ToMyDate(fmt.Sprintf("Function '%s' parameter %d is error!", f.name, index))
}

// GetText 求值第 index 个参数并转文本（index 从 1 开始）。
func (f *BaseFunction) GetText(index int, engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.EvalArg(index-1, engine, tp)
	if a.IsText() {
		return a
	}
	return f.ConvertToText(a, index)
}

// GetNumber 求值第 index 个参数并转数值（index 从 1 开始）。
func (f *BaseFunction) GetNumber(index int, engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.EvalArg(index-1, engine, tp)
	if a.IsNumber() {
		return a
	}
	return f.ConvertToNumber(a, index)
}

// GetDate 求值第 index 个参数并转日期。
func (f *BaseFunction) GetDate(index int, engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.EvalArg(index-1, engine, tp)
	if a.IsDate() {
		return a
	}
	return f.ConvertToDate(a, index)
}

// GetBoolean 求值第 index 个参数并转布尔。
func (f *BaseFunction) GetBoolean(index int, engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.EvalArg(index-1, engine, tp)
	if a.IsBoolean() {
		return a
	}
	return f.ConvertToBoolean(a, index)
}

// GetArray 求值第 index 个参数并转数组。
func (f *BaseFunction) GetArray(index int, engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.EvalArg(index-1, engine, tp)
	if a.IsArray() {
		return a
	}
	return f.ConvertToArray(a, index)
}

// ------------------------------------------------------------------ 错误

// ParameterError 参数错误。
func (f *BaseFunction) ParameterError(index int) Operand {
	return Errorf("Function '{0}' parameter {1} is error!", f.name, index)
}

// FunctionError 函数参数错误。
func (f *BaseFunction) FunctionError() Operand {
	return Errorf("Function '{0}' parameter is error!", f.name)
}

// CompareError 比较错误。
func (f *BaseFunction) CompareError() Operand {
	return Errorf("Function '{0}' compare is error.", f.name)
}

// Div0Error 除零错误。
func (f *BaseFunction) Div0Error() Operand {
	return Errorf("Function '{0}' Div 0 error!", f.name)
}

// NumError 数值错误。
func (f *BaseFunction) NumError() Operand {
	return Errorf("Function '{0}' Num error!", f.name)
}

// ------------------------------------------------------------------ 参数校验

// MustArgs 校验参数个数，不满足时 panic（由 Parse 转成解析错误）。
func MustArgs(args []Function, name string, minimum int, maximum int) {
	count := len(args)
	if maximum < 0 {
		if count < minimum {
			panic(fmt.Sprintf("Function '%s' requires at least %d parameter.", name, minimum))
		}
		return
	}
	if minimum == maximum {
		if count != minimum {
			panic(fmt.Sprintf("Function '%s' requires exactly %d parameter.", name, minimum))
		}
		return
	}
	if count < minimum || count > maximum {
		panic(fmt.Sprintf("Function '%s' requires %d to %d parameters.", name, minimum, maximum))
	}
}

// RequireArgs 是 MustArgs 的可读别名。
func RequireArgs(args []Function, name string, minimum int, maximum int) {
	MustArgs(args, name, minimum, maximum)
}

// EvaluateAll 依次求值所有参数；遇到错误或未指定值时提前返回该值。
func (f *BaseFunction) EvaluateAll(engine *AlgorithmEngine, tp TempParameter, args *[]Operand) Operand {
	for _, fn := range f.funcs {
		value := fn.Evaluate(engine, tp)
		if value.IsErrorOrNone() {
			return value
		}
		*args = append(*args, value)
	}
	return nil
}

// NewBase 构造基础节点。
func NewBase(name string, result OperandType, args []Function) BaseFunction {
	return BaseFunction{name: name, resultType: result, funcs: args}
}
