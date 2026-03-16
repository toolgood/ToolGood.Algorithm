using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{
	internal sealed class Function_SPLIT : Function_2
	{
		public Function_SPLIT(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "Split";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if(args1.IsErrorOrNone) { return args1; }

			var args2 = GetText_2(engine, tempParameter);
			if(args2.IsErrorOrNone) { return args2; }

			if (string.IsNullOrEmpty(args2.TextValue)) {
				return ParameterError(2);
			}

			return Operand.Create(args1.TextValue.Split(args2.TextValue.ToArray()));
		}
		public override OperandType GetResultType()
		{
			return OperandType.ARRAY;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
			func2.GetParameterTypes(noneEngine, result, OperandType.TEXT);
		}
	}

}
