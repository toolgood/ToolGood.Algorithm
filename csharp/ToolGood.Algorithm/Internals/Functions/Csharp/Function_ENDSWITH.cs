using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{
	internal class Function_ENDSWITH : Function_3
	{
		public Function_ENDSWITH(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
		{
		}

		public override string Name => "EndsWith";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter);
			args1 = FunctionUtil.ConvertToText(args1, "EndsWith", 1);
			if(args1.IsError) { return args1; }

			var args2 = func2.Evaluate(work, tempParameter);
			args2 = FunctionUtil.ConvertToText(args2, "EndsWith", 2);
			if(args2.IsError) { return args2; }

			var text = args1.TextValue;
			if(func3 == null) {
				return Operand.Create(text.AsSpan().EndsWith(args2.TextValue.AsSpan()));
			}

			var args3 = func3.Evaluate(work, tempParameter);
			args3 = FunctionUtil.ConvertToBoolean(args3, "EndsWith", 3);
			if(args3.IsError) { return args3; }

			return Operand.Create(text.AsSpan().EndsWith(args2.TextValue.AsSpan(), FunctionUtil.GetStringComparison(args3.BooleanValue)));
		}

	}


}
