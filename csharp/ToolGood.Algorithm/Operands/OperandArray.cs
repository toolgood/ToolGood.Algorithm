using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.LitJson;

namespace ToolGood.Algorithm.Operands
{
	internal sealed class OperandArray : Operand
	{
		private readonly List<Operand> _value;
		public OperandArray(List<Operand> obj)
		{
			_value = obj;
		}
		public override bool IsArray => true;
		public override OperandType Type => OperandType.ARRAY;
		public override List<Operand> ArrayValue => _value;

		public override Operand ToText(string errorMessage = null)
		{
			return Create(ToString());
		}
		public override Operand ToText(string errorMessage, params object[] args)
		{
			return Create(ToString());
		}
		internal override bool TryGetText(out Operand value) { value = Create(ToString()); return true; }

		public override Operand ToArray(string errorMessage) { return this; }
		public override Operand ToArray(string errorMessage, params object[] args) { return this; }
		internal override bool TryGetArray(out Operand value) { value = this; return true; }

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
			json.SetJsonType(JsonType.Array);
			for (var i = 0; i < _value.Count; i++) {
				var data = OperandToJsonData(_value[i]);
				if (data == null) return null;
				json.Add(data);
			}
			return json;
		}

		/// <summary>
		/// 按元素原 ToString() → JsonMapper.ToObject 往返的等价结果直接构造 JsonData；
		/// 无法等价转换的类型（OperandError/OperandNone，其 ToString 非法 JSON，原往返必然失败）返回 null
		/// </summary>
		internal static JsonData OperandToJsonData(Operand o)
		{
			switch (o) {
				case OperandString s: { var d = new JsonData(); d.SetString(s.TextValue); return d; }
				case OperandInt i: { var d = new JsonData(); d.SetDouble(i.NumberValue); return d; }
				case OperandDecimal dec: { var d = new JsonData(); d.SetDouble(OperandDecimal.Normalize(dec.NumberValue)); return d; }
				case OperandBoolean b: { var d = new JsonData(); d.SetBoolean(b.BooleanValue); return d; }
				case OperandMyDate md: { var d = new JsonData(); d.SetString(md.DateValue.ToString()); return d; }
				case OperandNull _: { var d = new JsonData(); d.SetNull(); return d; }
				case OperandJson j: return j.JsonValue;
				case OperandArray arr: return arr.ToJsonData();
				case OperandKeyValueList kvl: return kvl.ToJsonData();
				default: return null;
			}
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