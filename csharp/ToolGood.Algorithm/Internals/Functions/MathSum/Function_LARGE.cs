using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal sealed class Function_LARGE : Function_2
	{
		public Function_LARGE(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "Large";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(engine, tempParameter);
			args1 = ConvertToArray(args1, 1);
			if(args1.IsErrorOrNone) { return args1; }

			var args2 = func2.Evaluate(engine, tempParameter);
			args2 = ConvertToNumber(args2, 2);
			if(args2.IsErrorOrNone) { return args2; }

			var list = new List<decimal>();
			var o = FunctionUtil.FlattenToList(args1, list);
			if(o == false) { return ParameterError(1); }
			if(list.Count == 0) { return ParameterError(1); }

			int k = args2.IntValue - engine.ExcelIndex;
			if(k < 0 || k >= list.Count) {
				return ParameterError(2);
			}
			return Operand.Create(FunctionUtil.QuickSelect(list, k, true));
		}
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.ARRAY);
			func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}

}
