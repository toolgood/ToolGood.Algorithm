using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{
	internal sealed class Function_TRIMEND : Function_2
	{
		public Function_TRIMEND(FunctionBase[] funcs) : base(funcs)
		{
		}

		

		public override string Name => "TrimEnd";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if(args1.IsError) { return args1; }

			if(func2 == null) {
				return Operand.Create(args1.TextValue.TrimEnd());
			}

			var args2 = GetText_2(engine, tempParameter);
			if(args2.IsError) { return args2; }


			char[] trimChars = args2.TextValue.ToCharArray();
			return Operand.Create(args1.TextValue.TrimEnd(trimChars));
		}

	}
}