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
		private const decimal Epsilon = 0.0000000000000000001M;

		/// <summary>
		///     Represents one as decimal.
		/// </summary>
		private const decimal One = 1.0M;

		/// <summary>
		///     Represents PI.
		/// </summary>
		public const decimal PI = 3.14159265358979323846264338327950288419716939937510M;

		/// <summary>
		///     Represents a zero as decimal.
		/// </summary>
		private const decimal Zero = 0.0M;

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
		///     Represents ln(2).
		/// </summary>
		private const decimal Ln2 = 0.6931471805599453094172321215M;

		/// <summary>
		///     x*x 仍然安全(不溢出)的上限。
		///     超过该值时需用等价变形代替直接平方,避免中间量溢出 decimal。
		/// </summary>
		private const decimal SquareSafeMax = 100000000000000.0M;

		/// <summary>
		///     |x| 超过该值后 e^-|x| 小到可忽略(相对误差远小于 decimal 精度),
		///     双曲函数可改用 e^(|x|-ln2) 计算,避免 Exp(x) 提前上溢/下溢。
		/// </summary>
		private const decimal HyperbolicSafeMax = 60.0M;

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
			if(x < 1) {
				throw new ArgumentOutOfRangeException(nameof(x), "x must be >= 1");
			}
			// |x| 很大时 x*x 会溢出 decimal,改用等价变形:
			// acosh(x)=ln(x+sqrt(x²-1))=ln(x)+ln(1+sqrt(1-1/x²))
			if(x > SquareSafeMax) {
				var inverted = One / x;
				return Log(x) + Log(One + Sqrt(One - inverted * inverted));
			}
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
			// 泰勒斯级数在 |x|<=1/sqrt(3)(约0.577)时约60次即收敛,收敛后由 cachedResult == result 退出
			while(cachedResult != result && i < MaximumIterations);

			return y;
		}

		public static decimal Asinh(decimal x)
		{
			// 奇函数,先归一到非负,便于用等价变形规避 x*x 溢出
			if(x < Zero) {
				return -Asinh(-x);
			}
			// x 很大时 x*x 会溢出 decimal,改用等价变形:
			// asinh(x)=ln(x+sqrt(x²+1))=ln(x)+ln(1+sqrt(1+1/x²))
			if(x > SquareSafeMax) {
				var inverted = One / x;
				return Log(x) + Log(One + Sqrt(One + inverted * inverted));
			}
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
			}

			// |x|>1 时用 atan(x)=±π/2-atan(1/x) 归约到 [-1,1],
			// 既避免 1+x*x 溢出 decimal,又保证级数收敛速度
			if(x > One) {
				return HalfPi - Atan(One / x);
			}
			if(x < -One) {
				return -Atan(-x);
			}

			return Asin(x / Sqrt(One + x * x));
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
			// 不用 Math.Abs:它会因 decimal.MinValue 取绝对值溢出而抛异常
			if(x >= One || x <= -One) {
				throw new ArgumentOutOfRangeException(nameof(x), "x must be |x|<1");
			}

			// |x| 较小时改用级数 atanh(x)=x+x³/3+x⁵/5+...
			// 0.5*ln((1+x)/(1-x)) 在 x→0 时算式趋近 ln(1),会被 Log 在近 1 处的
			// 收敛下限(约 1e-22)吞掉有效位,例如 atanh(1e-15) 只能得到 8 位有效数字
			if(x <= Half && x >= -Half) {
				var xx = x * x;
				var result = x;
				var term = x;
				var k = 3;
				decimal cachedResult;
				do {
					cachedResult = result;
					term *= xx;
					result += term / k;
					k += 2;
				}
				// |x|<=0.5 时每项按 x² 递减,约 50 项即收敛,收敛后由 cachedResult == result 退出
				while(cachedResult != result && k < 2 * MaximumIterations);

				return result;
			}

			return 0.5m * Log((1 + x) / (1 - x));
		}

		/// <summary>
		///     Analogy of Math.Cos.
		/// </summary>
		/// <param name="x">The value to get the cosinus value from.</param>
		/// <returns>The cosinus value from the given value.</returns>
		public static decimal Cos(decimal x)
		{
			// O(1) 归约:一次性取模,避免原先按 2π 逐次递减的线性循环
			// (Cos(1e9) 需迭代约 1.6e8 次、耗时数秒;Cos(1e12) 更久,构成计算型 DoS)
			if(x > TwoPi || x < -TwoPi) {
				x -= Math.Floor(x / TwoPi) * TwoPi;
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
			var absolute = x < Zero ? -x : x;
			if(absolute > HyperbolicSafeMax) {
				// cosh(x)=(e^|x|+e^-|x|)/2≈e^(|x|-ln2),
				// 把可表示上界由 ln(decimal.MaxValue)≈66.542 提升到 ln(2*decimal.MaxValue)≈67.235
				return Exp(absolute - Ln2);
			}
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
			// O(1) 归约:一次取出整数部分,避免原先按 1 逐次递减的线性循环
			// (入参绝对值很大时,如 Exp(-1e9),线性循环会退化成耗时数秒以上的 DoS)
			var integral = Math.Floor(x);
			x -= integral;

			// decimal 的取值范围远大于 int,整数部分超出 int 时 E^count 必然溢出或下溢
			if(integral > int.MaxValue) {
				throw new OverflowException();
			}
			if(integral < int.MinValue) {
				return Zero;
			}
			var count = (int)integral;

			var iteration = 1;
			var result = One;
			var factor = One;
			decimal cachedResult;
			do {
				cachedResult = result;
				factor *= x / iteration++;
				result += factor;
			}
			// 归约后 x 属于 [0,1),级数约30次即收敛,收敛后由 cachedResult == result 退出
			while(cachedResult != result && iteration < MaximumIterations);

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

			// ln(1)=0。必须短路:归约中 EInverted*E 的舍入误差会把 x 变成 1-1e-28,
			// 最终把 ln(1) 算成 1e-28 而不是 0
			if(x == One) {
				return Zero;
			}

			// O(1) 归约:先用 double 对数估算数量级并一次性缩放,再用下面的循环做微调,
			// 避免原先按 EInverted/E 逐次乘除的线性循环(最大约 66 次)
			var count = (int)Math.Floor(Math.Log((double)x));
			if(count > 0) {
				x /= PowerN(E, count);
			} else if(count < 0) {
				x *= PowerN(E, -count);
			}

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
					throw new InvalidOperationException("Invalid Operation: zero base and negative power");
			}

			if(pow == -One) {
				return One / value;
			}

			var isPowerInteger = IsInteger(pow);
			if(value < Zero && !isPowerInteger) {
				throw new InvalidOperationException("Invalid Operation: negative base and non-integer power");
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
						if(q == 0) {
							// 防止最后一次无谓平方导致溢出(如 E^64 完成后再算 E^128)
							break;
						}
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
			// O(1) 归约到 [-π,π],避免按 2π 逐次递减的线性循环
			if(x > PI || x < -PI) {
				x -= Math.Floor((x + PI) / TwoPi) * TwoPi;
			}

			// 利用 sin(π-x)=sin(x) 把参数压到 [-π/2,π/2]:
			// 一方面级数在此区间收敛最快,另一方面避免原先 1-cos²x 的灾难性抵消
			// (小角度时 cos≈1,1-cos² 相减后有效位全部丢失,sin(1e-15) 会被算成 0)
			if(x > HalfPi) {
				x = PI - x;
			} else if(x < -HalfPi) {
				x = -PI - x;
			}

			// sin(x)=x-x³/3!+x⁵/5!-...
			var xx = x * x;
			var result = x;
			var term = x;
			var i = 1;
			decimal cachedResult;
			do {
				cachedResult = result;
				term *= -xx / (2 * i * (2 * i + 1));
				result += term;
				i++;
			}
			// 归约后 x 属于 [-π/2,π/2],级数约13次即收敛,收敛后由 cachedResult == result 退出
			while(cachedResult != result && i < MaximumIterations);

			return result;
		}

		/// <summary>
		///     Analogy of Math.Sinh.
		/// </summary>
		/// <param name="x">The value to get the sinus h function value from.</param>
		/// <returns>The sinus h function value from the given value.</returns>
		public static decimal Sinh(decimal x)
		{
			// 奇函数,先归一到非负,避免 Exp(x) 下溢为 0 后 1/y 除零
			if(x < Zero) {
				return -Sinh(-x);
			}
			if(x > HyperbolicSafeMax) {
				// sinh(x)=(e^x-e^-x)/2≈e^(x-ln2),
				// 把可表示上界由 ln(decimal.MaxValue)≈66.542 提升到 ln(2*decimal.MaxValue)≈67.235
				return Exp(x - Ln2);
			}
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

	}
}
