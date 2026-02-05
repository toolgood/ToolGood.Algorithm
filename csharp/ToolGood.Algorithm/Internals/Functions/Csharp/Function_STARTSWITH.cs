using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{
	internal class Function_STARTSWITH : Function_3
	{
		public Function_STARTSWITH(FunctionBase[] funcs) : base(funcs)
		{
		}

		

		public override string Name => "StartsWith";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(work, tempParameter);
			if(args1.IsError) { return args1; }

			var args2 = GetText_2(work, tempParameter);
			if(args2.IsError) { return args2; }

			var text = args1.TextValue;
			if(func3 == null) {
				return Operand.Create(text.AsSpan().StartsWith(args2.TextValue.AsSpan()));
			}

			var args3 = GetBoolean_3(work, tempParameter);
			if(args3.IsError) { return args3; }

			return Operand.Create(text.AsSpan().StartsWith(args2.TextValue.AsSpan(), FunctionUtil.GetStringComparison(args3.BooleanValue)));
		}

	}


}
