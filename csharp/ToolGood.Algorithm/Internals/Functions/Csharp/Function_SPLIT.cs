using System;
using System.Linq;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{
	internal sealed class Function_SPLIT : Function_2
	{
		public Function_SPLIT(FunctionBase[] funcs) : base(funcs)
		{
		}

		

		public override string Name => "Split";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if(args1.IsError) { return args1; }

			var args2 = GetText_2(engine, tempParameter);
			if(args2.IsError) { return args2; }

			return Operand.Create(args1.TextValue.Split(args2.TextValue.ToArray()));
		}

	}


}
