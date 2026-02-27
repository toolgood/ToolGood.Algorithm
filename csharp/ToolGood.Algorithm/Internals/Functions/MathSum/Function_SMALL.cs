using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_SMALL : Function_2
	{
		public Function_SMALL(FunctionBase[] funcs) : base(funcs)
		{
		}

		

		public override string Name => "Small";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(engine, tempParameter);
			args1 = ConvertToArray(args1, 1);
			if(args1.IsError) { return args1; }

			var args2 = func2.Evaluate(engine, tempParameter);
			args2 = ConvertToNumber(args2, 2);
			if(args2.IsError) { return args2; }

			var list = new List<decimal>();
			var o = FunctionUtil.F_base_GetList(args1, list);
			if(o == false) { return ParameterError(1); }
			list = list.OrderBy(q => q).ToList();
			int k = args2.IntValue;
			if(k < 1 - engine.ExcelIndex || k > list.Count - engine.ExcelIndex) {
				return ParameterError(2);
			}
			return Operand.Create(list[k - engine.ExcelIndex]);
		}

	}

}
