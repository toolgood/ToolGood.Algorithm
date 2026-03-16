using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{
	internal sealed class Function_REMOVESTART : Function_3
	{
		public Function_REMOVESTART(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "RemoveStart";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if(args1.IsErrorOrNone) { return args1; }

			var args2 = GetText_2(engine, tempParameter);
			if(args2.IsErrorOrNone) { return args2; }

			var comparison = StringComparison.Ordinal;
			if(func3 != null) {
				var args3 = GetBoolean_3(engine, tempParameter);
				if(args3.IsErrorOrNone) { return args3; }
				comparison = FunctionUtil.GetStringComparison(args3.BooleanValue);
			}

			var text = args1.TextValue;
			if(text.StartsWith(args2.TextValue, comparison)) {
				return Operand.Create(text.Substring(args2.TextValue.Length));
			}
			return args1;
		}
		public override OperandType GetResultType()
		{
			return OperandType.TEXT;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
			func2.GetParameterTypes(noneEngine, result, OperandType.TEXT);
			if(func3 != null) {
				func3.GetParameterTypes(noneEngine, result, OperandType.BOOLEAN);
			}
		}
	}

}
