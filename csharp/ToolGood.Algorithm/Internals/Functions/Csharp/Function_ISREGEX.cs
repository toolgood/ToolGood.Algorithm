using System;
using System.Text;
using System.Text.RegularExpressions;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{
	internal class Function_ISREGEX : Function_2
	{
		public Function_ISREGEX(FunctionBase[] funcs) : base(funcs)
		{
		}

		

		public override string Name => "IsRegex";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(work, tempParameter);
			if(args1.IsError) { return args1; }

			var args2 = GetText_2(work, tempParameter);
			if(args2.IsError) { return args2; }

			var b = Regex.IsMatch(args1.TextValue, args2.TextValue);
			return Operand.Create(b);
		}

	}


}
