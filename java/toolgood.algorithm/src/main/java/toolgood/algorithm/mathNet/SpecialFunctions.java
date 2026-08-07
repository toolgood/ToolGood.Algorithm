package toolgood.algorithm.mathNet;

public class SpecialFunctions {
    static double[] _factorialCache;

    /// <summary>
    /// Initializes static members of the SpecialFunctions class.
    /// </summary>
    static   {
        InitializeFactorial();
    }

    static void InitializeFactorial() {
        _factorialCache = new double[171];
        _factorialCache[0] = 1.0;
        for (int i = 1; i < _factorialCache.length; i++) {
            _factorialCache[i] = _factorialCache[i - 1] * i;
        }
    }

    public static double Binomial(int n, int k) {
        if (k < 0 || n < 0 || k > n) {
            return 0.0;
        }

        return Math.floor(0.5 + Math.exp(FactorialLn(n) - FactorialLn(k) - FactorialLn(n - k)));
    }

    public static double FactorialLn(int x) {
        // if (x < 0) {
        // throw new ArgumentOutOfRangeException("x", "ArgumentPositive");
        // }

        if (x <= 1) {
            return 0d;
        }

        if (x < _factorialCache.length) {
            return Math.log(_factorialCache[x]);
        }

        return GammaLn(x + 1.0);
    }

    public static double BinomialLn(int n, int k) {
        if (k < 0 || n < 0 || k > n) {
            return Double.NEGATIVE_INFINITY;
        }

        return FactorialLn(n) - FactorialLn(k) - FactorialLn(n - k);
    }

    public static double GammaLn(double z) {
        if (z < 0.5) {
            double s = GammaDk[0];
            for (int i = 1; i <= GammaN; i++) {
                s += GammaDk[i] / (i - z);
            }

            return Constants.LnPi - Math.log(Math.sin(Math.PI * z)) - Math.log(s) - Constants.LogTwoSqrtEOverPi
                    - ((0.5 - z) * Math.log((0.5 - z + GammaR) / Math.E));
        } else {
            double s = GammaDk[0];
            for (int i = 1; i <= GammaN; i++) {
                s += GammaDk[i] / (z + i - 1.0);
            }

            return Math.log(s) + Constants.LogTwoSqrtEOverPi + ((z - 0.5) * Math.log((z - 0.5 + GammaR) / Math.E));
        }
    }

    public static double BetaRegularized(double a, double b, double x) {
        // if (a < 0.0) {
        // throw new ArgumentOutOfRangeException("a", Resources.ArgumentNotNegative);
        // }

        // if (b < 0.0) {
        // throw new ArgumentOutOfRangeException("b", Resources.ArgumentNotNegative);
        // }

        // if (x < 0.0 || x > 1.0) {
        // throw new ArgumentOutOfRangeException("x",
        // Resources.ArgumentInIntervalXYInclusive);
        // }

        double bt = (x == 0.0 || x == 1.0) ? 0.0
                : Math.exp(GammaLn(a + b) - GammaLn(a) - GammaLn(b) + (a * Math.log(x)) + (b * Math.log(1.0 - x)));

        boolean symmetryTransformation = x >= (a + 1.0) / (a + b + 2.0);

        /* Continued fraction representation */
        double eps = Precision.DoublePrecision;
        double fpmin = Precision.Increment(0.0) / eps;

        if (symmetryTransformation) {
            x = 1.0 - x;
            double swap = a;
            a = b;
            b = swap;
        }

        double qab = a + b;
        double qap = a + 1.0;
        double qam = a - 1.0;
        double c = 1.0;
        double d = 1.0 - (qab * x / qap);

        if (Math.abs(d) < fpmin) {
            d = fpmin;
        }

        d = 1.0 / d;
        double h = d;

        for (int m = 1, m2 = 2; m <= 140; m++, m2 += 2) {
            double aa = m * (b - m) * x / ((qam + m2) * (a + m2));
            d = 1.0 + (aa * d);

            if (Math.abs(d) < fpmin) {
                d = fpmin;
            }

            c = 1.0 + (aa / c);
            if (Math.abs(c) < fpmin) {
                c = fpmin;
            }

            d = 1.0 / d;
            h *= d * c;
            aa = -(a + m) * (qab + m) * x / ((a + m2) * (qap + m2));
            d = 1.0 + (aa * d);

            if (Math.abs(d) < fpmin) {
                d = fpmin;
            }

            c = 1.0 + (aa / c);

            if (Math.abs(c) < fpmin) {
                c = fpmin;
            }

            d = 1.0 / d;
            double del = d * c;
            h *= del;

            if (Math.abs(del - 1.0) <= eps) {
                return symmetryTransformation ? 1.0 - (bt * h / a) : bt * h / a;
            }
        }

        return symmetryTransformation ? 1.0 - (bt * h / a) : bt * h / a;
    }

    final static int GammaN = 10;

    /// <summary>
    /// Auxiliary variable when evaluating the <see cref="GammaLn"/> function.
    /// </summary>
    final static double GammaR = 10.900511;

    static double[] GammaDk = { 2.48574089138753565546e-5, 1.05142378581721974210, -3.45687097222016235469,
            4.51227709466894823700, -2.98285225323576655721, 1.05639711577126713077, -1.95428773191645869583e-1,
            1.70970543404441224307e-2, -5.71926117404305781283e-4, 4.63399473359905636708e-6,
            -2.71994908488607703910e-9 };

    public static double GammaLowerRegularized(double a, double x) {
        final double epsilon = 0.000000000000001;
        final double big = 4503599627370496.0;
        final double bigInv = 2.22044604925031308085e-16;

        // if (a < 0d) {
        // throw new ArgumentOutOfRangeException("a",
        // Properties.Resources.ArgumentNotNegative);
        // }

        // if (x < 0d) {
        // throw new ArgumentOutOfRangeException("x",
        // Properties.Resources.ArgumentNotNegative);
        // }

        if (Precision.AlmostEqual(a, 0.0)) {
            if (Precision.AlmostEqual(x, 0.0)) {
                // use right hand limit value because so that regularized upper/lower gamma
                // definition holds.
                return 1d;
            }

            return 1d;
        }

        if (Precision.AlmostEqual(x, 0.0)) {
            return 0d;
        }

        double ax = (a * Math.log(x)) - x - GammaLn(a);
        if (ax < -709.78271289338399) {
            return a < x ? 1d : 0d;
        }

        if (x <= 1 || x <= a) {
            double r2 = a;
            double c2 = 1;
            double ans2 = 1;

            do {
                r2 = r2 + 1;
                c2 = c2 * x / r2;
                ans2 += c2;
            } while ((c2 / ans2) > epsilon);

            return Math.exp(ax) * ans2 / a;
        }

        int c = 0;
        double y = 1 - a;
        double z = x + y + 1;

        double p3 = 1;
        double q3 = x;
        double p2 = x + 1;
        double q2 = z * x;
        double ans = p2 / q2;

        double error;

        do {
            c++;
            y += 1;
            z += 2;
            double yc = y * c;

            double p = (p2 * z) - (p3 * yc);
            double q = (q2 * z) - (q3 * yc);

            if (q != 0) {
                double nextans = p / q;
                error = Math.abs((ans - nextans) / nextans);
                ans = nextans;
            } else {
                // zero div, skip
                error = 1;
            }

            // shift
            p3 = p2;
            p2 = p;
            q3 = q2;
            q2 = q;

            // normalize fraction when the numerator becomes large
            if (Math.abs(p) > big) {
                p3 *= bigInv;
                p2 *= bigInv;
                q3 *= bigInv;
                q2 *= bigInv;
            }
        } while (error > epsilon);

        return 1d - (Math.exp(ax) * ans);
    }

    public static double GammaLowerRegularizedInv(double a, double y0) {
        final double epsilon = 0.000000000000001;
        final double big = 4503599627370496.0;
        final double threshold = 5 * epsilon;

        if (Double.isNaN(a) || Double.isNaN(y0)) {
            return Double.NaN;
        }

        // if (a < 0 || a.AlmostEqual(0.0)) {
        // throw new ArgumentOutOfRangeException("a");
        // }

        // if (y0 < 0 || y0 > 1) {
        // throw new ArgumentOutOfRangeException("y0");
        // }

        if (Precision.AlmostEqual(y0, 0.0)) {
            return 0d;
        }

        if (Precision.AlmostEqual(y0, 1.0)) {
            return Double.POSITIVE_INFINITY;
        }

        y0 = 1 - y0;

        double xUpper = big;
        double xLower = 0;
        double yUpper = 1;
        double yLower = 0;

        // Initial Guess
        double d = 1 / (9 * a);
        double y = 1 - d - (0.98 * Constants.Sqrt2 * ErfInv((2.0 * y0) - 1.0) * Math.sqrt(d));
        double x = a * y * y * y;
        double lgm = GammaLn(a);

        for (int i = 0; i < 10; i++) {
            if (x < xLower || x > xUpper) {
                d = 0.0625;
                break;
            }

            y = 1 - GammaLowerRegularized(a, x);
            if (y < yLower || y > yUpper) {
                d = 0.0625;
                break;
            }

            if (y < y0) {
                xUpper = x;
                yLower = y;
            } else {
                xLower = x;
                yUpper = y;
            }

            d = ((a - 1) * Math.log(x)) - x - lgm;
            if (d < -709.78271289338399) {
                d = 0.0625;
                break;
            }

            d = -Math.exp(d);
            d = (y - y0) / d;
            if (Math.abs(d / x) < epsilon) {
                return x;
            }

            if ((d > (x / 4)) && (y0 < 0.05)) {
                // Naive heuristics for cases near the singularity
                d = x / 10;
            }

            x -= d;
        }

        if (xUpper == big) {
            if (x <= 0) {
                x = 1;
            }

            while (xUpper == big) {
                x = (1 + d) * x;
                y = 1 - GammaLowerRegularized(a, x);
                if (y < y0) {
                    xUpper = x;
                    yLower = y;
                    break;
                }

                d = d + d;
            }
        }

        int dir = 0;
        d = 0.5;
        for (int i = 0; i < 400; i++) {
            x = xLower + (d * (xUpper - xLower));
            y = 1 - GammaLowerRegularized(a, x);
            lgm = (xUpper - xLower) / (xLower + xUpper);
            if (Math.abs(lgm) < threshold) {
                return x;
            }

            lgm = (y - y0) / y0;
            if (Math.abs(lgm) < threshold) {
                return x;
            }

            if (x <= 0d) {
                return 0d;
            }

            if (y >= y0) {
                xLower = x;
                yUpper = y;
                if (dir < 0) {
                    dir = 0;
                    d = 0.5;
                } else {
                    if (dir > 1) {
                        d = (0.5 * d) + 0.5;
                    } else {
                        d = (y0 - yLower) / (yUpper - yLower);
                    }
                }

                dir = dir + 1;
            } else {
                xUpper = x;
                yLower = y;
                if (dir > 0) {
                    dir = 0;
                    d = 0.5;
                } else {
                    if (dir < -1) {
                        d = 0.5 * d;
                    } else {
                        d = (y0 - yLower) / (yUpper - yLower);
                    }
                }

                dir = dir - 1;
            }
        }

        return x;
    }

    public static double Gamma(double z) {
        if (z < 0.5) {
            double s = GammaDk[0];
            for (int i = 1; i <= GammaN; i++) {
                s += GammaDk[i] / (i - z);
            }

            return Math.PI / (Math.sin(Math.PI * z) * s * Constants.TwoSqrtEOverPi
                    * Math.pow((0.5 - z + GammaR) / Math.E, 0.5 - z));
        } else {
            double s = GammaDk[0];
            for (int i = 1; i <= GammaN; i++) {
                s += GammaDk[i] / (z + i - 1.0);
            }

            return s * Constants.TwoSqrtEOverPi * Math.pow((z - 0.5 + GammaR) / Math.E, z - 0.5);
        }
    }

    private static double[] ErfImpAn = { 0.00337916709551257388990745, -0.00073695653048167948530905,
            -0.374732337392919607868241, 0.0817442448733587196071743, -0.0421089319936548595203468,
            0.0070165709512095756344528, -0.00495091255982435110337458, 0.000871646599037922480317225 };
    private static double[] ErfImpAd = { 1, -0.218088218087924645390535, 0.412542972725442099083918,
            -0.0841891147873106755410271, 0.0655338856400241519690695, -0.0120019604454941768171266,
            0.00408165558926174048329689, -0.000615900721557769691924509 };
    private static double[] ErfImpBn = { -0.0361790390718262471360258, 0.292251883444882683221149,
            0.281447041797604512774415, 0.125610208862766947294894, 0.0274135028268930549240776,
            0.00250839672168065762786937 };
    private static double[] ErfImpBd = { 1, 1.8545005897903486499845, 1.43575803037831418074962,
            0.582827658753036572454135, 0.124810476932949746447682, 0.0113724176546353285778481 };
    private static double[] ErfImpCn = { -0.0397876892611136856954425, 0.153165212467878293257683,
            0.191260295600936245503129, 0.10276327061989304213645, 0.029637090615738836726027,
            0.0046093486780275489468812, 0.000307607820348680180548455 };
    private static double[] ErfImpCd = { 1, 1.95520072987627704987886, 1.64762317199384860109595,
            0.768238607022126250082483, 0.209793185936509782784315, 0.0319569316899913392596356,
            0.00213363160895785378615014 };
    private static double[] ErfImpDn = { -0.0300838560557949717328341, 0.0538578829844454508530552,
            0.0726211541651914182692959, 0.0367628469888049348429018, 0.00964629015572527529605267,
            0.00133453480075291076745275, 0.778087599782504251917881e-4 };
    private static double[] ErfImpDd = { 1, 1.75967098147167528287343, 1.32883571437961120556307,
            0.552528596508757581287907, 0.133793056941332861912279, 0.0179509645176280768640766,
            0.00104712440019937356634038, -0.106640381820357337177643e-7 };
    private static double[] ErfImpEn = { -0.0117907570137227847827732, 0.014262132090538809896674,
            0.0202234435902960820020765, 0.00930668299990432009042239, 0.00213357802422065994322516,
            0.00025022987386460102395382, 0.120534912219588189822126e-4 };
    private static double[] ErfImpEd = { 1, 1.50376225203620482047419, 0.965397786204462896346934,
            0.339265230476796681555511, 0.0689740649541569716897427, 0.00771060262491768307365526,
            0.000371421101531069302990367 };
    private static double[] ErfImpFn = { -0.00546954795538729307482955, 0.00404190278731707110245394,
            0.0054963369553161170521356, 0.00212616472603945399437862, 0.000394984014495083900689956,
            0.365565477064442377259271e-4, 0.135485897109932323253786e-5 };
    private static double[] ErfImpFd = { 1, 1.21019697773630784832251, 0.620914668221143886601045,
            0.173038430661142762569515, 0.0276550813773432047594539, 0.00240625974424309709745382,
            0.891811817251336577241006e-4, -0.465528836283382684461025e-11 };
    private static double[] ErfImpGn = { -0.00270722535905778347999196, 0.0013187563425029400461378,
            0.00119925933261002333923989, 0.00027849619811344664248235, 0.267822988218331849989363e-4,
            0.923043672315028197865066e-6 };
    private static double[] ErfImpGd = { 1, 0.814632808543141591118279, 0.268901665856299542168425,
            0.0449877216103041118694989, 0.00381759663320248459168994, 0.000131571897888596914350697,
            0.404815359675764138445257e-11 };
    private static double[] ErfImpHn = { -0.00109946720691742196814323, 0.000406425442750422675169153,
            0.000274499489416900707787024, 0.465293770646659383436343e-4, 0.320955425395767463401993e-5,
            0.778286018145020892261936e-7 };
    private static double[] ErfImpHd = { 1, 0.588173710611846046373373, 0.139363331289409746077541,
            0.0166329340417083678763028, 0.00100023921310234908642639, 0.24254837521587225125068e-4 };
    private static double[] ErfImpIn = { -0.00056907993601094962855594, 0.000169498540373762264416984,
            0.518472354581100890120501e-4, 0.382819312231928859704678e-5, 0.824989931281894431781794e-7 };
    private static double[] ErfImpId = { 1, 0.339637250051139347430323, 0.043472647870310663055044,
            0.00248549335224637114641629, 0.535633305337152900549536e-4, -0.117490944405459578783846e-12 };
    private static double[] ErfImpJn = { -0.000241313599483991337479091, 0.574224975202501512365975e-4,
            0.115998962927383778460557e-4, 0.581762134402593739370875e-6, 0.853971555085673614607418e-8 };
    private static double[] ErfImpJd = { 1, 0.233044138299687841018015, 0.0204186940546440312625597,
            0.000797185647564398289151125, 0.117019281670172327758019e-4 };
    private static double[] ErfImpKn = { -0.000146674699277760365803642, 0.162666552112280519955647e-4,
            0.269116248509165239294897e-5, 0.979584479468091935086972e-7, 0.101994647625723465722285e-8 };
    private static double[] ErfImpKd = { 1, 0.165907812944847226546036, 0.0103361716191505884359634,
            0.000286593026373868366935721, 0.298401570840900340874568e-5 };
    private static double[] ErfImpLn = { -0.583905797629771786720406e-4, 0.412510325105496173512992e-5,
            0.431790922420250949096906e-6, 0.993365155590013193345569e-8, 0.653480510020104699270084e-10 };
    private static double[] ErfImpLd = { 1, 0.105077086072039915406159, 0.00414278428675475620830226,
            0.726338754644523769144108e-4, 0.477818471047398785369849e-6 };
    private static double[] ErfImpMn = { -0.196457797609229579459841e-4, 0.157243887666800692441195e-5,
            0.543902511192700878690335e-7, 0.317472492369117710852685e-9 };
    private static double[] ErfImpMd = { 1, 0.052803989240957632204885, 0.000926876069151753290378112,
            0.541011723226630257077328e-5, 0.535093845803642394908747e-15 };
    private static double[] ErfImpNn = { -0.789224703978722689089794e-5, 0.622088451660986955124162e-6,
            0.145728445676882396797184e-7, 0.603715505542715364529243e-10 };
    private static double[] ErfImpNd = { 1, 0.0375328846356293715248719, 0.000467919535974625308126054,
            0.193847039275845656900547e-5 };

    private static double[] ErvInvImpAn = { -0.000508781949658280665617, -0.00836874819741736770379,
            0.0334806625409744615033, -0.0126926147662974029034, -0.0365637971411762664006, 0.0219878681111168899165,
            0.00822687874676915743155, -0.00538772965071242932965 };
    private static double[] ErvInvImpAd = { 1, -0.970005043303290640362, -1.56574558234175846809,
            1.56221558398423026363, 0.662328840472002992063, -0.71228902341542847553, -0.0527396382340099713954,
            0.0795283687341571680018, -0.00233393759374190016776, 0.000886216390456424707504 };
    private static double[] ErvInvImpBn = { -0.202433508355938759655, 0.105264680699391713268, 8.37050328343119927838,
            17.6447298408374015486, -18.8510648058714251895, -44.6382324441786960818, 17.445385985570866523,
            21.1294655448340526258, -3.67192254707729348546 };
    private static double[] ErvInvImpBd = { 1, 6.24264124854247537712, 3.9713437953343869095, -28.6608180499800029974,
            -20.1432634680485188801, 48.5609213108739935468, 10.8268667355460159008, -22.6436933413139721736,
            1.72114765761200282724 };
    private static double[] ErvInvImpCn = { -0.131102781679951906451, -0.163794047193317060787, 0.117030156341995252019,
            0.387079738972604337464, 0.337785538912035898924, 0.142869534408157156766, 0.0290157910005329060432,
            0.00214558995388805277169, -0.679465575181126350155e-6, 0.285225331782217055858e-7,
            -0.681149956853776992068e-9 };
    private static double[] ErvInvImpCd = { 1, 3.46625407242567245975, 5.38168345707006855425, 4.77846592945843778382,
            2.59301921623620271374, 0.848854343457902036425, 0.152264338295331783612, 0.01105924229346489121 };
    private static double[] ErvInvImpDn = { -0.0350353787183177984712, -0.00222426529213447927281,
            0.0185573306514231072324, 0.00950804701325919603619, 0.00187123492819559223345, 0.000157544617424960554631,
            0.460469890584317994083e-5, -0.230404776911882601748e-9, 0.266339227425782031962e-11 };
    private static double[] ErvInvImpDd = { 1, 1.3653349817554063097, 0.762059164553623404043, 0.220091105764131249824,
            0.0341589143670947727934, 0.00263861676657015992959, 0.764675292302794483503e-4 };
    private static double[] ErvInvImpEn = { -0.0167431005076633737133, -0.00112951438745580278863,
            0.00105628862152492910091, 0.000209386317487588078668, 0.149624783758342370182e-4,
            0.449696789927706453732e-6, 0.462596163522878599135e-8, -0.281128735628831791805e-13,
            0.99055709973310326855e-16 };
    private static double[] ErvInvImpEd = { 1, 0.591429344886417493481, 0.138151865749083321638,
            0.0160746087093676504695, 0.000964011807005165528527, 0.275335474764726041141e-4,
            0.282243172016108031869e-6 };
    private static double[] ErvInvImpFn = { -0.0024978212791898131227, -0.779190719229053954292e-5,
            0.254723037413027451751e-4, 0.162397777342510920873e-5, 0.396341011304801168516e-7,
            0.411632831190944208473e-9, 0.145596286718675035587e-11, -0.116765012397184275695e-17 };
    private static double[] ErvInvImpFd = { 1, 0.207123112214422517181, 0.0169410838120975906478,
            0.000690538265622684595676, 0.145007359818232637924e-4, 0.144437756628144157666e-6,
            0.509761276599778486139e-9 };
    private static double[] ErvInvImpGn = { -0.000539042911019078575891, -0.28398759004727721098e-6,
            0.899465114892291446442e-6, 0.229345859265920864296e-7, 0.225561444863500149219e-9,
            0.947846627503022684216e-12, 0.135880130108924861008e-14, -0.348890393399948882918e-21 };
    private static double[] ErvInvImpGd = { 1, 0.0845746234001899436914, 0.00282092984726264681981,
            0.468292921940894236786e-4, 0.399968812193862100054e-6, 0.161809290887904476097e-8,
            0.231558608310259605225e-11 };

    public static double Erfc(double x) {
        if (x == 0) {
            return 1;
        }

        if (Double.isInfinite(x) && x > 0) {
            return 0;
        }

        if (Double.isInfinite(x) && x < 0) {
            return 2;
        }

        if (Double.isNaN(x)) {
            return Double.NaN;
        }

        return ErfImp(x, true);
    }

    static double ErfImp(double z, boolean invert) {
        if (z < 0) {
            if (!invert) {
                return -ErfImp(-z, false);
            }

            if (z < -0.5) {
                return 2 - ErfImp(-z, true);
            }

            return 1 + ErfImp(-z, false);
        }

        double result;

        // Big bunch of selection statements now to pick which
        // implementation to use, try to put most likely options
        // first:
        if (z < 0.5) {
            // We're going to calculate erf:
            if (z < 1e-10) {
                result = (z * 1.125) + (z * 0.003379167095512573896158903121545171688);
            } else {
                // Worst case absolute error found: 6.688618532e-21
                result = (z * 1.125) + (z * Evaluate.Polynomial(z, ErfImpAn) / Evaluate.Polynomial(z, ErfImpAd));
            }
        } else if ((z < 110) || ((z < 110) && invert)) {
            // We'll be calculating erfc:
            invert = !invert;
            double r, b;
            if (z < 0.75) {
                // Worst case absolute error found: 5.582813374e-21
                r = Evaluate.Polynomial(z - 0.5, ErfImpBn) / Evaluate.Polynomial(z - 0.5, ErfImpBd);
                b = 0.3440242112F;
            } else if (z < 1.25) {
                // Worst case absolute error found: 4.01854729e-21
                r = Evaluate.Polynomial(z - 0.75, ErfImpCn) / Evaluate.Polynomial(z - 0.75, ErfImpCd);
                b = 0.419990927F;
            } else if (z < 2.25) {
                // Worst case absolute error found: 2.866005373e-21
                r = Evaluate.Polynomial(z - 1.25, ErfImpDn) / Evaluate.Polynomial(z - 1.25, ErfImpDd);
                b = 0.4898625016F;
            } else if (z < 3.5) {
                // Worst case absolute error found: 1.045355789e-21
                r = Evaluate.Polynomial(z - 2.25, ErfImpEn) / Evaluate.Polynomial(z - 2.25, ErfImpEd);
                b = 0.5317370892F;
            } else if (z < 5.25) {
                // Worst case absolute error found: 8.300028706e-22
                r = Evaluate.Polynomial(z - 3.5, ErfImpFn) / Evaluate.Polynomial(z - 3.5, ErfImpFd);
                b = 0.5489973426F;
            } else if (z < 8) {
                // Worst case absolute error found: 1.700157534e-21
                r = Evaluate.Polynomial(z - 5.25, ErfImpGn) / Evaluate.Polynomial(z - 5.25, ErfImpGd);
                b = 0.5571740866F;
            } else if (z < 11.5) {
                // Worst case absolute error found: 3.002278011e-22
                r = Evaluate.Polynomial(z - 8, ErfImpHn) / Evaluate.Polynomial(z - 8, ErfImpHd);
                b = 0.5609807968F;
            } else if (z < 17) {
                // Worst case absolute error found: 6.741114695e-21
                r = Evaluate.Polynomial(z - 11.5, ErfImpIn) / Evaluate.Polynomial(z - 11.5, ErfImpId);
                b = 0.5626493692F;
            } else if (z < 24) {
                // Worst case absolute error found: 7.802346984e-22
                r = Evaluate.Polynomial(z - 17, ErfImpJn) / Evaluate.Polynomial(z - 17, ErfImpJd);
                b = 0.5634598136F;
            } else if (z < 38) {
                // Worst case absolute error found: 2.414228989e-22
                r = Evaluate.Polynomial(z - 24, ErfImpKn) / Evaluate.Polynomial(z - 24, ErfImpKd);
                b = 0.5638477802F;
            } else if (z < 60) {
                // Worst case absolute error found: 5.896543869e-24
                r = Evaluate.Polynomial(z - 38, ErfImpLn) / Evaluate.Polynomial(z - 38, ErfImpLd);
                b = 0.5640528202F;
            } else if (z < 85) {
                // Worst case absolute error found: 3.080612264e-21
                r = Evaluate.Polynomial(z - 60, ErfImpMn) / Evaluate.Polynomial(z - 60, ErfImpMd);
                b = 0.5641309023F;
            } else {
                // Worst case absolute error found: 8.094633491e-22
                r = Evaluate.Polynomial(z - 85, ErfImpNn) / Evaluate.Polynomial(z - 85, ErfImpNd);
                b = 0.5641584396F;
            }

            double g = Math.exp(-z * z) / z;
            result = (g * b) + (g * r);
        } else {
            // Any value of z larger than 28 will underflow to zero:
            result = 0;
            invert = !invert;
        }

        if (invert) {
            result = 1 - result;
        }

        return result;
    }

    public static double ErfcInv(double z) {
        if (z <= 0.0) {
            return Double.POSITIVE_INFINITY;
        }

        if (z >= 2.0) {
            return Double.NEGATIVE_INFINITY;
        }

        double p, q, s;
        if (z > 1) {
            q = 2 - z;
            p = 1 - q;
            s = -1;
        } else {
            p = 1 - z;
            q = z;
            s = 1;
        }

        return ErfInvImpl(p, q, s);
    }

    static double ErfInvImpl(double p, double q, double s) {
        double result;

        if (p <= 0.5) {
            // Evaluate inverse erf using the rational approximation:
            //
            // x = p(p+10)(Y+R(p))
            //
            // Where Y is a constant, and R(p) is optimized for a low
            // absolute error compared to |Y|.
            //
            // double: Max error found: 2.001849e-18
            // long double: Max error found: 1.017064e-20
            // Maximum Deviation Found (actual error term at infinite precision) 8.030e-21
            final float y = 0.0891314744949340820313f;
            double g = p * (p + 10);
            double r = Evaluate.Polynomial(p, ErvInvImpAn) / Evaluate.Polynomial(p, ErvInvImpAd);
            result = (g * y) + (g * r);
        } else if (q >= 0.25) {
            // Rational approximation for 0.5 > q >= 0.25
            //
            // x = sqrt(-2*log(q)) / (Y + R(q))
            //
            // Where Y is a constant, and R(q) is optimized for a low
            // absolute error compared to Y.
            //
            // double : Max error found: 7.403372e-17
            // long double : Max error found: 6.084616e-20
            // Maximum Deviation Found (error term) 4.811e-20
            final float y = 2.249481201171875f;
            double g = Math.sqrt(-2 * Math.log(q));
            double xs = q - 0.25;
            double r = Evaluate.Polynomial(xs, ErvInvImpBn) / Evaluate.Polynomial(xs, ErvInvImpBd);
            result = g / (y + r);
        } else {
            // For q < 0.25 we have a series of rational approximations all
            // of the general form:
            //
            // let: x = sqrt(-log(q))
            //
            // Then the result is given by:
            //
            // x(Y+R(x-B))
            //
            // where Y is a constant, B is the lowest value of x for which
            // the approximation is valid, and R(x-B) is optimized for a low
            // absolute error compared to Y.
            //
            // Note that almost all code will really go through the first
            // or maybe second approximation. After than we're dealing with very
            // small input values indeed: 80 and 128 bit long double's go all the
            // way down to ~ 1e-5000 so the "tail" is rather long...
            double x = Math.sqrt(-Math.log(q));
            if (x < 3) {
                // Max error found: 1.089051e-20
                final float y = 0.807220458984375f;
                double xs = x - 1.125;
                double r = Evaluate.Polynomial(xs, ErvInvImpCn) / Evaluate.Polynomial(xs, ErvInvImpCd);
                result = (y * x) + (r * x);
            } else if (x < 6) {
                // Max error found: 8.389174e-21
                final float y = 0.93995571136474609375f;
                double xs = x - 3;
                double r = Evaluate.Polynomial(xs, ErvInvImpDn) / Evaluate.Polynomial(xs, ErvInvImpDd);
                result = (y * x) + (r * x);
            } else if (x < 18) {
                // Max error found: 1.481312e-19
                final float y = 0.98362827301025390625f;
                double xs = x - 6;
                double r = Evaluate.Polynomial(xs, ErvInvImpEn) / Evaluate.Polynomial(xs, ErvInvImpEd);
                result = (y * x) + (r * x);
            } else if (x < 44) {
                // Max error found: 5.697761e-20
                final float y = 0.99714565277099609375f;
                double xs = x - 18;
                double r = Evaluate.Polynomial(xs, ErvInvImpFn) / Evaluate.Polynomial(xs, ErvInvImpFd);
                result = (y * x) + (r * x);
            } else {
                // Max error found: 1.279746e-20
                final float y = 0.99941349029541015625f;
                double xs = x - 44;
                double r = Evaluate.Polynomial(xs, ErvInvImpGn) / Evaluate.Polynomial(xs, ErvInvImpGd);
                result = (y * x) + (r * x);
            }
        }

        return s * result;
    }

    public static double ErfInv(double z) {
        if (z == 0.0) {
            return 0.0;
        }

        if (z >= 1.0) {
            return Double.POSITIVE_INFINITY;
        }

        if (z <= -1.0) {
            return Double.NEGATIVE_INFINITY;
        }

        double p, q, s;
        if (z < 0) {
            p = -z;
            q = 1 - p;
            s = -1;
        } else {
            p = z;
            q = 1 - z;
            s = 1;
        }

        return ErfInvImpl(p, q, s);
    }

    public static double Erf(double x) {
        if (x == 0) {
            return 0;
        }

        if (Double.isInfinite(x) && x > 0) {
            return 1;
        }

        if (Double.isInfinite(x) && x < 0) {
            return -1;
        }

        if (Double.isNaN(x)) {
            return Double.NaN;
        }

        return ErfImp(x, false);
    }

    public static double ExponentialMinusOne(double power)
        {
            double x = Math.abs(power);
            if (x > 0.1) {
                return Math.exp(power) - 1.0;
            }

            if (x <Precision.PositiveEpsilonOf(x)) {
                return x;
            }

            // Series Expansion to x^k / k!
            // int k = 0;
            // double term = 1.0;
            // Function<Double,Double> f= (yyy)->{
            //     k++;
            //     term *= power;
            //     term /= k;
            //     return term;
            // };

            return Series(power);
        }
        public static double Series(double power) {
            double compensation = 0.0;
            double current;
            double factor = 1 << 16;
            int k = 0;
            double term = 1.0;

            k++;
            term *= power;
            term /= k;
            double sum = term;
    
            do {
                k++;
                term *= power;
                term /= k;
                current=term;
                // Kahan Summation
                // NOTE (ruegg): do NOT optimize. Now, how to tell that the compiler?
                // current = f.apply(0.0);
                double y = current - compensation;
                double t = sum + y;
                compensation = t - sum;
                compensation -= y;
                sum = t;
            } while (Math.abs(sum) < Math.abs(factor * current));
    
            return sum;
        }

    // ==== 修正贝塞尔函数 ModifiedBessel (I/K, Cephes/CERN Chebyshev 展开, 移植自 MathNet.Numerics) ====

    // Chebyshev coefficients for exp(-x) I0(x) in the interval [0, 8].
    private static double[] BesselI0A = {
        -4.41534164647933937950e-18, 3.33079451882223809783e-17, -2.43127984654795469359e-16, 1.71539128555513303061e-15,
        -1.16853328779934516808e-14, 7.67618549860493561688e-14, -4.85644678311192946090e-13, 2.95505266312963983461e-12,
        -1.72682629144155570723e-11, 9.67580903537323691224e-11, -5.18979560163526290666e-10, 2.65982372468238665035e-9,
        -1.30002500998624804212e-8, 6.04699502254191894932e-8, -2.67079385394061173391e-7, 1.11738753912010371815e-6,
        -4.41673835845875056359e-6, 1.64484480707288970893e-5, -5.75419501008210370398e-5, 1.88502885095841655729e-4,
        -5.76375574538582365885e-4, 1.63947561694133579842e-3, -4.32430999505057594430e-3, 1.05464603945949983183e-2,
        -2.37374148058994688156e-2, 4.93052842396707084878e-2, -9.49010970480476444210e-2, 1.71620901522208775349e-1,
        -3.04682672343198398683e-1, 6.76795274409476084995e-1
    };

    // Chebyshev coefficients for exp(-x) sqrt(x) I0(x) in the inverted interval [8, infinity].
    private static double[] BesselI0B = {
        -7.23318048787475395456e-18, -4.83050448594418207126e-18, 4.46562142029675999901e-17, 3.46122286769746109310e-17,
        -2.82762398051658348494e-16, -3.42548561967721913462e-16, 1.77256013305652638360e-15, 3.81168066935262242075e-15,
        -9.55484669882830764870e-15, -4.15056934728722208663e-14, 1.54008621752140982691e-14, 3.85277838274214270114e-13,
        7.18012445138366623367e-13, -1.79417853150680611778e-12, -1.32158118404477131188e-11, -3.14991652796324136454e-11,
        1.18891471078464383424e-11, 4.94060238822496958910e-10, 3.39623202570838634515e-9, 2.26666899049817806459e-8,
        2.04891858946906374183e-7, 2.89137052083475648297e-6, 6.88975834691682398426e-5, 3.36911647825569408990e-3,
        8.04490411014108831608e-1
    };

    // Chebyshev coefficients for exp(-x) I1(x) / x in the interval [0, 8].
    private static double[] BesselI1A = {
        2.77791411276104639959e-18, -2.11142121435816608115e-17, 1.55363195773620046921e-16, -1.10559694773538630805e-15,
        7.60068429473540693410e-15, -5.04218550472791168711e-14, 3.22379336594557470981e-13, -1.98397439776494371520e-12,
        1.17361862988909016308e-11, -6.66348972350202774223e-11, 3.62559028155211703701e-10, -1.88724975172282928790e-9,
        9.38153738649577178388e-9, -4.44505912879632808065e-8, 2.00329475355213526229e-7, -8.56872026469545474066e-7,
        3.47025130813767847674e-6, -1.32731636560394358279e-5, 4.78156510755005422638e-5, -1.61760815825896745588e-4,
        5.12285956168575772895e-4, -1.51357245063125314899e-3, 4.15642294431288815669e-3, -1.05640848946261981558e-2,
        2.47264490306265168283e-2, -5.29459812080949914269e-2, 1.02643658689847095384e-1, -1.76416518357834055153e-1,
        2.52587186443633654823e-1
    };

    // Chebyshev coefficients for exp(-x) sqrt(x) I1(x) in the inverted interval [8, infinity].
    private static double[] BesselI1B = {
        7.51729631084210481353e-18, 4.41434832307170791151e-18, -4.65030536848935832153e-17, -3.20952592199342395980e-17,
        2.96262899764595013876e-16, 3.30820231092092828324e-16, -1.88035477551078244854e-15, -3.81440307243700780478e-15,
        1.04202769841288027642e-14, 4.27244001671195135429e-14, -2.10154184277266431302e-14, -4.08355111109219731823e-13,
        -7.19855177624590851209e-13, 2.03562854414708950722e-12, 1.41258074366137813316e-11, 3.25260358301548823856e-11,
        -1.89749581235054123450e-11, -5.58974346219658380687e-10, -3.83538038596423702205e-9, -2.63146884688951950684e-8,
        -2.51223623787020892529e-7, -3.88256480887769039346e-6, -1.10588938762623716291e-4, -9.76109749136146840777e-3,
        7.78576235018280120474e-1
    };

    // Chebyshev coefficients for K0(x) + log(x/2) I0(x) in the interval [0, 2].
    private static double[] BesselK0A = {
        1.37446543561352307156e-16, 4.25981614279661018399e-14, 1.03496952576338420167e-11, 1.90451637722020886025e-9,
        2.53479107902614945675e-7, 2.28621210311945178607e-5, 1.26461541144692592338e-3, 3.59799365153615016266e-2,
        3.44289899924628486886e-1, -5.35327393233902768720e-1
    };

    // Chebyshev coefficients for exp(x) sqrt(x) K0(x) in the inverted interval [2, infinity].
    private static double[] BesselK0B = {
        5.30043377268626276149e-18, -1.64758043015242134646e-17, 5.21039150503902756861e-17, -1.67823109680541210385e-16,
        5.51205597852431940784e-16, -1.84859337734377901440e-15, 6.34007647740507060557e-15, -2.22751332699166985548e-14,
        8.03289077536357521100e-14, -2.98009692317273043925e-13, 1.14034058820847496303e-12, -4.51459788337394416547e-12,
        1.85594911495471785253e-11, -7.95748924447710747776e-11, 3.57739728140030116597e-10, -1.69753450938905987466e-9,
        8.57403401741422608519e-9, -4.66048989768794782956e-8, 2.76681363944501510342e-7, -1.83175552271911948767e-6,
        1.39498137188764993662e-5, -1.28495495816278026384e-4, 1.56988388573005337491e-3, -3.14481013119645005427e-2,
        2.44030308206595545468e0
    };

    // Chebyshev coefficients for x(K1(x) - log(x/2) I1(x)) in the interval [0, 2].
    private static double[] BesselK1A = {
        -7.02386347938628759343e-18, -2.42744985051936593393e-15, -6.66690169419932900609e-13, -1.41148839263352776110e-10,
        -2.21338763073472585583e-8, -2.43340614156596823496e-6, -1.73028895751305206302e-4, -6.97572385963986435018e-3,
        -1.22611180822657148235e-1, -3.53155960776544875667e-1, 1.52530022733894777053e0
    };

    // Chebyshev coefficients for exp(x) sqrt(x) K1(x) in the interval [2, infinity].
    private static double[] BesselK1B = {
        -5.75674448366501715755e-18, 1.79405087314755922667e-17, -5.68946255844285935196e-17, 1.83809354436663880070e-16,
        -6.05704724837331885336e-16, 2.03870316562433424052e-15, -7.01983709041831346144e-15, 2.47715442448130437068e-14,
        -8.97670518232499435011e-14, 3.34841966607842919884e-13, -1.28917396095102890680e-12, 5.13963967348173025100e-12,
        -2.12996783842756842877e-11, 9.21831518760500529508e-11, -4.19035475934189648750e-10, 2.01504975519703286596e-9,
        -1.03457624656780970260e-8, 5.74108412545004946722e-8, -3.50196060308781257119e-7, 2.40648494783721712015e-6,
        -1.93619797416608296024e-5, 1.95215518471351631108e-4, -2.85781685962277938680e-3, 1.03923736576817238437e-1,
        2.72062619048444266945e0
    };

    /**
     * 返回第一类修正贝塞尔函数 I_n(x)。
     */
    public static double BesselI(int n, double x) {
        if (x < 0) {
            return (n % 2 == 0 ? 1 : -1) * BesselI(n, -x);
        }
        if (x == 0) {
            return (n == 0) ? 1.0 : 0.0;
        }

        double ax = Math.abs(x);
        if (ax < 1e-10) {
            return (n == 0) ? 1.0 : 0.0;
        }

        if (n < 0) n = -n;

        if (ax > 700) {
            double factor = Math.exp(ax) / Math.sqrt(2 * Math.PI * ax);
            return factor * (1.0 - (4.0 * n * n - 1.0) / (8.0 * ax));
        }

        if (n == 0) return BesselI0(x);
        if (n == 1) return BesselI1(x);

        // 递推关系: I_{k+1}(x) = I_{k-1}(x) - 2k/x * I_k(x)
        double I0 = BesselI0(x);
        double I1 = BesselI1(x);
        double In = 0;
        for (int k = 1; k < n; k++) {
            In = I0 - 2.0 * k / x * I1;
            I0 = I1;
            I1 = In;
        }

        return I1;
    }

    /**
     * 返回第二类修正贝塞尔函数 K_n(x),要求 x &gt; 0。
     */
    public static double BesselK(int n, double x) {
        if (n < 0) n = -n;

        if (n == 0) return BesselK0(x);
        if (n == 1) return BesselK1(x);

        // 递推关系: K_{k+1}(x) = K_{k-1}(x) + 2k/x * K_k(x)
        double K0 = BesselK0(x);
        double K1 = BesselK1(x);
        double Kn = 0;
        for (int k = 1; k < n; k++) {
            Kn = K0 + 2.0 * k / x * K1;
            K0 = K1;
            K1 = Kn;
        }

        return K1;
    }

    private static double BesselI0(double x) {
        if (x < 0) {
            x = -x;
        }

        if (x <= 8.0) {
            double y = (x / 2.0) - 2.0;
            return Math.exp(x) * Evaluate.ChebyshevA(BesselI0A, y);
        }

        double x1 = 32.0 / x - 2.0;
        return Math.exp(x) * Evaluate.ChebyshevA(BesselI0B, x1) / Math.sqrt(x);
    }

    private static double BesselI1(double x) {
        double z = Math.abs(x);
        if (z <= 8.0) {
            double y = (z / 2.0) - 2.0;
            z = Evaluate.ChebyshevA(BesselI1A, y) * z * Math.exp(z);
        } else {
            double x1 = 32.0 / z - 2.0;
            z = Math.exp(z) * Evaluate.ChebyshevA(BesselI1B, x1) / Math.sqrt(z);
        }

        if (x < 0.0) {
            z = -z;
        }

        return z;
    }

    private static double BesselK0(double x) {
        if (x <= 2.0) {
            double y = x * x - 2.0;
            return Evaluate.ChebyshevA(BesselK0A, y) - Math.log(0.5 * x) * BesselI0(x);
        }

        double z = 8.0 / x - 2.0;
        return Math.exp(-x) * Evaluate.ChebyshevA(BesselK0B, z) / Math.sqrt(x);
    }

    private static double BesselK1(double x) {
        double z = 0.5 * x;
        if (x <= 2.0) {
            double y = x * x - 2.0;
            return Math.log(z) * BesselI1(x) + Evaluate.ChebyshevA(BesselK1A, y) / x;
        }

        double x1 = 8.0 / x - 2.0;
        return Math.exp(-x) * Evaluate.ChebyshevA(BesselK1B, x1) / Math.sqrt(x);
    }

    // ==== 贝塞尔函数 Bessel (J/Y, Numerical Recipes 式实现) ====

    /**
     * 返回第一类贝塞尔函数 J_n(x)。
     */
    public static double BesselJ(int n, double x) {
        if (x == 0) {
            return (n == 0) ? 1.0 : 0.0;
        }

        if (n < 0) n = -n;

        double ax = Math.abs(x);
        if (ax < 1e-10) {
            return (n == 0) ? 1.0 : 0.0;
        }

        if (n == 0) return BesselJ0(x);
        if (n == 1) return BesselJ1(x);

        if (ax > n) {
            // 递推关系: J_{k+1}(x) = 2k/x * J_k(x) - J_{k-1}(x)
            double J0 = BesselJ0(x);
            double J1 = BesselJ1(x);
            double Jn = 0;

            for (int k = 1; k < n; k++) {
                Jn = (2.0 * k / x) * J1 - J0;
                J0 = J1;
                J1 = Jn;
            }
            return J1;
        }

        // Miller 算法(向下递推)计算 J_n
        int m = (int) (1.5 * n + 10);
        double[] J = new double[m + 2];
        J[m + 1] = 0.0;
        J[m] = 1.0;

        for (int k = m; k >= 1; k--) {
            J[k - 1] = (2.0 * k / x) * J[k] - J[k + 1];
        }

        double sum = 0.0;
        for (int k = 0; k <= m; k += 2) {
            sum += 2.0 * J[k];
        }
        sum -= J[0];

        return J[n] / sum;
    }

    /**
     * 返回第二类贝塞尔函数 Y_n(x),要求 x &gt; 0。
     */
    public static double BesselY(int n, double x) {
        if (n < 0) n = -n;

        if (n == 0) return BesselY0(x);
        if (n == 1) return BesselY1(x);

        // 递推关系: Y_{k+1}(x) = 2k/x * Y_k(x) - Y_{k-1}(x)
        double Y0 = BesselY0(x);
        double Y1 = BesselY1(x);
        double Yn = 0;

        for (int k = 1; k < n; k++) {
            Yn = (2.0 * k / x) * Y1 - Y0;
            Y0 = Y1;
            Y1 = Yn;
        }

        return Y1;
    }

    private static double BesselJ0(double x) {
        double ax = Math.abs(x);
        if (ax < 8.0) {
            double y1 = x * x;
            double ans1 = 57568490574.0 + y1 * (-13362590354.0 + y1 * (651619640.7
                    + y1 * (-11214424.18 + y1 * (77392.33017 + y1 * (-184.9052456)))));
            double ans2 = 57568490411.0 + y1 * (1029532985.0 + y1 * (9494680.718
                    + y1 * (59272.64853 + y1 * (267.8532712 + y1 * 1.0))));
            return ans1 / ans2;
        }
        double z = 8.0 / ax;
        double y2 = z * z;
        double xx = ax - 0.78539816339744830962;
        double ans3 = 1.0 + y2 * (-0.1098628627e-2 + y2 * (0.2734510407e-4
                + y2 * (-0.2073370639e-5 + y2 * 0.2093887211e-6)));
        double ans4 = -0.1562499995e-1 + y2 * (0.1430488765e-3
                + y2 * (-0.6911147651e-5 + y2 * (0.7621095161e-6
                - y2 * 0.934935152e-7)));
        return Math.sqrt(0.63661977236758134308 / ax) * (Math.cos(xx) * ans3 - z * Math.sin(xx) * ans4);
    }

    private static double BesselJ1(double x) {
        double ax = Math.abs(x);
        if (ax < 8.0) {
            double y1 = x * x;
            double ans1 = x * (72362614232.0 + y1 * (-7895059235.0 + y1 * (242396853.1
                    + y1 * (-2972611.439 + y1 * (15704.48260 + y1 * (-30.16036606))))));
            double ans2 = 144725228442.0 + y1 * (2300535178.0 + y1 * (18583304.74
                    + y1 * (99447.43394 + y1 * (376.9991397 + y1 * 1.0))));
            return ans1 / ans2;
        }
        double z = 8.0 / ax;
        double y2 = z * z;
        double xx = ax - 2.35619449019234492885;
        double ans3 = 1.0 + y2 * (0.183105e-2 + y2 * (-0.3516396496e-4
                + y2 * (0.2457520174e-5 + y2 * (-0.240337019e-6))));
        double ans4 = 0.04687499995 + y2 * (-0.2002690873e-3
                + y2 * (0.8449199096e-5 + y2 * (-0.88228987e-6
                + y2 * 0.105787412e-6)));
        double ans = Math.sqrt(0.63661977236758134308 / ax) * (Math.cos(xx) * ans3 - z * Math.sin(xx) * ans4);
        return (x < 0) ? -ans : ans;
    }

    private static double BesselY0(double x) {
        if (x < 8.0) {
            double y1 = x * x;
            double ans1 = -2957821389.0 + y1 * (7062834065.0 + y1 * (-512359803.6
                    + y1 * (10879881.29 + y1 * (-86327.92757 + y1 * 228.4622733))));
            double ans2 = 40076544269.0 + y1 * (745249964.8 + y1 * (7189466.438
                    + y1 * (47447.26470 + y1 * (226.1030244 + y1 * 1.0))));
            return (ans1 / ans2) + 0.63661977236758134308 * BesselJ0(x) * Math.log(x);
        }
        double z = 8.0 / x;
        double y2 = z * z;
        double xx = x - 0.78539816339744830962;
        double ans3 = 1.0 + y2 * (-0.1098628627e-2 + y2 * (0.2734510407e-4
                + y2 * (-0.2073370639e-5 + y2 * 0.2093887211e-6)));
        double ans4 = -0.1562499995e-1 + y2 * (0.1430488765e-3
                + y2 * (-0.6911147651e-5 + y2 * (0.7621095161e-6
                - y2 * 0.934935152e-7)));
        return Math.sqrt(0.63661977236758134308 / x) * (Math.sin(xx) * ans3 + z * Math.cos(xx) * ans4);
    }

    private static double BesselY1(double x) {
        if (x < 8.0) {
            double y1 = x * x;
            double ans1 = x * (-0.4900604943e13 + y1 * (0.1275274390e13
                    + y1 * (-0.5153438139e11 + y1 * (0.7349264551e9
                    + y1 * (-0.4237922726e7 + y1 * 0.8511937935e4)))));
            double ans2 = 0.2499580570e14 + y1 * (0.4244419664e12
                    + y1 * (0.3733650367e10 + y1 * (0.2245904002e8
                    + y1 * (0.1020426050e6 + y1 * (0.3549632885e3 + y1)))));
            return (ans1 / ans2) + 0.63661977236758134308 * (BesselJ1(x) * Math.log(x) - 1.0 / x);
        }
        double z = 8.0 / x;
        double y2 = z * z;
        double xx = x - 2.35619449019234492885;
        double ans3 = 1.0 + y2 * (0.183105e-2 + y2 * (-0.3516396496e-4
                + y2 * (0.2457520174e-5 + y2 * (-0.240337019e-6))));
        double ans4 = 0.04687499995 + y2 * (-0.2002690873e-3
                + y2 * (0.8449199096e-5 + y2 * (-0.88228987e-6
                + y2 * 0.105787412e-6)));
        return Math.sqrt(0.63661977236758134308 / x) * (Math.sin(xx) * ans3 + z * Math.cos(xx) * ans4);
    }
}