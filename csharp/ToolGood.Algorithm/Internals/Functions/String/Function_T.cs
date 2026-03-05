using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal sealed class Function_T : Function_1
	{
		public Function_T(FunctionBase func1) : base(func1)
		{
		}

		public override string Name => "T";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(engine, tempParameter);
			if (args1.IsText) {
				return args1;
			}
			return Operand.Create("");
		}

	}

}
