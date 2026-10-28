using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals.Functions;
using ToolGood.Algorithm.LitJson;

namespace ToolGood.Algorithm.Operands
{
	internal sealed class OperandKeyValueList : Operand
	{
		private readonly List<KeyValue> _keyValueList;
		public OperandKeyValueList()
		{
			_keyValueList = new List<KeyValue>();
		}

		public override bool IsArrayJson => true;
		public override OperandType Type => OperandType.ARRAYJSON;
		public override List<Operand> ArrayValue {
			get {
				var list = new List<Operand>(_keyValueList.Count);
				for(var i = 0; i < _keyValueList.Count; i++) {
					list.Add(_keyValueList[i].Value);
				}
				return list;
			}
		}

		public override Operand ToText(string errorMessage = null)
		{
			return Create(ToString());
		}
		public override Operand ToText(string errorMessage, params object[] args)
		{
			return Create(ToString());
		}
		internal override bool TryGetText(out Operand value) { value = Create(ToString()); return true; }

		public override Operand ToArray(string errorMessage)
		{
			return Create(ArrayValue);
		}
		public override Operand ToArray(string errorMessage, params object[] args)
		{
			return Create(ArrayValue);
		}
		internal override bool TryGetArray(out Operand value) { value = Create(ArrayValue); return true; }

		public override Operand ToJson(string errorMessage = null)
		{
			// 直接构造成 JsonData，免去 ToString 序列化再 JsonMapper 解析的完整往返
			var json = ToJsonData();
			if (json == null) return Error(errorMessage ?? "Convert to json error!");
			return Operand.Create(json);
		}

		internal JsonData ToJsonData()
		{
			var json = new JsonData();
			json.SetJsonType(JsonType.Object);
			for (var i = 0; i < _keyValueList.Count; i++) {
				var data = OperandArray.OperandToJsonData(_keyValueList[i].Value);
				if (data == null) return null;
				json.Set(_keyValueList[i].Key, data);
			}
			return json;
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
			var key = value.TextValue;
			for(var i = 0; i < _keyValueList.Count; i++) {
				if(_keyValueList[i].Key == key) { return true; }
			}
			return false;
		}

		public bool ContainsValue(Operand value)
		{
			var v = value.TextValue;
			for(var i = 0; i < _keyValueList.Count; i++) {
				if(FunctionUtil.OperandValueEquals(_keyValueList[i].Value, v)) { return true; }
			}
			return false;
		}

		public override string ToString()
		{
			var stringBuilder = new StringBuilder(_keyValueList.Count * 32);
			stringBuilder.Append('{');
			for (var i = 0; i < _keyValueList.Count; i++) {
				if (i > 0) stringBuilder.Append(',');
				stringBuilder.Append(new OperandString(_keyValueList[i].Key).ToString());
				stringBuilder.Append(':');
				stringBuilder.Append(_keyValueList[i].Value.ToString());
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}
	}
}