using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

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
			if (xArg.IsError) return xArg;
			var x = xArg.NumberValue;

			var nArg = GetNumber(engine, tempParameter, 1);
			if (nArg.IsError) return nArg;
			var n = nArg.NumberValue;

			var mArg = GetNumber(engine, tempParameter, 2);
			if (mArg.IsError) return mArg;
			var m = mArg.NumberValue;

			var coefficientsArg = GetArray(engine, tempParameter, 3);
			if (coefficientsArg.IsError) return coefficientsArg;

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
		public override OperandType GetRestltType()
		{
			return OperandType.NUMBER;
		}
	}
}
