package mathsum

import "github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals"

// FunctionSUM 对应 C# Function_SUM。
type FunctionSUM struct{ BaseFunction }

func newFunctionSUM(args []Function) Function {
	RequireArgs(args, "Sum", 1, -1)
	return &FunctionSUM{BaseFunction: NewBase("Sum", OperandTypeNumber, args)}
}

func (f *FunctionSUM) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	var args []Operand
	if err := f.EvaluateAll(engine, tp, &args); err != nil {
		return err
	}

	sum := DecimalZero
	for _, item := range args {
		if item.IsArray() {
			for _, elem := range item.ArrayValue() {
				if elem.IsNumber() {
					sum = sum.Add(elem.NumberValue())
				} else if elem.IsArray() || elem.IsJson() {
					var list []Decimal
					if internals.FlattenToNumberList(elem, &list) {
						for _, v := range list {
							sum = sum.Add(v)
						}
					} else {
						return f.FunctionError()
					}
				} else {
					converted := elem.ToNumber("")
					if converted.IsError() {
						return f.FunctionError()
					}
					sum = sum.Add(converted.NumberValue())
				}
			}
		} else if item.IsNumber() {
			sum = sum.Add(item.NumberValue())
		} else if item.IsJson() {
			var list []Decimal
			if !internals.FlattenToNumberList(item, &list) {
				return f.FunctionError()
			}
			for _, v := range list {
				sum = sum.Add(v)
			}
		} else {
			converted := item.ToNumber("")
			if converted.IsError() {
				return f.FunctionError()
			}
			sum = sum.Add(converted.NumberValue())
		}
	}
	return CreateDecimal(sum)
}

func (f *FunctionSUM) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	argTypes(engine, &f.BaseFunction, result, OperandTypeArray, OperandTypeNumber)
}

func init() { RegisterFunction("SUM", newFunctionSUM) }
