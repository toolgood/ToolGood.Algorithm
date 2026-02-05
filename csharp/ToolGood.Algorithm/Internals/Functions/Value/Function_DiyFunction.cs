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

		public override string Name => funName;

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args = new List<Operand>();
			foreach (var item in funcs) { var aa = item.Evaluate(work, tempParameter); args.Add(aa); }
			return work.ExecuteDiyFunction(funName, args);
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, funName);
		}
	}

}
