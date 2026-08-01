using System;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Value
{
	internal sealed class Function_ValueText : Function_0
	{
		private readonly Operand _value;

		public Function_ValueText(Operand value)
		{
			_value = value;
		}
		 
		public override string Name => "Value";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			return _value;
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			stringBuilder.Append('"');
			// 单遍转义,避免链式 Replace 多次创建中间字符串
			foreach(var c in _value.TextValue) {
				switch(c) {
					case '\\': stringBuilder.Append("\\\\"); break;
					case '"': stringBuilder.Append("\\\""); break;
					case '\r': stringBuilder.Append("\\r"); break;
					case '\n': stringBuilder.Append("\\n"); break;
					case '\t': stringBuilder.Append("\\t"); break;
					case '\0': stringBuilder.Append("\\0"); break;
					case '\v': stringBuilder.Append("\\v"); break;
					case '\a': stringBuilder.Append("\\a"); break;
					case '\b': stringBuilder.Append("\\b"); break;
					case '\f': stringBuilder.Append("\\f"); break;
					default: stringBuilder.Append(c); break;
				}
			}
			stringBuilder.Append('"');
		}
		public override OperandType GetResultType()
		{
			return OperandType.TEXT;
		}
	}

}
