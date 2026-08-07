using System;

namespace ToolGood.Algorithm.MathNet.Numerics
{
    internal partial class SpecialFunctions
    {
        // 贝塞尔函数 BesselJ / BesselY,沿用原有 Numerical Recipes 式 decimal 实现
        // (mathnet-numerics 中 BesselJ/BesselY 仅提供基于 Amos 算法的复数实现,不适合移植为 decimal)

        /// <summary>返回第一类贝塞尔函数 J_n(x)。</summary>
        public static decimal BesselJ(int n, decimal x)
        {
            if(x == 0) {
                return (n == 0) ? 1.0m : 0.0m;
            }

            if(n < 0) n = -n;

            decimal ax = Math.Abs(x);
            if(ax < 1e-10m) {
                return (n == 0) ? 1.0m : 0.0m;
            }

            if(n == 0) return BesselJ0(x);
            if(n == 1) return BesselJ1(x);

            if(ax > n) {
                // 递推关系: J_{k+1}(x) = 2k/x * J_k(x) - J_{k-1}(x)
                decimal J0 = BesselJ0(x);
                decimal J1 = BesselJ1(x);
                decimal Jn = 0;

                for(int k = 1; k < n; k++) {
                    Jn = (2.0m * k / x) * J1 - J0;
                    J0 = J1;
                    J1 = Jn;
                }
                return J1;
            }

            // Miller 算法(向下递推)计算 J_n
            int m = (int)(1.5 * n + 10);
            decimal[] J = new decimal[m + 2];
            J[m + 1] = 0.0m;
            J[m] = 1.0m;

            for(int k = m; k >= 1; k--) {
                J[k - 1] = (2.0m * k / x) * J[k] - J[k + 1];
            }

            decimal sum = 0.0m;
            for(int k = 0; k <= m; k += 2) {
                sum += 2.0m * J[k];
            }
            sum -= J[0];

            return J[n] / sum;
        }

        /// <summary>返回第二类贝塞尔函数 Y_n(x),要求 x &gt; 0。</summary>
        public static decimal BesselY(int n, decimal x)
        {
            if(n < 0) n = -n;

            if(n == 0) return BesselY0(x);
            if(n == 1) return BesselY1(x);

            // 递推关系: Y_{k+1}(x) = 2k/x * Y_k(x) - Y_{k-1}(x)
            decimal Y0 = BesselY0(x);
            decimal Y1 = BesselY1(x);
            decimal Yn = 0;

            for(int k = 1; k < n; k++) {
                Yn = (2.0m * k / x) * Y1 - Y0;
                Y0 = Y1;
                Y1 = Yn;
            }

            return Y1;
        }

        private static decimal BesselJ0(decimal x)
        {
            decimal ax = Math.Abs(x);
            if(ax < 8.0m) {
                decimal y1 = x * x;
                decimal ans1 = 57568490574.0m + y1 * (-13362590354.0m + y1 * (651619640.7m
                    + y1 * (-11214424.18m + y1 * (77392.33017m + y1 * (-184.9052456m)))));
                decimal ans2 = 57568490411.0m + y1 * (1029532985.0m + y1 * (9494680.718m
                    + y1 * (59272.64853m + y1 * (267.8532712m + y1 * 1.0m))));
                return ans1 / ans2;
            }
            decimal z = 8.0m / ax;
            decimal y2 = z * z;
            decimal xx = ax - 0.78539816339744830962m;
            decimal ans3 = 1.0m + y2 * (-0.1098628627e-2m + y2 * (0.2734510407e-4m
                + y2 * (-0.2073370639e-5m + y2 * 0.2093887211e-6m)));
            decimal ans4 = -0.1562499995e-1m + y2 * (0.1430488765e-3m
                + y2 * (-0.6911147651e-5m + y2 * (0.7621095161e-6m
                - y2 * 0.934935152e-7m)));
            return MathEx.Sqrt(0.63661977236758134308m / ax) * (MathEx.Cos(xx) * ans3 - z * MathEx.Sin(xx) * ans4);
        }

        private static decimal BesselJ1(decimal x)
        {
            decimal ax = Math.Abs(x);
            if(ax < 8.0m) {
                decimal y1 = x * x;
                decimal ans1 = x * (72362614232.0m + y1 * (-7895059235.0m + y1 * (242396853.1m
                    + y1 * (-2972611.439m + y1 * (15704.48260m + y1 * (-30.16036606m))))));
                decimal ans2 = 144725228442.0m + y1 * (2300535178.0m + y1 * (18583304.74m
                    + y1 * (99447.43394m + y1 * (376.9991397m + y1 * 1.0m))));
                return ans1 / ans2;
            }
            decimal z = 8.0m / ax;
            decimal y2 = z * z;
            decimal xx = ax - 2.35619449019234492885m;
            decimal ans3 = 1.0m + y2 * (0.183105e-2m + y2 * (-0.3516396496e-4m
                + y2 * (0.2457520174e-5m + y2 * (-0.240337019e-6m))));
            decimal ans4 = 0.04687499995m + y2 * (-0.2002690873e-3m
                + y2 * (0.8449199096e-5m + y2 * (-0.88228987e-6m
                + y2 * 0.105787412e-6m)));
            decimal ans = MathEx.Sqrt(0.63661977236758134308m / ax) * (MathEx.Cos(xx) * ans3 - z * MathEx.Sin(xx) * ans4);
            return (x < 0) ? -ans : ans;
        }

        private static decimal BesselY0(decimal x)
        {
            if(x < 8.0m) {
                decimal y1 = x * x;
                decimal ans1 = -2957821389.0m + y1 * (7062834065.0m + y1 * (-512359803.6m
                    + y1 * (10879881.29m + y1 * (-86327.92757m + y1 * 228.4622733m))));
                decimal ans2 = 40076544269.0m + y1 * (745249964.8m + y1 * (7189466.438m
                    + y1 * (47447.26470m + y1 * (226.1030244m + y1 * 1.0m))));
                return (ans1 / ans2) + 0.63661977236758134308m * BesselJ0(x) * MathEx.Log(x);
            }
            decimal z = 8.0m / x;
            decimal y2 = z * z;
            decimal xx = x - 0.78539816339744830962m;
            decimal ans3 = 1.0m + y2 * (-0.1098628627e-2m + y2 * (0.2734510407e-4m
                + y2 * (-0.2073370639e-5m + y2 * 0.2093887211e-6m)));
            decimal ans4 = -0.1562499995e-1m + y2 * (0.1430488765e-3m
                + y2 * (-0.6911147651e-5m + y2 * (0.7621095161e-6m
                - y2 * 0.934935152e-7m)));
            return MathEx.Sqrt(0.63661977236758134308m / x) * (MathEx.Sin(xx) * ans3 + z * MathEx.Cos(xx) * ans4);
        }

        private static decimal BesselY1(decimal x)
        {
            if(x < 8.0m) {
                decimal y1 = x * x;
                decimal ans1 = x * (-0.4900604943e13m + y1 * (0.1275274390e13m
                    + y1 * (-0.5153438139e11m + y1 * (0.7349264551e9m
                    + y1 * (-0.4237922726e7m + y1 * 0.8511937935e4m)))));
                decimal ans2 = 0.2499580570e14m + y1 * (0.4244419664e12m
                    + y1 * (0.3733650367e10m + y1 * (0.2245904002e8m
                    + y1 * (0.1020426050e6m + y1 * (0.3549632885e3m + y1)))));
                return (ans1 / ans2) + 0.63661977236758134308m * (BesselJ1(x) * MathEx.Log(x) - 1.0m / x);
            }
            decimal z = 8.0m / x;
            decimal y2 = z * z;
            decimal xx = x - 2.35619449019234492885m;
            decimal ans3 = 1.0m + y2 * (0.183105e-2m + y2 * (-0.3516396496e-4m
                + y2 * (0.2457520174e-5m + y2 * (-0.240337019e-6m))));
            decimal ans4 = 0.04687499995m + y2 * (-0.2002690873e-3m
                + y2 * (0.8449199096e-5m + y2 * (-0.88228987e-6m
                + y2 * 0.105787412e-6m)));
            return MathEx.Sqrt(0.63661977236758134308m / x) * (MathEx.Sin(xx) * ans3 + z * MathEx.Cos(xx) * ans4);
        }
    }
}
