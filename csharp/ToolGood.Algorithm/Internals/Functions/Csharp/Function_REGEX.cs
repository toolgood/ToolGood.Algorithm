using System;
using System.Text;
using System.Text.RegularExpressions;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{
	internal class Function_REGEX : Function_2
	{
		public Function_REGEX(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override string Name => "Regex";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter);
			args1 = FunctionUtil.ConvertToText(args1, "Regex", 1);
			if(args1.IsError) { return args1; }

			var args2 = func2.Evaluate(work, tempParameter);
			args2 = FunctionUtil.ConvertToText(args2, "Regex", 2);
			if(args2.IsError) { return args2; }

			var b = Regex.Match(args1.TextValue, args2.TextValue);
			if(b.Success == false) {
				return Operand.Error("Function '{0}' is error!", "Regex");
			}
			return Operand.Create(b.Value);
		}

	}


}
