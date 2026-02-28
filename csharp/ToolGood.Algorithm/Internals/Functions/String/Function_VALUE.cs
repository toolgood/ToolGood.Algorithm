using System;
using System.Globalization;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.String
{

	internal sealed class Function_VALUE : Function_1
	{
		public Function_VALUE(FunctionBase func1) : base(func1)
		{
		}

		public override string Name => "Value";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if (args1.IsNumber) { return args1; }
			if (args1.IsBoolean) { return args1.BooleanValue ? Operand.One : Operand.Zero; }
			if (args1.IsError) { return args1; }

			if (decimal.TryParse(args1.TextValue.AsSpan(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
				return Operand.Create(d);
			}
			return FunctionError();
		}

	}

}
