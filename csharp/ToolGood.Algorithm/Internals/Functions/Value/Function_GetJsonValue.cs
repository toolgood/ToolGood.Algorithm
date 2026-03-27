using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.LitJson;
using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm.Internals.Functions.Value
{
	internal sealed class Function_GetJsonValue : Function_2
	{

		public Function_GetJsonValue(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override string Name => "GetJsonValue";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var obj = func1.Evaluate(engine, tempParameter); if(obj.IsErrorOrNone) { return obj; }
			var op = func2.Evaluate(engine, tempParameter); if(op.IsErrorOrNone) { return op; }

			if(obj.IsArray) {
				op = ConvertToNumber(op, 2);
				if(op.IsErrorOrNone) { return op; }
				var index = op.IntValue - engine.ExcelIndex;
				if(index < obj.ArrayValue.Count && index >= 0)
					return obj.ArrayValue[index];
				return Operand.Error("Function '{0}' ARRARY index {1} greater than maximum length!", "GetJsonValue", index);
			}
			if(obj.IsArrayJson) {
				if(op.IsNumber) {
					if(((OperandKeyValueList)obj).TryGetValue(op.NumberValue.ToString(), out Operand operand)) {
						return operand;
					}
					return Operand.Error("Function '{0}' Parameter name '{1}' is missing!", "GetJsonValue", op.TextValue);
				} else if(op.IsText) {
					if(((OperandKeyValueList)obj).TryGetValue(op.TextValue, out Operand operand)) {
						return operand;
					}
					return Operand.Error("Function '{0}' Parameter name '{1}' is missing!", "GetJsonValue", op.TextValue);
				}
				return Operand.Error("Function '{0}' Parameter name is missing!", "GetJsonValue");
			}

			if(obj.IsJson) {
				var json = obj.JsonValue;
				if(json.IsArray) {
					op = ConvertToNumber(op, 2);
					if(op.IsErrorOrNone) { return op; }
					var index = op.IntValue - engine.ExcelIndex;
					if(index < json.Count && index >= 0) {
						return ConvertJsonDataToOperand(json[index]);
					}
					return Operand.Error("Function '{0}' JSON index {1} greater than maximum length!", "GetJsonValue", index);
				} else {
					op = ConvertToText(op, 2);
					if(op.IsErrorOrNone) { return op; }
					var v = json[op.TextValue];
					if(v != null) {
						return ConvertJsonDataToOperand(v);
					}
				}
			}
			return Operand.Error("Function '{0}' Operator is error!", "GetJsonValue");
		}

		private static Operand ConvertJsonDataToOperand(JsonData v)
		{
			return v switch {
				_ when v.IsString => Operand.Create(v.StringValue),
				_ when v.IsBoolean => Operand.Create(v.BooleanValue),
				_ when v.IsDouble => Operand.Create(v.NumberValue),
				_ when v.IsObject => Operand.Create(v),
				_ when v.IsArray => Operand.Create(v),
				_ when v.IsNull => Operand.Null,
				_ => Operand.Create(v)
			};
		}

		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			func1.ToString(stringBuilder, false);
			stringBuilder.Append('[');
			func2.ToString(stringBuilder, false);
			stringBuilder.Append(']');
		}
		public override OperandType GetResultType()
		{
			return OperandType.NONE;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NONE);
			func2.GetParameterTypes(noneEngine, result, OperandType.NONE);
		}
	}

}
