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

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter); if(args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "Regex", 1); if(args1.IsError) { return args1; } }
			var args2 = func2.Evaluate(work, tempParameter); if(args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "Regex", 2); if(args2.IsError) { return args2; } }

			var b = Regex.Match(args1.TextValue, args2.TextValue);
			if(b.Success == false) {
				return Operand.Error("Function 'Regex' is error!");
			}
			return Operand.Create(b.Value);
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, "Regex");
		}
	}


}
