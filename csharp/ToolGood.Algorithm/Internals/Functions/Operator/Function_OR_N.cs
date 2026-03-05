using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Operator
{

	internal sealed class Function_OR_N : Function_N
	{
		public Function_OR_N(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "OrN";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			bool b = false;
			for(int i = 0; i < funcs.Length; i++) {
				var a = GetBoolean(engine, tempParameter, i);
				if(a.IsError) { return a; }
				if(a.BooleanValue) b = true;
			}
			return b ? Operand.True : Operand.False;
		}


	}
}