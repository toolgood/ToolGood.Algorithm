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
		public override bool IsNotArray => false;
		public override OperandType Type => OperandType.ARRARY;
		public override List<Operand> ArrayValue => _value;

		public override Operand ToText(string errorMessage = null)
		{
			return Create(this.ToString());
		}
		public override Operand ToText(string errorMessage, params object[] args)
		{
			return Create(this.ToString());
		}

		public override Operand ToArray(string errorMessage) { return this; }
		public override Operand ToArray(string errorMessage, params object[] args) { return this; }

		public override Operand ToJson(string errorMessage = null)
		{
			var txt = this.ToString();
			try {
				var json = JsonMapper.ToObject(txt);
				return Operand.Create(json);
			} catch(Exception) { }
			return Error(errorMessage ?? "Convert to json error!");
		}

		public override string ToString()
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.Append('[');
			for(int i = 0; i < ArrayValue.Count; i++) {
				if(i > 0) stringBuilder.Append(',');
				stringBuilder.Append(ArrayValue[i].ToString());
			}
			stringBuilder.Append(']');
			return stringBuilder.ToString();
		}
	}
}