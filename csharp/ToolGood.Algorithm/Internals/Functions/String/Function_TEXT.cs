using System;
using System.Globalization;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal class Function_TEXT : Function_2
	{
		public Function_TEXT(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter); if (args1.IsError) { return args1; }
			var args2 = func2.Evaluate(work, tempParameter);
			args2 = FunctionUtil.ConvertToText(args2, "Text", 2);
			if (args2.IsError) { return args2; }

			if (args1.IsText) {
				return args1;
			} else if (args1.IsBoolean) {
				return Operand.Create(args1.BooleanValue ? "TRUE" : "FALSE");
			} else if (args1.IsNumber) {
				return Operand.Create(args1.NumberValue.ToString(args2.TextValue, CultureInfo.InvariantCulture));
			} else if (args1.IsDate) {
				return Operand.Create(args1.DateValue.ToString(args2.TextValue));
			}
			args1 = FunctionUtil.ConvertToText(args1, "Text", 1);
			if (args1.IsError) { return args1; }
			return Operand.Create(args1.TextValue.ToString());
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, "Text");
		}

	}

}
