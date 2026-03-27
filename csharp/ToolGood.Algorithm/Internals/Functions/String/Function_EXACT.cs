using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal sealed class Function_EXACT : Function_2
	{
		public Function_EXACT(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "Exact";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if (args1.IsErrorOrNone) { return args1; }
			var args2 = GetText_2(engine, tempParameter);
			if (args2.IsErrorOrNone) { return args2; }
			return Operand.Create(args1.TextValue == args2.TextValue);
		}
		public override OperandType GetResultType()
		{
			return OperandType.BOOLEAN;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			var t1 = func1.GetResultType();
			var t2 = func2.GetResultType();
			if(t1 == OperandType.NONE) {
				var p = noneEngine.Evaluate(func2).ToText();
				if(t2 != OperandType.ERROR && p.IsErrorOrNone == false) {
					func1.GetParameterTypes(noneEngine, result, OperandType.TEXT, "==", p.TextValue);
					func2.GetParameterTypes(noneEngine, result, OperandType.TEXT);
					return;
				}
			} else if(t2 == OperandType.NONE) {
				var p = noneEngine.Evaluate(func1).ToText();
				if(t1 != OperandType.ERROR && p.IsErrorOrNone == false) {
					func2.GetParameterTypes(noneEngine, result, OperandType.TEXT, "==", p.TextValue);
					func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
					return;
				}
			}
			func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
			func2.GetParameterTypes(noneEngine, result, OperandType.TEXT);
		}
	}

}
