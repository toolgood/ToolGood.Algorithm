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
		private readonly List<KeyValue> _keyValueList;
		public OperandKeyValueList()
		{
			_keyValueList = new List<KeyValue>();
		}

		public override bool IsArrayJson => true;
		public override OperandType Type => OperandType.ARRARYJSON;
		public override List<Operand> ArrayValue => _keyValueList.Select(q => q.Value).ToList();

		public override Operand ToText(string errorMessage = null)
		{
			return Create(ToString());
		}
		public override Operand ToText(string errorMessage, params object[] args)
		{
			return Create(ToString());
		}

		public override Operand ToArray(string errorMessage)
		{
			return Create(ArrayValue);
		}
		public override Operand ToArray(string errorMessage, params object[] args)
		{
			return Create(ArrayValue);
		}

		public override Operand ToJson(string errorMessage = null)
		{
			var txt = ToString();
			try {
				var json = JsonMapper.ToObject(txt);
				return Operand.Create(json);
			} catch { }
			return Error(errorMessage ?? "Convert to json error!");
		}

		public void AddValue(KeyValue keyValue)
		{
			_keyValueList.Add(keyValue);
		}

		public bool TryGetValue(string key, out Operand value)
		{
			foreach (var item in _keyValueList) {
				if (item.Key == key) {
					value = item.Value;
					return true;
				}
			}
			value = null;
			return false;
		}

		public bool ContainsKey(Operand value)
		{
			return _keyValueList.Any(item => item.Key == value.TextValue);
		}

		public bool ContainsValue(Operand value)
		{
			foreach (var item in _keyValueList) {
				var op = item.Value;
				if (value.Type != op.Type) continue;
				if (value.IsText && value.TextValue == op.TextValue) {
					return true;
				}
			}
			return false;
		}

		public override string ToString()
		{
			var stringBuilder = new StringBuilder(_keyValueList.Count * 32);
			stringBuilder.Append('{');
			for (var i = 0; i < _keyValueList.Count; i++) {
				if (i > 0) stringBuilder.Append(',');
				stringBuilder.Append('"');
				stringBuilder.Append(_keyValueList[i].Key);
				stringBuilder.Append('"');
				stringBuilder.Append(':');
				stringBuilder.Append(_keyValueList[i].Value.ToString());
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}
	}
}