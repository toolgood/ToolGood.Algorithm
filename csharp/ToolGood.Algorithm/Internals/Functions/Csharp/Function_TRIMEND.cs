using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{
	internal class Function_TRIMEND : Function_2
	{
		public Function_TRIMEND(FunctionBase[] funcs) : base(funcs)
		{
		}

		public Function_TRIMEND(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override string Name => "TrimEnd";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(work, tempParameter);
			if(args1.IsError) { return args1; }

			if(func2 == null) {
				return Operand.Create(args1.TextValue.TrimEnd());
			}

			var args2 = GetText_2(work, tempParameter);
			if(args2.IsError) { return args2; }


			char[] trimChars = args2.TextValue.ToCharArray();
			return Operand.Create(args1.TextValue.TrimEnd(trimChars));
		}

	}
}