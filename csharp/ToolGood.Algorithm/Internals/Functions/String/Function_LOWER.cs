using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal class Function_LOWER : Function_1
	{
		public Function_LOWER(FunctionBase func1) : base(func1)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter is error!", "Lower"); if (args1.IsError) { return args1; } }
			return Operand.Create(args1.TextValue.ToLower());
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, "Lower");
		}
	}

}
