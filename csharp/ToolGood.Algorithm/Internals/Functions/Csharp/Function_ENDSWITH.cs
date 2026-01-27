using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{
	internal class Function_ENDSWITH : Function_3
	{
		public Function_ENDSWITH(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter); if(args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "EndsWith", 1); if(args1.IsError) { return args1; } }
			var args2 = func2.Evaluate(work, tempParameter); if(args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "EndsWith", 2); if(args2.IsError) { return args2; } }
			var text = args1.TextValue;
			if(func3 == null) {
				return Operand.Create(text.AsSpan().EndsWith(args2.TextValue.AsSpan()));
			}
			var args3 = func3.Evaluate(work, tempParameter); if(args3.IsNotBoolean) { args3 = args3.ToBoolean("Function '{0}' parameter {1} is error!", "EndsWith", 3); if(args3.IsError) { return args3; } }
			return Operand.Create(text.AsSpan().EndsWith(args2.TextValue.AsSpan(), args3.BooleanValue ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal));
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, "EndsWith");
		}
	}


}
