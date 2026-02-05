using System;
using System.Text;
using System.Text.RegularExpressions;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{
	internal class Function_REGEXREPALCE : Function_3
	{
		public Function_REGEXREPALCE(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
		{
		}

		public override string Name => "RegexReplace";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter);
			args1 = ConvertToText(args1, "RegexReplace", 1);
			if(args1.IsError) { return args1; }

			var args2 = func2.Evaluate(work, tempParameter);
			args2 = ConvertToText(args2, "RegexReplace", 2);
			if(args2.IsError) { return args2; }

			var args3 = func3.Evaluate(work, tempParameter);
			args3 = ConvertToText(args3, "RegexReplace", 3);
			if(args3.IsError) { return args3; }

			var b = Regex.Replace(args1.TextValue, args2.TextValue, args3.TextValue);
			return Operand.Create(b);
		}

	}


}
