using System;
using System.Globalization;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal sealed class Function_TEXT : Function_2
	{
		public Function_TEXT(FunctionBase[] funcs) : base(funcs)
		{
		}

		

		public override string Name => "Text";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(engine, tempParameter); if(args1.IsError) { return args1; }
			var args2 = GetText_2(engine, tempParameter);
			if(args2.IsError) { return args2; }

			if(args1.IsText) {
				return args1;
			} else if(args1.IsBoolean) {
				return Operand.Create(args1.BooleanValue ? "TRUE" : "FALSE");
			} else if(args1.IsNumber) {
				return Operand.Create(args1.NumberValue.ToString(args2.TextValue, CultureInfo.InvariantCulture));
			} else if(args1.IsDate) {
				return Operand.Create(args1.DateValue.ToString(args2.TextValue));
			}
			args1 = ConvertToText(args1, 1);
			if(args1.IsError) { return args1; }
			return Operand.Create(args1.TextValue.ToString());
		}


	}

}
