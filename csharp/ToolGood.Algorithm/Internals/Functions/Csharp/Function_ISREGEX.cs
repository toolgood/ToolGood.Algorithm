using System;
using System.Text;
using System.Text.RegularExpressions;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{
	internal class Function_ISREGEX : Function_2
	{
		public Function_ISREGEX(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override string Name => "IsRegex";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter);
			args1 = FunctionUtil.ConvertToText(args1, "IsRegex", 1);
			if(args1.IsError) { return args1; }

			var args2 = func2.Evaluate(work, tempParameter);
			args2 = FunctionUtil.ConvertToText(args2, "IsRegex", 2);
			if(args2.IsError) { return args2; }

			var b = Regex.IsMatch(args1.TextValue, args2.TextValue);
			return Operand.Create(b);
		}

	}


}
