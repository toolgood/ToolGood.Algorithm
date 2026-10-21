package mathsum

import (
	"strings"

	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals"
)

// FunctionSUMIF 对应 C# Function_SUMIF。
type FunctionSUMIF struct{ BaseFunction }

func newFunctionSUMIF(args []Function) Function {
	RequireArgs(args, "SumIf", 2, 3)
	return &FunctionSUMIF{BaseFunction: NewBase("SumIf", OperandTypeNumber, args)}
}

func (f *FunctionSUMIF) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
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

	var sumdbs []Decimal
	if f.Arg(2) != nil {
		args3 := f.GetArray(3, engine, tp)
		if args3.IsErrorOrNone() {
			return args3
		}
		if !internals.FlattenToNumberList(args3, &sumdbs) {
			return f.ParameterError(3)
		}
	} else {
		sumdbs = list
	}

	var sum Decimal
	if args2.IsNumber() {
		sum = internals.GetSumIf(list, args2.NumberValue(), sumdbs)
	} else if args2.IsText() {
		text := strings.TrimSpace(args2.TextValue())
		if d, ok := TryParseDecimal(text); ok {
			sum = internals.GetSumIf(list, d, sumdbs)
		} else {
			op, value, matched := internals.ParseSumIfMatch(text)
			if matched {
				sum = internals.GetSumIfOp(list, op, value, sumdbs)
			} else {
				return f.ParameterError(2)
			}
		}
	} else {
		return f.ParameterError(2)
	}
	return CreateDecimal(sum)
}

func (f *FunctionSUMIF) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	f.Arg(0).GetParameterTypes(engine, result, OperandTypeArray)
	f.Arg(1).GetParameterTypes(engine, result, OperandTypeNone)
	if f.Arg(2) != nil {
		f.Arg(2).GetParameterTypes(engine, result, OperandTypeArray)
	}
}

func init() { RegisterFunction("SUMIF", newFunctionSUMIF) }
