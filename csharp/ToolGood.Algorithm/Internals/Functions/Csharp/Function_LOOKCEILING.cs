using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{


	class Function_LOOKCEILING : Function_2
	{
		public Function_LOOKCEILING(FunctionBase[] funcs) : base(funcs)
		{
		}

		

		public override string Name => "LookCeiling";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetNumber_1(engine, tempParameter);
			if(args1.IsError) { return args1; }

			var args2 = GetArray_2(engine, tempParameter);
			if(args2.IsError) { return args2; }

			List<decimal> list = new List<decimal>();
			FunctionUtil.F_base_GetList(args2, list);
			if(list.Count == 0) { return ParameterError(2); }
			list.Sort();
			var value = args1.NumberValue;
			var result = list[list.Count - 1];
			if(result == value) { return args1; }
			for(int i = list.Count - 2; i >= 0; i--) {
				var val = list[i];
				if(val > value) {
					result = val;
				} else if(val == value) {
					return args1;
				} else {
					break;
				}
			}
			return Operand.Create(result);
		}

	}

}
