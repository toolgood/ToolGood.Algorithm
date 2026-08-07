using System;

namespace ToolGood.Algorithm.MathNet.Numerics
{
    internal partial class SpecialFunctions
    {
        // 移植自 MathNet.Numerics.SpecialFunctions.ModifiedBessel (Cephes/CERN Chebyshev 展开)

        /// <summary> Chebyshev coefficients for exp(-x) I0(x) in the interval [0, 8]. </summary>
        private static readonly decimal[] BesselI0A = {
            -4.41534164647933937950e-18M, 3.33079451882223809783e-17M, -2.43127984654795469359e-16M, 1.71539128555513303061e-15M,
            -1.16853328779934516808e-14M, 7.67618549860493561688e-14M, -4.85644678311192946090e-13M, 2.95505266312963983461e-12M,
            -1.72682629144155570723e-11M, 9.67580903537323691224e-11M, -5.18979560163526290666e-10M, 2.65982372468238665035e-9M,
            -1.30002500998624804212e-8M, 6.04699502254191894932e-8M, -2.67079385394061173391e-7M, 1.11738753912010371815e-6M,
            -4.41673835845875056359e-6M, 1.64484480707288970893e-5M, -5.75419501008210370398e-5M, 1.88502885095841655729e-4M,
            -5.76375574538582365885e-4M, 1.63947561694133579842e-3M, -4.32430999505057594430e-3M, 1.05464603945949983183e-2M,
            -2.37374148058994688156e-2M, 4.93052842396707084878e-2M, -9.49010970480476444210e-2M, 1.71620901522208775349e-1M,
            -3.04682672343198398683e-1M, 6.76795274409476084995e-1M
        };

        /// <summary> Chebyshev coefficients for exp(-x) sqrt(x) I0(x) in the inverted interval [8, infinity]. </summary>
        private static readonly decimal[] BesselI0B = {
            -7.23318048787475395456e-18M, -4.83050448594418207126e-18M, 4.46562142029675999901e-17M, 3.46122286769746109310e-17M,
            -2.82762398051658348494e-16M, -3.42548561967721913462e-16M, 1.77256013305652638360e-15M, 3.81168066935262242075e-15M,
            -9.55484669882830764870e-15M, -4.15056934728722208663e-14M, 1.54008621752140982691e-14M, 3.85277838274214270114e-13M,
            7.18012445138366623367e-13M, -1.79417853150680611778e-12M, -1.32158118404477131188e-11M, -3.14991652796324136454e-11M,
            1.18891471078464383424e-11M, 4.94060238822496958910e-10M, 3.39623202570838634515e-9M, 2.26666899049817806459e-8M,
            2.04891858946906374183e-7M, 2.89137052083475648297e-6M, 6.88975834691682398426e-5M, 3.36911647825569408990e-3M,
            8.04490411014108831608e-1M
        };

        /// <summary> Chebyshev coefficients for exp(-x) I1(x) / x in the interval [0, 8]. </summary>
        private static readonly decimal[] BesselI1A = {
            2.77791411276104639959e-18M, -2.11142121435816608115e-17M, 1.55363195773620046921e-16M, -1.10559694773538630805e-15M,
            7.60068429473540693410e-15M, -5.04218550472791168711e-14M, 3.22379336594557470981e-13M, -1.98397439776494371520e-12M,
            1.17361862988909016308e-11M, -6.66348972350202774223e-11M, 3.62559028155211703701e-10M, -1.88724975172282928790e-9M,
            9.38153738649577178388e-9M, -4.44505912879632808065e-8M, 2.00329475355213526229e-7M, -8.56872026469545474066e-7M,
            3.47025130813767847674e-6M, -1.32731636560394358279e-5M, 4.78156510755005422638e-5M, -1.61760815825896745588e-4M,
            5.12285956168575772895e-4M, -1.51357245063125314899e-3M, 4.15642294431288815669e-3M, -1.05640848946261981558e-2M,
            2.47264490306265168283e-2M, -5.29459812080949914269e-2M, 1.02643658689847095384e-1M, -1.76416518357834055153e-1M,
            2.52587186443633654823e-1M
        };

        /// <summary> Chebyshev coefficients for exp(-x) sqrt(x) I1(x) in the inverted interval [8, infinity]. </summary>
        private static readonly decimal[] BesselI1B = {
            7.51729631084210481353e-18M, 4.41434832307170791151e-18M, -4.65030536848935832153e-17M, -3.20952592199342395980e-17M,
            2.96262899764595013876e-16M, 3.30820231092092828324e-16M, -1.88035477551078244854e-15M, -3.81440307243700780478e-15M,
            1.04202769841288027642e-14M, 4.27244001671195135429e-14M, -2.10154184277266431302e-14M, -4.08355111109219731823e-13M,
            -7.19855177624590851209e-13M, 2.03562854414708950722e-12M, 1.41258074366137813316e-11M, 3.25260358301548823856e-11M,
            -1.89749581235054123450e-11M, -5.58974346219658380687e-10M, -3.83538038596423702205e-9M, -2.63146884688951950684e-8M,
            -2.51223623787020892529e-7M, -3.88256480887769039346e-6M, -1.10588938762623716291e-4M, -9.76109749136146840777e-3M,
            7.78576235018280120474e-1M
        };

        /// <summary> Chebyshev coefficients for K0(x) + log(x/2) I0(x) in the interval [0, 2]. </summary>
        private static readonly decimal[] BesselK0A = {
            1.37446543561352307156e-16M, 4.25981614279661018399e-14M, 1.03496952576338420167e-11M, 1.90451637722020886025e-9M,
            2.53479107902614945675e-7M, 2.28621210311945178607e-5M, 1.26461541144692592338e-3M, 3.59799365153615016266e-2M,
            3.44289899924628486886e-1M, -5.35327393233902768720e-1M
        };

        /// <summary> Chebyshev coefficients for exp(x) sqrt(x) K0(x) in the inverted interval [2, infinity]. </summary>
        private static readonly decimal[] BesselK0B = {
            5.30043377268626276149e-18M, -1.64758043015242134646e-17M, 5.21039150503902756861e-17M, -1.67823109680541210385e-16M,
            5.51205597852431940784e-16M, -1.84859337734377901440e-15M, 6.34007647740507060557e-15M, -2.22751332699166985548e-14M,
            8.03289077536357521100e-14M, -2.98009692317273043925e-13M, 1.14034058820847496303e-12M, -4.51459788337394416547e-12M,
            1.85594911495471785253e-11M, -7.95748924447710747776e-11M, 3.57739728140030116597e-10M, -1.69753450938905987466e-9M,
            8.57403401741422608519e-9M, -4.66048989768794782956e-8M, 2.76681363944501510342e-7M, -1.83175552271911948767e-6M,
            1.39498137188764993662e-5M, -1.28495495816278026384e-4M, 1.56988388573005337491e-3M, -3.14481013119645005427e-2M,
            2.44030308206595545468e0M
        };

        /// <summary> Chebyshev coefficients for x(K1(x) - log(x/2) I1(x)) in the interval [0, 2]. </summary>
        private static readonly decimal[] BesselK1A = {
            -7.02386347938628759343e-18M, -2.42744985051936593393e-15M, -6.66690169419932900609e-13M, -1.41148839263352776110e-10M,
            -2.21338763073472585583e-8M, -2.43340614156596823496e-6M, -1.73028895751305206302e-4M, -6.97572385963986435018e-3M,
            -1.22611180822657148235e-1M, -3.53155960776544875667e-1M, 1.52530022733894777053e0M
        };

        /// <summary> Chebyshev coefficients for exp(x) sqrt(x) K1(x) in the interval [2, infinity]. </summary>
        private static readonly decimal[] BesselK1B = {
            -5.75674448366501715755e-18M, 1.79405087314755922667e-17M, -5.68946255844285935196e-17M, 1.83809354436663880070e-16M,
            -6.05704724837331885336e-16M, 2.03870316562433424052e-15M, -7.01983709041831346144e-15M, 2.47715442448130437068e-14M,
            -8.97670518232499435011e-14M, 3.34841966607842919884e-13M, -1.28917396095102890680e-12M, 5.13963967348173025100e-12M,
            -2.12996783842756842877e-11M, 9.21831518760500529508e-11M, -4.19035475934189648750e-10M, 2.01504975519703286596e-9M,
            -1.03457624656780970260e-8M, 5.74108412545004946722e-8M, -3.50196060308781257119e-7M, 2.40648494783721712015e-6M,
            -1.93619797416608296024e-5M, 1.95215518471351631108e-4M, -2.85781685962277938680e-3M, 1.03923736576817238437e-1M,
            2.72062619048444266945e0M
        };

        /// <summary>返回第一类修正贝塞尔函数 I_n(x)。</summary>
        public static decimal BesselI(int n, decimal x)
        {
            if(x < 0) {
                return (n % 2 == 0 ? 1 : -1) * BesselI(n, -x);
            }
            if(x == 0) {
                return (n == 0) ? 1.0m : 0.0m;
            }

            decimal ax = Math.Abs(x);
            if(ax < 1e-10m) {
                return (n == 0) ? 1.0m : 0.0m;
            }

            if(n < 0) n = -n;

            if(ax > 700) {
                decimal factor = MathEx.Exp(ax) / MathEx.Sqrt(2 * MathEx.PI * ax);
                return factor * (1.0m - (4.0m * n * n - 1.0m) / (8.0m * ax));
            }

            if(n == 0) return BesselI0(x);
            if(n == 1) return BesselI1(x);

            // 递推关系: I_{k+1}(x) = I_{k-1}(x) - 2k/x * I_k(x)
            decimal I0 = BesselI0(x);
            decimal I1 = BesselI1(x);
            decimal In = 0;
            for(int k = 1; k < n; k++) {
                In = I0 - 2.0m * k / x * I1;
                I0 = I1;
                I1 = In;
            }

            return I1;
        }

        /// <summary>返回第二类修正贝塞尔函数 K_n(x),要求 x &gt; 0。</summary>
        public static decimal BesselK(int n, decimal x)
        {
            if(n < 0) n = -n;

            if(n == 0) return BesselK0(x);
            if(n == 1) return BesselK1(x);

            // 递推关系: K_{k+1}(x) = K_{k-1}(x) + 2k/x * K_k(x)
            decimal K0 = BesselK0(x);
            decimal K1 = BesselK1(x);
            decimal Kn = 0;
            for(int k = 1; k < n; k++) {
                Kn = K0 + 2.0m * k / x * K1;
                K0 = K1;
                K1 = Kn;
            }

            return K1;
        }

        private static decimal BesselI0(decimal x)
        {
            if(x < 0) {
                x = -x;
            }

            if(x <= 8.0m) {
                decimal y = (x / 2.0m) - 2.0m;
                return MathEx.Exp(x) * Evaluate.ChebyshevA(BesselI0A, y);
            }

            decimal x1 = 32.0m / x - 2.0m;
            return MathEx.Exp(x) * Evaluate.ChebyshevA(BesselI0B, x1) / MathEx.Sqrt(x);
        }

        private static decimal BesselI1(decimal x)
        {
            decimal z = Math.Abs(x);
            if(z <= 8.0m) {
                decimal y = (z / 2.0m) - 2.0m;
                z = Evaluate.ChebyshevA(BesselI1A, y) * z * MathEx.Exp(z);
            } else {
                decimal x1 = 32.0m / z - 2.0m;
                z = MathEx.Exp(z) * Evaluate.ChebyshevA(BesselI1B, x1) / MathEx.Sqrt(z);
            }

            if(x < 0.0m) {
                z = -z;
            }

            return z;
        }

        private static decimal BesselK0(decimal x)
        {
            if(x <= 2.0m) {
                decimal y = x * x - 2.0m;
                return Evaluate.ChebyshevA(BesselK0A, y) - MathEx.Log(0.5m * x) * BesselI0(x);
            }

            decimal z = 8.0m / x - 2.0m;
            return MathEx.Exp(-x) * Evaluate.ChebyshevA(BesselK0B, z) / MathEx.Sqrt(x);
        }

        private static decimal BesselK1(decimal x)
        {
            decimal z = 0.5m * x;
            if(x <= 2.0m) {
                decimal y = x * x - 2.0m;
                return MathEx.Log(z) * BesselI1(x) + Evaluate.ChebyshevA(BesselK1A, y) / x;
            }

            decimal x1 = 8.0m / x - 2.0m;
            return MathEx.Exp(-x) * Evaluate.ChebyshevA(BesselK1B, x1) / MathEx.Sqrt(x);
        }
    }
}
