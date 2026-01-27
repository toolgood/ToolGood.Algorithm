using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal class Function_EXACT : Function_2
	{
		public Function_EXACT(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "EXACT", 1); if (args1.IsError) { return args1; } }
			var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "EXACT", 2); if (args2.IsError) { return args2; } }
			return Operand.Create(args1.TextValue == args2.TextValue);
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, "Exact");
		}
	}

}
