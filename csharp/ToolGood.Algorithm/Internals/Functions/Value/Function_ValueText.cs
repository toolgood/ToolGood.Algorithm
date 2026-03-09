using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.Value
{
	internal sealed class Function_ValueText : FunctionBase
	{
		private readonly Operand _value;
		private readonly string _showName;

		public Function_ValueText(Operand value)
		{
			_value = value;
		}
		public Function_ValueText(Operand value, string showName)
		{
			_value = value;
			_showName = showName;
		}

		public override string Name => "Value";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			return _value;
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			if(string.IsNullOrEmpty(_showName) == false) {
				stringBuilder.Append(_showName);
				return;
			}
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
		}
		public override OperandType GetResultType()
		{
			return OperandType.TEXT;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
		}
	}

}
