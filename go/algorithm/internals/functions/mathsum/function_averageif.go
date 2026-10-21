package mathsum

import (
	"strings"

	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals"
)

// FunctionAVERAGEIF 对应 C# Function_AVERAGEIF。
type FunctionAVERAGEIF struct{ BaseFunction }

func newFunctionAVERAGEIF(args []Function) Function {
	RequireArgs(args, "AverageIf", 2, 3)
	return &FunctionAVERAGEIF{BaseFunction: NewBase("AverageIf", OperandTypeNumber, args)}
}

func (f *FunctionAVERAGEIF) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
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
	var count int
	if args2.IsNumber() {
		count = internals.GetCountIf(list, args2.NumberValue())
		sum = internals.GetSumIf(list, args2.NumberValue(), sumdbs)
	} else if args2.IsText() {
		text := strings.TrimSpace(args2.TextValue())
		if d, ok := TryParseDecimal(text); ok {
			count = internals.GetCountIf(list, d)
			sum = internals.GetSumIf(list, d, sumdbs)
		} else {
			op, value, matched := internals.ParseSumIfMatch(text)
			if matched {
				count = internals.GetCountIfOp(list, op, value)
				sum = internals.GetSumIfOp(list, op, value, sumdbs)
			} else {
				return f.ParameterError(2)
			}
		}
	} else {
		return f.ParameterError(2)
	}
	if count == 0 {
		return f.Div0Error()
	}
	return CreateDecimal(sum.Div(DecFromInt(int64(count))))
}

func (f *FunctionAVERAGEIF) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	f.Arg(0).GetParameterTypes(engine, result, OperandTypeArray)
	f.Arg(1).GetParameterTypes(engine, result, OperandTypeNone)
	if f.Arg(2) != nil {
		f.Arg(2).GetParameterTypes(engine, result, OperandTypeArray)
	}
}

func init() { RegisterFunction("AVERAGEIF", newFunctionAVERAGEIF) }
