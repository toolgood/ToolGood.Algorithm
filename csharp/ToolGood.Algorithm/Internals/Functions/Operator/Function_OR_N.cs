using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Operator
{

	internal class Function_OR_N : Function_N
	{
		public Function_OR_N(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var index = 1;
			bool b = false;
			foreach(var item in funcs) {
				var a = item.Evaluate(work, tempParameter);
				if(a.IsNotBoolean) { a = a.ToBoolean("Function '{0}' parameter {1} is error!", "OR", index++); if(a.IsError) { return a; } }
				if(a.BooleanValue) b = true;
			}
			return b ? Operand.True : Operand.False;
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, "OR");
		}
	}


}