package flow

import "strings"

// 移植自 Internals/Functions/Flow/Function_ISNULLORWHITESPACE.cs。

// FunctionISNULLORWHITESPACE 判断是否空值或空白字符串。
type FunctionISNULLORWHITESPACE struct{ BaseFunction }

func newFunctionISNULLORWHITESPACE(args []Function) Function {
	RequireArgs(args, "IsNullOrWhitespace", 1, 1)
	return &FunctionISNULLORWHITESPACE{BaseFunction: NewBase("IsNullOrWhitespace", OperandTypeBoolean, args)}
}

// Evaluate 判断是否空值或只含空白字符。
func (f *FunctionISNULLORWHITESPACE) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	a := f.EvalArg(0, engine, tp)
	if a.IsErrorOrNone() {
		return a
	}
	if a.IsNull() {
		return True
	}
	textArg := f.ConvertToText(a, 1)
	if textArg.IsErrorOrNone() {
		return textArg
	}
	return CreateBool(strings.TrimSpace(textArg.TextValue()) == "")
}

func (f *FunctionISNULLORWHITESPACE) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if arg := f.Arg(0); arg != nil {
		arg.GetParameterTypes(engine, result, OperandTypeNone)
	}
}

func init() { RegisterFunction("ISNULLORWHITESPACE", newFunctionISNULLORWHITESPACE) }
