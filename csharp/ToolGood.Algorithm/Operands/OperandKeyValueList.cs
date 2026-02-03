using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.LitJson;
using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm
{
	internal sealed class OperandKeyValueList : Operand
	{
		private readonly List<KeyValue> TextList;
		public OperandKeyValueList()
		{
			TextList = new List<KeyValue>();
		}

		public override bool IsArrayJson => true;
		public override bool IsNotArrayJson => false;
		public override OperandType Type => OperandType.ARRARYJSON;
		public override List<Operand> ArrayValue => TextList.Select(q => q.Value).ToList();

		public override Operand ToText(string errorMessage = null)
		{
			return Create(this.ToString());
		}
		public override Operand ToText(string errorMessage, params object[] args)
		{
			return Create(this.ToString());
		}

		public override Operand ToArray(string errorMessage)
		{
			return Create(this.ArrayValue);
		}
		public override Operand ToArray(string errorMessage, params object[] args)
		{
			return Create(this.ArrayValue);
		}

		public override Operand ToJson(string errorMessage = null)
		{
			var txt = this.ToString();
			try {
				var json = JsonMapper.ToObject(txt);
				return Operand.Create(json);
			} catch(Exception) { }
			return Error(errorMessage ?? "Convert to json error!");
		}

		public void AddValue(KeyValue keyValue)
		{
			TextList.Add(keyValue);
		}

		public bool TryGetValue(string key, out Operand value)
		{
			foreach(var item in TextList) {
				if(item.Key == key.ToString()) {
					value = item.Value;
					return true;
				}
			}
			value = null;
			return false;
		}

		public bool ContainsKey(Operand value)
		{
			foreach(var item in TextList) {
				if(value.TextValue == item.Key) {
					return true;
				}
			}
			return false;
		}

		public bool ContainsValue(Operand value)
		{
			foreach(var item in TextList) {
				var op = item.Value;
				if(value.Type != op.Type) { continue; }
				if(value.IsText) {
					if(value.TextValue == op.TextValue) {
						return true;
					}
				}
			}
			return false;
		}

		public override string ToString()
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.Append('{');
			for(int i = 0; i < TextList.Count; i++) {
				if(i > 0) stringBuilder.Append(',');
				stringBuilder.Append('"');
				stringBuilder.Append(TextList[i].Key);
				stringBuilder.Append('"');
				stringBuilder.Append(':');
				stringBuilder.Append(TextList[i].Value.ToString());
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}
	}
}