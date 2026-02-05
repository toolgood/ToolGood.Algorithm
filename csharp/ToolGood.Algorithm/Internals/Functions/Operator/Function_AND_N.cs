using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Operator
{

	internal class Function_AND_N : Function_N
	{
		public Function_AND_N(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "AndN";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var index = 1;
			bool b = true;
			foreach(var item in funcs) {
				var a = item.Evaluate(work, tempParameter);
				a = ConvertToBoolean(a, "AND", index++);
				if(a.IsError) { return a; }
				if(a.BooleanValue == false) b = false;
			}
			return b ? Operand.True : Operand.False;
		}


	}


}