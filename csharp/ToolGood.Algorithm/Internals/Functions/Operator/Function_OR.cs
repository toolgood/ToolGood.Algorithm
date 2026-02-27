using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Operator
{
	internal sealed class Function_OR : Function_2
	{
		public Function_OR(FunctionBase[] funcs) : base(funcs)
		{
		}

		public Function_OR(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override string Name => "Or";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			// 程序 && and || or �?excel�? AND(x,y) OR(x,y) 有区�?
			// 在excel�?AND(x,y) OR(x,y) 先报错，
			// 在程序中�?& and  有true 直接返回true 就不会检测下一个会不会报错
			// 在程序中，|| or  有false 直接返回false 就不会检测下一个会不会报错
			var args1 = GetBoolean_1(engine, tempParameter);
			if (args1.IsError) { return args1; }
			if(args1.BooleanValue) {
				var args2 = GetBoolean_2(engine, tempParameter);
				if(args2.IsError) { return args2; }
				return Operand.True;
			}
			return GetBoolean_2(engine, tempParameter);
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