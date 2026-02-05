using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{
	internal class Function_TRIMEND : Function_2
	{
		public Function_TRIMEND(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override string Name => "TrimEnd";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter);
			args1 = FunctionUtil.ConvertToText(args1, "TrimEnd", 1);
			if(args1.IsError) { return args1; }

			if(func2 == null) {
				return Operand.Create(args1.TextValue.TrimEnd());
			}

			var args2 = func2.Evaluate(work, tempParameter);
			args2 = FunctionUtil.ConvertToText(args2, "TrimEnd", 2);
			if(args2.IsError) { return args2; }

			char[] trimChars = args2.TextValue.ToCharArray();
			return Operand.Create(args1.TextValue.TrimEnd(trimChars));
		}

	}


}
