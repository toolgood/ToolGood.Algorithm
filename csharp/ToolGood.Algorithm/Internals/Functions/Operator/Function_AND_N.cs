using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Operator
{

	internal sealed class Function_AND_N : Function_N
	{
		public Function_AND_N(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "AndN";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			bool b = true;
			for(int i = 0; i < funcs.Length; i++) {
				var a = GetBoolean(engine, tempParameter, i);
				if(a.IsError) { return a; }
				if(a.BooleanValue == false) b = false;
			}
			return b ? Operand.True : Operand.False;
		}


	}
}