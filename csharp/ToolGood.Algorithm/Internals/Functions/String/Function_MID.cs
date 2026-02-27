using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal class Function_MID : Function_3
	{
		public Function_MID(FunctionBase[] funcs) : base(funcs)
		{
		}

		

		public override string Name => "Mid";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if (args1.IsError) { return args1; }
			var args2 = GetNumber_2(engine, tempParameter);
			if (args2.IsError) { return args2; }
			var args3 = GetNumber_3(engine, tempParameter);
			if (args3.IsError) { return args3; }
			return Operand.Create(args1.TextValue.AsSpan(args2.IntValue - engine.ExcelIndex, args3.IntValue).ToString());
		}

	}

}
