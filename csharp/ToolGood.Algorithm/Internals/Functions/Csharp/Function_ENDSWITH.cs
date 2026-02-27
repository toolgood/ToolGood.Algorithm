using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{
	internal class Function_ENDSWITH : Function_3
	{
		public Function_ENDSWITH(FunctionBase[] funcs) : base(funcs)
		{
		}

		

		public override string Name => "EndsWith";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if(args1.IsError) { return args1; }

			var args2 = GetText_2(engine, tempParameter);
			if(args2.IsError) { return args2; }

			var text = args1.TextValue;
			if(func3 == null) {
				return Operand.Create(text.AsSpan().EndsWith(args2.TextValue.AsSpan()));
			}

			var args3 = GetBoolean_3(engine, tempParameter);
			if(args3.IsError) { return args3; }

			return Operand.Create(text.AsSpan().EndsWith(args2.TextValue.AsSpan(), FunctionUtil.GetStringComparison(args3.BooleanValue)));
		}

	}


}
