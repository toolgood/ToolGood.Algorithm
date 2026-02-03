using System;
using System.Globalization;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;
using ToolGood.Algorithm.Internals.Functions;
using ToolGood.Algorithm.LitJson;
using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm
{
	internal sealed class OperandString : Operand
	{
		private readonly string _value;
		public OperandString(string obj)
		{
			_value = obj;
		}

		public override bool IsText => true;
		public override bool IsNotText => false;
		public override OperandType Type => OperandType.TEXT;
		public override string TextValue => _value;

		public override Operand ToNumber(string errorMessage)
		{
			if(TextValue.Contains('.') == false && int.TryParse(TextValue, out int num)) {
				return Operand.Create(num);
			}
			if(decimal.TryParse(TextValue, out decimal d)) {
				return Operand.Create(d);
			}
			if(errorMessage == null) {
				return Error("Convert to number error!");
			}
			return Error(errorMessage);
		}
		public override Operand ToNumber(string errorMessage, params object[] args)
		{
			if(TextValue.Contains('.') == false && int.TryParse(TextValue, out int num)) {
				return Operand.Create(num);
			}
			if(decimal.TryParse(TextValue, out decimal d)) {
				return Operand.Create(d);
			}
			if(errorMessage == null) {
				return Error("Convert to number error!");
			}
			return Error(string.Format(errorMessage, args));
		}


		public override Operand ToText(string errorMessage) { return this; }
		public override Operand ToText(string errorMessage, params object[] args) { return this; }

		public override Operand ToBoolean(string errorMessage)
		{
			if(FunctionUtil.TryParseBoolean(TextValue, out bool b)) {
				return b ? Operand.True : Operand.False;
			}
			if(errorMessage == null) {
				return Error("Convert to bool error!");
			}
			return Error(errorMessage);
		}
		public override Operand ToBoolean(string errorMessage, params object[] args)
		{
			if(FunctionUtil.TryParseBoolean(TextValue, out bool b)) {
				return b ? Operand.True : Operand.False;
			}
			if(errorMessage == null) {
				return Error("Convert to bool error!");
			}
			return Error(string.Format(errorMessage, args));
		}


		public override Operand ToMyDate(string errorMessage)
		{
			if(TimeSpan.TryParse(TextValue, CultureInfo.InvariantCulture, out TimeSpan t)) { return Create(new MyDate(t)); }
			if(DateTime.TryParse(TextValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime d)) { return Create(new MyDate(d)); }
			if(errorMessage == null) {
				return Error("Convert to date error!");
			}
			return Error(errorMessage);
		}
		public override Operand ToMyDate(string errorMessage, params object[] args)
		{
			if(TimeSpan.TryParse(TextValue, CultureInfo.InvariantCulture, out TimeSpan t)) { return Create(new MyDate(t)); }
			if(DateTime.TryParse(TextValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime d)) { return Create(new MyDate(d)); }
			if(errorMessage == null) {
				return Error("Convert to date error!");
			}
			return Error(string.Format(errorMessage, args));
		}

		public override Operand ToArray(string errorMessage)
		{
			return Error(errorMessage ?? "Convert to array error!");
		}
		public override Operand ToJson(string errorMessage = null)
		{
			var txt = this.TextValue.Trim();
			if((txt.StartsWith('{') && txt.EndsWith('}')) || (txt.StartsWith('[') && txt.EndsWith(']'))) {
				try {
					var json = JsonMapper.ToObject(txt);
					return Operand.Create(json);
				} catch(Exception) { }
			}
			return Error(errorMessage ?? "Convert to json error!");
		}


		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append('"');
			foreach(char c in _value) {
				switch(c) {
					case '"': sb.Append("\\\""); break;
					case '\n': sb.Append("\\n"); break;
					case '\r': sb.Append("\\r"); break;
					case '\t': sb.Append("\\t"); break;
					case '\0': sb.Append("\\0"); break;
					case '\v': sb.Append("\\v"); break;
					case '\a': sb.Append("\\a"); break;
					case '\b': sb.Append("\\b"); break;
					case '\f': sb.Append("\\f"); break;
					default: sb.Append(c); break;
				}
			}
			sb.Append('"');
			return sb.ToString();
		}
	}
}