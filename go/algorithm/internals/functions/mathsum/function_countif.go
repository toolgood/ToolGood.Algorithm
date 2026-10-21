package mathsum

import (
	"strings"

	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals"
)

// FunctionCOUNTIF 对应 C# Function_COUNTIF。
type FunctionCOUNTIF struct{ BaseFunction }

func newFunctionCOUNTIF(args []Function) Function {
	RequireArgs(args, "CountIf", 2, 2)
	return &FunctionCOUNTIF{BaseFunction: NewBase("CountIf", OperandTypeNumber, args)}
}

func (f *FunctionCOUNTIF) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	args1 := f.GetArray(1, engine, tp)
	if args1.IsErrorOrNone() {
		return args1
	}
	args2 := f.EvalArg(1, engine, tp)
	if args2.IsErrorOrNone() {
		return args2
	}

	var list []Decimal
	if !internals.FlattenToNumberList(args1, &list) {
		return f.ParameterError(1)
	}

	var count int
	if args2.IsNumber() {
		count = internals.GetCountIf(list, args2.NumberValue())
	} else if args2.IsText() {
		text := strings.TrimSpace(args2.TextValue())
		if d, ok := TryParseDecimal(text); ok {
			count = internals.GetCountIf(list, d)
		} else {
			op, value, matched := internals.ParseSumIfMatch(text)
			if matched {
				count = internals.GetCountIfOp(list, op, value)
			} else {
				return f.ParameterError(2)
			}
		}
	} else {
		return f.ParameterError(2)
	}
	return CreateInt(count)
}

func (f *FunctionCOUNTIF) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	f.Arg(0).GetParameterTypes(engine, result, OperandTypeArray)
	f.Arg(1).GetParameterTypes(engine, result, OperandTypeNone)
}

func init() { RegisterFunction("COUNTIF", newFunctionCOUNTIF) }
