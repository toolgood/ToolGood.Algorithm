using System;
using System.Globalization;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.String
{

	internal class Function_VALUE : Function_1
	{
		public Function_VALUE(FunctionBase func1) : base(func1)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter);
			if (args1.IsNumber) { return args1; }
			if (args1.IsBoolean) { return args1.BooleanValue ? Operand.One : Operand.Zero; }
			if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter is error!", "Value"); if (args1.IsError) { return args1; } }

			if (decimal.TryParse(args1.TextValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
				return Operand.Create(d);
			}
			return Operand.Error("Function '{0}' parameter is error!", "Value");
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, "Value");
		}
	}

}
