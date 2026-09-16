using System;
using System.Globalization;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Value
{
	internal sealed class Function_Number : Function_0
	{
		private readonly Operand _value;

		public Function_Number(Operand value)
		{
			_value = value;
		}

		public override string Name => "Num";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter = null)
		{
			return _value;
		}

		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			// 必须使用 InvariantCulture, 否则在 de-DE 等区域下小数点会输出为逗号, 导致 ToString 结果无法被本引擎重新解析
			stringBuilder.Append(_value.NumberValue.ToString(CultureInfo.InvariantCulture));
		}
	}
}
