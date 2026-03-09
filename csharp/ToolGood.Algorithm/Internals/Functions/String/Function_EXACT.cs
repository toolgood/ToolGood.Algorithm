using System;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal sealed class Function_EXACT : Function_2
	{
		public Function_EXACT(FunctionBase[] funcs) : base(funcs)
		{
		}

		

		public override string Name => "Exact";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if (args1.IsErrorOrNone) { return args1; }
			var args2 = GetText_2(engine, tempParameter);
			if (args2.IsErrorOrNone) { return args2; }
			return Operand.Create(args1.TextValue == args2.TextValue);
		}
		public override OperandType GetResultType()
		{
			return OperandType.BOOLEAN;
		}
	}

}
