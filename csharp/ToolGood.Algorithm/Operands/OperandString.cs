using System;
using System.Globalization;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals.Functions;
using ToolGood.Algorithm.LitJson;

namespace ToolGood.Algorithm.Operands
{
	internal sealed class OperandString : Operand
	{
		private readonly string _value;
		public OperandString(string obj)
		{
			_value = obj;
		}

		public override bool IsText => true;
		public override OperandType Type => OperandType.TEXT;
		public override string TextValue => _value;

		#region ToNumber

		public override Operand ToNumber(string errorMessage)
		{
			if(TryGetNumber(out var value)) { return value; }
			return Error(errorMessage ?? "Convert to number error!");
		}
		public override Operand ToNumber(string errorMessage, params object[] args)
		{
			// 先解析、失败才格式化：字符串→数字是热路径（每次公式里的 '文本'+数字 都会走这里），
			// 若在解析前 string.Format，成功的转换也要白造一条用不上的错误消息（63ns + 120B）。
			if(TryGetNumber(out var value)) { return value; }
			return Error(string.Format(errorMessage, args));
		}

		internal override bool TryGetNumber(out Operand value)
		{
			var span = _value.AsSpan();
			if(span.IndexOf('.') < 0 && int.TryParse(span, NumberStyles.Integer, CultureInfo.InvariantCulture, out var num)) {
				value = Create(num);
				return true;
			}
			if(decimal.TryParse(span, NumberStyles.Any, CultureInfo.InvariantCulture, out var d)) {
				value = Create(d);
				return true;
			}
			value = null;
			return false;
		}

		#endregion

		public override Operand ToText(string errorMessage) { return this; }
		public override Operand ToText(string errorMessage, params object[] args) { return this; }

		#region ToBoolean

		public override Operand ToBoolean(string errorMessage)
		{
			if(TryGetBoolean(out var value)) { return value; }
			return Error(errorMessage ?? "Convert to bool error!");
		}
		public override Operand ToBoolean(string errorMessage, params object[] args)
		{
			if(TryGetBoolean(out var value)) { return value; }
			return Error(string.Format(errorMessage, args));
		}

		internal override bool TryGetBoolean(out Operand value)
		{
			if(FunctionUtil.TryParseBoolean(_value, out var b)) {
				value = b ? True : False;
				return true;
			}
			value = null;
			return false;
		}

		#endregion

		#region ToMyDate

		public override Operand ToMyDate(string errorMessage)
		{
			if(TryGetDate(out var value)) { return value; }
			return Error(errorMessage ?? "Convert to date error!");
		}
		public override Operand ToMyDate(string errorMessage, params object[] args)
		{
			if(TryGetDate(out var value)) { return value; }
			return Error(string.Format(errorMessage, args));
		}

		internal override bool TryGetDate(out Operand value)
		{
			var span = _value.AsSpan();
			if(TimeSpan.TryParse(span, CultureInfo.InvariantCulture, out var t)) {
				value = Create(new MyDate(t));
				return true;
			}
			if(DateTime.TryParse(span, CultureInfo.InvariantCulture, DateTimeStyles.None, out var d)) {
				value = Create(new MyDate(d));
				return true;
			}
			value = null;
			return false;
		}

		#endregion

		public override Operand ToArray(string errorMessage)
		{
			return Error(errorMessage ?? "Convert to array error!");
		}
		public override Operand ToJson(string errorMessage = null)
		{
			var span = TextValue.Trim();
			if ((span.Length > 0 && span[0] == '{' && span[span.Length - 1] == '}') || 
				(span.Length > 0 && span[0] == '[' && span[span.Length - 1] == ']')) {
				try {
					var json = JsonMapper.ToObject(span);
					return Operand.Create(json);
				} catch { 
					return Error(errorMessage ?? "Convert to json error!");
				}
			}
			return Error(errorMessage ?? "Convert to json error!");
		}

		public override string ToString()
		{
			var len = _value.Length;
			if(len == 0) return "\"\"";
			
			int escapeCount = 0;
			for(int i = 0; i < len; i++) {
				var c = _value[i];
				if(c == '"' || c == '\\' || c < ' ') {
					escapeCount++;
				}
			}
			
			if(escapeCount == 0) {
				return "\"" + _value + "\"";
			}
			
			return ToStringInternal(escapeCount);
		}

		private string ToStringInternal(int escapeCount)
		{
			var len = _value.Length;
			var sb = new StringBuilder(len + escapeCount + 2);
			sb.Append('"');
			for(int i = 0; i < len; i++) {
				var c = _value[i];
				switch(c) {
					case '"': sb.Append("\\\""); break;
					case '\\': sb.Append("\\\\"); break;
					case '\n': sb.Append("\\n"); break;
					case '\r': sb.Append("\\r"); break;
					case '\t': sb.Append("\\t"); break;
					case '\b': sb.Append("\\b"); break;
					case '\f': sb.Append("\\f"); break;
					default: 
						if(c < ' ') {
							sb.Append("\\u");
							sb.Append(((int)c).ToString("x4"));
						} else {
							sb.Append(c);
						}
						break;
				}
			}
			sb.Append('"');
			return sb.ToString();
		}
	}
}