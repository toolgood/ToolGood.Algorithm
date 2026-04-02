using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{
	internal sealed class Function_STARTSWITH : Function_3
	{
		public Function_STARTSWITH(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length < 2) {
				throw new ArgumentException($"Function '{Name}' requires at least 2 parameters.");
			}
		}

		public override string Name => "StartsWith";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if(args1.IsErrorOrNone) { return args1; }

			var args2 = GetText_2(engine, tempParameter);
			if(args2.IsErrorOrNone) { return args2; }

			var text = args1.TextValue;
			if(func3 == null) {
				return Operand.Create(text.AsSpan().StartsWith(args2.TextValue.AsSpan()));
			}

			var args3 = GetBoolean_3(engine, tempParameter);
			if(args3.IsErrorOrNone) { return args3; }

			return Operand.Create(text.AsSpan().StartsWith(args2.TextValue.AsSpan(), FunctionUtil.GetStringComparison(args3.BooleanValue)));
		}
		public override OperandType GetResultType()
		{
			return OperandType.BOOLEAN;
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
