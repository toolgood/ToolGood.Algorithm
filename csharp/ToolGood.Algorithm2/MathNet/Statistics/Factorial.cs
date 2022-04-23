using System;

namespace ToolGood.Algorithm.MathNet.Numerics
{
    partial class SpecialFunctions
    {
        static double[] _factorialCache;

        /// <summary>
        /// Initializes static members of the SpecialFunctions class.
        /// </summary>
        static SpecialFunctions()
        {
            InitializeFactorial();
        }

        static void InitializeFactorial()
        {
            _factorialCache = new double[171];
            _factorialCache[0] = 1.0;
            for (int i = 1; i < _factorialCache.Length; i++) {
                _factorialCache[i] = _factorialCache[i - 1] * i;
            }
        }

        public static double Binomial(int n, int k)
        {
            if (k < 0 || n < 0 || k > n) {
                return 0.0;
            }

            return Math.Floor(0.5 + Math.Exp(FactorialLn(n) - FactorialLn(k) - FactorialLn(n - k)));
        }
        public static double FactorialLn(int x)
        {
            //if (x < 0) {
            //    throw new ArgumentOutOfRangeException("x", "ArgumentPositive");
            //}

            if (x <= 1) {
                return 0d;
            }

            if (x < _factorialCache.Length) {
                return Math.Log(_factorialCache[x]);
            }

            return GammaLn(x + 1.0);
        }
        public static double BinomialLn(int n, int k)
        {
            if (k < 0 || n < 0 || k > n) {
                return double.NegativeInfinity;
            }

            return FactorialLn(n) - FactorialLn(k) - FactorialLn(n - k);
        }


    }
}
