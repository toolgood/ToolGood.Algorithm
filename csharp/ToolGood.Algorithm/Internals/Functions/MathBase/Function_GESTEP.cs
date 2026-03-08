using System;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal sealed class Function_GESTEP : Function_N
	{
		public Function_GESTEP(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "GeStep";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetNumber(engine, tempParameter, 0);
			if(args1.IsError) { return args1; }
			var number = args1.NumberValue;

			decimal step = 0;
			if(funcs.Length >= 2) {
				var args2 = GetNumber(engine, tempParameter, 1);
				if(args2.IsError) { return args2; }
				step = args2.NumberValue;
			}

			return Operand.Create(number >= step ? 1 : 0);
		}
		public override OperandType GetRestltType()
		{
			return OperandType.NUMBER;
		}
	}
}
