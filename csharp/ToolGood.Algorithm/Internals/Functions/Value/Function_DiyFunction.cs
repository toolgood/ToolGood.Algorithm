using System;
using System.Collections.Generic;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Value
{
	internal class Function_DiyFunction : Function_N
	{
		private readonly string funName;

		public Function_DiyFunction(string name, FunctionBase[] funcs) : base(funcs)
		{
			this.funName = name;
		}

		public override string Name => "DiyFunction";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args = new List<Operand>(funcs.Length);
			foreach (var item in funcs) { var aa = item.Evaluate(engine, tempParameter); args.Add(aa); }
			return engine.ExecuteDiyFunction(funName, args);
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, funName);
		}
	}

}
