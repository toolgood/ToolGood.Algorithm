
class SpecialFunctions{}
SpecialFunctions._factorialCache=[];

function InitializeFactorial() {
    _factorialCache = [];
    _factorialCache[0] = 1.0;
    for (var i = 1; i < 171; i++) {
        _factorialCache[i] = _factorialCache[i - 1] * i;
    }
}
InitializeFactorial();

SpecialFunctions.Binomial=function(n, k) {
    if (k < 0 || n < 0 || k > n) {        return 0.0;    }
    return Math.floor(0.5 + Math.exp(FactorialLn(n) - FactorialLn(k) - FactorialLn(n - k)));
}

SpecialFunctions.FactorialLn=function(x) {
    if (x <= 1) {        return 0;    }
    if (x < _factorialCache.length) {        return Math.log(_factorialCache[x]);    }
    return GammaLn(x + 1.0);
}

SpecialFunctions.BinomialLn=function(n, k) {
    if (k < 0 || n < 0 || k > n) {        return Infinity;    }
    return FactorialLn(n) - FactorialLn(k) - FactorialLn(n - k);
}

SpecialFunctions.GammaLn=function(z) {
    if (z < 0.5) {
        var s = GammaDk[0];
        for (var i = 1; i <= GammaN; i++) {
            s += GammaDk[i] / (i - z);
        }

        return Constants.LnPi - Math.log(Math.sin(Math.PI * z)) - Math.log(s) - Constants.LogTwoSqrtEOverPi
                - ((0.5 - z) * Math.log((0.5 - z + GammaR) / Math.E));
    } else {
        var s = GammaDk[0];
        for (var i = 1; i <= GammaN; i++) {
            s += GammaDk[i] / (z + i - 1.0);
        }

        return Math.log(s) + Constants.LogTwoSqrtEOverPi + ((z - 0.5) * Math.log((z - 0.5 + GammaR) / Math.E));
    }
}

SpecialFunctions.BetaRegularized=function(a, b, x) {
    var bt = (x == 0.0 || x == 1.0) ? 0.0
            : Math.exp(GammaLn(a + b) - GammaLn(a) - GammaLn(b) + (a * Math.log(x)) + (b * Math.log(1.0 - x)));

    var symmetryTransformation = x >= (a + 1.0) / (a + b + 2.0);

    var eps = Precision.DoublePrecision;
    var fpmin = Precision.Increment(0.0) / eps;

    if (symmetryTransformation) {
        x = 1.0 - x;
        var swap = a;
        a = b;
        b = swap;
    }

    var qab = a + b;
    var qap = a + 1.0;
    var qam = a - 1.0;
    var c = 1.0;
    var d = 1.0 - (qab * x / qap);

    if (Math.abs(d) < fpmin) {
        d = fpmin;
    }

    d = 1.0 / d;
    var h = d;

    for (var m = 1, m2 = 2; m <= 140; m++, m2 += 2) {
        var aa = m * (b - m) * x / ((qam + m2) * (a + m2));
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
        var del = d * c;
        h *= del;

        if (Math.abs(del - 1.0) <= eps) {
            return symmetryTransformation ? 1.0 - (bt * h / a) : bt * h / a;
        }
    }

    return symmetryTransformation ? 1.0 - (bt * h / a) : bt * h / a;
}

SpecialFunctions.GammaN = 10;
SpecialFunctions.GammaR = 10.900511;
SpecialFunctions.GammaDk = [ 2.48574089138753565546e-5, 1.05142378581721974210, -3.45687097222016235469,
        4.51227709466894823700, -2.98285225323576655721, 1.05639711577126713077, -1.95428773191645869583e-1,
        1.70970543404441224307e-2, -5.71926117404305781283e-4, 4.63399473359905636708e-6,
        -2.71994908488607703910e-9 ];

SpecialFunctions.GammaLowerRegularized=function(a, x) {
    var epsilon = 0.000000000000001;
    var big = 4503599627370496.0;
    var bigInv = 2.22044604925031308085e-16;
 

    if (Precision.AlmostEqual(a, 0.0)) {
        if (Precision.AlmostEqual(x, 0.0)) {
            // use right hand limit value because so that regularized upper/lower gamma
            // definition holds.
            return 1;
        }

        return 1;
    }

    if (Precision.AlmostEqual(x, 0.0)) {
        return 0;
    }

    var ax = (a * Math.log(x)) - x - GammaLn(a);
    if (ax < -709.78271289338399) {        return a < x ? 1 : 0;    }

    if (x <= 1 || x <= a) {
        var r2 = a;
        var c2 = 1;
        var ans2 = 1;

        do {
            r2 = r2 + 1;
            c2 = c2 * x / r2;
            ans2 += c2;
        } while ((c2 / ans2) > epsilon);

        return Math.exp(ax) * ans2 / a;
    }

    var c = 0;
    var y = 1 - a;
    var z = x + y + 1;

    var p3 = 1;
    var q3 = x;
    var p2 = x + 1;
    var q2 = z * x;
    var ans = p2 / q2;

    var error;

    do {
        c++;
        y += 1;
        z += 2;
        var yc = y * c;

        var p = (p2 * z) - (p3 * yc);
        var q = (q2 * z) - (q3 * yc);

        if (q != 0) {
            var nextans = p / q;
            error = Math.abs((ans - nextans) / nextans);
            ans = nextans;
        } else {
            error = 1;
        }

        p3 = p2;
        p2 = p;
        q3 = q2;
        q2 = q;

        if (Math.abs(p) > big) {
            p3 *= bigInv;
            p2 *= bigInv;
            q3 *= bigInv;
            q2 *= bigInv;
        }
    } while (error > epsilon);

    return 1 - (Math.exp(ax) * ans);
}


SpecialFunctions.GammaLowerRegularizedInv=function(a, y0) {
    var epsilon = 0.000000000000001;
    var big = 4503599627370496.0;
    var threshold = 5 * epsilon;

    if (isNaN(a) || isNaN(y0)) {        return NaN;    }
 
    if (Precision.AlmostEqual(y0, 0.0)) {        return 0;    }

    if (Precision.AlmostEqual(y0, 1.0)) {        return Infinity;    }

    y0 = 1 - y0;

    var xUpper = big;
    var xLower = 0;
    var yUpper = 1;
    var yLower = 0;

    var d = 1 / (9 * a);
    var y = 1 - d - (0.98 * Constants.Sqrt2 * ErfInv((2.0 * y0) - 1.0) * Math.sqrt(d));
    var x = a * y * y * y;
    var lgm = GammaLn(a);

    for (var i = 0; i < 10; i++) {
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

    var dir = 0;
    d = 0.5;
    for (var i = 0; i < 400; i++) {
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

        if (x <= 0) {
            return 0;
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

SpecialFunctions.Gamma=function(z) {
    if (z < 0.5) {
        var s = GammaDk[0];
        for (var i = 1; i <= GammaN; i++) {
            s += GammaDk[i] / (i - z);
        }

        return Math.PI / (Math.sin(Math.PI * z) * s * Constants.TwoSqrtEOverPi
                * Math.pow((0.5 - z + GammaR) / Math.E, 0.5 - z));
    } else {
        var s = GammaDk[0];
        for (var i = 1; i <= GammaN; i++) {
            s += GammaDk[i] / (z + i - 1.0);
        }

        return s * Constants.TwoSqrtEOverPi * Math.pow((z - 0.5 + GammaR) / Math.E, z - 0.5);
    }
}

SpecialFunctions.ErfImpAn = [ 0.00337916709551257388990745, -0.00073695653048167948530905,
    -0.374732337392919607868241, 0.0817442448733587196071743, -0.0421089319936548595203468,
    0.0070165709512095756344528, -0.00495091255982435110337458, 0.000871646599037922480317225 ];
SpecialFunctions.ErfImpAd = [  1, -0.218088218087924645390535, 0.412542972725442099083918,
    -0.0841891147873106755410271, 0.0655338856400241519690695, -0.0120019604454941768171266,
    0.00408165558926174048329689, -0.000615900721557769691924509 ];
SpecialFunctions.ErfImpBn =[  -0.0361790390718262471360258, 0.292251883444882683221149,
    0.281447041797604512774415, 0.125610208862766947294894, 0.0274135028268930549240776,
    0.00250839672168065762786937 ];
SpecialFunctions.ErfImpBd = [ 1, 1.8545005897903486499845, 1.43575803037831418074962,
    0.582827658753036572454135, 0.124810476932949746447682, 0.0113724176546353285778481 ];
SpecialFunctions.ErfImpCn =[  -0.0397876892611136856954425, 0.153165212467878293257683,
    0.191260295600936245503129, 0.10276327061989304213645, 0.029637090615738836726027,
    0.0046093486780275489468812, 0.000307607820348680180548455 ];
SpecialFunctions.ErfImpCd = [ 1, 1.95520072987627704987886, 1.64762317199384860109595,
    0.768238607022126250082483, 0.209793185936509782784315, 0.0319569316899913392596356,
    0.00213363160895785378615014 ];
SpecialFunctions.ErfImpDn =[  -0.0300838560557949717328341, 0.0538578829844454508530552,
    0.0726211541651914182692959, 0.0367628469888049348429018, 0.00964629015572527529605267,
    0.00133453480075291076745275, 0.778087599782504251917881e-4 ];
SpecialFunctions.ErfImpDd = [  1, 1.75967098147167528287343, 1.32883571437961120556307,
    0.552528596508757581287907, 0.133793056941332861912279, 0.0179509645176280768640766,
    0.00104712440019937356634038, -0.106640381820357337177643e-7 ];
SpecialFunctions.ErfImpEn = [  -0.0117907570137227847827732, 0.014262132090538809896674,
    0.0202234435902960820020765, 0.00930668299990432009042239, 0.00213357802422065994322516,
    0.00025022987386460102395382, 0.120534912219588189822126e-4 ];
SpecialFunctions.ErfImpEd = [ 1, 1.50376225203620482047419, 0.965397786204462896346934,
    0.339265230476796681555511, 0.0689740649541569716897427, 0.00771060262491768307365526,
    0.000371421101531069302990367 ];
SpecialFunctions.ErfImpFn = [  -0.00546954795538729307482955, 0.00404190278731707110245394,
    0.0054963369553161170521356, 0.00212616472603945399437862, 0.000394984014495083900689956,
    0.365565477064442377259271e-4, 0.135485897109932323253786e-5 ];
SpecialFunctions.ErfImpFd = [  1, 1.21019697773630784832251, 0.620914668221143886601045,
    0.173038430661142762569515, 0.0276550813773432047594539, 0.00240625974424309709745382,
    0.891811817251336577241006e-4, -0.465528836283382684461025e-11 ];
SpecialFunctions.ErfImpGn = [ -0.00270722535905778347999196, 0.0013187563425029400461378,
    0.00119925933261002333923989, 0.00027849619811344664248235, 0.267822988218331849989363e-4,
    0.923043672315028197865066e-6 ];
SpecialFunctions.ErfImpGd = [  1, 0.814632808543141591118279, 0.268901665856299542168425,
    0.0449877216103041118694989, 0.00381759663320248459168994, 0.000131571897888596914350697,
    0.404815359675764138445257e-11 ];
SpecialFunctions.ErfImpHn = [  -0.00109946720691742196814323, 0.000406425442750422675169153,
    0.000274499489416900707787024, 0.465293770646659383436343e-4, 0.320955425395767463401993e-5,
    0.778286018145020892261936e-7 ];
SpecialFunctions.ErfImpHd = [  1, 0.588173710611846046373373, 0.139363331289409746077541,
    0.0166329340417083678763028, 0.00100023921310234908642639, 0.24254837521587225125068e-4 ];
SpecialFunctions.ErfImpIn = [  -0.00056907993601094962855594, 0.000169498540373762264416984,
    0.518472354581100890120501e-4, 0.382819312231928859704678e-5, 0.824989931281894431781794e-7 ];
SpecialFunctions.ErfImpId = [  1, 0.339637250051139347430323, 0.043472647870310663055044,
    0.00248549335224637114641629, 0.535633305337152900549536e-4, -0.117490944405459578783846e-12 ];
SpecialFunctions.ErfImpJn = [  -0.000241313599483991337479091, 0.574224975202501512365975e-4,
    0.115998962927383778460557e-4, 0.581762134402593739370875e-6, 0.853971555085673614607418e-8 ];
SpecialFunctions.ErfImpJd = [ 1, 0.233044138299687841018015, 0.0204186940546440312625597,
    0.000797185647564398289151125, 0.117019281670172327758019e-4 ];
SpecialFunctions.ErfImpKn = [  -0.000146674699277760365803642, 0.162666552112280519955647e-4,
    0.269116248509165239294897e-5, 0.979584479468091935086972e-7, 0.101994647625723465722285e-8 ];
SpecialFunctions.ErfImpKd = [  1, 0.165907812944847226546036, 0.0103361716191505884359634,
    0.000286593026373868366935721, 0.298401570840900340874568e-5 ];
SpecialFunctions.ErfImpLn = [ -0.583905797629771786720406e-4, 0.412510325105496173512992e-5,
    0.431790922420250949096906e-6, 0.993365155590013193345569e-8, 0.653480510020104699270084e-10 ];
SpecialFunctions.ErfImpLd = [ 1, 0.105077086072039915406159, 0.00414278428675475620830226,
    0.726338754644523769144108e-4, 0.477818471047398785369849e-6 ];
SpecialFunctions.ErfImpMn = [ -0.196457797609229579459841e-4, 0.157243887666800692441195e-5,
    0.543902511192700878690335e-7, 0.317472492369117710852685e-9 ];
SpecialFunctions.ErfImpMd = [ 1, 0.052803989240957632204885, 0.000926876069151753290378112,
    0.541011723226630257077328e-5, 0.535093845803642394908747e-15 ];
SpecialFunctions.ErfImpNn = [  -0.789224703978722689089794e-5, 0.622088451660986955124162e-6,
    0.145728445676882396797184e-7, 0.603715505542715364529243e-10 ];
SpecialFunctions.ErfImpNd = [ 1, 0.0375328846356293715248719, 0.000467919535974625308126054,
    0.193847039275845656900547e-5 ];

SpecialFunctions.ErvInvImpAn = [ -0.000508781949658280665617, -0.00836874819741736770379,
    0.0334806625409744615033, -0.0126926147662974029034, -0.0365637971411762664006, 0.0219878681111168899165,
    0.00822687874676915743155, -0.00538772965071242932965 ];
SpecialFunctions.ErvInvImpAd = [ 1, -0.970005043303290640362, -1.56574558234175846809,
    1.56221558398423026363, 0.662328840472002992063, -0.71228902341542847553, -0.0527396382340099713954,
    0.0795283687341571680018, -0.00233393759374190016776, 0.000886216390456424707504 ];
SpecialFunctions.ErvInvImpBn = [ -0.202433508355938759655, 0.105264680699391713268, 8.37050328343119927838,
    17.6447298408374015486, -18.8510648058714251895, -44.6382324441786960818, 17.445385985570866523,
    21.1294655448340526258, -3.67192254707729348546 ];
SpecialFunctions.ErvInvImpBd = [ 1, 6.24264124854247537712, 3.9713437953343869095, -28.6608180499800029974,
    -20.1432634680485188801, 48.5609213108739935468, 10.8268667355460159008, -22.6436933413139721736,
    1.72114765761200282724 ];
SpecialFunctions.ErvInvImpCn = [ -0.131102781679951906451, -0.163794047193317060787, 0.117030156341995252019,
    0.387079738972604337464, 0.337785538912035898924, 0.142869534408157156766, 0.0290157910005329060432,
    0.00214558995388805277169, -0.679465575181126350155e-6, 0.285225331782217055858e-7,
    -0.681149956853776992068e-9 ];
SpecialFunctions.ErvInvImpCd = [ 1, 3.46625407242567245975, 5.38168345707006855425, 4.77846592945843778382,
    2.59301921623620271374, 0.848854343457902036425, 0.152264338295331783612, 0.01105924229346489121 ];
SpecialFunctions.ErvInvImpDn = [ -0.0350353787183177984712, -0.00222426529213447927281,
    0.0185573306514231072324, 0.00950804701325919603619, 0.00187123492819559223345, 0.000157544617424960554631,
    0.460469890584317994083e-5, -0.230404776911882601748e-9, 0.266339227425782031962e-11 ];
SpecialFunctions.ErvInvImpDd = [ 1, 1.3653349817554063097, 0.762059164553623404043, 0.220091105764131249824,
    0.0341589143670947727934, 0.00263861676657015992959, 0.764675292302794483503e-4 ];
SpecialFunctions.ErvInvImpEn = [ -0.0167431005076633737133, -0.00112951438745580278863,
    0.00105628862152492910091, 0.000209386317487588078668, 0.149624783758342370182e-4,
    0.449696789927706453732e-6, 0.462596163522878599135e-8, -0.281128735628831791805e-13,
    0.99055709973310326855e-16 ];
SpecialFunctions.ErvInvImpEd = [ 1, 0.591429344886417493481, 0.138151865749083321638,
    0.0160746087093676504695, 0.000964011807005165528527, 0.275335474764726041141e-4,
    0.282243172016108031869e-6 ];
SpecialFunctions.ErvInvImpFn = [ -0.0024978212791898131227, -0.779190719229053954292e-5,
    0.254723037413027451751e-4, 0.162397777342510920873e-5, 0.396341011304801168516e-7,
    0.411632831190944208473e-9, 0.145596286718675035587e-11, -0.116765012397184275695e-17 ];
SpecialFunctions.ErvInvImpFd = [ 1, 0.207123112214422517181, 0.0169410838120975906478,
    0.000690538265622684595676, 0.145007359818232637924e-4, 0.144437756628144157666e-6,
    0.509761276599778486139e-9 ];
SpecialFunctions.ErvInvImpGn = [ -0.000539042911019078575891, -0.28398759004727721098e-6,
    0.899465114892291446442e-6, 0.229345859265920864296e-7, 0.225561444863500149219e-9,
    0.947846627503022684216e-12, 0.135880130108924861008e-14, -0.348890393399948882918e-21 ];
SpecialFunctions.ErvInvImpGd = [ 1, 0.0845746234001899436914, 0.00282092984726264681981,
    0.468292921940894236786e-4, 0.399968812193862100054e-6, 0.161809290887904476097e-8,
    0.231558608310259605225e-11 ];

SpecialFunctions.Erfc=function(x) {
    if (x == 0) {        return 1;    }
    if (Double.isInfinite(x) && x > 0) {        return 0;    }
    if (Double.isInfinite(x) && x < 0) {        return 2;    }
    if (isNaN(x)) {        return NaN;    }
    return ErfImp(x, true);
}

SpecialFunctions.ErfImp=function(z, invert) {
    if (z < 0) {
        if (!invert) {            return -ErfImp(-z, false);        }

        if (z < -0.5) {            return 2 - ErfImp(-z, true);        }

        return 1 + ErfImp(-z, false);
    }

    var result;

    if (z < 0.5) {
        if (z < 1e-10) {
            result = (z * 1.125) + (z * 0.003379167095512573896158903121545171688);
        } else {
            result = (z * 1.125) + (z * Evaluate.Polynomial(z, ErfImpAn) / Evaluate.Polynomial(z, ErfImpAd));
        }
    } else if ((z < 110) || ((z < 110) && invert)) {
        // We'll be calculating erfc:
        invert = !invert;
        var r, b;
        if (z < 0.75) {
            // Worst case absolute error found: 5.582813374e-21
            r = Evaluate.Polynomial(z - 0.5, ErfImpBn) / Evaluate.Polynomial(z - 0.5, ErfImpBd);
            b = 0.3440242112;
        } else if (z < 1.25) {
            // Worst case absolute error found: 4.01854729e-21
            r = Evaluate.Polynomial(z - 0.75, ErfImpCn) / Evaluate.Polynomial(z - 0.75, ErfImpCd);
            b = 0.419990927;
        } else if (z < 2.25) {
            // Worst case absolute error found: 2.866005373e-21
            r = Evaluate.Polynomial(z - 1.25, ErfImpDn) / Evaluate.Polynomial(z - 1.25, ErfImpDd);
            b = 0.4898625016;
        } else if (z < 3.5) {
            // Worst case absolute error found: 1.045355789e-21
            r = Evaluate.Polynomial(z - 2.25, ErfImpEn) / Evaluate.Polynomial(z - 2.25, ErfImpEd);
            b = 0.5317370892;
        } else if (z < 5.25) {
            // Worst case absolute error found: 8.300028706e-22
            r = Evaluate.Polynomial(z - 3.5, ErfImpFn) / Evaluate.Polynomial(z - 3.5, ErfImpFd);
            b = 0.5489973426;
        } else if (z < 8) {
            // Worst case absolute error found: 1.700157534e-21
            r = Evaluate.Polynomial(z - 5.25, ErfImpGn) / Evaluate.Polynomial(z - 5.25, ErfImpGd);
            b = 0.5571740866;
        } else if (z < 11.5) {
            // Worst case absolute error found: 3.002278011e-22
            r = Evaluate.Polynomial(z - 8, ErfImpHn) / Evaluate.Polynomial(z - 8, ErfImpHd);
            b = 0.5609807968;
        } else if (z < 17) {
            // Worst case absolute error found: 6.741114695e-21
            r = Evaluate.Polynomial(z - 11.5, ErfImpIn) / Evaluate.Polynomial(z - 11.5, ErfImpId);
            b = 0.5626493692;
        } else if (z < 24) {
            // Worst case absolute error found: 7.802346984e-22
            r = Evaluate.Polynomial(z - 17, ErfImpJn) / Evaluate.Polynomial(z - 17, ErfImpJd);
            b = 0.5634598136;
        } else if (z < 38) {
            // Worst case absolute error found: 2.414228989e-22
            r = Evaluate.Polynomial(z - 24, ErfImpKn) / Evaluate.Polynomial(z - 24, ErfImpKd);
            b = 0.5638477802;
        } else if (z < 60) {
            // Worst case absolute error found: 5.896543869e-24
            r = Evaluate.Polynomial(z - 38, ErfImpLn) / Evaluate.Polynomial(z - 38, ErfImpLd);
            b = 0.5640528202;
        } else if (z < 85) {
            // Worst case absolute error found: 3.080612264e-21
            r = Evaluate.Polynomial(z - 60, ErfImpMn) / Evaluate.Polynomial(z - 60, ErfImpMd);
            b = 0.5641309023;
        } else {
            // Worst case absolute error found: 8.094633491e-22
            r = Evaluate.Polynomial(z - 85, ErfImpNn) / Evaluate.Polynomial(z - 85, ErfImpNd);
            b = 0.5641584396;
        }

        var g = Math.exp(-z * z) / z;
        result = (g * b) + (g * r);
    } else {
        result = 0;
        invert = !invert;
    }
    if (invert) {        result = 1 - result;    }

    return result;
}

SpecialFunctions.ErfcInv=function(z) {
    if (z <= 0.0) {        return Infinity;    }

    if (z >= 2.0) {        return -Infinity;    }

    var p, q, s;
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

SpecialFunctions.ErfInvImpl=function(p, q, s) {
    var result;

    if (p <= 0.5) {
        var y = 0.0891314744949340820313;
        var g = p * (p + 10);
        var r = Evaluate.Polynomial(p, ErvInvImpAn) / Evaluate.Polynomial(p, ErvInvImpAd);
        result = (g * y) + (g * r);
    } else if (q >= 0.25) {
        var y = 2.249481201171875;
        var g = Math.sqrt(-2 * Math.log(q));
        var xs = q - 0.25;
        var r = Evaluate.Polynomial(xs, ErvInvImpBn) / Evaluate.Polynomial(xs, ErvInvImpBd);
        result = g / (y + r);
    } else {
        var x = Math.sqrt(-Math.log(q));
        if (x < 3) {
            // Max error found: 1.089051e-20
            var y = 0.807220458984375;
            var xs = x - 1.125;
            var r = Evaluate.Polynomial(xs, ErvInvImpCn) / Evaluate.Polynomial(xs, ErvInvImpCd);
            result = (y * x) + (r * x);
        } else if (x < 6) {
            // Max error found: 8.389174e-21
            var y = 0.93995571136474609375;
            var xs = x - 3;
            var r = Evaluate.Polynomial(xs, ErvInvImpDn) / Evaluate.Polynomial(xs, ErvInvImpDd);
            result = (y * x) + (r * x);
        } else if (x < 18) {
            // Max error found: 1.481312e-19
            var y = 0.98362827301025390625;
            var xs = x - 6;
            var r = Evaluate.Polynomial(xs, ErvInvImpEn) / Evaluate.Polynomial(xs, ErvInvImpEd);
            result = (y * x) + (r * x);
        } else if (x < 44) {
            // Max error found: 5.697761e-20
            var y = 0.99714565277099609375;
            var xs = x - 18;
            var r = Evaluate.Polynomial(xs, ErvInvImpFn) / Evaluate.Polynomial(xs, ErvInvImpFd);
            result = (y * x) + (r * x);
        } else {
            // Max error found: 1.279746e-20
            var y = 0.99941349029541015625;
            var xs = x - 44;
            var r = Evaluate.Polynomial(xs, ErvInvImpGn) / Evaluate.Polynomial(xs, ErvInvImpGd);
            result = (y * x) + (r * x);
        }
    }

    return s * result;
}

SpecialFunctions.ErfInv=function( z) {
    if (z == 0.0) {        return 0.0;    }
    if (z >= 1.0) {        return Infinity;    }

    if (z <= -1.0) {        return -Infinity;    }

    var p, q, s;
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

SpecialFunctions.Erf=function( x) {
    if (x == 0) {        return 0;    }
    if (isFinite(x) && x > 0) {        return 1;    }
    if (isFinite(x) && x < 0) {        return -1;    }
    if (isNaN(x)) {        return NaN;    }
    return ErfImp(x, false);
}

SpecialFunctions.ExponentialMinusOne=function(power)    {
    var x = Math.abs(power);
    if (x > 0.1) {        return Math.exp(power) - 1.0;    }

    if (x <Precision.PositiveEpsilonOf(x)) {        return x;    }
    return Series(power);
}
SpecialFunctions.Series=function(power) {
    var compensation = 0.0;
    var current;
    var factor = 1 << 16;
    var k = 0;
    var term = 1.0;

    k++;
    term *= power;
    term /= k;
    var sum = term;

    do {
        k++;
        term *= power;
        term /= k;
        current=term;
        var y = current - compensation;
        var t = sum + y;
        compensation = t - sum;
        compensation -= y;
        sum = t;
    } while (Math.abs(sum) < Math.abs(factor * current));

    return sum;
}

module.exports=SpecialFunctions;