package value

import "strings"

// 移植自 Internals/Functions/Value/Function_GetJsonValue.cs。

// FunctionGetJsonValue 取数组下标或 JSON / 键值集合的成员。
type FunctionGetJsonValue struct{ BaseFunction }

// NewFunctionGetJsonValue 构造取值节点（对应 C# 的 Function_GetJsonValue(FunctionBase, FunctionBase)）。
func NewFunctionGetJsonValue(obj, key Function) Function {
	return &FunctionGetJsonValue{BaseFunction: NewBase("GetJsonValue", OperandTypeNone, []Function{obj, key})}
}

// Evaluate 按下标或属性名取值。
func (f *FunctionGetJsonValue) Evaluate(engine *AlgorithmEngine, tp TempParameter) Operand {
	obj := f.EvalArg(0, engine, tp)
	if obj.IsErrorOrNone() {
		return obj
	}
	op := f.EvalArg(1, engine, tp)
	if op.IsErrorOrNone() {
		return op
	}

	if obj.IsArray() {
		op = f.ConvertToNumber(op, 2)
		if op.IsErrorOrNone() {
			return op
		}
		index := op.IntValue() - engine.ExcelIndex
		list := obj.ArrayValue()
		if index < len(list) && index >= 0 {
			return list[index]
		}
		return Errorf("Function '{0}' ARRAY index {1} out of range!", "GetJsonValue", index)
	}
	if obj.IsArrayJson() {
		keyValueList, ok := obj.(*OperandKeyValueList)
		if !ok {
			return Errorf("Function '{0}' Operator is error!", "GetJsonValue")
		}
		if op.IsNumber() {
			if value := keyValueList.GetValue(op.NumberValue().String()); value != nil {
				return value
			}
			return Errorf("Function '{0}' Parameter name '{1}' is missing!", "GetJsonValue", op.NumberValue())
		} else if op.IsText() {
			if value := keyValueList.GetValue(op.TextValue()); value != nil {
				return value
			}
			return Errorf("Function '{0}' Parameter name '{1}' is missing!", "GetJsonValue", op.TextValue())
		}
		return Errorf("Function '{0}' Parameter name is missing!", "GetJsonValue")
	}

	if obj.IsJson() {
		json := obj.JsonValue()
		if json.IsArray() {
			op = f.ConvertToNumber(op, 2)
			if op.IsErrorOrNone() {
				return op
			}
			index := op.IntValue() - engine.ExcelIndex
			if index < json.Count() && index >= 0 {
				return convertJsonDataToOperand(json.GetChild(index))
			}
			return Errorf("Function '{0}' JSON index {1} out of range!", "GetJsonValue", index)
		}
		op = f.ConvertToText(op, 2)
		if op.IsErrorOrNone() {
			return op
		}
		if v := json.GetChild(op.TextValue()); v != nil {
			return convertJsonDataToOperand(v)
		}
	}
	return Errorf("Function '{0}' Operator is error!", "GetJsonValue")
}

// convertJsonDataToOperand 把 JsonData 转换为操作数（对应 C# 的 ConvertJsonDataToOperand）。
func convertJsonDataToOperand(v *JsonData) Operand {
	switch {
	case v.IsString():
		return CreateString(v.StringValue())
	case v.IsBoolean():
		return CreateBool(v.BooleanValue())
	case v.IsDouble():
		return CreateDecimal(v.NumberValue())
	case v.IsObject():
		return CreateJsonData(v)
	case v.IsArray():
		return CreateJsonData(v)
	case v.IsNull():
		return Null
	}
	return CreateJsonData(v)
}

// WriteTo 输出 `对象[键]`。
func (f *FunctionGetJsonValue) WriteTo(sb *strings.Builder, addBrackets bool) {
	if obj := f.Arg(0); obj != nil {
		obj.WriteTo(sb, false)
	}
	sb.WriteByte('[')
	if key := f.Arg(1); key != nil {
		key.WriteTo(sb, false)
	}
	sb.WriteByte(']')
}

func (f *FunctionGetJsonValue) GetParameterTypes(engine *AlgorithmEngine, result *[]ParameterType, operandType OperandType, opVal ...string) {
	if obj := f.Arg(0); obj != nil {
		obj.GetParameterTypes(engine, result, OperandTypeNone)
	}
	if key := f.Arg(1); key != nil {
		key.GetParameterTypes(engine, result, OperandTypeNone)
	}
}
