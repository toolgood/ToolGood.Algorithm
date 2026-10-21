package string

import "strings"

// FunctionCONCATENATE 对应 C# Function_CONCATENATE：拼接任意个文本参数。
type FunctionCONCATENATE struct{ BaseFunction }

func newFunctionCONCATENATE(args []Function) Function {
	RequireArgs(args, "Concatenate", 1, -1)
	return &FunctionCONCATENATE{BaseFunction: NewBase("Concatenate", OperandTypeText, args)}
}

func (f *FunctionCONCATENATE) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	if f.ArgCount() == 0 {
		return CreateString("")
	}
	if f.ArgCount() == 1 {
		a := f.GetText(1, engine, tp)
		if a.IsErrorOrNone() {
			return a
		}
		return a
	}
	var sb strings.Builder
	for i := 1; i <= f.ArgCount(); i++ {
		a := f.GetText(i, engine, tp)
		if a.IsErrorOrNone() {
			return a
		}
		sb.WriteString(a.TextValue())
	}
	return CreateString(sb.String())
}

func init() { RegisterFunction("CONCATENATE", newFunctionCONCATENATE) }
