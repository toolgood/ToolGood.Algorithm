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

		internal override bool TryGetText(out Operand value) { value = this; return true; }
		internal override bool TryAppendText(System.Text.StringBuilder sb) { sb.Append(_value); return true; }

		#region ToNumber

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

		#region ToBoolean

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

		// 常用日期格式的精确匹配快路径：DateTime.TryParse 无格式约束，内部要按多种模式逐个尝试；
		// "M"/"d"/"H"/"m"/"s" 自定义格式符接受 1~2 位数字，可同时覆盖 "2026-1-1" 与 "2026-01-01" 两种写法。
		// 先用 s[4]=='-'/'/' 预判形状再进 TryParseExact，其它形状（如 "1/1/2026"）直接走原路径，
		// 避免非命中格式白付多次 TryParseExact 失败的代价
		private static readonly string[] s_dateDashFormats = {
			"yyyy-M-d H:m:s", "yyyy-M-d H:m", "yyyy-M-dTH:m:s", "yyyy-M-dTH:m", "yyyy-M-d",
		};
		private static readonly string[] s_dateSlashFormats = {
			"yyyy/M/d H:m:s", "yyyy/M/d H:m", "yyyy/M/d",
		};

		internal override bool TryGetDate(out Operand value)
		{
			var span = _value.AsSpan();
			if(span.Length >= 8 && span[4] == '-') {
				if(DateTime.TryParseExact(_value, s_dateDashFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var exact)) {
					value = Create(new MyDate(exact));
					return true;
				}
			}
			else if(span.Length >= 8 && span[4] == '/') {
				if(DateTime.TryParseExact(_value, s_dateSlashFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var exact)) {
					value = Create(new MyDate(exact));
					return true;
				}
			}
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