using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{
	internal class Function_REMOVESTART : Function_3
	{
		public Function_REMOVESTART(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
		{
		}

		public override string Name => "RemoveStart";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter);
			args1 = ConvertToText(args1, "RemoveStart", 1);
			if(args1.IsError) { return args1; }

			var args2 = func2.Evaluate(work, tempParameter);
			args2 = ConvertToText(args2, "RemoveStart", 2);
			if(args2.IsError) { return args2; }

			var comparison = StringComparison.Ordinal;
			if(func3 != null) {
				var args3 = func3.Evaluate(work, tempParameter);
				args3 = ConvertToBoolean(args3, "RemoveStart", 3);
				if(args3.IsError) { return args3; }
				comparison = FunctionUtil.GetStringComparison(args3.BooleanValue);
			}

			var text = args1.TextValue;
			if(text.StartsWith(args2.TextValue, comparison)) {
				return Operand.Create(text.AsSpan(args2.TextValue.Length).ToString());
			}
			return args1;
		}

	}


}
