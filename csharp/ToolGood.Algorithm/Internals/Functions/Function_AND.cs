using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions
{

	internal class Function_AND : Function_2
	{
		public Function_AND(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			// 程序 && and || or 与 excel的  AND(x,y) OR(x,y) 有区别
			// 在excel内 AND(x,y) OR(x,y) 先报错，
			// 在程序中，&& and  有true 直接返回true 就不会检测下一个会不会报错
			// 在程序中，|| or  有false 直接返回false 就不会检测下一个会不会报错
			var args1 = func1.Evaluate(work, tempParameter); if(args1.IsNotBoolean) { args1 = args1.ToBoolean(); if(args1.IsError) { return args1; } }
			if(args1.BooleanValue == false) {
				var args2 = func2.Evaluate(work, tempParameter).ToBoolean();
				if(args2.IsError) { return args2; }
				return Operand.False;
			}
			return func2.Evaluate(work, tempParameter).ToBoolean();
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			if(addBrackets) stringBuilder.Append('(');
			func1.ToString(stringBuilder, false);
			stringBuilder.Append(" && ");
			func2.ToString(stringBuilder, false);
			if(addBrackets) stringBuilder.Append(')');
		}
	}


}