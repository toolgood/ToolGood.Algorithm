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
			var args1 = GetText_1(work, tempParameter);
			if(args1.IsError) { return args1; }

			var args2 = GetText_2(work, tempParameter);
			if(args2.IsError) { return args2; }

			var b = Regex.Match(args1.TextValue, args2.TextValue);
			if(b.Success == false) {
				return Operand.Error("Function '{0}' is error!", "Regex");
			}
			return Operand.Create(b.Value);
		}

	}


}
