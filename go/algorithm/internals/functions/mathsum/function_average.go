package mathsum

import "github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals"

// FunctionAVERAGE 对应 C# Function_AVERAGE。
type FunctionAVERAGE struct{ BaseFunction }

func newFunctionAVERAGE(args []Function) Function {
	RequireArgs(args, "Average", 1, -1)
	return &FunctionAVERAGE{BaseFunction: NewBase("Average", OperandTypeNumber, args)}
}

func (f *FunctionAVERAGE) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	var args []Operand
	if err := f.EvaluateAll(engine, tp, &args); err != nil {
		return err
	}

	sum := DecimalZero
	count := 0

	for _, item := range args {
		if item.IsArray() {
			for _, elem := range item.ArrayValue() {
				if elem.IsNumber() {
					sum = sum.Add(elem.NumberValue())
					count++
				} else if elem.IsArray() || elem.IsJson() {
					var list []Decimal
					if !internals.FlattenToNumberList(elem, &list) {
						return f.FunctionError()
					}
					for _, v := range list {
						sum = sum.Add(v)
					}
					count += len(list)
				} else {
					converted := elem.ToNumber("")
					if converted.IsError() {
						return f.FunctionError()
					}
					sum = sum.Add(converted.NumberValue())
					count++
				}
			}
		} else if item.IsNumber() {
			sum = sum.Add(item.NumberValue())
			count++
		} else if item.IsJson() {
			var list []Decimal
			if !internals.FlattenToNumberList(item, &list) {
				return f.FunctionError()
			}
			for _, v := range list {
				sum = sum.Add(v)
			}
			count += len(list)
		} else {
			converted := item.ToNumber("")
			if converted.IsError() {
				return f.FunctionError()
			}
			sum = sum.Add(converted.NumberValue())
			count++
		}
	}

	if count == 0 {
		return f.Div0Error()
	}
	return CreateDecimal(sum.Div(DecFromInt(int64(count))))
}

func (f *FunctionAVERAGE) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	argTypes(engine, &f.BaseFunction, result, OperandTypeArray, OperandTypeNumber)
}

func init() { RegisterFunction("AVERAGE", newFunctionAVERAGE) }
