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

		public override string Name => "Array";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args = new List<Operand>(funcs.Length);
			foreach (var item in funcs) { var aa = item.Evaluate(engine, tempParameter); if (aa.IsError) { return aa; } args.Add(aa); }
			return Operand.Create(args);
		}

	}

}
