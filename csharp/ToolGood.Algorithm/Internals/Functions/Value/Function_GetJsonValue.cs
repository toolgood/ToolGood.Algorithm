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
				op = op.ToNumber("ARRARY index is error!");
				if (op.IsError) { return op; }
				var index = op.IntValue - work.ExcelIndex;
				if (index < obj.ArrayValue.Count)
					return obj.ArrayValue[index];
				return Operand.Error("ARRARY index {0} greater than maximum length!", index);
			}
			if (obj.IsArrayJson) {
				if (op.IsNumber) {
					if (((OperandKeyValueList)obj).TryGetValue(op.NumberValue.ToString(), out Operand operand)) {
						return operand;
					}
					return Operand.Error("Parameter name '{0}' is missing!", op.TextValue);
				} else if (op.IsText) {
					if (((OperandKeyValueList)obj).TryGetValue(op.TextValue, out Operand operand)) {
						return operand;
					}
					return Operand.Error("Parameter name '{0}' is missing!", op.TextValue);
				}
				return Operand.Error("Parameter name is missing!");
			}

			if (obj.IsJson) {
				var json = obj.JsonValue;
				if (json.IsArray) {
					op = op.ToNumber("JSON parameter index is error!");
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
					return Operand.Error("JSON index {0} greater than maximum length!", index);
				} else {
					op = op.ToText("JSON parameter name is error!");
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
			return Operand.Error("Operator is error!");
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
