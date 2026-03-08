using System;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Operator
{
	internal sealed class Function_XOR : Function_N
	{
		public Function_XOR(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "Xor";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			int trueCount = 0;
			for(int i = 0; i < funcs.Length; i++) {
				var a = GetBoolean(engine, tempParameter, i);
				if(a.IsError) { return a; }
				if(a.BooleanValue) trueCount++;
			}
			return (trueCount % 2 == 1) ? Operand.True : Operand.False;
		}
		public override OperandType GetRestltType()
		{
			return OperandType.BOOLEAN;
		}
	}
}
