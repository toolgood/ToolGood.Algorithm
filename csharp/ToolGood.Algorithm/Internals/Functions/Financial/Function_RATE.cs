using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Financial
{
	internal sealed class Function_RATE : Function_6
	{
		public Function_RATE(FunctionBase[] funcs) : base(funcs) {
			if (funcs.Length < 3 || funcs.Length > 6) {
				throw new ArgumentException($"Function '{Name}' requires 3 to 6 parameters.");
			}
		}

		public override string Name => "RATE";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var nperArg = GetNumber_1(engine, tempParameter);
			if (nperArg.IsErrorOrNone) return nperArg;
			var nper = nperArg.NumberValue;

			var pmtArg = GetNumber_2(engine, tempParameter);
			if (pmtArg.IsErrorOrNone) return pmtArg;
			var pmt = pmtArg.NumberValue;

			var pvArg = GetNumber_3(engine, tempParameter);
			if (pvArg.IsErrorOrNone) return pvArg;
			var pv = pvArg.NumberValue;

			if (nper <= 0) {
				return ParameterError(1);
			}

			decimal fv = 0;
			if (func4 != null) {
				var fvArg = GetNumber_4(engine, tempParameter);
				if (fvArg.IsErrorOrNone) return fvArg;
				fv = fvArg.NumberValue;
			}

			int type = 0;
			if (func5 != null) {
				var typeArg = GetNumber_5(engine, tempParameter);
				if (typeArg.IsErrorOrNone) return typeArg;
				type = typeArg.IntValue;
				if (type != 0 && type != 1) {
					return ParameterError(5);
				}
			}

			decimal guess = 0.1m;
			if (func6 != null) {
				var guessArg = GetNumber_6(engine, tempParameter);
				if (guessArg.IsErrorOrNone) return guessArg;
				guess = guessArg.NumberValue;
			}

			try {
				// 牛顿迭代中 rate=0 除零、MathEx.Pow 负底/溢出会抛异常,捕获并返回错误
				var rate = NewtonRaphson(nper, pmt, pv, fv, type, guess);
				return Operand.Create(rate);
			} catch (Exception ex) when (ex is OverflowException || ex is DivideByZeroException || ex is InvalidOperationException) {
				return FunctionError();
			}
		}

		private decimal NewtonRaphson(decimal n, decimal p, decimal v, decimal f, decimal type, decimal rate)
		{
			for (int i = 0; i < 100; i++) {
				decimal onePlusRate = 1 + rate;
				decimal factor = MathEx.Pow(onePlusRate, n);
				decimal powNMinus1 = MathEx.Pow(onePlusRate, n - 1);
				decimal fn;
				
				if (type == 1) {
					fn = v * factor + p * onePlusRate * (factor - 1) / rate + f;
				} else {
					fn = v * factor + p * (factor - 1) / rate + f;
				}

				decimal dfn;
				if (type == 1) {
					dfn = v * n * powNMinus1 + 
						  p * onePlusRate * (n * rate * powNMinus1 - (factor - 1)) / (rate * rate) +
						  p * (factor - 1) / rate;
				} else {
					dfn = v * n * powNMinus1 + 
						  p * (n * rate * powNMinus1 - (factor - 1)) / (rate * rate);
				}

				if (Math.Abs(dfn) < 1e-12m) break;
				var newRate = rate - fn / dfn;

				if (Math.Abs(newRate - rate) < 1e-10m) {
					return newRate;
				}
				rate = newRate;
			}
			return rate;
		}
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			if(func4 != null) func4.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			if(func5 != null) func5.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			if(func6 != null) func6.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}
}
