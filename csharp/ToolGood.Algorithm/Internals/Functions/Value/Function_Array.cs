using System;
using System.Collections.Generic;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Value
{
	internal class Function_Array : Function_N
	{
		public Function_Array(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args = new List<Operand>();
			foreach (var item in funcs) { var aa = item.Evaluate(work, tempParameter); if (aa.IsError) { return aa; } args.Add(aa); }
			return Operand.Create(args);
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, "Array");
		}
	}

}
