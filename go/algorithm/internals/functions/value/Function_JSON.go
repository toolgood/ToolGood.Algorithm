package value

import "strings"

// 移植自 Internals/Functions/Value/Function_JSON.cs。

// FunctionJSON 把文本解析为 JSON 节点。
type FunctionJSON struct{ BaseFunction }

func newFunctionJSON(args []Function) Function {
	RequireArgs(args, "Json", 1, 1)
	return &FunctionJSON{BaseFunction: NewBase("Json", OperandTypeJson, args)}
}

// Evaluate 解析 JSON 文本。
func (f *FunctionJSON) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.EvalArg(0, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	if a.IsJson() {
		return a
	}
	if a.IsArrayJson() {
		a = a.ToText("")
	}
	if !a.IsText() {
		return f.ParameterError(1)
	}
	txt := a.TextValue()
	if (strings.HasPrefix(txt, "{") && strings.HasSuffix(txt, "}")) ||
		(strings.HasPrefix(txt, "[") && strings.HasSuffix(txt, "]")) {
		json, err := JsonMapperToObject(txt)
		if err != nil {
			return f.ParameterError(1)
		}
		return CreateJsonData(json)
	}
	return f.ParameterError(1)
}

func (f *FunctionJSON) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNone)
	}
}

func init() { RegisterFunction("JSON", newFunctionJSON) }
