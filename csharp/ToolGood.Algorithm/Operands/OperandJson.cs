using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.LitJson;

namespace ToolGood.Algorithm
{
	internal sealed class OperandJson : Operand
	{
		private readonly JsonData _value;
		public OperandJson(JsonData obj)
		{
			_value = obj;
		}
		public override bool IsJson => true;
		public override bool IsNotJson => false;
		public override OperandType Type => OperandType.JSON;
		internal override JsonData JsonValue => _value;

		public override Operand ToText(string errorMessage = null)
		{
			return Create(_value.ToString());
		}
		public override Operand ToText(string errorMessage, params object[] args)
		{
			return Create(_value.ToString());
		}

		public override Operand ToArray(string errorMessage)
		{
			return ToArrayInternal(errorMessage);
		}
		public override Operand ToArray(string errorMessage, params object[] args)
		{
			return ToArrayInternal(string.Format(errorMessage, args));
		}

		private Operand ToArrayInternal(string errorMessage)
		{
			if (JsonValue.IsArray) {
				var list = new List<Operand>(JsonValue.Count);
				foreach (JsonData v in JsonValue) {
					if (v.IsString)
						list.Add(Operand.Create(v.StringValue));
					else if (v.IsBoolean)
						list.Add(Operand.Create(v.BooleanValue));
					else if (v.IsDouble)
						list.Add(Operand.Create(v.NumberValue));
					else if (v.IsNull)
						list.Add(Operand.CreateNull());
					else
						list.Add(Operand.Create(v));
				}
				return Create(list);
			}
			return Error(errorMessage ?? "Convert to array error!");
		}

		public override Operand ToJson(string errorMessage = null)
		{
			return this;
		}
		public override string ToString()
		{
			return _value.ToString();
		}
	}
}