using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{
	class Function_LOOKFLOOR : Function_2
	{
		public Function_LOOKFLOOR(FunctionBase[] funcs) : base(funcs)
		{
		}

		

		public override string Name => "LookFloor";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetNumber_1(work, tempParameter);
			if(args1.IsError) { return args1; }

			var args2 = GetArray_2(work, tempParameter);
			if(args2.IsError) { return args2; }

			List<decimal> list = new List<decimal>();
			FunctionUtil.F_base_GetList(args2, list);
			if(list.Count == 0) { return ParameterError(2); }
			list = list.OrderBy(n => n).ToList();
			var value = args1.NumberValue;
			var result = list[0];
			if(result == value) { return args1; }
			for(int i = 1; i < list.Count; i++) {
				var val = list[i];
				if(val < value) {
					result = val;
				} else if(val == value) {
					return args1;
				} else /*if(val > value)*/ {
					break;
				}
			}
			return Operand.Create(result);
		}

	}

}
