using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
	internal class MathEx
	{
		/// <summary>
		///     Represents E.
		/// </summary>
		public const decimal E = 2.7182818284590452353602874713526624977572470936999595749M;

		/// <summary>
		///     Represents PI.
		/// </summary>
		public const decimal Epsilon = 0.0000000000000000001M;

		/// <summary>
		///     Represents one as decimal.
		/// </summary>
		public const decimal One = 1.0M;

		/// <summary>
		///     Represents PI.
		/// </summary>
		public const decimal PI = 3.14159265358979323846264338327950288419716939937510M;

		/// <summary>
		///     Represents a zero as decimal.
		/// </summary>
		public const decimal Zero = 0.0M;

		/// <summary>
		///     Represents 1.0/E.
		/// </summary>
		private const decimal EInverted = 0.3678794411714423215955237701614608674458111310317678M;

		/// <summary>
		///     Represents a half as decimal.
		/// </summary>
		private const decimal Half = 0.5M;

		/// <summary>
		///     Represents a log(10,E) factor.
		/// </summary>
		private const decimal Log10Inv = 0.434294481903251827651128918916605082294397005803666566114M;

		/// <summary>
		///     The maximum iterations count in a Taylor series.
		/// </summary>
		private const int MaximumIterations = 100;

		/// <summary>
		///     Represents PI/2.
		/// </summary>
		private const decimal HalfPi = 1.570796326794896619231321691639751442098584699687552910487M;

		/// <summary>
		///     Represents PI/4.
		/// </summary>
		private const decimal QuarterPi = 0.785398163397448309615660845819875721049292349843776455243M;

		/// <summary>
		///     Represents 2*PI.
		/// </summary>
		private const decimal TwoPi = 6.28318530717958647692528676655900576839433879875021M;


		/// <summary>
		///     Analogy of Math.Acos.
		/// </summary>
		/// <param name="x">The value to get the arcus cosinus value from.</param>
		/// <returns>The arcus cosinus value from the given value.</returns>
		public static decimal Acos(decimal x)
		{
			switch(x) {
				case Zero:
					return HalfPi;
				case One:
					return Zero;
			}

			if(x < Zero) {
				return PI - Acos(-x);
			}

			return HalfPi - Asin(x);
		}

		public static decimal Acosh(decimal x)
		{
			return Log(x + Sqrt(x * x - 1));
		}
		/// <summary>
		///     Analogy of Math.Asin.
		/// </summary>
		/// <param name="x">The value to get the arcus sinus value from.</param>
		/// <returns>The arcus sinus value from the given value.</returns>
		public static decimal Asin(decimal x)
		{
			if(x > One || x < -One) {
				throw new ArgumentException("x must be in [-1,1]");
			}

			switch(x) {
				// Known values
				case Zero:
					return Zero;
				case One:
					return HalfPi;
			}

			// Asin function is an odd function
			if(x < Zero) {
				return -Asin(-x);
			}

			// Used a math formula to speed up: asin(x)=0.5*(pi/2-asin(1-2*x*x)) if x>=0 is true
			var newX = One - 2 * x * x;

			// For calculating a new value nearer to zero than current because we gain more speed with values near to zero
			if(Math.Abs(x) > Math.Abs(newX)) {
				var t = Asin(newX);
				return Half * (HalfPi - t);
			}

			var y = Zero;
			var result = x;
			decimal cachedResult;
			var i = 1;
			y += result;
			var xx = x * x;
			do {
				cachedResult = result;
				result *= xx * (One - Half / i);
				y += result / (2 * i + 1);
				i++;
			}
			while(cachedResult != result);

			return y;
		}

		public static decimal Asinh(decimal x)
		{
			return Log(x + Sqrt(x * x + 1));
		}
		/// <summary>
		///     Analogy of Math.Atan.
		/// </summary>
		/// <param name="x">The value to get the arcus tangens value from.</param>
		/// <returns>The arcus tangens value from the given value.</returns>
		public static decimal Atan(decimal x)
		{
			switch(x) {
				case Zero:
					return Zero;
				case One:
					return QuarterPi;
				default:
					return Asin(x / Sqrt(One + x * x));
			}
		}

		/// <summary>
		///     Analogy of Math.Atan2.
		/// </summary>
		/// <param name="y">The y value.</param>
		/// <param name="x">The x value.</param>
		/// <returns>The arcus tangens value from the given values.</returns>
		public static decimal Atan2(decimal y, decimal x)
		{
			if(x > Zero) {
				return Atan(y / x);
			}

			if(x < Zero && y >= Zero) {
				return Atan(y / x) + PI;
			}

			if(x < Zero && y < Zero) {
				return Atan(y / x) - PI;
			}

			switch(x) {
				case Zero when y > Zero:
					return HalfPi;
				case Zero when y < Zero:
					return -HalfPi;
				default:
					throw new ArgumentException("invalid atan2 arguments");
			}
		}

		public static decimal Atanh(decimal x)
		{
			return 0.5m * Log((1 + x) / (1 - x));
		}

		/// <summary>
		///     Analogy of Math.Cos.
		/// </summary>
		/// <param name="x">The value to get the cosinus value from.</param>
		/// <returns>The cosinus value from the given value.</returns>
		public static decimal Cos(decimal x)
		{
			while(x > TwoPi) {
				x -= TwoPi;
			}

			while(x < -TwoPi) {
				x += TwoPi;
			}

			// Now x is in (-2pi,2pi)
			if(x >= PI && x <= TwoPi) {
				return -Cos(x - PI);
			}

			if(x >= -TwoPi && x <= -PI) {
				return -Cos(x + PI);
			}

			x *= x;

			// y=1-x/2!+x^2/4!-x^3/6!...
			var xx = -x * Half;
			var y = One + xx;
			var cachedY = y - One; // init cache  with different value
			for(var i = 1; cachedY != y && i < MaximumIterations; i++) {
				cachedY = y;

				// 2i^2+2i+i+1=2i^2+3i+1
				decimal factor = i * (i + i + 3) + 1;
				factor = -Half / factor;
				xx *= x * factor;
				y += xx;
			}

			return y;
		}

		/// <summary>
		///     Analogy of Math.Cosh.
		/// </summary>
		/// <param name="x">The value to get the cosinus h value from.</param>
		/// <returns>The cosinus h value from the given value.</returns>
		public static decimal Cosh(decimal x)
		{
			var y = Exp(x);
			var yy = One / y;
			return (y + yy) * Half;
		}

		/// <summary>
		///     Analogy of Math.Exp.
		/// </summary>
		/// <param name="x">The value to get the exponential function value from.</param>
		/// <returns>The exponential function value from the given value.</returns>
		public static decimal Exp(decimal x)
		{
			var count = 0;
			while(x > One) {
				x--;
				count++;
			}

			while(x < Zero) {
				x++;
				count--;
			}

			var iteration = 1;
			var result = One;
			var factor = One;
			decimal cachedResult;
			do {
				cachedResult = result;
				factor *= x / iteration++;
				result += factor;
			}
			while(cachedResult != result);

			if(count != 0) {
				result *= PowerN(E, count);
			}

			return result;
		}

		/// <summary>
		///     Analogy of Math.Log.
		/// </summary>
		/// <param name="x">The value to get the logarithmic function value from.</param>
		/// <returns>The logarithmic function value from the given value.</returns>
		public static decimal Log(decimal x)
		{
			if(x <= Zero) {
				throw new ArgumentException("x must be greater than zero");
			}

			var count = 0;
			while(x >= One) {
				x *= EInverted;
				count++;
			}

			while(x <= EInverted) {
				x *= E;
				count--;
			}

			x--;

			if(x == 0) {
				return count;
			}

			var result = Zero;
			var iteration = 0;
			var y = One;
			var cacheResult = result - One;
			while(cacheResult != result && iteration < MaximumIterations) {
				iteration++;
				cacheResult = result;
				y *= -x;
				result += y / iteration;
			}

			return count - result;
		}


		/// <summary>
		/// Returns the logarithm of a specified number in a specified base.
		/// </summary>
		/// <param name="d">A number whose logarithm is to be found.</param>
		/// <param name="newBase">The base of the logarithm.</param>
		/// <remarks>
		/// This is a relatively naive implementation that simply divides the
		/// natural log of <paramref name="d"/> by the natural log of the base.
		/// </remarks>
		public static decimal Log(decimal d, decimal newBase)
		{
			// Short circuit the checks below if d is 1 because
			// that will yield 0 in the numerator below and give us
			// 0 for any base, even ones that would yield infinity.
			if(d == 1) return 0m;

			if(newBase == 1) throw new InvalidOperationException("Logarithm for base 1 is undefined.");
			if(d < 0) throw new ArgumentException("Logarithm is a complex number for values less than zero!", nameof(d));
			if(d == 0) throw new OverflowException("Logarithm is defined as negative infinity at zero which the Decimal data type can't represent!");
			if(newBase < 0) throw new ArgumentException("Logarithm base would be a complex number for values less than zero!", nameof(newBase));
			if(newBase == 0) throw new OverflowException("Logarithm base would be negative infinity at zero which the Decimal data type can't represent!");

			return Log(d) / Log(newBase);
		}

		/// <summary>
		///     Analogy of Math.Log10.
		/// </summary>
		/// <param name="x">The value to get the logarithmic function value to base ten from.</param>
		/// <returns>The logarithmic function value to base ten from the given value.</returns>
		public static decimal Log10(decimal x)
		{
			return Log(x) * Log10Inv;
		}

		/// <summary>
		///     Analogy of Math.Pow.
		/// </summary>
		/// <param name="value">The value to get the power function value from.</param>
		/// <param name="pow">The power value to calculate with it.</param>
		/// <returns>The power function value from the given value.</returns>
		public static decimal Pow(decimal value, decimal pow)
		{
			switch(pow) {
				case Zero:
					return One;
				case One:
					return value;
			}

			switch(value) {
				case One:
				case Zero when pow == Zero:
					return One;
				case Zero when pow > Zero:
					return Zero;
				case Zero:
					throw new Exception("Invalid Operation: zero base and negative power");
			}

			if(pow == -One) {
				return One / value;
			}

			var isPowerInteger = IsInteger(pow);
			if(value < Zero && !isPowerInteger) {
				throw new Exception("Invalid Operation: negative base and non-integer power");
			}

			if(isPowerInteger && value > Zero) {
				var powerInt = (int)pow;
				return PowerN(value, powerInt);
			}

			if(!isPowerInteger || value >= Zero) {
				return Exp(pow * Log(value));
			}

			var powerInt2 = (int)pow;
			if(powerInt2 % 2 == 0) {
				return Exp(pow * Log(-value));
			}

			return -Exp(pow * Log(-value));
		}

		/// <summary>
		///     Analogy of Math.Pow, but with an integer value.
		/// </summary>
		/// <param name="value">The value to get the power function value from.</param>
		/// <param name="power">The power value to calculate with it.</param>
		/// <returns>The power function value from the given value.</returns>
		public static decimal PowerN(decimal value, int power)
		{
			while(true) {
				if(power == Zero) {
					return One;
				}

				if(power < Zero) {
					value = One / value;
					power = -power;
					continue;
				}

				var q = power;
				var prod = One;
				var current = value;

				while(q > 0) {
					if(q % 2 == 1) {
						// Detects the ones in the binary expression of power.
						// Picks up the relevant power.
						prod = current * prod;
						q--;
					}

					// value^i -> value^(2*i)
					current *= current;
					q /= 2;
				}

				return prod;
			}
		}


		/// <summary>
		///     Analogy of Math.Sin.
		/// </summary>
		/// <param name="x">The value to get the sinus function value from.</param>
		/// <returns>The sinus function value from the given value.</returns>
		public static decimal Sin(decimal x)
		{
			var cos = Cos(x);
			var moduleOfSin = Sqrt(One - cos * cos);
			var sineIsPositive = IsSignOfSinusPositive(x);
			if(sineIsPositive) {
				return moduleOfSin;
			}

			return -moduleOfSin;
		}

		/// <summary>
		///     Analogy of Math.Sinh.
		/// </summary>
		/// <param name="x">The value to get the sinus h function value from.</param>
		/// <returns>The sinus h function value from the given value.</returns>
		public static decimal Sinh(decimal x)
		{
			var y = Exp(x);
			var yy = One / y;
			return (y - yy) * Half;
		}

		/// <summary>
		///     Analogy of Math.Sqrt.
		/// </summary>
		/// <param name="x">The value to get the sqrt function value from.</param>
		/// <param name="epsilon">Last iteration while error is less than this epsilon.</param>
		/// <returns>The sqrt function value from the given value.</returns>
		public static decimal Sqrt(decimal x, decimal epsilon = Zero)
		{
			if(x < Zero) {
				throw new OverflowException("Cannot calculate square root from a negative number");
			}

			// Initial approximation.
			decimal current = (decimal)Math.Sqrt((double)x), previous;
			do {
				previous = current;
				if(previous == Zero) {
					return Zero;
				}

				current = (previous + x / previous) * Half;
			}
			while(Math.Abs(previous - current) > epsilon);

			return current;
		}

		/// <summary>
		///     Analogy of Math.Tan.
		/// </summary>
		/// <param name="x">The value to get the tangens function value from.</param>
		/// <returns>The tangens function value from the given value.</returns>
		public static decimal Tan(decimal x)
		{
			var cos = Cos(x);
			if(cos == Zero) {
				throw new ArgumentException(nameof(x));
			}

			return Sin(x) / cos;
		}

		/// <summary>
		///     Analogy of Math.Tanh.
		/// </summary>
		/// <param name="x">The value to get the tangens h function value from.</param>
		/// <returns>The tangens h function value from the given value.</returns>
		public static decimal Tanh(decimal x)
		{
			var y = Exp(x);
			var yy = One / y;
			return (y - yy) / (y + yy);
		}

		/// <summary>
		/// Checks whether the decimal value is an integer or not.
		/// </summary>
		/// <param name="x">The value to check.</param>
		/// <returns>A <c>bool</c> value indicating whether the value is an integer or not.</returns>
		private static bool IsInteger(decimal x)
		{
			var longValue = (long)x;
			return Math.Abs(x - longValue) <= Epsilon;
		}

		/// <summary>
		/// Checks whether the sign of the sinus value is positive.
		/// </summary>
		/// <param name="x">The value to check.</param>
		/// <returns>A <c>bool</c> value indicating whether the sign of the sinus value is positive or not.</returns>
		private static bool IsSignOfSinusPositive(decimal x)
		{
			// Truncating to [-2*PI;2*PI]
			while(x >= TwoPi) {
				x -= TwoPi;
			}

			while(x <= -TwoPi) {
				x += TwoPi;
			}

			// Now x is in [-2*PI;2*PI]
			if(x >= -TwoPi && x <= -PI) {
				return true;
			}

			if(x >= -PI && x <= Zero) {
				return false;
			}

			if(x >= Zero && x <= PI) {
				return true;
			}

			if(x >= PI && x <= TwoPi) {
				return false;
			}

			// Will not be reached.
			throw new ArgumentException(nameof(x));
		}
	}
}
