using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.Financial
{
	internal sealed class Function_IRR : Function_2
	{
		public Function_IRR(FunctionBase[] funcs) : base(funcs) { }

		public override string Name => "IRR";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var valuesArg = GetArray_1(engine, tempParameter);
			if (valuesArg.IsErrorOrNone) return valuesArg;
			var values = new List<decimal>();
			foreach (var v in valuesArg.ArrayValue) {
				values.Add(v.NumberValue);
			}

			if (values.Count == 0) {
				return ParameterError(1);
			}

			bool hasPositive = false;
			bool hasNegative = false;
			foreach (var v in values) {
				if (v > 0) hasPositive = true;
				if (v < 0) hasNegative = true;
			}
			if (!hasPositive || !hasNegative) {
				return ParameterError(1);
			}

			decimal guess = 0.1m;
			if (func2 != null) {
				var guessArg = GetNumber_2(engine, tempParameter);
				if (guessArg.IsErrorOrNone) return guessArg;
				guess = guessArg.NumberValue;
			}

			var irr = NewtonRaphsonIRR(values, guess);
			return Operand.Create(irr);
		}

		private decimal NewtonRaphsonIRR(List<decimal> values, decimal guess)
		{
			var rate = guess;
			for (int iter = 0; iter < 100; iter++) {
				decimal npv = 0;
				decimal dnpv = 0;

				for (int i = 0; i < values.Count; i++) {
					var factor = (decimal)MathEx.Pow((1 + rate), i);
					npv += values[i] / factor;
					dnpv -= i * values[i] / (factor * (1 + rate));
				}

				if (Math.Abs(dnpv) < 1e-12m) break;
				var newRate = rate - npv / dnpv;

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
			func1.GetParameterTypes(noneEngine, result, OperandType.ARRARY);
			if(func2 != null) func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}
}
