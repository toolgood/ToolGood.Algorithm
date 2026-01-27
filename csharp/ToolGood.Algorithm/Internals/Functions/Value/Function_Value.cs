using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Value
{
	internal class Function_Value : FunctionBase
	{
		private readonly Operand _value;
		private readonly string _showName;

		public Function_Value(Operand value)
		{
			_value = value;
		}
		public Function_Value(Operand value, string showName)
		{
			_value = value;
			_showName = showName;
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			return _value;
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			if (string.IsNullOrEmpty(_showName) == false) {
				stringBuilder.Append(_showName);
				return;
			}
			if (_value.IsText) {
				stringBuilder.Append('"');
				var stringValue = _value.TextValue;
				stringValue = stringValue.Replace("\\", "\\\\");
				stringValue = stringValue.Replace("\r", "\\r");
				stringValue = stringValue.Replace("\n", "\\n");
				stringValue = stringValue.Replace("\t", "\\t");
				stringValue = stringValue.Replace("\0", "\\0");
				stringValue = stringValue.Replace("\v", "\\v");
				stringValue = stringValue.Replace("\a", "\\a");
				stringValue = stringValue.Replace("\b", "\\b");
				stringValue = stringValue.Replace("\f", "\\f");
				stringValue = stringValue.Replace("\"", "\\\"");
				stringBuilder.Append(stringValue);
				stringBuilder.Append('"');
			} else if (_value.IsDate) {
				stringBuilder.Append('"');
				stringBuilder.Append(_value.DateValue.ToString());
				stringBuilder.Append('"');
			} else if (_value.IsBoolean) {
				stringBuilder.Append(_value.BooleanValue ? "true" : "false");
			} else {
				stringBuilder.Append(_value.ToString());
			}
		}
	}

}
