using System;
using System.Text;
using System.Text.RegularExpressions;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{
	internal class Function_REGEXREPALCE : Function_3
	{
		public Function_REGEXREPALCE(FunctionBase[] funcs) : base(funcs)
		{
		}

		public Function_REGEXREPALCE(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
		{
		}

		public override string Name => "RegexReplace";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(work, tempParameter);
			if(args1.IsError) { return args1; }

			var args2 = GetText_2(work, tempParameter);
			if(args2.IsError) { return args2; }

			var args3 = GetText_3(work, tempParameter);
			if(args3.IsError) { return args3; }

			var b = Regex.Replace(args1.TextValue, args2.TextValue, args3.TextValue);
			return Operand.Create(b);
		}

	}


}
