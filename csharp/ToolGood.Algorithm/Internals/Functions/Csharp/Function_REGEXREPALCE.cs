using System;
using System.Text;
using System.Text.RegularExpressions;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{
	internal sealed class Function_REGEXREPALCE : Function_3
	{
		public Function_REGEXREPALCE(FunctionBase[] funcs) : base(funcs)
		{
		}

		

		public override string Name => "RegexReplace";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if(args1.IsError) { return args1; }

			var args2 = GetText_2(engine, tempParameter);
			if(args2.IsError) { return args2; }

			var args3 = GetText_3(engine, tempParameter);
			if(args3.IsError) { return args3; }

			var b = Regex.Replace(args1.TextValue, args2.TextValue, args3.TextValue);
			return Operand.Create(b);
		}

	}


}
