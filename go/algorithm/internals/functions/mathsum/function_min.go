package mathsum

import "github.com/toolgood/ToolGood.Algorithm/go/algorithm/internals"

// FunctionMIN 对应 C# Function_MIN。
type FunctionMIN struct{ BaseFunction }

func newFunctionMIN(args []Function) Function {
	RequireArgs(args, "Min", 1, -1)
	return &FunctionMIN{BaseFunction: NewBase("Min", OperandTypeNumber, args)}
}

func (f *FunctionMIN) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	var args []Operand
	if err := f.EvaluateAll(engine, tp, &args); err != nil {
		return err
	}

	found := false
	min := DecimalMax

	for _, item := range args {
		if item.IsArray() {
			for _, elem := range item.ArrayValue() {
				if elem.IsNumber() {
					if !found || elem.NumberValue().Less(min) {
						min = elem.NumberValue()
						found = true
					}
				} else if elem.IsArray() || elem.IsJson() {
					var list []Decimal
					if internals.FlattenToNumberList(elem, &list) {
						for _, v := range list {
							if !found || v.Less(min) {
								min = v
								found = true
							}
						}
					} else {
						return f.FunctionError()
					}
				} else {
					converted := elem.ToNumber("")
					if converted.IsError() {
						return f.FunctionError()
					}
					if !found || converted.NumberValue().Less(min) {
						min = converted.NumberValue()
						found = true
					}
				}
			}
		} else if item.IsNumber() {
			if !found || item.NumberValue().Less(min) {
				min = item.NumberValue()
				found = true
			}
		} else if item.IsJson() {
			var list []Decimal
			if !internals.FlattenToNumberList(item, &list) {
				return f.FunctionError()
			}
			for _, v := range list {
				if !found || v.Less(min) {
					min = v
					found = true
				}
			}
		} else {
			converted := item.ToNumber("")
			if converted.IsError() {
				return f.FunctionError()
			}
			if !found || converted.NumberValue().Less(min) {
				min = converted.NumberValue()
				found = true
			}
		}
	}

	if !found {
		return f.FunctionError()
	}
	return CreateDecimal(min)
}

func (f *FunctionMIN) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	count := f.ArgCount()
	if count == 1 {
		f.Arg(0).GetParameterTypes(engine, result, OperandTypeArray)
		return
	}
	if count == 2 && engine != nil {
		t1 := f.Arg(0).ResultType()
		t2 := f.Arg(1).ResultType()
		if t1 == OperandTypeNone && t2 == OperandTypeNumber {
			if p := engine.Evaluate(f.Arg(1)).ToText(""); !p.IsErrorOrNone() {
				f.Arg(0).GetParameterTypes(engine, result, t2, f.Name(), p.TextValue())
				f.Arg(1).GetParameterTypes(engine, result, t2)
				return
			}
		} else if t1 == OperandTypeNumber && t2 == OperandTypeNone {
			if p := engine.Evaluate(f.Arg(0)).ToText(""); !p.IsErrorOrNone() {
				f.Arg(1).GetParameterTypes(engine, result, t1, f.Name(), p.TextValue())
				f.Arg(0).GetParameterTypes(engine, result, t1)
				return
			}
		}
	}
	for i := 0; i < count; i++ {
		f.Arg(i).GetParameterTypes(engine, result, OperandTypeNumber)
	}
}

func init() { RegisterFunction("MIN", newFunctionMIN) }
