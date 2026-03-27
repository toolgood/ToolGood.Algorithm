using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.LitJson;

namespace ToolGood.Algorithm.Operands
{
	internal sealed class OperandJson : Operand
	{
		private readonly JsonData _value;
		public OperandJson(JsonData obj)
		{
			_value = obj;
		}
		public override bool IsJson => true;
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
					list.Add(v switch
					{
						_ when v.IsString => Operand.Create(v.StringValue),
						_ when v.IsBoolean => Operand.Create(v.BooleanValue),
						_ when v.IsDouble => Operand.Create(v.NumberValue),
						_ when v.IsNull => Operand.Null,
						_ => Operand.Create(v)
					});
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