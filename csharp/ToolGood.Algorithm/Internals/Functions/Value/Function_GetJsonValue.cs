using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Value
{
	internal class Function_GetJsonValue : Function_2
	{
		public Function_GetJsonValue(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var obj = func1.Evaluate(work, tempParameter); if (obj.IsError) { return obj; }
			var op = func2.Evaluate(work, tempParameter); if (op.IsError) { return op; }

			if (obj.IsArray) {
				op = op.ToNumber("Function '{0}' parameter {1} is error!", "GetJsonValue", 2);
				if (op.IsError) { return op; }
				var index = op.IntValue - work.ExcelIndex;
				if (index < obj.ArrayValue.Count)
					return obj.ArrayValue[index];
				return Operand.Error("Function '{0}' ARRARY index {1} greater than maximum length!", "GetJsonValue", index);
			}
			if (obj.IsArrayJson) {
				if (op.IsNumber) {
					if (((OperandKeyValueList)obj).TryGetValue(op.NumberValue.ToString(), out Operand operand)) {
						return operand;
					}
					return Operand.Error("Function '{0}' Parameter name '{1}' is missing!", "GetJsonValue", op.TextValue);
				} else if (op.IsText) {
					if (((OperandKeyValueList)obj).TryGetValue(op.TextValue, out Operand operand)) {
						return operand;
					}
					return Operand.Error("Function '{0}' Parameter name '{1}' is missing!", "GetJsonValue", op.TextValue);
				}
				return Operand.Error("Function '{0}' Parameter name is missing!", "GetJsonValue");
			}

			if (obj.IsJson) {
				var json = obj.JsonValue;
				if (json.IsArray) {
					op = op.ToNumber("Function '{0}' parameter {1} is error!", "GetJsonValue", 2);
					if (op.IsError) { return op; }
					var index = op.IntValue - work.ExcelIndex;
					if (index < json.Count) {
						var v = json[index];
						if (v.IsString) return Operand.Create(v.StringValue);
						if (v.IsBoolean) return Operand.Create(v.BooleanValue);
						if (v.IsDouble) return Operand.Create(v.NumberValue);
						if (v.IsObject) return Operand.Create(v);
						if (v.IsArray) return Operand.Create(v);
						if (v.IsNull) return Operand.CreateNull();
						return Operand.Create(v);
					}
					return Operand.Error("Function '{0}' JSON index {1} greater than maximum length!", "GetJsonValue", index);
				} else {
					op = op.ToText("Function '{0}' parameter {1} is error!", "GetJsonValue", 2);
					if (op.IsError) { return op; }
					var v = json[op.TextValue];
					if (v != null) {
						if (v.IsString) return Operand.Create(v.StringValue);
						if (v.IsBoolean) return Operand.Create(v.BooleanValue);
						if (v.IsDouble) return Operand.Create(v.NumberValue);
						if (v.IsObject) return Operand.Create(v);
						if (v.IsArray) return Operand.Create(v);
						if (v.IsNull) return Operand.CreateNull();
						return Operand.Create(v);
					}
				}
			}
			return Operand.Error("Function '{0}' Operator is error!", "GetJsonValue");
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			func1.ToString(stringBuilder, false);
			stringBuilder.Append('[');
			func2.ToString(stringBuilder, false);
			stringBuilder.Append(']');
		}
	}

}
