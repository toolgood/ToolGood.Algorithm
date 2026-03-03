using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal sealed class Function_UNICHAR : Function_1
	{
		public Function_UNICHAR(FunctionBase func1) : base(func1)
		{
		}

		public override string Name => "UniChar";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetNumber_1(engine, tempParameter);
			if (args1.IsError) { return args1; }
			try {
				return Operand.Create(char.ConvertFromUtf32((int)args1.DoubleValue));
			} catch {
				return FunctionError();
			}
		}
	}
}
