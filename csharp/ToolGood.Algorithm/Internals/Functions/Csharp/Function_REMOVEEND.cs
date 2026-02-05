using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{
	internal class Function_REMOVEEND : Function_3
	{
		public Function_REMOVEEND(FunctionBase[] funcs) : base(funcs)
		{
		}

		public Function_REMOVEEND(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
		{
		}

		public override string Name => "RemoveEnd";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(work, tempParameter);
			if(args1.IsError) { return args1; }

			var args2 = GetText_2(work, tempParameter);
			if(args2.IsError) { return args2; }

			var comparison = StringComparison.Ordinal;
			if(func3 != null) {
				var args3 = GetBoolean_3(work, tempParameter);
				if(args3.IsError) { return args3; }
				comparison = FunctionUtil.GetStringComparison(args3.BooleanValue);
			}

			var text = args1.TextValue;
			if(text.EndsWith(args2.TextValue, comparison)) {
				return Operand.Create(text.AsSpan(0, text.Length - args2.TextValue.Length).ToString());
			}
			return args1;
		}

	}


}
