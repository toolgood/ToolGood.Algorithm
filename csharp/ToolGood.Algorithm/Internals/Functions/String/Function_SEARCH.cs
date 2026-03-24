using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal sealed class Function_SEARCH : Function_3
	{
		public Function_SEARCH(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "Search";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if(args1.IsErrorOrNone) { return args1; }
			var args2 = GetText_2(engine, tempParameter);
			if(args2.IsErrorOrNone) { return args2; }

			if(func3 == null) {
				var p = args2.TextValue.AsSpan().IndexOf(args1.TextValue, StringComparison.OrdinalIgnoreCase) + engine.ExcelIndex;
				return Operand.Create(p);
			}
			var args3 = GetNumber_3(engine, tempParameter);
			if(args3.IsErrorOrNone) { return args3; }
			var startIndex = args3.IntValue - engine.ExcelIndex;
			if(startIndex < 0 || startIndex >= args2.TextValue.Length) {
				return FunctionError();
			}
			var p2 = args2.TextValue.AsSpan(startIndex).IndexOf(args1.TextValue, StringComparison.OrdinalIgnoreCase);
			if(p2 < 0) {
				return FunctionError();
			}
			return Operand.Create(p2 + startIndex + engine.ExcelIndex);
		}
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
			func2.GetParameterTypes(noneEngine, result, OperandType.TEXT);
			if(func3 != null) {
				func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			}
		}
	}

}
