using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal class Function_CHAR : Function_1
	{
		public Function_CHAR(FunctionBase func1) : base(func1)
		{
		}

		public override string Name => "Char";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetNumber_1(engine, tempParameter);
			if (args1.IsError) { return args1; }
			char c = (char)args1.IntValue;
			return Operand.Create(new string(c, 1));
		}

	}

}
