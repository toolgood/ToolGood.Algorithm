using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.LitJson;

namespace ToolGood.Algorithm
{
	internal sealed class OperandArray : Operand
	{
		private readonly List<Operand> _value;
		public OperandArray(List<Operand> obj)
		{
			_value = obj;
		}
		public override bool IsArray => true;
		public override OperandType Type => OperandType.ARRARY;
		public override List<Operand> ArrayValue => _value;

		public override Operand ToText(string errorMessage = null)
		{
			return Create(ToString());
		}
		public override Operand ToText(string errorMessage, params object[] args)
		{
			return Create(ToString());
		}

		public override Operand ToArray(string errorMessage) { return this; }
		public override Operand ToArray(string errorMessage, params object[] args) { return this; }

		public override Operand ToJson(string errorMessage = null)
		{
			var txt = ToString();
			try {
				var json = JsonMapper.ToObject(txt);
				return Operand.Create(json);
			} catch { }
			return Error(errorMessage ?? "Convert to json error!");
		}

		public override string ToString()
		{
			var stringBuilder = new StringBuilder(_value.Count * 16);
			stringBuilder.Append('[');
			for (var i = 0; i < _value.Count; i++) {
				if (i > 0) stringBuilder.Append(',');
				stringBuilder.Append(_value[i].ToString());
			}
			stringBuilder.Append(']');
			return stringBuilder.ToString();
		}
	}
}