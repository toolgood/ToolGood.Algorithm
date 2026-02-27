using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal sealed class Function_LEN : Function_1
	{
		public Function_LEN(FunctionBase func1) : base(func1)
		{
		}

		public override string Name => "Len";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if (args1.IsError) { return args1; }
			return Operand.Create(args1.TextValue.Length);
		}

	}

}
