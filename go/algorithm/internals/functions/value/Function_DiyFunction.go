package value

import "strings"

// 移植自 Internals/Functions/Value/Function_DiyFunction.cs。

// FunctionDiyFunction 自定义函数调用节点。
type FunctionDiyFunction struct {
	BaseFunction
	funName string
}

// NewFunctionDiyFunction 构造自定义函数节点。
func NewFunctionDiyFunction(name string, args []Function) Function {
	return &FunctionDiyFunction{BaseFunction: NewBase("DiyFunction", OperandTypeNone, args), funName: name}
}

// Evaluate 先求值全部参数，再交给引擎执行自定义函数。
func (f *FunctionDiyFunction) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args := make([]Operand, 0, f.ArgCount())
	if err := f.EvaluateAll(engine, tp, &args); err != nil {
		return err
	}
	return engine.ExecuteDiyFunction(f.funName, args)
}

// WriteTo 输出 `函数名(参数...)`。
func (f *FunctionDiyFunction) WriteTo(sb *strings.Builder, addBrackets bool) {
	sb.WriteString(f.funName)
	sb.WriteByte('(')
	for i := 0; i < f.ArgCount(); i++ {
		if i > 0 {
			sb.WriteString(", ")
		}
		f.Arg(i).WriteTo(sb, false)
	}
	sb.WriteByte(')')
}

func (f *FunctionDiyFunction) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	for i := 0; i < f.ArgCount(); i++ {
		f.Arg(i).GetParameterTypes(engine, result, OperandTypeNone)
	}
}
