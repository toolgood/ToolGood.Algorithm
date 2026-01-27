using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{
	class Function_LOOKFLOOR : Function_2
	{
		public Function_LOOKFLOOR(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter); if(args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "LookFloor", 1); if(args1.IsError) return args1; }
			var args2 = func2.Evaluate(work, tempParameter); if(args2.IsNotArray) { args2 = args2.ToArray("Function '{0}' parameter {1} is error!", "LookFloor", 2); if(args2.IsError) return args2; }

			List<decimal> list = new List<decimal>();
			FunctionUtil.F_base_GetList(args2, list);
			if(list.Count == 0) { return Operand.Error("Function '{0}' parameter {1} is error!", "LookFloor", 2); }
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
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, "LookFloor");
		}
	}

}
