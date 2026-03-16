using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal sealed class Function_SERIESSUM : Function_N
	{
		public Function_SERIESSUM(FunctionBase[] funcs) : base(funcs) { }

		public override string Name => "SERIESSUM";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			if (funcs.Length < 4) return ParameterError(1);

			var xArg = GetNumber(engine, tempParameter, 0);
			if (xArg.IsErrorOrNone) return xArg;
			var x = xArg.NumberValue;

			var nArg = GetNumber(engine, tempParameter, 1);
			if (nArg.IsErrorOrNone) return nArg;
			var n = nArg.NumberValue;

			var mArg = GetNumber(engine, tempParameter, 2);
			if (mArg.IsErrorOrNone) return mArg;
			var m = mArg.NumberValue;

			var coefficientsArg = GetArray(engine, tempParameter, 3);
			if (coefficientsArg.IsErrorOrNone) return coefficientsArg;

			decimal result = 0;
			int i = 0;
			foreach (var coef in coefficientsArg.ArrayValue) {
				if (coef.IsNumber) {
					var power = n + i * m;
					result += coef.NumberValue * MathEx.Pow(x, power);
					i++;
				}
			}

			return Operand.Create(result);
		}
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			funcs[0].GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			funcs[1].GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			funcs[2].GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			funcs[3].GetParameterTypes(noneEngine, result, OperandType.ARRAY);
		}
	}
}
