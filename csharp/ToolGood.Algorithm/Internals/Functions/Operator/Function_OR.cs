using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Operator
{
	internal class Function_OR : Function_2
	{
		public Function_OR(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override string Name => "Or";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			// 程序 && and || or 与 excel的  AND(x,y) OR(x,y) 有区别
			// 在excel内 AND(x,y) OR(x,y) 先报错，
			// 在程序中，&& and  有true 直接返回true 就不会检测下一个会不会报错
			// 在程序中，|| or  有false 直接返回false 就不会检测下一个会不会报错
			var args1 = func1.Evaluate(work, tempParameter);
			args1 = FunctionUtil.ConvertToBoolean(args1, "OR", 1);
			if (args1.IsError) { return args1; }
			if(args1.BooleanValue) {
				var args2 = FunctionUtil.ConvertToBoolean(func2.Evaluate(work, tempParameter), "OR", 2);
				if(args2.IsError) { return args2; }
				return Operand.True;
			}
			return FunctionUtil.ConvertToBoolean(func2.Evaluate(work, tempParameter), "OR", 2);
		}

		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			if(addBrackets) stringBuilder.Append('(');
			func1.ToString(stringBuilder, false);
			stringBuilder.Append(" || ");
			func2.ToString(stringBuilder, false);
			if(addBrackets) stringBuilder.Append(')');
		}
	}
}