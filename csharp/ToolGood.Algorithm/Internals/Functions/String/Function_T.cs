using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal class Function_T : Function_1
	{
		public Function_T(FunctionBase func1) : base(func1)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter);
			if (args1.IsText) {
				return args1;
			}
			return Operand.Create("");
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, "T");
		}
	}

}
