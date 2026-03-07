using System;
using System.Text;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum2
{
	internal sealed class Function_WEIBULL : Function_4
	{
		public Function_WEIBULL(FunctionBase[] funcs) : base(funcs)
		{
		}



		public override string Name => "Weibull";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetNumber_1(engine, tempParameter);
			if(args1.IsError) return args1;

			var args2 = GetNumber_2(engine, tempParameter);
			if(args2.IsError) return args2;

			var args3 = GetNumber_3(engine, tempParameter);
			if(args3.IsError) return args3;

			var args4 = GetBoolean_4(engine, tempParameter);
			if(args4.IsError) return args4;
			var x = args1.NumberValue;
			var shape = args2.NumberValue;
			var scale = args3.NumberValue;
			var state = args4.BooleanValue;
			if(shape <= 0.0m) {
				return ParameterError(2);
			}
			if(scale <= 0.0m) {
				return ParameterError(3);
			}

			return Operand.Create(Weibull(x, shape, scale, state));
		}

		public decimal Weibull(decimal x, decimal shape, decimal scale, bool state)
		{
			if(state == false) {
				return PDF(shape, scale, x);
			}
			return CDF(shape, scale, x);
		}

		public decimal PDF(decimal shape, decimal scale, decimal x)
		{

			if(x >= 0.0m) {
				if(x == 0.0m && shape == 1.0m) {
					return shape / scale;
				}

				return shape
					   * MathEx.Pow(x / scale, shape - 1.0m)
					   * MathEx.Exp(-MathEx.Pow(x, shape) * MathEx.Pow(scale, -shape))
					   / scale;
			}
			return 0.0m;
		}

		/// <summary>
		/// Computes the cumulative distribution (CDF) of the distribution at x, i.e. P(X ≤ x).
		/// </summary>
		/// <param name="x">The location at which to compute the cumulative distribution function.</param>
		/// <param name="shape">The shape (k) of the Weibull distribution. Range: k > 0.</param>
		/// <param name="scale">The scale (λ) of the Weibull distribution. Range: λ > 0.</param>
		/// <returns>the cumulative distribution at location <paramref name="x"/>.</returns>
		///// <seealso cref="CumulativeDistribution"/>
		public decimal CDF(decimal shape, decimal scale, decimal x)
		{
			if(x < 0.0m) {
				return 0.0m;
			}

			return -ExponentialMinusOne(-MathEx.Pow(x, shape) * MathEx.Pow(scale, -shape));
		}
		public decimal ExponentialMinusOne(decimal power)
		{
			decimal x = Math.Abs(power);
			if(x > 0.1m) {
				return MathEx.Exp(power) - 1.0m;
			}

			if(x < PositiveEpsilonOf(x)) {
				return x;
			}

			// Series Expansion to x^k / k!
			int k = 0;
			decimal term = 1.0m;
			return Series(
				() => {
					k++;
					term *= power;
					term /= k;
					return term;
				});
		}

		public decimal PositiveEpsilonOf(decimal value)
		{
			return 2 * EpsilonOf(value);
		}
		public static decimal EpsilonOf(decimal value)
		{
			if(value == 0) {
				return 0.0000000000000000000000000001m;
			}

			decimal epsilon = 0.0000000000000000000000000001m;
			while(value - epsilon == value) {
				epsilon *= 0.1m;
			}
			return epsilon;
		}
		internal decimal Series(Func<decimal> nextSummand)
		{
			decimal compensation = 0.0m;
			decimal current;
			const decimal factor = 1 << 16;

			decimal sum = nextSummand();

			do {
				// Kahan Summation
				// NOTE (ruegg): do NOT optimize. Now, how to tell that the compiler?
				current = nextSummand();
				decimal y = current - compensation;
				decimal t = sum + y;
				compensation = t - sum;
				compensation -= y;
				sum = t;
			}
			while(Math.Abs(sum) < Math.Abs(factor * current));
			return sum;
		}
	}

}
