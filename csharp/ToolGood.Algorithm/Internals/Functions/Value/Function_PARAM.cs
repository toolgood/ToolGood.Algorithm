using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Value
{
	internal sealed class Function_PARAM : Function_2
	{
		public Function_PARAM(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length < 1 || funcs.Length > 2) {
				throw new ArgumentException($"Function '{Name}' requires 1 to 2 parameters.");
			}
		}

		

		public override string Name => "Param";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if (args1.IsErrorOrNone) { return args1; }
			if (tempParameter != null) {
				var r = tempParameter(engine, args1.TextValue);
				if (r != null) return r;
			}
			var result = engine.GetParameter(args1.TextValue);
			if (result.IsErrorOrNone) {
				if (func2 != null) {
					return func2.Evaluate(engine, tempParameter);
				}
			}
			return result;
		}
		public override OperandType GetResultType()
		{
			return OperandType.NONE;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
			if(func2 != null) {
				func2.GetParameterTypes(noneEngine, result, OperandType.NONE);
			}
		}
	}

}
